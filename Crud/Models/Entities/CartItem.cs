using System.ComponentModel.DataAnnotations.Schema;

namespace Crud.Models.Entities
{
    public class CartItem
    {

        [Column("ID")]
        public Guid Id { get; set; }

        [Column("User_ID")]
        public Guid? UserId { get; set; }

        [Column("Product_ID")]
        public Guid? ProductId { get; set; }

        [Column("Quantity")]
        public int Quantity { get; set; }

        [Column("Created_Date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
    }
}
