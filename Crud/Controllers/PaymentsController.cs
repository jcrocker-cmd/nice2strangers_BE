using Crud.Contracts;
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
        private readonly IPaymentService _stripePaymentService;

        public PaymentsController(IPaymentService stripePaymentService)
        {
            _stripePaymentService = stripePaymentService;
        }

        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> CreateCheckoutSession()
        {
            var successUrl = Constants.URL.SuccessOrder;
            var cancelUrl = Constants.URL.FailedOrder;

            var session = await _stripePaymentService.CreateCheckoutSession(successUrl, cancelUrl);
            return Ok(new { sessionId = session.Id, url = session.Url });
        }

        [HttpPost("create-checkout")]
        public async Task<IActionResult> CreateCheckoutSession([FromBody] List<CheckoutViewModel> selectedProducts)
        {
            var successUrl = Constants.URL.SuccessOrder;
            var cancelUrl = Constants.URL.FailedOrder;

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
