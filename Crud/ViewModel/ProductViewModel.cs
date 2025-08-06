namespace Crud.ViewModel
{
    public class ProductViewModel
    {
        public string ProductName { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public int Stocks { get; set; }
        public decimal PriceInCents { get; set; }
        public IFormFile Image { get; set; }
        public bool isActive { get; set; }
    }
}