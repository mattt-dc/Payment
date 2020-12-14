using Newtonsoft.Json;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
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
            ByteArrayContent byteContent = GetByteContentFromDynamicObject(authorizationRequest);
            return byteContent;
        }

        private static ByteArrayContent GetByteContentFromDynamicObject(dynamic dynamicObject)
        {
            var json = JsonConvert.SerializeObject(dynamicObject);
            var buffer = System.Text.Encoding.UTF8.GetBytes(json);
            var byteContent = new ByteArrayContent(buffer);
            return byteContent;
        }

        public async Task<TransactionOutput> CapturePayment(PaymentRequest paymentRequest)
        {
            //Check valid
            Transaction transaction = await _paymentRepository.GetTransaction(paymentRequest.AuthorizationId);
            if (transaction == null || transaction.Refunds.Count > 0)
            {
                string error = "Invalid payment";
                TransactionOutput transactionOutput = GetFailedTransactionOutput(error);
                return transactionOutput;
            }
            decimal totalAvailable = GetTotalAmountAvailable(transaction);
            if (totalAvailable < paymentRequest.Amount)
            {
                TransactionOutput transactionOutput = GetFailedTransactionOutput("Invalid amount");
                return transactionOutput;
            }
            //Send to bank
            string responseMessage = await SendPaymentToBank(paymentRequest, transaction);
            if (responseMessage != "success")
            {
                TransactionOutput transactionOutput = GetFailedTransactionOutput("Payment declined");
                return transactionOutput;
            }

            bool recorded = await _paymentRepository.RecordPayment(paymentRequest.AuthorizationId, paymentRequest.Amount);
            //Todo: log if payment fails to record

            TransactionOutput output = new TransactionOutput
            {
                AmountAvailable = totalAvailable - paymentRequest.Amount,
                Currency = transaction.Currency,
                Error = null,
                Success = true
            };
            return output;
        }

        private async Task<string> SendPaymentToBank(PaymentRequest paymentRequest, Transaction transaction)
        {
            dynamic recordPaymentRequest = new
            {
                ID = transaction.ExternalId,
                Amount = paymentRequest.Amount
            };
            ByteArrayContent requestContent = GetByteContentFromDynamicObject(recordPaymentRequest);
            HttpResponseMessage response = await _client.PostAsync(bankApiAddress + "recordPayment", requestContent);
            string responseMessage = await response.Content.ReadAsStringAsync();
            return responseMessage;
        }

        private static decimal GetTotalAmountAvailable(Transaction transaction)
        {
            decimal paymentAmountsTotal = transaction.Payments.Sum(x => x.Amount);
            decimal refundAmountsTotal = transaction.Refunds.Sum(x => x.Amount);
            decimal totalAvailable = transaction.AmountAvailable + refundAmountsTotal - paymentAmountsTotal;
            return totalAvailable;
        }

        private static TransactionOutput GetFailedTransactionOutput(string error)
        {
            return new TransactionOutput
            {
                Success = false,
                Error = error
            };
        }

        public async Task<TransactionOutput> VoidTransaction(long authorizationId)
        {
            Transaction transaction = await _paymentRepository.GetTransaction(authorizationId);
            if (transaction == null)
            {
                TransactionOutput transactionOutput = GetFailedTransactionOutput("Invalid authorization");
                return transactionOutput;
            }
            //Send to bank
            string responseMessage = await SendVoidRequestToBank(transaction);
            if (responseMessage != "success")
            {
                TransactionOutput transactionOutput = GetFailedTransactionOutput("Void failed");
                return transactionOutput;
            }

            bool recorded = await _paymentRepository.MarkTransactionAsVoid(authorizationId);
            //Todo: log if fails to void

            TransactionOutput output = new TransactionOutput
            {
                AmountAvailable = 0,
                Currency = transaction.Currency,
                Error = null,
                Success = true
            };
            return output;
        }

        private async Task<string> SendVoidRequestToBank(Transaction transaction)
        {
            dynamic voidRequest = new
            {
                ID = transaction.ExternalId
            };
            ByteArrayContent requestContent = GetByteContentFromDynamicObject(voidRequest);
            HttpResponseMessage response = await _client.PostAsync(bankApiAddress + "void", requestContent);
            string responseMessage = await response.Content.ReadAsStringAsync();
            return responseMessage;
        }

        public async Task<TransactionOutput> RefundPayment(PaymentRequest paymentRequest)
        {
            //Check valid
            Transaction transaction = await _paymentRepository.GetTransaction(paymentRequest.AuthorizationId);
            if (transaction == null)
            {
                string error = "Invalid refund";
                TransactionOutput transactionOutput = GetFailedTransactionOutput(error);
                return transactionOutput;
            }
            decimal paymentAmountsTotal = transaction.Payments.Sum(x => x.Amount);
            decimal totalAvailable = GetTotalAmountAvailable(transaction);
            if (paymentAmountsTotal < paymentRequest.Amount || totalAvailable < paymentRequest.Amount)
            {
                TransactionOutput transactionOutput = GetFailedTransactionOutput("Invalid amount");
                return transactionOutput;
            }
            //Send to bank
            string responseMessage = await SendRefundToBank(paymentRequest, transaction);
            if (responseMessage != "success")
            {
                TransactionOutput transactionOutput = GetFailedTransactionOutput("Refund declined");
                return transactionOutput;
            }

            bool recorded = await _paymentRepository.RecordRefund(paymentRequest.AuthorizationId, paymentRequest.Amount);
            //Todo: log if fails to record

            TransactionOutput output = new TransactionOutput
            {
                AmountAvailable = totalAvailable + paymentRequest.Amount,
                Currency = transaction.Currency,
                Error = null,
                Success = true
            };
            return output;
        }

        private async Task<string> SendRefundToBank(PaymentRequest paymentRequest, Transaction transaction)
        {
            dynamic recordRefundRequest = new
            {
                ID = transaction.ExternalId,
                Amount = paymentRequest.Amount
            };
            ByteArrayContent requestContent = GetByteContentFromDynamicObject(recordRefundRequest);
            HttpResponseMessage response = await _client.PostAsync(bankApiAddress + "recordRefund", requestContent);
            string responseMessage = await response.Content.ReadAsStringAsync();
            return responseMessage;
        }
    }
}
