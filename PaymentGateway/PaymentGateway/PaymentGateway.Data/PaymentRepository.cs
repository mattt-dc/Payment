using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Data
{
    public class PaymentRepository : IPaymentRepository
    {
        protected readonly PaymentContext _dbContext;

        public PaymentRepository(PaymentContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Transaction> GetAuthorization()
        {
            throw new NotImplementedException();
        }

        public Task<Transaction> GetAuthorization(decimal amount, string currency)
        {
            throw new NotImplementedException();
        }

        public Task<bool> MarkTransactionAsVoid(long authorizationId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RecordPayment(long authorizationId, decimal amount)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RecordRefund(long authorizationId, decimal amount)
        {
            throw new NotImplementedException();
        }
    }
}
