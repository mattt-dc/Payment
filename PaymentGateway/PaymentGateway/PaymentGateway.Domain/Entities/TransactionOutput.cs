using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Domain.Entities
{
    public class TransactionOutput
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public decimal AmountAvailable { get; set; }
        //Todo: Check what type should be used for currency
        public string Currency { get; set; }
    }
}
