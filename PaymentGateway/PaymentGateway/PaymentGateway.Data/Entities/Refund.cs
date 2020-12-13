using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Data.Entities
{
    public class Refund
    {
        public long Id { get; set; }
        public long AuthorizationId { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
    }
}
