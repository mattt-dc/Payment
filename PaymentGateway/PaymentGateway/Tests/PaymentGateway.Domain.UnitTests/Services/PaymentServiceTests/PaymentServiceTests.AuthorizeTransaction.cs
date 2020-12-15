using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace PaymentGateway.Domain.UnitTests.Services.PaymentServiceTests
{
    public partial class PaymentServiceTests
    {
        [TestMethod]
        public void AuthorizeTransaction_ServiceAuthorizes_ReturnsAuthorizationId()
        {
            Mock<IPaymentRepository> mockRepository = GetMockRepository();
            Mock<HttpMessageHandler> mockHttpHandler = GetHttpHandlerMock("{success: true}");
            AuthorizationInput input = new AuthorizationInput
            {
                Card = new CreditCard
                {
                    CardNumber = "124142112411241"
                },
                Amount = 5.00M,
                Currency = "GBP"
            };

            PaymentService service = GetService(mockRepository, mockHttpHandler);

            AuthorizationOutput output = service.AuthorizeTransaction(input).Result;

            Assert.IsNotNull(output.AuthorizationId);
        }

        [TestMethod]
        public void AuthorizeTransaction_InvalidCardNumber_ReturnsErrorMessage()
        {
            Mock<IPaymentRepository> mockRepository = GetMockRepository();
            Mock<HttpMessageHandler> mockHttpHandler = GetHttpHandlerMock("{success: true}");
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

            PaymentService service = GetService(mockRepository, mockHttpHandler);

            AuthorizationOutput output = service.AuthorizeTransaction(input).Result;

            Assert.IsNotNull(output.TransactionOutput.Error);
            Assert.IsTrue(output.TransactionOutput.Error.Length > 0);
        }

        [TestMethod]
        public void AuthorizeTransaction_ServiceReturnsInvalid_ReturnsErrorMessage()
        {
            Mock<IPaymentRepository> mockRepository = GetMockRepository();
            Mock<HttpMessageHandler> mockHttpHandler = GetHttpHandlerMock("{success: false}");
            AuthorizationInput input = new AuthorizationInput
            {
                Card = new CreditCard
                {
                    CardNumber = "1412411114124111",
                    ExpiryYear = 2000
                },
                Amount = 5.00M,
                Currency = "GBP"
            };

            PaymentService service = GetService(mockRepository, mockHttpHandler);

            AuthorizationOutput output = service.AuthorizeTransaction(input).Result;

            Assert.IsNotNull(output.TransactionOutput.Error);
            Assert.IsTrue(output.TransactionOutput.Error.Length > 0);
        }
    }
}
