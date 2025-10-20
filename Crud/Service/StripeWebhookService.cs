using Crud.Contracts;
using Crud.Data;
using Crud.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;
using System.Text.Json;


namespace Crud.Service
{
    public class StripeWebhookService : IStripeWebhookService
    {

        private readonly IConfiguration _config;
        private readonly ApplicationDBContext dbContext;

        public StripeWebhookService(IConfiguration config, ApplicationDBContext dbContext)
        {
            _config = config;
            this.dbContext = dbContext;
        }

        public async Task UpdateProductStocks(string json, string stripeSignature)
        {
            var _webhookSecret = _config["Stripe:WebhookSecret"];
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, _webhookSecret);

                if (stripeEvent.Type == "checkout.session.completed" ||
                    stripeEvent.Type == "checkout.session.async_payment_succeeded" ||
                    stripeEvent.Type == "payment_intent.succeeded")
                {
                    var session = stripeEvent.Data.Object as Session;

                    if (session == null) return;

                    var sessionService = new SessionService();
                    session = await sessionService.GetAsync(session.Id, new SessionGetOptions
                    {
                        Expand = new List<string> { "line_items", "line_items.data.price.product" }
                    });

                    var lineItems = session.LineItems?.Data;
                    if (lineItems == null || lineItems.Count == 0) return;

                    foreach (var item in lineItems)
                    {
                        var quantity = item.Quantity ?? 0;
                        var stripeProduct = item.Price?.Product as Stripe.Product;

                        if (stripeProduct?.Metadata != null &&
                            stripeProduct.Metadata.TryGetValue("product_id", out string dbIdStr) &&
                            Guid.TryParse(dbIdStr, out Guid dbId))
                        {
                            var dbProduct = await dbContext.Products.FindAsync(dbId);
                            if (dbProduct != null)
                            {
                                dbProduct.Stocks -= (int)quantity;
                                if (dbProduct.Stocks < 0) dbProduct.Stocks = 0;
                                dbProduct.UpdatedDate = DateTime.UtcNow;

                                dbContext.Products.Update(dbProduct);
                            }
                        }
                    }

                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Webhook Error] {ex.Message}");
            }
        }




        public async Task SavePurchasedProducts(string json, string stripeSignature)
        {
            var webhookSecret = _config["Stripe:WebhookSecretBuyed"];

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, webhookSecret);

                if (stripeEvent.Type == "checkout.session.completed" ||
                    stripeEvent.Type == "checkout.session.async_payment_succeeded" ||
                    stripeEvent.Type == "payment_intent.succeeded")
                {
                    var session = stripeEvent.Data.Object as Session;
                    if (session == null) return;

                    var sessionService = new SessionService();
                    session = await sessionService.GetAsync(session.Id, new SessionGetOptions
                    {
                        Expand = new List<string> { "line_items", "line_items.data.price.product" }
                    });

                    var lineItems = session.LineItems?.Data;
                    if (lineItems == null || lineItems.Count == 0) return;

                    foreach (var item in lineItems)
                    {
                        var metadata = item.Price?.Product?.Metadata;
                        var userIdStr = metadata?.GetValueOrDefault("user_id");
                        var productIdStr = metadata?.GetValueOrDefault("product_id");

                        if (!Guid.TryParse(productIdStr, out Guid productId)) continue;
                        if (!Guid.TryParse(userIdStr, out Guid userId)) continue;

                        var quantity = item.Quantity ?? 1;
                        var totalAmount = (int)(item.AmountSubtotal);

                        var order = new BuyedProduct
                        {
                            UserId = userId,
                            ProductId = productId,
                            Quantity = (int)quantity,
                            TotalAmount = totalAmount,
                            PurchaseDate = DateTime.UtcNow
                        };

                        dbContext.BuyedProducts.Add(order);

                        var cartItem = await dbContext.CartItems
                            .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

                        if (cartItem != null)
                        {
                            dbContext.CartItems.Remove(cartItem);
                        }
                    }

                    await dbContext.SaveChangesAsync();
                }
            }
            catch (StripeException ex)
            {
                Console.WriteLine($"⚠️ Stripe webhook error (SavePurchasedProducts): {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ General webhook error (SavePurchasedProducts): {ex.Message}");
            }
        }

    }
}
