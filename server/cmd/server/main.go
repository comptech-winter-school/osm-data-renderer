package main

import (
	"fmt"
	"net/http"

	"github.com/gofrs/uuid"
)

const PORT = ":8090"

func hello(w http.ResponseWriter, req *http.Request) {
	id, _ := uuid.NewGen().NewV4()
	fmt.Fprintf(w, id.String())
}

func main() {

	http.HandleFunc("/generate_uuid", hello)
	fmt.Printf("Server was started at %s port\n", PORT)
	http.ListenAndServe(PORT, nil)
}