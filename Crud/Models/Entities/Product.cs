

namespace Crud.Models.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public int PriceInCents { get; set; }
        public string Image { get; set; }
        public bool? isActive{ get; set; }

        //Category
        //Description
        //stocks
        //created at

    }
}
