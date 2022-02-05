package model

type Point struct {
	X float32 `json:"x"`
	Y float32 `json:"y"`
}

type Building struct {
	Levels  uint    `json:"levels,omitempty"`
	Polygon []Point `json:"polygon"`
}

type Highways struct {
	Polygon []Point `json:"polygon"`
}

type GetObjectsResponse struct {
	Buildings []Building `json:"buildings"`
	Highways  []Highways `json:"highways"`
}

type GetObjectsRequest struct {
	Position Point   `json:"position"`
	Radius   float32 `json:"radius"`
}
