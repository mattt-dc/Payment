using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Data.Entities
{
    public class Authorization
    {
        public long Id { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public bool Void { get; set; }
        public long ExternalId { get; set; }
        public List<Payment> Payments { get; set; }
        public List<Refund> Refunds { get; set; }
    }
}
