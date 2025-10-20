namespace Crud.ViewModel
{
    public class BuyedProductViewModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public int TotalAmount { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
