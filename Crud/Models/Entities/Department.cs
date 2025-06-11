using System.ComponentModel.DataAnnotations.Schema;

namespace Crud.Models.Entities
{
    public class Department
    {
        [Column("ID")]
        public int Id { get; set; }
        [Column("Name")]
        public required string Name { get; set; }

    }
}
