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

        public async Task<Transaction> GetAuthorization(decimal amount, string currency, long externalId)
        {
            var newAuthorization = new Authorization
            {
                Amount = amount,
                Currency = currency,
                Void = false,
                ExternalId = externalId
            };
            _dbContext.Authorizations.Add(newAuthorization);
            await _dbContext.SaveChangesAsync();

            //Todo: Check the Id is set when the authorization is added
            Transaction transaction = new Transaction
            {
                AmountAvailable = newAuthorization.Amount,
                Currency = newAuthorization.Currency,
                Id = newAuthorization.Id,
                ExternalId = newAuthorization.ExternalId
            };
            return transaction;
        }

        public async Task<Transaction> GetTransaction(long authorizationId)
        {
            var authorization = await _dbContext.Authorizations.Include(x => x.Payments)
                .Include(x => x.Refunds)
                .FirstAsync(x => x.Id.Equals(authorizationId) && !x.Void);

            Transaction transaction = new Transaction
            {
                AmountAvailable = authorization.Amount,
                Currency = authorization.Currency,
                ExternalId = authorization.ExternalId,
                Id = authorization.Id
            };
            List<Domain.Entities.Payment> payments = GetPayments(authorization);
            transaction.Payments = payments;
            List<Domain.Entities.Refund> refunds = GetRefunds(authorization);
            transaction.Refunds = refunds;
            return transaction;
        }

        private static List<Domain.Entities.Refund> GetRefunds(Authorization authorization)
        {
            List<Domain.Entities.Refund> refunds = new List<Domain.Entities.Refund>();
            foreach (var refund in authorization.Refunds)
            {
                refunds.Add(new Domain.Entities.Refund
                {
                    Amount = refund.Amount,
                    Date = refund.Date
                });
            }

            return refunds;
        }

        private static List<Domain.Entities.Payment> GetPayments(Authorization authorization)
        {
            List<Domain.Entities.Payment> payments = new List<Domain.Entities.Payment>();
            foreach (var payment in authorization.Payments)
            {
                payments.Add(new Domain.Entities.Payment
                {
                    Amount = payment.Amount,
                    Date = payment.Date
                });
            }

            return payments;
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
            Entities.Payment payment = new Entities.Payment
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
            Entities.Refund refund = new Entities.Refund
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
