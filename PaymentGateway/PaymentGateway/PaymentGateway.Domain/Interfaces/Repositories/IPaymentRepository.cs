using PaymentGateway.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Domain.Interfaces.Repositories
{
    public interface IPaymentRepository
    {
        public Task<Transaction> GetAuthorization(decimal amount, string currency);

        public Task<bool> RecordPayment(long authorizationId, decimal amount);

        public Task<bool> MarkTransactionAsVoid(long authorizationId);

        public Task<bool> RecordRefund(long authorizationId, decimal amount);
    }
}
