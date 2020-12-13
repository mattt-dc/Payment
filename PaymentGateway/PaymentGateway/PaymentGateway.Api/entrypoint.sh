#!/bin/bash

set -e
run_cmd="dotnet PaymentGateway.Api.dll"

until sleep 30; do
>&2 echo "SQL Server is starting up"
sleep 1
done

>&2 echo "SQL Server is up - executing command"
exec $run_cmd