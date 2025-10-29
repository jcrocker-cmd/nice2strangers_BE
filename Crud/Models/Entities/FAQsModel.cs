using System.ComponentModel.DataAnnotations.Schema;

namespace Crud.Models.Entities
{
    public class FAQsModel
    {
        [Column("ID")]
        public Guid Id { get; set; }

        [Column("Question")]
        public string Question { get; set; }

        [Column("Answer")]
        public string Answer { get; set; }

        [Column("Is_Active")]
        public bool? isActive { get; set; }

        [Column("Created_Date")]
        public DateTime CreatedDate { get; set; }

        [Column("Updated_Date")]
        public DateTime UpdatedDate { get; set; }
    }
}
