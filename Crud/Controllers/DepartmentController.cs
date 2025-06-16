using Crud.Data;
using Crud.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crud.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly ApplicationDBContext dbContext;

        public DepartmentsController(ApplicationDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        {
            return await dbContext.Departments.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartment(Guid id)
        {
            var department = await dbContext.Departments.FindAsync(id);
            if (department == null) return NotFound();
            return department;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment([FromBody] Department department)
        {
            dbContext.Departments.Add(department);
            await dbContext.SaveChangesAsync();
            return Ok(new { department.Id, department.Name });
        }
    }
}
