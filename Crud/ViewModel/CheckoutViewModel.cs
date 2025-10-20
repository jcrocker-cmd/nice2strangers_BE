namespace Crud.ViewModel
{
    public class CheckoutViewModel
    {
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public int PriceInCents { get; set; }
    }
}
