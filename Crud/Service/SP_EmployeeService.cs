using Crud.Models.Entities;
using Crud.Data;
using Microsoft.EntityFrameworkCore;
using Crud.Contracts;

namespace Crud.Service{
    public class SP_EmployeeService : ISP_EmployeeService
    {
        private readonly ApplicationDBContext dBContext;

        public SP_EmployeeService(ApplicationDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        public async Task<List<SP_Employee>> GetAllAsync()
        {
            return await dBContext.SP_Employees
                .FromSqlRaw("EXEC SP_Employee @ID = 0, @Email = NULL, @Emp_Name = NULL, @Designation = NULL, @type = 'get'")
                .ToListAsync();
        }

        public async Task<SP_Employee?> GetByIdAsync(int id)
        {
            return dBContext.SP_Employees
                .FromSqlRaw("EXEC SP_Employee @ID = {0}, @Email = NULL, @Emp_Name = NULL, @Designation = NULL, @type = 'getid'", id)
                .AsEnumerable()
                .FirstOrDefault();
        }

        public async Task InsertAsync(SP_Employee emp)
        {
            await dBContext.Database.ExecuteSqlRawAsync(
                "EXEC SP_Employee @ID = 0, @Email = {0}, @Emp_Name = {1}, @Designation = {2}, @type = 'insert'",
                emp.Email, emp.Emp_Name, emp.Designation);
        }

        public async Task UpdateAsync(SP_Employee emp)
        {
            await dBContext.Database.ExecuteSqlRawAsync(
                "EXEC SP_Employee @ID = {0}, @Email = {1}, @Emp_Name = {2}, @Designation = {3}, @type = 'update'",
                emp.ID, emp.Email, emp.Emp_Name, emp.Designation);
        }

        public async Task DeleteAsync(int id)
        {
            await dBContext.Database.ExecuteSqlRawAsync(
                "EXEC SP_Employee @ID = {0}, @Email = NULL, @Emp_Name = NULL, @Designation = NULL, @type = 'delete'", id);
        }
    }
}