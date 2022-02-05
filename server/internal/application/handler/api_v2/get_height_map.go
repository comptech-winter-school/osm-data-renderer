package api_v2

import (
	"encoding/json"
	"github.com/comptech-winter-school/osm-data-renderer/server/internal/application/handler/api_v1/model"
	"github.com/comptech-winter-school/osm-data-renderer/server/pkg/utils/FileToBase64Encoding"
	"log"
	"net/http"
	"os"
)

func GetFileNameByLatLonRadius(lat, lon, radius float32) string {
	return os.Getenv("HEIGHTMAPS_PATH") + os.Getenv("LATEST_HEIGHTMAP_NAME")
}

func GetHeightMapResponse(lat, lon, radius float32) model.HeightMapResponse {
	heightMapPath := GetFileNameByLatLonRadius(lat, lon, radius)
	border := model.Border{
		XMin: 37.6100,
		YMin: 55.740,
		XMax: 37.6300,
		YMax: 55.750,
	}

	encodedHeightMap, mapSize, err := FileToBase64Encoding.GetEncodedSliceOfFile(heightMapPath, border)
	if err != nil {
		log.Println("ERROR: Some error has occurred while encoding file: " + err.Error())
	}
	return model.HeightMapResponse{Heightmap: encodedHeightMap, Size: mapSize, Border: border}
}

func GetHeightMap(w http.ResponseWriter, r *http.Request) {
	resp := GetHeightMapResponse(0, 0, 1)
	json.NewEncoder(w).Encode(resp)
}
