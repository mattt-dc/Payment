using PaymentGateway.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Domain
{
    public class PaymentService
    {
        public AuthorizationOutput AuthorizeTransaction(AuthorizationInput authorizationInput)
        {

        }

        public TransactionOutput CapturePayment(PaymentRequest paymentRequest)
        {

        }

        public TransactionOutput VoidTransaction(long authorizationId)
        {

        }

        public TransactionOutput RefundPayment(PaymentRequest paymentRequest)
        {

        }
    }
}
