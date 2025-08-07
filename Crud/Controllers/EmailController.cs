﻿using Crud.Contracts;
using Crud.Models;
using Crud.Service;
using Crud.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Crud.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : Controller
    {

        private readonly IEmailService _emailService;

        public EmailController(IEmailService _emailService)
        {
            this._emailService = _emailService;
        }

        [HttpPost("send-email")]
        public async Task <IActionResult> SendEmail(EmailRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ToEmail))
                return BadRequest("ToEmail is required.");

            await _emailService.SendEmailAsync(request.ToEmail, request.Subject, request.Body);
            return Ok("Email Sent");
        }

        [HttpPost("sendInquiry")]
        public async Task<IActionResult> Refund([FromBody] EmailRequest request)
        {
            var subject = Constants.Subject.Inquiry;
            var message = request.Body;
            var name = 
            return Ok(refund);
        }
    }
}
