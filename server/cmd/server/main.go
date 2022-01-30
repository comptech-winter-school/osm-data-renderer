package main

import (
	"context"
	"fmt"
	"log"
	"net/http"
	"os"

	"github.com/gofrs/uuid"
	"github.com/jackc/pgx/v4"
	"github.com/joho/godotenv"
)

const PORT = ":8090"

func hello(w http.ResponseWriter, req *http.Request) {
	id, _ := uuid.NewGen().NewV4()
	fmt.Fprintf(w, id.String())
}

func main() {
	err := godotenv.Load()
	if err != nil {
		log.Fatal("Error loading .env file")
	}

	conn, err := pgx.Connect(context.Background(), os.Getenv("DATABASE_URL"))
	if err != nil {
		log.Fatal("Unable to connect to database")
	}
	defer conn.Close(context.Background())

	http.HandleFunc("/generate_uuid", hello)
	fmt.Printf("Server was started at %s port\n", PORT)
	http.ListenAndServe(PORT, nil)
}
