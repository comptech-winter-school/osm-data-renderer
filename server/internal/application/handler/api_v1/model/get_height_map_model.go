package model

type Border struct {
	XMin float64 `json:"x_min"`
	YMin float64 `json:"y_min"`
	XMax float64 `json:"x_max"`
	YMax float64 `json:"y_max"`
}

type HeightMapSize struct {
	Height int `json:"height"`
	Width  int `json:"width"`
}

type HeightMapResponse struct {
	Heightmap []byte        `json:"highmap"`
	Size      HeightMapSize `json:"size"`
	Border    Border        `json:"border"`
}
