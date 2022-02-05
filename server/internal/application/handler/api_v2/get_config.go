package api_v2

import (
	"encoding/json"
	model2 "github.com/comptech-winter-school/osm-data-renderer/server/internal/application/handler/api_v2/model"
	"net/http"
)

func GetConfig(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Content-Type", "application/json")
	conf := model2.Config{
		NullBuildingHeight: model2.NullBuildingHeight,
		ChunkLatSize:       model2.ChunkLatSize,
		ChunkLonSize:       model2.ChunkLonSize,
	}
	json.NewEncoder(w).Encode(conf)
}
