namespace Crud.Models
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public int Stocks { get; set; }
        public int PriceInCents { get; set; }
        public string? Image { get; set; }
        public bool? isActive { get; set; }
        public string? CreatedDate { get; set; }

    }
}

