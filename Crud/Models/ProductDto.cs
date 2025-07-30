namespace Crud.Models
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public int PriceInCents { get; set; }
        public string? Image { get; set; }
        public bool? isActive { get; set; }
    }
}
