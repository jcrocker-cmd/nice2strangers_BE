using Crud.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Globalization;
using Crud.Contracts;


namespace Crud.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;

        public PaymentService(IConfiguration config, IEmailService emailService)
        {
            _config = config;
            StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
            _emailService = emailService;
        }

        public async Task<Session> CreateCheckoutSession(string successUrl, string cancelUrl)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "usd",
                            UnitAmount = 5000, // 50.00 USD
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "My Product",
                            },
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,
            };

            var service = new SessionService();
            return await service.CreateAsync(options);
        }


        public async Task<Session> CreateCheckout(List<CheckoutViewModel> products, string successUrl, string cancelUrl)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = products.Select(p => new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        UnitAmount = p.PriceInCents,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = p.ProductName,
                            Metadata = new Dictionary<string, string>
                            {
                              { "product_name", p.ProductName },
                              { "quantity", "1" }
                            }
                        }
                    },
                    Quantity = 1
                }).ToList(),
                Mode = "payment",
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,
            };

            var service = new SessionService();
            return await service.CreateAsync(options);
        }


        public List<Charge> GetPayments()
        {
            var service = new ChargeService();
            var options = new ChargeListOptions
            {
                Limit = 100,
                Expand = new List<string> { "data.refunds" }
            };
            var charge = service.List(options);
            return charge.Data.ToList();
        }


        public async Task<Refund> RefundCharge(string chargeId)
        {
            var refundService = new RefundService();
            var refundOptions = new RefundCreateOptions
            {
                Charge = chargeId
            };

            var refund = await refundService.CreateAsync(refundOptions);

            // Get charge details
            var chargeService = new ChargeService();
            var charge = await chargeService.GetAsync(chargeId);

            // Extract necessary info
            var buyerEmail = charge.BillingDetails?.Email ?? charge.ReceiptEmail;
            var amount = charge.Amount / 100.0m; // Stripe amount is in cents
            var currency = charge.Currency.ToUpper();
            var description = charge.Description ?? "No description";

            // Compose email
            string subject = "Refund Processed";
            string body = $"Your refund has been processed.\n\n" +
                          $"Product: {description}\n" +
                          $"Amount Refunded: {amount} {currency}\n" +
                          $"Charge ID: {chargeId}";

            // Send email to buyer
            if (!string.IsNullOrEmpty(buyerEmail))
            {
                await _emailService.SendEmailAsync(buyerEmail, subject, body);
            }

            // Send email to admin
            string adminEmail = "admin@example.com"; // Replace with your admin email or config value
            await _emailService.SendEmailAsync(adminEmail, $"Admin Notification - {subject}", body);

            return refund;
        }


        public TransactionSummaryViewModel GetTransactionSummary()
        {
            var charges =  GetPayments(); // must return Task<IEnumerable<Charge>>

            return new TransactionSummaryViewModel
            {
                All = charges.Count(),
                Succeeded = charges.Count(c => c.Status == "succeeded"),
                Refunded = charges.Count(c => c.Refunded),
                Disputed = charges.Count(c => c.Disputed != false),
                Failed = charges.Count(c => c.Status == "failed"),
                Uncaptured = charges.Count(c => c.Captured == false)
            };
        }

        public IEnumerable<TransactionViewModel> GetTransactions()
        {
            var charges = GetPayments();
            var results = charges.Select(c => new TransactionViewModel
            {
                Id = c.Id,
                Amount = c.Amount,
                Currency = c.Currency,
                Status = c.Status,
                Refunded = c.Refunded ? "Yes" : "No",
                RefundedDate = c.Refunded && c.Refunds?.Data?.Any() == true
                    ? DateTimeOffset.FromUnixTimeSeconds(((DateTimeOffset)c.Refunds.Data[0].Created).ToUnixTimeSeconds()).ToString("MMM, dd, yyyy h:mmtt", CultureInfo.InvariantCulture)
                    : null,
                Created = DateTimeOffset.FromUnixTimeSeconds(((DateTimeOffset)c.Created).ToUnixTimeSeconds()).ToString("MMM, dd, yyyy h:mmtt", CultureInfo.InvariantCulture),
                CustomerEmail = c.BillingDetails?.Email,
                CustomerName = c.BillingDetails?.Name,
                CardBrand = c.PaymentMethodDetails?.Card?.Brand,
                Last4 = c.PaymentMethodDetails?.Card?.Last4,
                DeclineReason = c.FailureMessage,
                ProductName = c.Metadata.ContainsKey("product_name") ? c.Metadata["product_name"] : null,
                Quantity = c.Metadata.ContainsKey("quantity") ? c.Metadata["quantity"] : null
            });

            return results;
        }

        public async Task<BalanceViewModel> GetStripeBalanceAsync()
        {
            var balanceService = new BalanceService();
            var balance = await balanceService.GetAsync();

            var available = balance.Available.FirstOrDefault();
            var pending = balance.Pending.FirstOrDefault();

            var result = new BalanceViewModel
            {
                Available = (available?.Amount ?? 0) / 100.0,
                Pending = (pending?.Amount ?? 0) / 100.0,
                //currency = available?.Currency?.ToUpper() ?? "USD"
            };
            return result;
        }


    }
}
