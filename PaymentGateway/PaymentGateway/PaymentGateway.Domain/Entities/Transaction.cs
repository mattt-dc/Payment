using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Domain.Entities
{
    public class Transaction
    {
        public long Id { get; set; }
        public decimal AmountAvailable { get; set; }
        public string Currency { get; set; }
        public long ExternalId { get; set; }
    }
}
