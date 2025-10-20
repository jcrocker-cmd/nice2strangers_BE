using System.ComponentModel.DataAnnotations.Schema;

namespace Crud.Models.Entities
{
    public class BuyedProduct
    {
        [Column("ID")]
        public Guid Id { get; set; }

        [Column("User_ID")]
        public Guid UserId { get; set; }

        [Column("Product_ID")]
        public Guid ProductId { get; set; }

        [Column("Quantity")]
        public int Quantity { get; set; }

        [Column("Total_Amount")]
        public int TotalAmount { get; set; } // in cents

        [Column("Purchase_Date")]
        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
    }
}
