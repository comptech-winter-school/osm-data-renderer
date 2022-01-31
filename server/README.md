Как запустить postgres:
docker-compose up

Как запустить приложение:
Скопировать .env-example и переименовать его в .env
Выполнить команду go run main.go в директории ./cmd

При необходимости можно взять .osm.pfb файл
И скопировать его в папку ./assets/protobuf
Например по ссылке (~16MB): 
http://download.geofabrik.de/russia/kaliningrad-latest.osm.pbf

