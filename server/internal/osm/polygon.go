package osm

import (
	"database/sql/driver"
	"encoding/json"
	"errors"
)

type Polygon struct {
	Vertex []Point `json:"vertex,omitempty"`
}

type Point struct {
	Lat float64 `json:"lat,omitempty"`
	Lon float64 `json:"lon,omitempty"`
}

func (a Polygon) Value() (driver.Value, error) {
	return json.Marshal(a)
}

func (a *Polygon) Scan(value interface{}) error {
	b, ok := value.([]byte)
	if !ok {
		return errors.New("type assertion to []byte failed")
	}

	return json.Unmarshal(b, &a)
}
