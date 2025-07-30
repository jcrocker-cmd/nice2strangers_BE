using Crud.Data;
using Crud.Contracts;

namespace Crud.Service
{
    public class EmployeeService : IEmployeeService
    {

        private readonly ApplicationDBContext _dbContext;

        public EmployeeService(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int GetEmployeeCount()
        {
            // Returns the total number of employees in the database
            return _dbContext.Employees.Count();
        }

        public int GetDepartmentCount()
        {
            // Returns the total number of departments in the database
            return _dbContext.Departments.Count();
        }
    }
}
