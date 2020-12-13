using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentGateway.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Domain.UnitTests.Services.PaymentServiceTests
{
    public partial class PaymentServiceTests
    {
        public void CapturePayment_Success_ReturnsCorrectAvailableAmount()
        {
            PaymentRequest request = new PaymentRequest
            {
                Amount = 5.00M,
                AuthorizationId = 1
            };

            PaymentService service = new PaymentService();

            TransactionOutput output = service.CapturePayment(request).Result;

            Assert.IsTrue(output.AmountAvailable == 5M);
        }

        public void CapturePayment_InvalidPayment_ReturnsError()
        {
            PaymentRequest request = new PaymentRequest
            {
                Amount = 15.00M,
                AuthorizationId = 1
            };

            PaymentService service = new PaymentService();

            TransactionOutput output = service.CapturePayment(request).Result;

            Assert.IsNotNull(output.Error);
            Assert.IsFalse(output.Success);
            Assert.IsTrue(output.Error.Length > 0);
        }
    }
}
