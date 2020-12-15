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
        public void CapturePayment_Success_ReturnsCorrectAvailableAmount()
        {
            Mock<HttpMessageHandler> mockHttpHandler = GetHttpHandlerMock("success");
            Mock<IPaymentRepository> mockRepository = GetMockRepository();
            PaymentRequest request = new PaymentRequest
            {
                Amount = 10.00M,
                AuthorizationId = 1
            };

            PaymentService service = GetService(mockRepository, mockHttpHandler);

            TransactionOutput output = service.CapturePayment(request).Result;

            Assert.IsTrue(output.AmountAvailable == 5M);
        }

        [TestMethod]
        public void CapturePayment_InvalidPaymentAmount_ReturnsError()
        {
            Mock<HttpMessageHandler> mockHttpHandler = GetHttpHandlerMock("success");
            Mock<IPaymentRepository> mockRepository = GetMockRepository();
            PaymentRequest request = new PaymentRequest
            {
                Amount = 20.00M,
                AuthorizationId = 1
            };

            PaymentService service = GetService(mockRepository, mockHttpHandler);

            TransactionOutput output = service.CapturePayment(request).Result;

            Assert.IsNotNull(output.Error);
            Assert.IsFalse(output.Success);
            Assert.IsTrue(output.Error.Length > 0);
        }

        [TestMethod]
        public void CapturePayment_InvalidAuthorizationId_ReturnsError()
        {
            Mock<HttpMessageHandler> mockHttpHandler = GetHttpHandlerMock("success");
            Mock<IPaymentRepository> mockRepository = GetMockRepository();
            PaymentRequest request = new PaymentRequest
            {
                Amount = 10.00M,
                AuthorizationId = 2
            };

            PaymentService service = GetService(mockRepository, mockHttpHandler);

            TransactionOutput output = service.CapturePayment(request).Result;

            Assert.IsNotNull(output.Error);
            Assert.IsFalse(output.Success);
            Assert.IsTrue(output.Error.Length > 0);
        }
    }
}
