package osm

import (
	"errors"
	"fmt"
	"github.com/comptech-winter-school/osm-data-renderer/server/internal/application/handler/api_v1/model"
	"strconv"
	"strings"
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

func StrToFloat(str string) (float32, error) {
	numb, err := strconv.ParseFloat(str, 64)
	if err != nil {
		return 0, err
	}
	return float32(numb), nil
}

func LineStringToLine(lineString string) ([]model.Point, error) {
	const (
		lineStringPrefixSize  = 11
		lineStringPostfixSize = 1
	)
	lineString = lineString[lineStringPrefixSize : len(lineString)-lineStringPostfixSize]
	if len(lineString) == 0 {
		return []model.Point{}, nil
	}
	stringPoints := strings.Split(lineString, ",")
	var points []model.Point
	for _, stringPoint := range stringPoints {
		stringCoords := strings.Split(stringPoint, " ")
		x, err := StrToFloat(stringCoords[0])
		if err != nil {
			return nil, err
		}
		y, err := StrToFloat(stringCoords[1])
		if err != nil {
			return nil, err
		}
		points = append(points, model.Point{
			X: x,
			Y: y,
		})
	}
	return points, nil
}
