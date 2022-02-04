package main

import (
	"fmt"
	"github.com/comptech-winter-school/osm-data-renderer/server/internal/application/handler/api_v1"
	"github.com/comptech-winter-school/osm-data-renderer/server/internal/application/handler/general"
	"log"
	"net/http"
	"os"

	"github.com/comptech-winter-school/osm-data-renderer/server/internal/infrastructure/db"
	"github.com/gorilla/mux"
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

	//osmStorage := osm.NewStorage(conn)

	r := mux.NewRouter()
	r.HandleFunc("/ping", general.Ping).Methods("GET")
	r.HandleFunc("/apiv1/config", api_v1.GetConfig).Methods("GET")
	r.HandleFunc("/apiv1/objects", api_v1.GetObjects).Methods("POST")
	r.HandleFunc("/apiv1/heightmap", api_v1.GetHeightMap).Methods("POST")
	log.Fatal(http.ListenAndServe(fmt.Sprintf(":%s", applicationPort), r))
}
