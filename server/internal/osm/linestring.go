package osm

import (
	"errors"
	"fmt"
	"github.com/comptech-winter-school/osm-data-renderer/server/internal/application/handler/api_v1/model"
	"strconv"
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

// Нужно переписать,  кричитно важная по скорости доступа функция
func LineStringToLine(lineString string) ([]model.Point, error) {
	lineString = lineString[11:]
	var points []model.Point
	slat, slon := "", ""
	isLat := true
	for _, ch := range lineString {
		if ch != ' ' && ch != ',' && ch != ')' {
			if isLat {
				slat += string(ch)
			} else {
				slon += string(ch)
			}
		} else if ch == ' ' {
			isLat = false
		} else if ch == ',' || ch == ')' {
			lat, err := StrToFloat(slat)
			if err != nil {
				return nil, err
			}
			lon, err := StrToFloat(slon)
			if err != nil {
				return nil, err
			}
			points = append(points, model.Point{lat, lon})
			slat, slon = "", ""
			isLat = true
		}
	}
	return points, nil
}
