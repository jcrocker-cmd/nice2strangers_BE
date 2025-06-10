namespace Crud.Models.Entities
{
    public class Employee
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; }
        public decimal Salary { get; set; }

        // Foreign Key
        public int DepartmentId { get; set; }
        public Department? Department { get; set; } = null!;
    }
}
