package main

import (
	"fmt"
	"github.com/comptech-winter-school/osm-data-renderer/server/cmd/cron"
	"log"
	"net/http"
	"os"

	"github.com/comptech-winter-school/osm-data-renderer/server/internal/application/handler/generateuuid"
	"github.com/comptech-winter-school/osm-data-renderer/server/internal/infrastructure/db"
	"github.com/comptech-winter-school/osm-data-renderer/server/internal/osm"

	"github.com/joho/godotenv"
)

func main() {
	err := godotenv.Load()
	if err != nil {
		log.Fatal("Error loading .env file")
	}

	applicationPort := os.Getenv("APP_PORT")

	conn := db.OpenDB()
	defer conn.Close()

	osmStorage := osm.NewStorage(conn)

	getuuidHandler := generateuuid.NewHandler(osmStorage)

	cron.WriteInDB(osmStorage, "kaliningrad-latest.osm.pbf")
	return
	http.HandleFunc("/generate_uuid", getuuidHandler.Handle)
	fmt.Printf("Server was started at :%s port\n", applicationPort)
	http.ListenAndServe(fmt.Sprintf(":%s", applicationPort), nil)
}
