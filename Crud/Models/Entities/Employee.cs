using System.ComponentModel.DataAnnotations.Schema;

namespace Crud.Models.Entities
{
    public class Employee
    {
        [Column("ID")]
        public Guid Id { get; set; }
        [Column("Name")]
        public required string Name { get; set; }
        [Column("Email")]
        public required string Email { get; set; }
        [Column("Phone")]
        public string? Phone { get; set; }
        [Column("Salary")]
        public decimal Salary { get; set; }
        [Column("Created_Date")]
        public DateTime CreatedDate { get; set; }

        // Foreign Key
        [Column("Department_ID")]         // Custom column name in DB
        [ForeignKey("Department")]        // Link to navigation property
        public int DepartmentId { get; set; }
        public Department? Department { get; set; } = null!;
    }
}
