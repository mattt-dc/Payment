﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Domain.UnitTests.Services.PaymentServiceTests
{
    public partial class PaymentServiceTests
    {
        public void AuthorizeTransaction_ServiceAuthorizes_ReturnsAuthorizationId()
        {
            Mock<IPaymentRepository> mockRepository = GetMockRepository();
            AuthorizationInput input = new AuthorizationInput
            {
                Card = new CreditCard
                {

                },
                Amount = 5.00M,
                Currency = "GBP"
            };

            PaymentService service = GetService(mockRepository);

            AuthorizationOutput output = service.AuthorizeTransaction(input).Result;

            Assert.IsNotNull(output.AuthorizationId);
        }

        public void AuthorizeTransaction_ServiceDoesNotAuthorize_ReturnsErrorMessage()
        {
            Mock<IPaymentRepository> mockRepository = GetMockRepository();
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

            PaymentService service = GetService(mockRepository);

            AuthorizationOutput output = service.AuthorizeTransaction(input).Result;

            Assert.IsNotNull(output.TransactionOutput.Error);
            Assert.IsTrue(output.TransactionOutput.Error.Length > 0);
        }
    }
}
