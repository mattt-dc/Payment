using Moq;
using PaymentGateway.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Domain.UnitTests.Services.PaymentServiceTests
{
    public partial class PaymentServiceTests
    {
        private Mock<IPaymentRepository> GetMockRepository()
        {
            return new Mock<IPaymentRepository>();
        }

        private PaymentService GetService(Mock<IPaymentRepository> mockRepository)
        {
            PaymentService service = new PaymentService(mockRepository.Object);
            return service;
        }
    }
}
