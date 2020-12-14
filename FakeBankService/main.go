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

type paymentRequest struct {
	ID     int64   `json:"id"`
	Amount float64 `json:"amount"`
}

type voidRequest struct {
	ID int64 `json:"id"`
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
	id := rand.Intn(1000000)
	//Forced ids are to allow for capture and refund failures for certain card numbers
	if authorizationRequest.CardNumber == "4000000000000259" {
		id = 614561
	}
	if authorizationRequest.CardNumber == "4000000000003238" {
		id = 614562
	}
	w.Write([]byte(fmt.Sprintf(`{"authorizedAmount": "%f", "id": "%d"}`, amount, id)))
}

func recordPayment(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Content-Type", "application/json")
	textBytes := textBytesFromHTTPRequest(r)
	paymentRequest := paymentRequest{}
	json.Unmarshal(textBytes, &paymentRequest)
	success := "failure"
	if paymentRequest.ID != 614561 {
		success = "success"
	}
	w.Write([]byte(fmt.Sprintf(success)))
}

func void(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Content-Type", "application/json")
	textBytes := textBytesFromHTTPRequest(r)
	voidRequest := voidRequest{}
	json.Unmarshal(textBytes, &voidRequest)
	w.Write([]byte(fmt.Sprintf("success")))
}

func handleRequests() {
	http.HandleFunc("/authorize", authorizePayment)
	http.HandleFunc("/recordPayment", recordPayment)
	http.HandleFunc("/void", void)
	log.Fatal(http.ListenAndServe(":8080", nil))
}

func main() {
	handleRequests()
}
