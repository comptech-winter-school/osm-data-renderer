package api_v2

import (
	"context"
	"encoding/json"
	"errors"
	"github.com/comptech-winter-school/osm-data-renderer/server/internal/application/handler/api_v1/model"
	model2 "github.com/comptech-winter-school/osm-data-renderer/server/internal/application/handler/api_v2/model"
	"github.com/comptech-winter-school/osm-data-renderer/server/internal/osm"
	"io/ioutil"
	"log"
	"net/http"
	"strconv"
)

type storage interface {
	GetOsmDataByBox(ctx context.Context, minLat, minLon, maxLat, maxLon float32) (*[]osm.OSM, error)
}

type Handler struct {
	storage storage
}

func NewHandler(storage storage) *Handler {
	return &Handler{storage: storage}
}

func ChunkPosToBox(chPos model2.IntPoint) model2.ChunkBox {
	return model2.ChunkBox{
		Min: model2.FloatPoint{
			X: float32(chPos.X) * model2.ChunkLatSize,
			Y: float32(chPos.Y) * model2.ChunkLatSize,
		},
		Max: model2.FloatPoint{
			X: float32(chPos.X+1) * model2.ChunkLonSize,
			Y: float32(chPos.Y+1) * model2.ChunkLonSize,
		},
	}
}

func GetObjectData(h *Handler, pos model2.IntPoint, loadDistance int, cache []model2.CacheInfo) ([]model2.Chunk, error) {
	var chunks []model2.Chunk
	for dx := -loadDistance; dx <= loadDistance; dx++ {
		for dy := -loadDistance; dy <= loadDistance; dy++ {
			curChunkPos := model2.IntPoint{
				X: pos.X + dx,
				Y: pos.Y + dy,
			}

			chunkBox := ChunkPosToBox(curChunkPos)

			osmData, err := h.storage.GetOsmDataByBox(context.Background(), chunkBox.Min.X, chunkBox.Min.Y, chunkBox.Max.X, chunkBox.Max.Y)
			if err != nil {
				return nil, err
			}

			var buildings []model.Building
			var highways []model.Highways
			for _, o := range *osmData {
				poly, err := osm.LineStringToLine(o.Polygon)
				if err != nil {
					return nil, err
				}

				var tags map[string]string
				err = json.Unmarshal([]byte(o.Tags), &tags)
				if err != nil {
					return nil, err
				}
				if o.Type == "building" {
					building := model.Building{
						Polygon: poly,
					}
					if len(tags["building:levels"]) > 0 {
						in, err := strconv.Atoi(tags["building:levels"])
						if err != nil {
							return nil, err
						}
						building.Levels = uint(in)
					}
					buildings = append(buildings, building)
				} else if o.Type == "highway" {

					highways = append(highways, model.Highways{
						Polygon: poly,
					})
				}
			}

			data := model2.Data{
				Buildings: buildings,
				Highways:  highways,
			}
			chunks = append(chunks, model2.Chunk{
				Position:  curChunkPos,
				IsUpdated: true,
				Data:      data,
			})
		}
	}
	return chunks, nil
}

func (h *Handler) GetObjects(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Content-Type", "application/json")

	body, err := ioutil.ReadAll(r.Body)
	if err != nil {
		w.WriteHeader(http.StatusBadRequest)
		log.Print(err)
		return
	}
	var t model2.GetObjectsRequest
	err = json.Unmarshal(body, &t)
	if err != nil {
		w.WriteHeader(http.StatusBadRequest)
		log.Print(err)
		return
	}

	if t.ChunkLoadingDistance < 0 {
		w.WriteHeader(http.StatusBadRequest)
		log.Print(errors.New("negative radius"))
		return
	}

	chunks, err := GetObjectData(h, t.Position, int(t.ChunkLoadingDistance), t.ChunkCache)
	if err != nil {
		w.WriteHeader(http.StatusBadRequest)
		log.Print(err)
		return
	}
	resp := model2.GetObjectsResponse{
		Chunks: chunks,
	}

	json.NewEncoder(w).Encode(resp)
}
