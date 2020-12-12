using Microsoft.EntityFrameworkCore;
using PaymentGateway.Data.Entities;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<Transaction> GetAuthorization(decimal amount, string currency)
        {
            var newAuthorization = new Authorization
            {
                Amount = amount,
                Currency = currency,
                Void = false
            };
            _dbContext.Authorizations.Add(newAuthorization);
            await _dbContext.SaveChangesAsync();

            //Todo: Check the Id is set when the authorization is added
            Transaction transaction = new Transaction
            {
                AmountAvailable = newAuthorization.Amount,
                Currency = newAuthorization.Currency,
                Id = newAuthorization.Id
            };
            return transaction;
        }

        public async Task<bool> MarkTransactionAsVoid(long authorizationId)
        {
            //Marking as void in this way will mean that the time when this was changed will not be in db
            var authorization = await _dbContext.Authorizations.FirstAsync(x => x.Id.Equals(authorizationId));
            authorization.Void = true;
            _dbContext.Authorizations.Update(authorization);

            return true;
        }

        public async Task<bool> RecordPayment(long authorizationId, decimal amount)
        {
            Payment payment = new Payment
            {
                Amount = amount,
                AuthorizationId = authorizationId,
                Date = DateTime.UtcNow
            };
            var authorization = await _dbContext.Authorizations.Include(x => x.Payments).FirstAsync(x => x.Id.Equals(authorizationId));
            authorization.Payments.Add(payment);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RecordRefund(long authorizationId, decimal amount)
        {
            Refund refund = new Refund
            {
                Amount = amount,
                AuthorizationId = authorizationId,
                Date = DateTime.UtcNow
            };
            var authorization = await _dbContext.Authorizations.Include(x => x.Refunds).FirstAsync(x => x.Id.Equals(authorizationId));
            authorization.Refunds.Add(refund);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
