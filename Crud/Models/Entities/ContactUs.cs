using System.ComponentModel.DataAnnotations.Schema;

namespace Crud.Models.Entities
{
    public class ContactUs
    {
        [Column("ID")]
        public Guid Id { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("Email")]
        public string Email { get; set; }

        [Column("Subject")]
        public string Subject { get; set; }

        [Column("Message")]
        public string Message { get; set; }

        [Column("Is_Replied")]
        public bool IsReplied { get; set; } = false;

        [Column("Created_Date")]
        public DateTime CreatedDate { get; set; }

    }
}
