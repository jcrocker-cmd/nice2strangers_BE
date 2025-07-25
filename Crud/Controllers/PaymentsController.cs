using Crud.Service;
using Crud.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Globalization;

namespace Crud.Controllers
{

    [ApiController]
    [Route("api/[controller]")]


    public class PaymentsController : ControllerBase
    {
        private readonly StripePaymentService _stripePaymentService;

        public PaymentsController(StripePaymentService stripePaymentService)
        {
            _stripePaymentService = stripePaymentService;
        }

        [HttpGet("products")]
        public IActionResult GetProducts()
        {
            var products = new List<ProductViewModel>
            {
                new() { Name = "Shirt", PriceInCents = 2000 },
                new() { Name = "Book", PriceInCents = 1500 },
                new() { Name = "Mug", PriceInCents = 1000 }
            };

            return Ok(products);
        }

        [HttpPost("create-checkout-session")]
        public IActionResult CreateCheckoutSession()
        {
            var successUrl = "https://localhost:5001/success";
            var cancelUrl = "https://localhost:5001/cancel";

            var session = _stripePaymentService.CreateCheckoutSession(successUrl, cancelUrl);
            return Ok(new { sessionId = session.Id, url = session.Url });
        }

        [HttpPost("create-checkout")]
        public IActionResult CreateCheckoutSession([FromBody] List<ProductViewModel> selectedProducts)
        {
            var successUrl = "http://localhost:3000/success";
            var cancelUrl = "http://localhost:3000/cancel";

            var session = _stripePaymentService.CreateCheckout(selectedProducts, successUrl, cancelUrl);

            return Ok(new { sessionId = session.Id, url = session.Url });
        }

        [HttpGet("transactions")]
        public IActionResult GetTransactions()
        {
            var charges = _stripePaymentService.GetPayments();
            var results = charges.Select(c => new
            {
                Id = c.Id,
                Amount = c.Amount,
                Currency = c.Currency,
                Status = c.Status,
                Refunded = c.Refunded ? "Yes" : "No",
                Cancel = c.Customer,
                RefundedDate = c.Refunded && c.Refunds?.Data?.Any() == true
                    ? DateTimeOffset.FromUnixTimeSeconds(((DateTimeOffset)c.Refunds.Data[0].Created).ToUnixTimeSeconds()).ToString("MMM, dd, yyyy h:mmtt", CultureInfo.InvariantCulture)
                    : null,
                Created = DateTimeOffset.FromUnixTimeSeconds(((DateTimeOffset)c.Created).ToUnixTimeSeconds()).ToString("MMM, dd, yyyy h:mmtt", CultureInfo.InvariantCulture),
                CustomerEmail = c.BillingDetails?.Email,
                CustomerName = c.BillingDetails?.Name,
                CardBrand = c.PaymentMethodDetails?.Card?.Brand,
                Last4 = c.PaymentMethodDetails?.Card?.Last4,
                DeclineReason = c.FailureMessage
            });

            return Ok(results);
        }


        [HttpPost("refund")]
        public IActionResult Refund([FromBody] RefundRequest request)
        {
            var refundService = new RefundService();
            var refundOptions = new RefundCreateOptions
            {
                Charge = request.ChargeId
            };

            var refund = refundService.Create(refundOptions);
            return Ok(refund);
        }

        [HttpGet("transactions/summary")]
        public IActionResult GetTransactionSummary()
        {
            var charges = _stripePaymentService.GetPayments();

            var allCount = charges.Count();
            var succeededCount = charges.Count(c => c.Status == "succeeded");
            var refundedCount = charges.Count(c => c.Refunded);
            var disputedCount = charges.Count(c => c.Disputed != false);
            var failedCount = charges.Count(c => c.Status == "failed");
            var uncapturedCount = charges.Count(c => c.Captured == false);

            var result = new
            {
                All = allCount,
                Succeeded = succeededCount,
                Refunded = refundedCount,
                Disputed = disputedCount,
                Failed = failedCount,
                Uncaptured = uncapturedCount
            };

            return Ok(result);
        }

    }

}
