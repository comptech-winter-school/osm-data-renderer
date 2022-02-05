#!/bin/bash

CGO_ENABLED=0 GOOS=linux go build -a -installsuffix cgo -o osm-renderer-server ./cmd/server/main.go
ssh osm-server@159.223.28.18 'sudo systemctl stop server'
scp ./osm-renderer-server osm-server@159.223.28.18:/home/osm-server/server/server
scp ./.env osm-server@159.223.28.18:/home/osm-server/server/server
ssh osm-server@159.223.28.18 'sudo systemctl start server'

