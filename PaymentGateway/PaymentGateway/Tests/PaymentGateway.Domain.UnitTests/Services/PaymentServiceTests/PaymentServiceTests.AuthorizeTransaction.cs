using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentGateway.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Domain.UnitTests.Services.PaymentServiceTests
{
    public partial class PaymentServiceTests
    {
        public void AuthorizeTransaction_ServiceAuthorizes_ReturnsAuthorizationId()
        {
            AuthorizationInput input = new AuthorizationInput
            {
                Card = new CreditCard
                {

                },
                Amount = 5.00M,
                Currency = "GBP"
            };

            PaymentService service = new PaymentService();

            AuthorizationOutput output = service.AuthorizeTransaction(input);

            Assert.IsNotNull(output.AuthorizationId);
        }

        public void AuthorizeTransaction_ServiceDoesNotAuthorize_ReturnsErrorMessage()
        {
            AuthorizationInput input = new AuthorizationInput
            {
                Card = new CreditCard
                {
                    CardNumber = "invalid",
                    ExpiryYear = 2000
                },
                Amount = 5.00M,
                Currency = "GBP"
            };

            PaymentService service = new PaymentService();

            AuthorizationOutput output = service.AuthorizeTransaction(input);

            Assert.IsNotNull(output.TransactionOutput.Error);
            Assert.IsTrue(output.TransactionOutput.Error.Length > 0);
        }
    }
}
