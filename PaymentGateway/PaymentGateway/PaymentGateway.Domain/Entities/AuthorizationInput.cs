using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Domain.Entities
{
    public class AuthorizationInput
    {
        public CreditCard Card { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}
