package osm

import (
	"github.com/comptech-winter-school/osm-data-renderer/server/internal/application/handler/api_v1/model"
	"testing"
)

func TestLineStringToLine(t *testing.T) {

	got, gotErr := LineStringToLine("LINESTRING(1 2,3 4)")
	want, wantErr := []model.Point{{1, 2}, {3, 4}}, error(nil)

	if gotErr != wantErr {
		t.Error()
	}
	for i, point := range want {
		if (point.X != got[i].X) || (point.Y != got[i].Y) {
			t.Error()
		}
	}

	got, gotErr = LineStringToLine("LINESTRING(1.312 2.2342342234)")
	want, wantErr = []model.Point{{1.312, 2.2342342234}}, error(nil)

	if gotErr != wantErr {
		t.Error()
	}
	for i, point := range want {
		if (point.X != got[i].X) || (point.Y != got[i].Y) {
			t.Error()
		}
	}

	got, gotErr = LineStringToLine("LINESTRING()")
	want, wantErr = []model.Point{}, error(nil)
	if gotErr != wantErr {
		t.Error()
	}
	for i, point := range want {
		if (point.X != got[i].X) || (point.Y != got[i].Y) {
			t.Error()
		}
	}
}
