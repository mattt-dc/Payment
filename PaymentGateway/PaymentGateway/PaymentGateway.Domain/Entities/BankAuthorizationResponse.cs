using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Domain.Entities
{
    public class BankAuthorizationResponse
    {
        public long id { get; set; }
        public decimal authorizedAmount { get; set; }
    }
}
