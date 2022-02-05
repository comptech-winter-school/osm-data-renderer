package api_v1

import (
	"context"
	"encoding/json"
	"errors"
	"github.com/comptech-winter-school/osm-data-renderer/server/internal/application/handler/api_v1/model"
	"github.com/comptech-winter-school/osm-data-renderer/server/internal/osm"
	"io/ioutil"
	"log"
	"net/http"
	"strconv"
)

type storage interface {
	GetOsmDataByRadius(ctx context.Context, Lat, Lng, RadiusMeters float64) (*[]osm.OSM, error)
}

type Handler struct {
	storage storage
}

func NewHandler(storage storage) *Handler {
	return &Handler{storage: storage}
}

func GetObjectData(h *Handler, lat, lon, radius float32) ([]model.Building, []model.Highways, error) {
	osmData, err := h.storage.GetOsmDataByRadius(context.Background(), float64(lat), float64(lon), float64(radius))
	if err != nil {
		return nil, nil, err
	}

	var buildings []model.Building
	var highways []model.Highways
	for _, o := range *osmData {
		poly, err := osm.LineStringToLine(o.Polygon)
		if err != nil {
			return nil, nil, err
		}

		var tags map[string]string
		err = json.Unmarshal([]byte(o.Tags), &tags)
		if err != nil {
			return nil, nil, err
		}
		if o.Type == "building" {
			building := model.Building{
				Polygon: poly,
			}
			if len(tags["building:levels"]) > 0 {
				in, err := strconv.Atoi(tags["building:levels"])
				if err != nil {
					return nil, nil, err
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

	return buildings, highways, nil
}

func (h *Handler) GetObjects(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Content-Type", "application/json")

	body, err := ioutil.ReadAll(r.Body)
	if err != nil {
		w.WriteHeader(http.StatusBadRequest)
		log.Print(err)
		return
	}
	var t model.GetObjectsRequest
	err = json.Unmarshal(body, &t)
	if err != nil {
		w.WriteHeader(http.StatusBadRequest)
		log.Print(err)
		return
	}

	if t.Radius < 0 {
		w.WriteHeader(http.StatusBadRequest)
		log.Print(errors.New("negative radius"))
		return
	}

	buildings, highways, err := GetObjectData(h, t.Position.X, t.Position.Y, t.Radius)
	if err != nil {
		w.WriteHeader(http.StatusBadRequest)
		log.Print(err)
		return
	}
	resp := model.GetObjectsResponse{
		Buildings: buildings,
		Highways:  highways,
	}

	json.NewEncoder(w).Encode(resp)
}
