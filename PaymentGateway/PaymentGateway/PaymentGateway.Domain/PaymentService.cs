using PaymentGateway.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Domain
{
    public class PaymentService
    {
        public Task<AuthorizationOutput> AuthorizeTransaction(AuthorizationInput authorizationInput)
        {

        }

        public Task<TransactionOutput> CapturePayment(PaymentRequest paymentRequest)
        {

        }

        public Task<TransactionOutput> VoidTransaction(long authorizationId)
        {

        }

        public Task<TransactionOutput> RefundPayment(PaymentRequest paymentRequest)
        {

        }
    }
}
