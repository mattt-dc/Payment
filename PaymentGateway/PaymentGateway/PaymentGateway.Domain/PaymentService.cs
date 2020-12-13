using Newtonsoft.Json;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Domain
{
    public class PaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<AuthorizationOutput> AuthorizeTransaction(AuthorizationInput authorizationInput)
        {
            //Do basic check of credit card data
            //Authorize transaction with bank
            dynamic authorizationRequest = new
            {
                cardNumber = authorizationInput.Card.CardNumber,
                amount = authorizationInput.Amount
            };
            var json = JsonConvert.SerializeObject(authorizationRequest);
            var buffer = System.Text.Encoding.UTF8.GetBytes(json);
            var byteContent = new ByteArrayContent(buffer);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync("http://app:8080/authorize", byteContent);
            dynamic responseContent = await response.Content.ReadAsAsync<object>();
            if (!response.IsSuccessStatusCode || responseContent.authorizedAmount.ToString() == "0.000000")
            {
                return new AuthorizationOutput
                {
                    TransactionOutput = new TransactionOutput
                    {
                        Success = false,
                        Error = "Failed to authorize"
                    }
                };
            }
            Transaction transaction = await _paymentRepository.GetAuthorization(authorizationInput.Amount, 
                authorizationInput.Currency);

            AuthorizationOutput output = new AuthorizationOutput
            {
                AuthorizationId = transaction.Id,
                TransactionOutput = new TransactionOutput
                {
                    AmountAvailable = responseContent.authorizedAmount,
                    Currency = authorizationInput.Currency,
                    Error = null,
                    Success = true
                }
            };
            return output;
        }

        public async Task<TransactionOutput> CapturePayment(PaymentRequest paymentRequest)
        {
            //Send to bank
            //If fails to send return here

            bool recorded = await _paymentRepository.RecordPayment(paymentRequest.AuthorizationId, paymentRequest.Amount);

            //Get amount available and currency from db

            TransactionOutput output = new TransactionOutput
            {
                
            };
            return output;
        }

        public async Task<TransactionOutput> VoidTransaction(long authorizationId)
        {
            //Send to bank
            //If fails to send return here

            bool recorded = await _paymentRepository.MarkTransactionAsVoid(authorizationId);

            //Get amount available and currency from db
            TransactionOutput output = new TransactionOutput
            {

            };
            return output;
        }

        public async Task<TransactionOutput> RefundPayment(PaymentRequest paymentRequest)
        {
            //Send to bank
            //If fails to send return here

            bool recorded = await _paymentRepository.RecordRefund(paymentRequest.AuthorizationId, paymentRequest.Amount);

            //Get amount available and currency from db
            TransactionOutput output = new TransactionOutput
            {

            };
            return output;
        }
    }
}
