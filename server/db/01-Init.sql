create table if not exists osm_data(
       way_id bigint not null,
       name varchar(512) not null,
       polygon varchar not null,
       lat double precision not null,
       lon double precision not null,
       tags varchar not null,
       type varchar not null,
       created_at timestamp without time zone default (now() at time zone 'utc'),
       updated_at timestamp without time zone default (now() at time zone 'utc')
);

CREATE UNIQUE INDEX IF NOT EXISTS way_id_idx ON osm_data (way_id);