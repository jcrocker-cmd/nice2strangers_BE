namespace Crud.ViewModel
{
    public class CartItemViewModel
    {
        public Guid? Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid? ProductId { get; set; }
        public string? ProductName { get; set; } = "";
        public string? Image { get; set; } = "";
        public int? Price { get; set; }
        public int Quantity { get; set; }
    }
}
