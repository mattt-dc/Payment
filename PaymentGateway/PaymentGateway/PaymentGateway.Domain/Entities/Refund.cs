using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Domain.Entities
{
    public class Refund
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
