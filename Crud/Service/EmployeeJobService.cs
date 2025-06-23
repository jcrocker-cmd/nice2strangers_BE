using Crud.Contracts;
using Crud.Data;
using Crud.Models.Entities;
using System.Numerics;

namespace Crud.Service
{
    public class EmployeeJobService : IEmployeeJobService
    {
        private readonly ApplicationDBContext _dbContext;
        public EmployeeJobService(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddRandomEmployeeAsync()
        {
           var random = new Random();
           var employee = new Employee
            {
                Id = Guid.NewGuid(),
                Name = $"Employee_{Guid.NewGuid().ToString().Substring(0, 8)}",
                Email = $"user{random.Next(1000, 9999)}@example.com",
                Phone = $"555-{random.Next(1000, 9999)}",
                Salary = random.Next(30000, 100000),
                CreatedDate = DateTime.UtcNow,
                DepartmentId = random.Next(1, 4)
           };

            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();
        }
    }


 }
