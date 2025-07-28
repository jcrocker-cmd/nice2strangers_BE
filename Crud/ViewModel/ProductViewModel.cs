namespace Crud.ViewModel
{
    public class ProductViewModel
    {
        public string Name { get; set; }
        public string ProductName { get; set; }
        public int PriceInCents { get; set; }
        public IFormFile Image { get; set; }
        public bool? isActive { get; set; } 
    }
}
