using Crud.Data;
using Crud.Models.Entities;
using Crud.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;
using System;


namespace Crud.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class StripeWebhookController : Controller
    {
        private readonly StripeWebhookService _stripeWebhookService;
        private readonly IConfiguration _config;

        public StripeWebhookController(ApplicationDBContext context, StripeWebhookService stripeWebhookService)
        {
            _stripeWebhookService = stripeWebhookService;
        }

        [HttpPost("saved-buyed-products-webhook")]
        public async Task<IActionResult> SavedBuyedProduct()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            await _stripeWebhookService.SavePurchasedProducts(
                json,
                Request.Headers["Stripe-Signature"]
            );

            return Ok();
        }


        [HttpPost("update-stocks-webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                await _stripeWebhookService.UpdateProductStocks(
                    json,
                    Request.Headers["Stripe-Signature"]
                );

                return Ok();
            }
            catch (StripeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Webhook processing error: {ex.Message}");
            }
        }

    }
}
