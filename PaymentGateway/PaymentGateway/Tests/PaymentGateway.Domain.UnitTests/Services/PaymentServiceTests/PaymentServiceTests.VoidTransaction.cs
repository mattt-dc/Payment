using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public void VoidTransaction_Success_ReturnsSuccess()
        {
            Mock<IPaymentRepository> mockRepository = GetMockRepository();
            PaymentService service = GetService(mockRepository);

            TransactionOutput output = service.VoidTransaction(1).Result;

            Assert.IsTrue(output.Success);
        }

        public void VoidTransaction_Fails_ReturnsError()
        {
            Mock<IPaymentRepository> mockRepository = GetMockRepository();
            PaymentService service = GetService(mockRepository);

            TransactionOutput output = service.VoidTransaction(1).Result;

            Assert.IsNotNull(output.Error);
            Assert.IsTrue(output.Error.Length > 0);
        }
    }
}
