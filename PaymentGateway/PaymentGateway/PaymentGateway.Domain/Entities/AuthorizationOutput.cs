using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Domain.Entities
{
    public class AuthorizationOutput
    {
        public long AuthorizationId { get; set; }
        public TransactionOutput TransactionOutput { get; set; }
    }
}
