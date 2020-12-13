package main

import (
	"encoding/json"
	"io/ioutil"
	"log"
	"net/http"
)

type authorizationRequest struct {
	CardNumber string `json:"cardNumber"`
	Amount     int    `json:"amount"`
}

func textBytesFromHTTPRequest(request *http.Request) []byte {
	b, err := ioutil.ReadAll(request.Body)
	if err != nil {
		panic(err)
	}
	textBytes := []byte(b)
	return textBytes
}

func authorizePayment(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Content-Type", "application/json")
	textBytes := textBytesFromHTTPRequest(r)
	authorizationRequest := authorizationRequest{}
	json.Unmarshal(textBytes, &authorizationRequest)
	w.Write([]byte(`{"message": "card authorized"}`))
}

func handleRequests() {
	http.HandleFunc("/authorize", authorizePayment)
	log.Fatal(http.ListenAndServe(":8080", nil))
}

func main() {
	handleRequests()
}
