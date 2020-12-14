package main

import (
	"encoding/json"
	"fmt"
	"io/ioutil"
	"log"
	"math/rand"
	"net/http"
)

type authorizationRequest struct {
	CardNumber string  `json:"cardNumber"`
	Amount     float64 `json:"amount"`
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
	amount := 0.0
	if authorizationRequest.CardNumber != "4000000000000119" {
		amount = authorizationRequest.Amount
	}
	w.Write([]byte(fmt.Sprintf(`{"authorizedAmount": "%f", "id": "%d"}`, amount, rand.Intn(1000000))))
}

func recordPayment(w http.ResponseWriter, r *http.Request) {

}

func handleRequests() {
	http.HandleFunc("/authorize", authorizePayment)
	http.HandleFunc("/recordPayment", recordPayment)
	log.Fatal(http.ListenAndServe(":8080", nil))
}

func main() {
	handleRequests()
}
