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
        private readonly PaymentService _stripePaymentService;

        public PaymentsController(PaymentService stripePaymentService)
        {
            _stripePaymentService = stripePaymentService;
        }

        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> CreateCheckoutSession()
        {
            var successUrl = "https://localhost:5001/success";
            var cancelUrl = "https://localhost:5001/cancel";

            var session = await _stripePaymentService.CreateCheckoutSession(successUrl, cancelUrl);
            return Ok(new { sessionId = session.Id, url = session.Url });
        }

        [HttpPost("create-checkout")]
        public async Task<IActionResult> CreateCheckoutSession([FromBody] List<CheckoutViewModel> selectedProducts)
        {
            var successUrl = "http://localhost:3000/success";
            var cancelUrl = "http://localhost:3000/cancel";

            var session = await _stripePaymentService.CreateCheckout(selectedProducts, successUrl, cancelUrl);

            return Ok(new { sessionId = session.Id, url = session.Url });
        }

        [HttpGet("transactions")]
        public IActionResult GetTransactions()
        {
            var transactions = _stripePaymentService.GetTransactions();
            return Ok(transactions);
        }


        [HttpPost("refund")]
        public async Task<IActionResult> Refund([FromBody] RefundRequest request)
        {
            var refund = await _stripePaymentService.RefundCharge(request.ChargeId);
            return Ok(refund);
        }

        [HttpGet("transactions/summary")]
        public IActionResult GetTransactionSummary()
        {
            var summary = _stripePaymentService.GetTransactionSummary();
            return Ok(summary);
        }

        [HttpGet("balance")]
        public async Task<IActionResult> GetTotalBalance()
        {
            var balance = await _stripePaymentService.GetStripeBalanceAsync();
            return Ok(balance);
        }


    }

}
