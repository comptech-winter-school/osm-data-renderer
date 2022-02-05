package generateuuid

import (
	"context"
	"fmt"
	"net/http"

	"github.com/comptech-winter-school/osm-data-renderer/server/internal/osm"
	"github.com/gofrs/uuid"
)

type storage interface {
	GetOsmDataByRadius(ctx context.Context, Lat, Lng, RadiusMeters float64) (*[]osm.OSM, error)
}

type Handler struct {
	storage storage
}

func NewHandler(storage storage) *Handler {
	return &Handler{storage: storage}
}

func (h *Handler) Handle(w http.ResponseWriter, req *http.Request) {
	id, _ := uuid.NewGen().NewV4()
	data, err := h.storage.GetOsmDataByRadius(req.Context(), 232.3, 2323.34, 34)
	if err != nil {
		//в жизни так делать нельзя(отдавать пользователю системные ошибки)
		fmt.Fprintf(w, fmt.Sprintf("error load data %w", err))
		return
	}

	fmt.Fprintf(w, fmt.Sprintf("%d", len(*data)))
	fmt.Fprintf(w, id.String())
}
