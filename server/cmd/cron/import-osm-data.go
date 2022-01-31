package cron

import (
	"context"
	"errors"
	"io"
	"os"
	"runtime"

	"github.com/comptech-winter-school/osm-data-renderer/server/internal/osm"
	"github.com/qedus/osmpbf"
)

/*
	При необходимости можно взять .osm.pfb файл
	И скопировать его в папку ./assets/protobuf
	Например по ссылке (~16MB):
	http://download.geofabrik.de/russia/kaliningrad-latest.osm.pbf
*/

func WritePbfToBD(store *osm.Storage, pbfFileName string) error {
	const PROTOBUF_PATH = "../assets/protobuf/" // под вынос

	f, err := os.Open(PROTOBUF_PATH + pbfFileName)
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

	for {
		if v, err := d.Decode(); err == io.EOF {
			break
		} else if err != nil {
			return err
		} else {
			switch v := v.(type) {
			case *osmpbf.Node:
				if len(v.Tags) > 0 {
					tg := v.Tags
					if _, ok := tg["building"]; ok {
						err := store.UpsertOsmData(context.Background(), osm.OSM{
							Name:      tg["name"],
							NodeId:    v.ID,
							Lat:       v.Lat,
							Lng:       v.Lon,
							CreatedAt: v.Info.Timestamp,
							UpdatedAt: v.Info.Timestamp,
						})
						if err != nil {
							return err
						}
					}
				}
			case *osmpbf.Way:
			case *osmpbf.Relation:
			default:
				return errors.New("unknown type")
			}
		}
	}
	return nil
}
