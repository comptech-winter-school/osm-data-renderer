create table if not exists osm_data(
       node_id bigint not null,
       name varchar(512) not null,
       lat DOUBLE PRECISION,
       lng DOUBLE PRECISION,
       created_at timestamp without time zone default (now() at time zone 'utc'),
       updated_at timestamp without time zone default (now() at time zone 'utc')
);

CREATE UNIQUE INDEX IF NOT EXISTS node_id_idx ON osm_data (node_id);