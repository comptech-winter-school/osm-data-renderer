package osm

import (
	"context"
	"fmt"

	sq "github.com/Masterminds/squirrel"
	"github.com/jmoiron/sqlx"
)

type Storage struct {
	db *sqlx.DB
}

func NewStorage(db *sqlx.DB) *Storage {
	return &Storage{db: db}
}

func (s *Storage) GetOsmData(ctx context.Context, Lat, Lng, RadiusMeters float64) (*[]OSM, error) {
	var osmData []OSM

	q := sq.StatementBuilder.PlaceholderFormat(sq.Dollar).
		Select("*").
		From("osm_data")

	q = q.Where("(earth_box(ll_to_earth(?, ?), ?) @> ll_to_earth(lat, lng))", Lat, Lng, RadiusMeters)
	q = q.Where("(earth_distance(ll_to_earth(?, ?), ll_to_earth(lat, lng)) < ?)", Lat, Lng, RadiusMeters)

	query, args, err := q.ToSql()
	if err != nil {
		return nil, err
	}

	err = s.db.SelectContext(ctx, osmData, query, args...)
	if err != nil {
		return nil, err
	}
	return &osmData, nil
}

func (s *Storage) UpsertOsmData(ctx context.Context, data OSM) error {
	q := sq.StatementBuilder.PlaceholderFormat(sq.Dollar).
		Insert("osm_data").Columns("way_id", "name", "polygon", "created_at", "updated_at")
	values, err := data.Values()
	if err != nil {
		return fmt.Errorf("getting values for upsert: %w", err)
	}
	q = q.Values(values...)
	q = q.Suffix("ON CONFLICT (way_id) DO " + //
		"UPDATE SET updated_at=(now() at time zone 'utc')")

	query, args, err := q.ToSql()
	if err != nil {
		return fmt.Errorf("compiling sql request: %w", err)
	}

	_, err = s.db.ExecContext(ctx, query, args...)
	if err != nil {
		return fmt.Errorf("executing db request: %w", err)
	}

	return nil
}
