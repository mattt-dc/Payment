using Moq;
using Moq.Protected;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentGateway.Domain.UnitTests.Services.PaymentServiceTests
{
    public partial class PaymentServiceTests
    {
        private Mock<IPaymentRepository> GetMockRepository()
        {
            var mock = new Mock<IPaymentRepository>();
            mock.Setup(x => x.GetTransaction(1))
                .ReturnsAsync(new Transaction
                {
                    AmountAvailable = 15M,
                    Id = 1,
                    ExternalId = 1,
                    Currency = "GBP",
                    Payments = new List<Payment>(),
                    Refunds = new List<Refund>()
                });
            mock.Setup(x => x.RecordPayment(It.IsAny<long>(), It.IsAny<decimal>()))
                .ReturnsAsync(true);
            return mock;
        }

        private Mock<HttpMessageHandler> GetHttpHandlerMock(string response)
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(response)
                })
                .Verifiable();
            return handlerMock;
        }

        private PaymentService GetService(Mock<IPaymentRepository> mockRepository, Mock<HttpMessageHandler> mockHttpHandler)
        {
            PaymentService service = new PaymentService(mockRepository.Object, new HttpClient(mockHttpHandler.Object));
            return service;
        }
    }
}
