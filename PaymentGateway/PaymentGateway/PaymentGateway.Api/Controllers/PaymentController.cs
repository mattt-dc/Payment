using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Api.Models.Request;
using PaymentGateway.Api.Models.Response;
using PaymentGateway.Domain;
using PaymentGateway.Domain.Entities;

namespace PaymentGateway.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentService _service;

        public PaymentController(PaymentService service)
        {
            _service = service;
        }

        [HttpPost("Authorize")]
        public async Task<IActionResult> Authorize(AuthorizationRequest request)
        {
            AuthorizationInput input = new AuthorizationInput
            {
                Card = new CreditCard
                {
                    CardNumber = request.CardNumber,
                    CVV = request.CVV,
                    ExpiryMonth = request.ExpiryMonth,
                    ExpiryYear = request.ExpiryYear
                },
                Amount = request.Amount,
                Currency = request.Currency
            };
            AuthorizationOutput serviceResponse = await _service.AuthorizeTransaction(input);

            AuthorizeResponse response = new AuthorizeResponse
            {
                AmountAvailable = serviceResponse.TransactionOutput.AmountAvailable,
                Currency = serviceResponse.TransactionOutput.Currency,
                Error = serviceResponse.TransactionOutput.Error,
                Id = serviceResponse.AuthorizationId,
                Success = serviceResponse.TransactionOutput.Success
            };

            return Ok(response);
        }

        [HttpPost("Capture")]
        public async Task<IActionResult> Capture(CaptureRequest request)
        {
            PaymentRequest paymentRequest = new PaymentRequest
            {
                AuthorizationId = request.AuthorizationId,
                Amount = request.Amount
            };
            TransactionOutput serviceResponse = await _service.CapturePayment(paymentRequest);

            StatusResponse response = new StatusResponse
            {
                AmountAvailable = serviceResponse.AmountAvailable,
                Currency = serviceResponse.Currency,
                Error = serviceResponse.Error,
                Success = serviceResponse.Success
            };

            return Ok(response);
        }

        [HttpPost("Void")]
        public async Task<IActionResult> Void(VoidRequest request)
        {
            TransactionOutput serviceResponse = await _service.VoidTransaction(request.AuthorizationId);

            StatusResponse response = new StatusResponse
            {
                AmountAvailable = serviceResponse.AmountAvailable,
                Currency = serviceResponse.Currency,
                Error = serviceResponse.Error,
                Success = serviceResponse.Success
            };

            return Ok(response);
        }

        [HttpPost("Refund")]
        public async Task<IActionResult> Refund(RefundRequest request)
        {
            PaymentRequest paymentRequest = new PaymentRequest
            {
                Amount = request.Amount,
                AuthorizationId = request.AuthorizationId
            };
            TransactionOutput serviceResponse = await _service.RefundPayment(paymentRequest);

            StatusResponse response = new StatusResponse
            {
                AmountAvailable = serviceResponse.AmountAvailable,
                Currency = serviceResponse.Currency,
                Error = serviceResponse.Error,
                Success = serviceResponse.Success
            };

            return Ok(response);
        }
    }
}