using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentGateway.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Domain.UnitTests.Services.PaymentServiceTests
{
    public partial class PaymentServiceTests
    {
        public void RefundPayment_Success_ReturnsSuccess()
        {
            PaymentRequest request = new PaymentRequest
            {
                Amount = 5M,
                AuthorizationId = 1
            };

            PaymentService service = new PaymentService();

            TransactionOutput output = service.RefundPayment(request);

            Assert.IsTrue(output.Success);
        }

        public void RefundPayment_InvalidAmount_ReturnsError()
        {
            PaymentRequest request = new PaymentRequest
            {
                Amount = 12M,
                AuthorizationId = 1
            };

            PaymentService service = new PaymentService();

            TransactionOutput output = service.RefundPayment(request);

            Assert.IsFalse(output.Success);
            Assert.IsNotNull(output.Error);
            Assert.IsTrue(output.Error.Length > 0);
        }
    }
}
