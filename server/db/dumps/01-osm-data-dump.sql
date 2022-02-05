CREATE EXTENSION IF NOT EXISTS postgis;

CREATE TABLE IF NOT EXISTS public.osm_data (
    way_id bigint NOT NULL,
    name character varying(512) NOT NULL,
    polygon public.geometry NOT NULL,
    lat double precision NOT NULL,
    lon double precision NOT NULL,
    tags character varying NOT NULL,
    type character varying(64) NOT NULL,
    created_at timestamp without time zone DEFAULT timezone('utc'::text, now()),
    updated_at timestamp without time zone DEFAULT timezone('utc'::text, now())
);

CREATE UNIQUE INDEX way_id_idx ON public.osm_data USING btree (way_id);