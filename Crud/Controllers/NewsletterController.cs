using Crud.Contracts;
using Crud.Data;
using Crud.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crud.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsletterController : Controller
    {

        private readonly IEmailService _emailService;
        private readonly INewsletterService _newsletterService;
        public readonly ApplicationDBContext dbContext;

        public NewsletterController(IEmailService _emailService, ApplicationDBContext dbContext, INewsletterService _newsletterService)
        {
            this._emailService = _emailService;
            this.dbContext = dbContext;
            this._newsletterService = _newsletterService;
        }

        [HttpPost("post-newsletter")]
        public async Task<IActionResult> Newsletter(NewsletterViewModel newsletterViewModel)
        {
            var newsletter = await _newsletterService.PostNewsletter(newsletterViewModel);
            return Ok(newsletter);
        }

        [HttpGet("get-newsletter")]
        public async Task<IActionResult> GetNewsletter()
        {
            var newsletter = await _newsletterService.GetNewsletterAsync();
            return Ok(newsletter);
        }

        [HttpPost("sendNewsletter")]
        public async Task<IActionResult> SendNewsletter([FromBody] SendNewsletterRequest request)
        {
            var result = await _newsletterService.SendNewsletterAsync(request);

            if (!result)
                return BadRequest("Subject and body are required.");

            return Ok("Newsletter sent successfully!");
        }
    }
}
