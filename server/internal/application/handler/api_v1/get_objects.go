package api_v1

import (
	"encoding/json"
	"errors"
	"github.com/comptech-winter-school/osm-data-renderer/server/internal/application/handler/api_v1/model"
	"io/ioutil"
	"log"
	"math"
	"net/http"
)

func GetObjectData(lat, lon, radius float32) ([]model.Building, [][]model.Point, error) {
	p1, p2, p3, p4 := model.Point{0, 0}, model.Point{1, 0}, model.Point{0, 1}, model.Point{0, 0}
	b1 := model.Building{Levels: 4, Polygon: []model.Point{p1, p2, p3, p4}}
	b2 := model.Building{Polygon: []model.Point{p4, p2, p3, {1, 1}, p4}}

	hw1 := []model.Point{p1, p2, p3}
	hw2 := []model.Point{p2, p3, p1}

	return []model.Building{b1, b2}, [][]model.Point{hw1, hw2}, nil
}

func GetObjects(w http.ResponseWriter, r *http.Request) {
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

	if (math.Abs(float64(t.Position.Y))+float64(t.Radius) > 85) ||
		(math.Abs(float64(t.Position.X))+float64(t.Radius) > 180) {
		w.WriteHeader(http.StatusBadRequest)
		log.Print(err)
		return
	}

	buildings, highways, err := GetObjectData(t.Position.X, t.Position.Y, t.Radius)
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
