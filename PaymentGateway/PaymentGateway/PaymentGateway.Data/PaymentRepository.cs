using PaymentGateway.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Data
{
    public class PaymentRepository : IPaymentRepository
    {
        protected readonly PaymentContext _dbContext;

        public PaymentRepository(PaymentContext dbContext)
        {
            _dbContext = dbContext;
        }

        public long GetAuthorization()
        {
            throw new NotImplementedException();
        }
    }
}
