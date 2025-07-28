﻿namespace Crud.Models
{
    public class AddEmployeeDto
    {
        //public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; }
        public decimal Salary { get; set; }
        public int DepartmentId { get; set; }
        //public DateTime CreatedDate { get; set; }

    }
}
