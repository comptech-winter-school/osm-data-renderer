package osm

import "time"

type OSM struct {
	Name      string    `db:"name"`
	NodeId    int64     `db:"node_id"`
	Lat       float64   `db:"lat"`
	Lng       float64   `db:"lng"`
	CreatedAt time.Time `db:"created_at"`
	UpdatedAt time.Time `db:"updated_at"`
	//Дальше сами =)
}

func (o OSM) Values() ([]interface{}, error) {
	return []interface{}{
		o.NodeId,
		o.Name,
		o.Lat,
		o.Lng,
		o.CreatedAt,
		o.UpdatedAt,
	}, nil
}
