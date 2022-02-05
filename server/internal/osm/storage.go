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

const (
	metersPerLatDegree = 111319.0
	metersPerLonDegree = 111134.0
)

func (s *Storage) GetOsmDataByRadius(ctx context.Context, Lat, Lon, RadiusMeters float64) (*[]OSM, error) {
	var osmData []OSM

	q := sq.StatementBuilder.PlaceholderFormat(sq.Dollar).
		Select("way_id, name, ST_AsText(polygon) AS polygon, lat, lon, tags, type").
		From("osm_data")

	latR, lonR := RadiusMeters/metersPerLatDegree, RadiusMeters/metersPerLonDegree

	q = q.Where(sq.And{sq.GtOrEq{"lat": Lat - latR}, sq.LtOrEq{"lat": Lat + latR}})
	q = q.Where(sq.And{sq.GtOrEq{"lon": Lon - lonR}, sq.LtOrEq{"lon": Lon + lonR}})

	query, args, err := q.ToSql()
	if err != nil {
		return nil, err
	}

	err = s.db.SelectContext(ctx, &osmData, query, args...)
	if err != nil {
		return nil, err
	}
	return &osmData, nil
}

func (s *Storage) GetOsmDataByBox(ctx context.Context, latMin, lonMin, latMax, lonMax float32) (*[]OSM, error) {
	var osmData []OSM

	q := sq.StatementBuilder.PlaceholderFormat(sq.Dollar).
		Select("way_id, name, ST_AsText(polygon) AS polygon, lat, lon, tags, type").
		From("osm_data")

	q = q.Where(sq.And{sq.GtOrEq{"lat": latMin}, sq.LtOrEq{"lat": latMax}})
	q = q.Where(sq.And{sq.GtOrEq{"lon": lonMin}, sq.LtOrEq{"lon": lonMax}})

	query, args, err := q.ToSql()
	if err != nil {
		return nil, err
	}

	err = s.db.SelectContext(ctx, &osmData, query, args...)
	if err != nil {
		return nil, err
	}
	return &osmData, nil
}

func (s *Storage) UpsertOsmData(ctx context.Context, data OSM) error {
	q := sq.StatementBuilder.PlaceholderFormat(sq.Dollar).
		Insert("osm_data").Columns("way_id", "name", "polygon", "lat", "lon", "tags", "type", "created_at", "updated_at")
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
