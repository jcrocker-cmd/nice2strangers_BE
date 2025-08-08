using Crud.Contracts;
using Crud.Data;
using Crud.Models;
using Crud.Models.Entities;
using Crud.Service;
using Crud.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Crud.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : Controller
    {

        private readonly IEmailService _emailService;
        private readonly INewsletterService _newsletterService;
        public readonly ApplicationDBContext dbContext;

        public EmailController(IEmailService _emailService, ApplicationDBContext dbContext, INewsletterService _newsletterService)
        {
            this._emailService = _emailService;
            this.dbContext = dbContext;
            this._newsletterService = _newsletterService;
        }

        [HttpPost("send-email")]
        public async Task <IActionResult> SendEmail(EmailRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ToEmail))
                return BadRequest("ToEmail is required.");

            await _emailService.SendEmailAsync(request.ToEmail, request.Subject, request.Body);
            return Ok("Email Sent");
        }

        [HttpPost("post-contact-us")]
        public async Task<IActionResult> ContactUs([FromBody] EmailRequest request)
        {
            var subject = Constants.Subject.Inquiry;
            var message = request.Body;
            var name = request.Name;
            var email = request.ToEmail;

            var body = Constants.ContactUsClient(name);
            var adminBody = Constants.ContactUs(name, message, email);

            if (!string.IsNullOrEmpty(email))
            {
                await _emailService.SendEmailAsync(email, subject, body);
            }

            await _emailService.SendEmailAsync(Constants.AdminEmail, $"Admin Notification - {subject}", adminBody);

            var contactus = new ContactUs
            {
                Name = request.Name,
                Subject =request.Subject,
                Email = request.ToEmail,
                Message = request.Body,
                CreatedDate = DateTime.UtcNow
            };
            dbContext.ContactUs.Add(contactus);
            await dbContext.SaveChangesAsync();
            return Ok(contactus);

        }

        [HttpGet("get-contact-us")]
        public async Task<IActionResult> GetContactus()
        {
            await dbContext.ContactUs
            .AsNoTracking()
            .Select(c => new EmailRequest
            {
                Name = c.Name,
                ToEmail = c.Email,
                Subject = c.Subject,
                Body = c.Message,
                CreatedDate = c.CreatedDate.ToString("MMM, dd, yyyy h:mmtt", CultureInfo.InvariantCulture)
            })
            .ToListAsync();
            return Ok(dbContext.ContactUs);
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
    }
}
