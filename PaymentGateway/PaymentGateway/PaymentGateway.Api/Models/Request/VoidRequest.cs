using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Models.Request
{
    public class VoidRequest
    {
        public long AuthorizationId { get; set; }
    }
}
