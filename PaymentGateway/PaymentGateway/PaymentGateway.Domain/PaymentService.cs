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
        private readonly HttpClient _client;
        string bankApiAddress = "http://app:8080/";

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
            _client = new HttpClient();
        }

        public async Task<AuthorizationOutput> AuthorizeTransaction(AuthorizationInput authorizationInput)
        {
            //Todo: do basic check of credit card data
            //Authorize transaction with bank
            BankAuthorizationResponse bankAuthorization = await GetAuthorizationFromBank(authorizationInput);
            if (bankAuthorization.authorizedAmount == 0)
            {
                string error = "Failed to authorize";
                AuthorizationOutput failedAuthorizationOutput = GetFailedAuthorizationOuput(error);
                return failedAuthorizationOutput;
            }
            Transaction transaction = await _paymentRepository.GetAuthorization(authorizationInput.Amount,
                authorizationInput.Currency, bankAuthorization.id);

            AuthorizationOutput output = new AuthorizationOutput
            {
                AuthorizationId = transaction.Id,
                TransactionOutput = new TransactionOutput
                {
                    AmountAvailable = bankAuthorization.authorizedAmount,
                    Currency = authorizationInput.Currency,
                    Error = null,
                    Success = true
                }
            };
            return output;
        }

        private static AuthorizationOutput GetFailedAuthorizationOuput(string error)
        {
            return new AuthorizationOutput
            {
                TransactionOutput = new TransactionOutput
                {
                    Success = false,
                    Error = error
                }
            };
        }

        private async Task<BankAuthorizationResponse> GetAuthorizationFromBank(AuthorizationInput authorizationInput)
        {
            ByteArrayContent byteContent = GetAuthorizationRequestByteContent(authorizationInput);
            HttpResponseMessage response = await _client.PostAsync(bankApiAddress + "authorize", byteContent);
            var responseContent = await response.Content.ReadAsAsync<BankAuthorizationResponse>();
            return responseContent;
        }

        private static ByteArrayContent GetAuthorizationRequestByteContent(AuthorizationInput authorizationInput)
        {
            dynamic authorizationRequest = new
            {
                cardNumber = authorizationInput.Card.CardNumber,
                amount = authorizationInput.Amount
            };
            var json = JsonConvert.SerializeObject(authorizationRequest);
            var buffer = System.Text.Encoding.UTF8.GetBytes(json);
            var byteContent = new ByteArrayContent(buffer);
            return byteContent;
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
