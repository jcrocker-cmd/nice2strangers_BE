using Crud.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Crud.Service;
using Crud.Contracts;

namespace Crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SP_EmployeesController : Controller
    {

        private readonly ISP_EmployeeService _service;

        public SP_EmployeesController(ISP_EmployeeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _service.GetAllAsync();
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var employee = await _service.GetByIdAsync(id);
            return employee == null ? NotFound() : Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SP_Employee emp)
        {
            await _service.InsertAsync(emp);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SP_Employee emp)
        {
            emp.ID = id;
            await _service.UpdateAsync(emp);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
    }
}
