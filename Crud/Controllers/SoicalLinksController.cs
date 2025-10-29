using Crud.ViewModel;
using Crud.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocialLinksController : ControllerBase
    {
        private readonly ISocialLinksService _service;

        public SocialLinksController(ISocialLinksService service)
        {
            _service = service;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var links = await _service.GetSocialLinksAsync();
            return Ok(links);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] SocialLinksViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _service.UpdateSocialLinksAsync(model);
            return Ok(updated);
        }
    }

}
