package osm

import (
	"github.com/comptech-winter-school/osm-data-renderer/server/internal/application/handler/api_v1/model"
	"testing"
)

func TestLineStringToLine(t *testing.T) {
	type FuncTest struct {
		Arg         string
		Expected    []model.Point
		ExpectedErr error
	}

	funcTest := []FuncTest{
		{"LINESTRING(1 2,3 4)", []model.Point{{1, 2}, {3, 4}}, nil},
		{"LINESTRING(1.312 2.2342342234)", []model.Point{{1.312, 2.2342342234}}, nil},
		{"LINESTRING()", []model.Point{}, nil},
	}

	for _, test := range funcTest {
		got, gotErr := LineStringToLine(test.Arg)
		if gotErr != test.ExpectedErr {
			t.Error()
		}

		if gotErr != test.ExpectedErr {
			t.Error()
		}
		for i, point := range test.Expected {
			if (point.X != got[i].X) || (point.Y != got[i].Y) {
				t.Error()
			}
		}
	}
}

func TestLineToLineString(t *testing.T) {
	type FuncTest struct {
		Arg         []Point
		Expected    string
		ExpectedErr error
	}

	funcTest := []FuncTest{
		{[]Point{{1, 2}, {3, 4}}, "LINESTRING(1.000000 2.000000,3.000000 4.000000)", nil},
		{[]Point{{1.312, 2.2342342234}}, "LINESTRING(1.312000 2.234234)", nil},
		{[]Point{}, "LINESTRING()", nil},
	}

	for _, test := range funcTest {
		got, gotErr := LineToLineString(test.Arg)
		if gotErr != test.ExpectedErr {
			t.Error()
		}

		if got != test.Expected {
			t.Error()
		}
	}
}
