using Crud.Service;
using Crud.ViewModel;
using Microsoft.AspNetCore.Mvc;

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

    }

}
