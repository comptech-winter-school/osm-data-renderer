package cron

import (
	"context"
	"encoding/json"
	"errors"
	"github.com/comptech-winter-school/osm-data-renderer/server/internal/osm"
	"github.com/qedus/osmpbf"
	"io"
	"os"
	"runtime"
)

//При необходимости можно взять .osm.pfb файл
//И скопировать его в папку ./assets/protobuf
//Например по ссылке (~16MB):
//http://download.geofabrik.de/russia/kaliningrad-latest.osm.pbf

func WritePbfToDataBase(store *osm.Storage, pbfFileName string) error {

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

	nodes := make(map[int64]osm.Point) //Под оптимизацию

	for {
		if v, err := d.Decode(); err == io.EOF {
			break
		} else if err != nil {
			return err
		} else {
			switch v := v.(type) {
			case *osmpbf.Node:
				p := osm.Point{
					Lat: v.Lat,
					Lon: v.Lon,
				}
				nodes[v.ID] = p
			case *osmpbf.Way:
				if len(v.Tags) > 0 {
					tg := v.Tags
					if _, ok := tg["building"]; ok {
						var poly osm.Polygon
						for _, element := range v.NodeIDs {
							poly.Vertex = append(poly.Vertex, nodes[element])
						}
						jsonPoly, err := json.Marshal(poly)
						if err != nil {
							return err
						}

						err = store.UpsertOsmData(context.Background(), osm.OSM{
							Name:      tg["name"],
							WayId:     v.ID,
							Polygon:   string(jsonPoly),
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
