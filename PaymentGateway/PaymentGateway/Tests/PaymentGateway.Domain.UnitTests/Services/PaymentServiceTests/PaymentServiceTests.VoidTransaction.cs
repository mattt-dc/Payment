using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentGateway.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Domain.UnitTests.Services.PaymentServiceTests
{
    public partial class PaymentServiceTests
    {
        public void VoidTransaction_Success_ReturnsSuccess()
        {
            PaymentService service = new PaymentService();

            TransactionOutput output = service.VoidTransaction(1).Result;

            Assert.IsTrue(output.Success);
        }

        public void VoidTransaction_Fails_ReturnsError()
        {
            PaymentService service = new PaymentService();

            TransactionOutput output = service.VoidTransaction(1).Result;

            Assert.IsNotNull(output.Error);
            Assert.IsTrue(output.Error.Length > 0);
        }
    }
}
