package model

type Config struct {
	NullBuildingHeight uint    `json:"null_building_height"`
	ChunkLatSize       float32 `json:"chunk_lat_size"`
	ChunkLonSize       float32 `json:"chunk_lon_size"`
}

const (
	NullBuildingHeight = 1
	ChunkLatSize       = 0.005
	ChunkLonSize       = 0.005
)
