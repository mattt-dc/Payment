using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Domain
{
    public class PaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public async Task<AuthorizationOutput> AuthorizeTransaction(AuthorizationInput authorizationInput)
        {
            //Do basic check of credit card data
            //Authorize transaction with bank
            //If authorized add to db
            //If not authorized return here
            //Todo: Use bank response for amount and currency when sending to db
            Transaction transaction = await _paymentRepository.GetAuthorization(authorizationInput.Amount, 
                authorizationInput.Currency);

            AuthorizationOutput output = new AuthorizationOutput
            {
                AuthorizationId = transaction.Id,

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
