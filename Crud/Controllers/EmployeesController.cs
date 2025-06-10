using Crud.Data;
using Crud.Models;
using Crud.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Crud.Controllers
{
    //localhost:xxxx/api/employees
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        //- It is used to store the injected database context instance for use throughout the controller.
        private readonly ApplicationDBContext dbContext;

        public EmployeesController(ApplicationDBContext dbContext)
        {
            //- The parameter dbContext is assigned to the private field this.dbContext, making it available throughout the controller.
            this.dbContext = dbContext;
        }
        
        [HttpGet("employees")]
        public IActionResult GetAllEmployees()
        {
          var allEmployees = dbContext.Employees.ToList();
          return Ok(allEmployees);
        }

        [HttpGet("{id:guid}")]
        //[Route("{id:guid}")]
        public IActionResult GetEmployeedById(Guid id)
        {
            var employee = dbContext.Employees.Find(id);
            if (employee is null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteEmployeeById(Guid id)
        {
            var employee = dbContext.Employees.Find(id);
            if (employee is null)
            {
                return NotFound();
            }
            dbContext.Employees.Remove(employee);
            dbContext.SaveChanges();
            return Ok($"Employee with ID {id} has been deleted.");

        }

        [HttpPut("{id:guid}")]
        public IActionResult UpdateEmployee(Guid id, UpdateEmployeeDto updateEmployeeDto)
        {
            var employee = dbContext.Employees.Find(id);
            if (employee is null)
            {
                return NotFound();
            }
            employee.Name = updateEmployeeDto.Name;
            employee.Email = updateEmployeeDto.Email;
            employee.Phone = updateEmployeeDto.Phone;
            employee.Salary = updateEmployeeDto.Salary;
            employee.DepartmentId = updateEmployeeDto.DepartmentId;


            dbContext.SaveChanges();
            return Ok($"Employee with ID {id} has been updated.");
        }

        [HttpPost("create")]
        public IActionResult AddEmployee(AddEmployeeDto addEmployeeDto)
        {
            var employeeEntity = new Employee() { 
                Name = addEmployeeDto.Name,
                Email = addEmployeeDto.Email,
                Phone = addEmployeeDto.Phone,  
                Salary = addEmployeeDto.Salary,  
                DepartmentId = addEmployeeDto.DepartmentId
            };
            dbContext.Employees.Add(employeeEntity);
            dbContext.SaveChanges();
            return Ok(employeeEntity);
        }
    }
}
