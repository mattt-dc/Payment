using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Models.Request
{
    public class RefundRequest
    {
        public long AuthorizationId { get; set; }
        public decimal Amount { get; set; }
    }
}
