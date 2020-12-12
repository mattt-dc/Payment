using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Models.Response
{
    public class StatusResponse
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public decimal AmountAvailable { get; set; }
        public string Currency { get; set; }
    }
}
