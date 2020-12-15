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
    [TestClass]
    public partial class PaymentServiceTests
    {
        [TestMethod]
        public void VoidTransaction_Success_ReturnsSuccess()
        {
            Mock<HttpMessageHandler> mockHttpHandler = GetHttpHandlerMock("success");
            Mock<IPaymentRepository> mockRepository = GetMockRepository();
            PaymentService service = GetService(mockRepository, mockHttpHandler);

            TransactionOutput output = service.VoidTransaction(1).Result;

            Assert.IsTrue(output.Success);
        }

        [TestMethod]
        public void VoidTransaction_ServiceReturnsFailure_ReturnsError()
        {
            Mock<HttpMessageHandler> mockHttpHandler = GetHttpHandlerMock("fail");
            Mock<IPaymentRepository> mockRepository = GetMockRepository();
            PaymentService service = GetService(mockRepository, mockHttpHandler);

            TransactionOutput output = service.VoidTransaction(1).Result;

            Assert.IsNotNull(output.Error);
            Assert.IsTrue(output.Error.Length > 0);
        }

        [TestMethod]
        public void VoidTransaction_InvalidAuthorizationId_ReturnsError()
        {
            Mock<HttpMessageHandler> mockHttpHandler = GetHttpHandlerMock("success");
            Mock<IPaymentRepository> mockRepository = GetMockRepository();
            PaymentService service = GetService(mockRepository, mockHttpHandler);

            TransactionOutput output = service.VoidTransaction(2).Result;

            Assert.IsNotNull(output.Error);
            Assert.IsTrue(output.Error.Length > 0);
        }
    }
}
