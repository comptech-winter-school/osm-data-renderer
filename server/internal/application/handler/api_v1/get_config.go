package api_v1

import (
	"encoding/json"
	"github.com/comptech-winter-school/osm-data-renderer/server/internal/application/handler/api_v1/model"
	"net/http"
)

func GetConfig(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Content-Type", "application/json")
	conf := model.Config{
		NullBuildingHeight: 1,
	}
	json.NewEncoder(w).Encode(conf)
}
