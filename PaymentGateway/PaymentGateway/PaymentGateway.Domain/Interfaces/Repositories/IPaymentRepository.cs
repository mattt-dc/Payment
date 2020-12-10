using PaymentGateway.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Domain.Interfaces.Repositories
{
    public interface IPaymentRepository
    {
        public long GetAuthorization();
    }
}
