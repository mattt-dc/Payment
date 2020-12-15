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
        public void RefundPayment_Success_ReturnsSuccess()
        {
            Mock<HttpMessageHandler> mockHttpHandler = GetHttpHandlerMock("success");
            Mock<IPaymentRepository> mockRepository = GetMockRepository();
            mockRepository.Setup(x => x.GetTransaction(It.IsAny<long>()))
                .ReturnsAsync(new Transaction
                {
                    AmountAvailable = 15M,
                    Id = 1,
                    Payments = new List<Payment>()
                    {
                        new Payment
                        {
                            Amount = 5M
                        } 
                    },
                    Refunds = new List<Refund>()
                });
            PaymentRequest request = new PaymentRequest
            {
                Amount = 5M,
                AuthorizationId = 1
            };

            PaymentService service = GetService(mockRepository, mockHttpHandler);

            TransactionOutput output = service.RefundPayment(request).Result;

            Assert.IsTrue(output.Success);
        }

        [TestMethod]
        public void RefundPayment_InvalidAmount_ReturnsError()
        {
            Mock<HttpMessageHandler> mockHttpHandler = GetHttpHandlerMock("success");
            Mock<IPaymentRepository> mockRepository = GetMockRepository();
            PaymentRequest request = new PaymentRequest
            {
                Amount = 12M,
                AuthorizationId = 1
            };

            PaymentService service = GetService(mockRepository, mockHttpHandler);

            TransactionOutput output = service.RefundPayment(request).Result;

            Assert.IsFalse(output.Success);
            Assert.IsNotNull(output.Error);
            Assert.IsTrue(output.Error.Length > 0);
        }
    }
}
