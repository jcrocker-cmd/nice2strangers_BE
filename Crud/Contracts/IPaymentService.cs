using Crud.ViewModel;
using Stripe;
using Stripe.Checkout;

namespace Crud.Contracts
{
    public interface IPaymentService
    {

        Task<Session> CreateCheckoutSession(string successUrl, string cancelUrl);
        Task<Session> CreateCheckout(List<ProductViewModel> products, string successUrl, string cancelUrl);
        List<Charge> GetPayments();
        Task<Refund> RefundCharge(string chargeId);
        TransactionSummaryViewModel GetTransactionSummary();
        IEnumerable<TransactionViewModel> GetTransactions();
        Task<BalanceViewModel> GetStripeBalanceAsync();
    }
}
