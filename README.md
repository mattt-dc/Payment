# Payment Gateway
## Description
This is a web api built with .NET for processing payments. 

The payments are sent to a fake bank service which has also been included in the repository.

The validity of the transaction is checked using the payment gateway's database before being sent to the bank service.

The fake bank service is a basic web app written in Go as I wanted to try out the language.
## Get started
To run the application first build it with 'docker-compose build' and then run it with 'docker-compose up'.
The Payment.postman_collection.json file includes tests for the endpoints.
## Prerequisites
* Docker (https://docs.docker.com/get-docker/)
* Docker Compose (included in the docker desktop for windows and mac installs)
## Application structure
The business logic can be found in the payment service in the PaymentGateway.Domain project. This is called from the controller in the Api project.

The database repository and context are in the Data project.
## Application database
The mssql database is hosted using docker and is started by the docker compose command. A database migration is run when the application starts.
## Authentication
I did not tackle authentication is this project due to time constraints. This could have been done using OAuth 2.
## Testing
Unit tests have been included using MSTEST and Moq.

More unit tests should be added to cover a wider range of inputs and different api responses.

Functional tests could have been added using the test web host built in to ASP.NET CORE. 
As they would require multiple containers to be running they would have to be run using docker compose or as part of a CI/CD pipeline.
## Other areas to improve
* Logging has not been added, this could have been done with the .NET Core logging API.
* Versioning is not currently supported in the project.
* The errors in the API responses are not very detailed.
