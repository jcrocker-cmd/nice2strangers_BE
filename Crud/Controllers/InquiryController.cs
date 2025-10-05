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
    public class InquiryController : Controller
    {
        private readonly IInquiryService _inquiryService;
        public readonly ApplicationDBContext dbContext;

        public InquiryController(ApplicationDBContext dbContext, IInquiryService _inquiryService)
        {
            this.dbContext = dbContext;
            this._inquiryService = _inquiryService;
        }

        [HttpPost("post-contact-us")]
        public async Task<IActionResult> ContactUs([FromBody] EmailRequest request)
        {
                var result = await _inquiryService.ContactUsAsync(request);
                return Ok(result);
        }

        [HttpGet("get-contact-us")]
        public async Task<IActionResult> GetContactus()
        {
            var result = await _inquiryService.GetContactusAsync();
            return Ok(result);
        }

        [HttpPost("send-reply")]
        public async Task<IActionResult> SendReply([FromBody] SendReplyRequest request)
        {
            var result = await _inquiryService.SendReplyAsync(request);
            return Ok(result);
        }

    }
}
