namespace Crud.ViewModel
{
    public class ProductViewModel
    {
        public string ProductName { get; set; }
        public decimal PriceInCents { get; set; }
        public IFormFile Image { get; set; }
        public bool isActive { get; set; } 
    }
}
