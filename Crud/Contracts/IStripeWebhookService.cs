namespace Crud.Contracts
{
    public interface IStripeWebhookService
    {

        Task UpdateProductStocks(string json, string stripeSignature);

        Task SavePurchasedProducts(string json, string stripeSignature);
    }
}
