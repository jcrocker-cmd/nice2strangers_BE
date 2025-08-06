

using System.ComponentModel.DataAnnotations.Schema;

namespace Crud.Models.Entities
{
    public class Product
    {
        [Column("ID")]
        public Guid Id { get; set; }
        [Column("Product_Name")]
        public string ProductName { get; set; }
        [Column("Category")]
        public string Category { get; set; }
        [Column("Description")]
        public string Description { get; set; }
        [Column("Stocks")]
        public int Stocks { get; set; }
        [Column("Price_In_Cents")]
        public int PriceInCents { get; set; }
        [Column("Image")]
        public string Image { get; set; }
        [Column("isActive")]
        public bool? isActive{ get; set; }
        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; }


        //Category
        //Description
        //stocks
        //created at

    }
}
