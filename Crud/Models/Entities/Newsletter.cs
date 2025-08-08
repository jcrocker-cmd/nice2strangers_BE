using System.ComponentModel.DataAnnotations.Schema;

namespace Crud.Models.Entities
{
    public class Newsletter
    {
        [Column("ID")]
        public Guid Id { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("Email")]
        public string Email { get; set; }

        [Column("Created_Date")]
        public DateTime CreatedDate { get; set; }
    }
}
