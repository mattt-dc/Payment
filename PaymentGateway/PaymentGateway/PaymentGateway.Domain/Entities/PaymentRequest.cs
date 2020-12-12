using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Domain.Entities
{
    public class PaymentRequest
    {
        public long AuthorizationId { get; set; }
        public decimal Amount { get; set; }
    }
}
