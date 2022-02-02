package osm

import (
	"errors"
	"fmt"
)

type Line struct {
	Points []Point
}

type Point struct {
	X float64
	Y float64
}

func LineToLineString(line []Point) (string, error) {
	if len(line) == 0 {
		errors.New("empty point list")
	}
	var stringCoords = ""
	for _, point := range line {
		stringCoords += fmt.Sprintf(",%f %f", point.X, point.Y)
	}
	return fmt.Sprintf("LINESTRING(%s)", stringCoords[1:]), nil
}

func LineStringToLine(lineString string) ([]Point, error) {
	return nil, nil
}
