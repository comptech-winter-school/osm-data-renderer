package model

import "github.com/comptech-winter-school/osm-data-renderer/server/internal/application/handler/api_v1/model"

type FloatPoint struct {
	X float32 `json:"x"`
	Y float32 `json:"y"`
}

type IntPoint struct {
	X int `json:"x"`
	Y int `json:"y"`
}

type Data struct {
	Buildings []model.Building `json:"buildings,omitempty"`
	Highways  []model.Highways `json:"highways,omitempty"`
}

type Chunk struct {
	Position  IntPoint `json:"position"`
	IsUpdated bool     `json:"is_updated"`
	Data      Data     `json:"data,omitempty"`
}

type CacheInfo struct {
	Position IntPoint `json:"position"`
	Hash     string   `json:"hash"`
}

type ChunkBox struct {
	Min FloatPoint `json:"min"`
	Max FloatPoint `json:"max"`
}

type GetObjectsResponse struct {
	Chunks []Chunk `json:"chunks"`
}

type GetObjectsRequest struct {
	Position             IntPoint    `json:"position"`
	ChunkLoadingDistance uint        `json:"chunk_loading_distance"`
	ChunkCache           []CacheInfo `json:"chunk_cache"`
}
