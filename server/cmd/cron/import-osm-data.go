package main

import (
	"context"
	"encoding/json"
	"errors"
	"flag"
	"github.com/comptech-winter-school/osm-data-renderer/server/internal/infrastructure/db"
	"github.com/comptech-winter-school/osm-data-renderer/server/internal/osm"
	filesystem "github.com/comptech-winter-school/osm-data-renderer/server/pkg/utils/file-system"
	"github.com/joho/godotenv"
	"github.com/qedus/osmpbf"
	"io"
	"log"
	"os"
	"runtime"
)

const (
	pbfBaseUrlArgName         = "base_url"
	pbfFilenameArgName        = "pbf_name"
	pbfDefaultBaseDownloadUrl = "http://download.geofabrik.de/russia/"
	pbfBaseDownloadUrlUsage   = "base download url"
	pbfDefaultName            = "kaliningrad-latest.osm.pbf"
	pbfFileNameUsage          = "pbf file name (central-fed-district-latest.osm.pbf for Moscow)"
)

func main() {
	var pbfFileName string
	var downloadBaseUrl string

	flag.StringVar(&downloadBaseUrl, pbfBaseUrlArgName, pbfDefaultBaseDownloadUrl, pbfBaseDownloadUrlUsage)
	flag.StringVar(&pbfFileName, pbfFilenameArgName, pbfDefaultName, pbfFileNameUsage)
	flag.Parse()

	err := godotenv.Load()
	if err != nil {
		log.Fatal("Error loading .env file")
	}

	conn := db.OpenDB()
	defer conn.Close()

	log.Println("Start downloading...")
	err = filesystem.DownloadFile(os.Getenv("PROTOBUF_PATH")+pbfFileName, downloadBaseUrl+pbfFileName)
	if err != nil {
		log.Fatal(err)
	}
	log.Println("Downloading finished")

	osmStorage := osm.NewStorage(conn)
	log.Println("Start importing...")
	err = ImportOsmData(osmStorage, pbfFileName)
	if err != nil {
		log.Fatal(err)
	}
	log.Println("Done")

	err = os.Remove(os.Getenv("PROTOBUF_PATH") + pbfFileName)
	log.Println("Temp files deleted")
}

func InversePolygon(poly []osm.Point) []osm.Point {
	var invPoly []osm.Point
	for i := len(poly); i >= 0; i-- {
		invPoly = append(invPoly, poly[i])
	}
	return invPoly
}

func ImportOsmData(store *osm.Storage, pbfFileName string) error {

	f, err := os.Open(os.Getenv("PROTOBUF_PATH") + pbfFileName)
	if err != nil {
		return err
	}
	defer f.Close()

	d := osmpbf.NewDecoder(f)

	d.SetBufferSize(osmpbf.MaxBlobSize)

	err = d.Start(runtime.GOMAXPROCS(-1))
	if err != nil {
		return err
	}

	nodes := make(map[int64]osm.Point)

	for {
		if v, err := d.Decode(); err == io.EOF {
			break
		} else if err != nil {
			return err
		} else {
			switch v := v.(type) {
			case *osmpbf.Node:
				p := osm.Point{
					X: v.Lat,
					Y: v.Lon,
				}
				nodes[v.ID] = p
			case *osmpbf.Way:
				if len(v.Tags) > 0 {
					tg := v.Tags

					wayType := "default"
					wayTypeWhiteList := []string{"building", "highway"}
					for _, s := range wayTypeWhiteList {
						if _, ok := tg[s]; ok {
							wayType = s
							break
						}
					}

					if wayType != "default" {
						var line []osm.Point
						for _, element := range v.NodeIDs {
							line = append(line, nodes[element])
						}
						if wayType == "building" {
							line = InversePolygon(line)
						}
						lineString, err := osm.LineToLineString(line)
						if err != nil {
							return err
						}

						jsonTags, err := json.Marshal(v.Tags)
						if err != nil {
							return err
						}

						if len(v.NodeIDs) == 0 {
							return errors.New("empty way")
						}
						wayPoint := nodes[v.NodeIDs[0]]
						err = store.UpsertOsmData(context.Background(), osm.OSM{
							WayId:     v.ID,
							Name:      tg["name"],
							Polygon:   lineString,
							Lat:       wayPoint.X,
							Lon:       wayPoint.Y,
							Tags:      string(jsonTags),
							Type:      wayType,
							CreatedAt: v.Info.Timestamp,
							UpdatedAt: v.Info.Timestamp,
						})
						if err != nil {
							return err
						}
					}
				}
			case *osmpbf.Relation:
			default:
				return errors.New("unknown type")
			}
		}
	}
	return nil
}
