using Crud.Data;
using Crud.Hubs;
using Crud.Models;
using Crud.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Text;
using Crud.Contracts;

namespace Crud.Controllers
{
    //localhost:xxxx/api/employees
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        //- It is used to store the injected database context instance   for use throughout the controller.
        private readonly ApplicationDBContext dbContext;

        private readonly IHubContext<EmployeeHub> _hubContext;

        private readonly IEmployeeService _employeeService;

        public EmployeesController(ApplicationDBContext dbContext, IHubContext<EmployeeHub> hubContext , IEmployeeService employeeService)
        {
            //- The parameter dbContext is assigned to the private field this.dbContext, making it available throughout the controller.
            this.dbContext = dbContext;
            _hubContext = hubContext;
            _employeeService = employeeService;
        }

        //[HttpGet("employees")]
        //public IActionResult GetAllEmployees()
        //{
        //    var allEmployees = dbContext.Employees.ToList();
        //    return Ok(allEmployees);
        //}

        [HttpGet("employees")]
        public IActionResult GetAllEmployees()
        {
            var allEmployees = dbContext.Employees
                .Include(e => e.Department) // Eager load Department (acts like JOIN)
                .Select(e => new
                {
                    e.Id,
                    e.Name,
                    e.Email,
                    e.Phone,
                    e.Salary,
                    e.CreatedDate,
                    e.DepartmentId,
                    DepartmentName = e.Department != null ? e.Department.Name : null
                })
                .ToList();

            return Ok(allEmployees);
        }

        [HttpGet("{id:guid}")]
        //public IActionResult GetEmployeedById(Guid id)
        //{
        //    var employee = dbContext.Employees.Find(id);

        //    if (employee is null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(employee);
        //}

        public IActionResult GetEmployeedById(Guid id)
        {
            var employee = dbContext.Employees
                .Include(e => e.Department) // Eager load Department (acts like JOIN)
                .Where(e => e.Id == id)
                .Select(e => new
                {
                    e.Id,
                    e.Name,
                    e.Email,
                    e.Phone,
                    e.Salary,
                    e.CreatedDate,
                    e.DepartmentId,
                    DepartmentName = e.Department != null ? e.Department.Name : null
                })
                .FirstOrDefault();

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
            var employeeEntity = new Employee()
            {
                Name = addEmployeeDto.Name,
                Email = addEmployeeDto.Email,
                Phone = addEmployeeDto.Phone,
                Salary = addEmployeeDto.Salary,
                DepartmentId = addEmployeeDto.DepartmentId,
                CreatedDate = addEmployeeDto.CreatedDate,
            };
            dbContext.Employees.Add(employeeEntity);
            dbContext.SaveChanges();

            // Broadcast to all connected clients
            _hubContext.Clients.All.SendAsync("EmployeeAdded", employeeEntity);

            return Ok(employeeEntity);
        }

        [HttpGet("count")]
        public IActionResult GetEmployeeCount()
        {
            int employeeCount = _employeeService.GetEmployeeCount();
            int departmentCount = _employeeService.GetDepartmentCount();
            return Ok(new { 
                emp = employeeCount,
                dept = departmentCount

            });
        }

        [HttpGet("export")]
        public IActionResult ExportEmployeesToCSV()
        {
            var employees = dbContext.Employees.ToList();

            var csv = new StringBuilder();
            csv.AppendLine(Constants.ExportEmployee.Header);

            foreach (var e in employees)
            {
                csv.AppendLine(string.Format(Constants.ExportEmployee.RowFormat,
                    e.Id,
                    e.Name,
                    e.Email,
                    e.Phone ?? string.Empty, // Handle null phone numbers
                    e.Salary.ToString("F2"), // Format salary to 2 decimal places
                    e.CreatedDate.ToString(Constants.Common.DateTimeFormat), // Use the defined format
                    e.DepartmentId));
            }
            //The File() method in ASP.NET Core(inside a controller) needs raw bytes to return a downloadable file to the browser.It doesn’t take a string directly.
            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", "employees.csv");
        }


        [HttpPost("import-employees")]
        public async Task<IActionResult> ImportEmployees(IFormFile file)
        {
            // Check if the uploaded file is null or empty
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            // Create a list to hold the imported employee records
            var employees = new List<Employee>();

            // Open the uploaded file for reading
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                bool isFirstLine = true; // Flag to skip the header row

                // Loop through the file until all lines are read
                while (!reader.EndOfStream)
                {
                    // Read one line at a time
                    var line = await reader.ReadLineAsync();

                    // Skip the first line (headers: Id, Name, Email, etc.)
                    if (isFirstLine)
                    {
                        isFirstLine = false; // Flip the flag after first line
                        continue;            // Skip processing this header line
                    }

                    // Split the line by commas into individual values
                    var values = line.Split(',');

                    // Map the values to a new Employee object
                    var employee = new Employee
                    {
                        Id = Guid.Parse(values[0]),                 // Parse Guid for Id
                        Name = values[1],                           // Name string
                        Email = values[2],                          // Email string
                        Phone = values[3],                          // Phone string
                        Salary = decimal.Parse(values[4]),          // Convert salary to decimal
                        CreatedDate = DateTime.Parse(values[5]),    // Convert to DateTime
                        DepartmentId = int.Parse(values[6])         // Convert to int for department
                    };

                    // Add the employee to the list
                    employees.Add(employee);
                }
            }

            // Add all employees to the database in a single batch
            await dbContext.Employees.AddRangeAsync(employees);

            // Save the changes to the database
            await dbContext.SaveChangesAsync();

            // Return success message with how many records were imported
            return Ok($"{employees.Count} employees imported successfully.");
        }

    }
}
