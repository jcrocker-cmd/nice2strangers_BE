using Crud.Contracts;
using Crud.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FAQsController : ControllerBase
    {
        private readonly IFAQsService _faqsService;
        public FAQsController(IFAQsService faqsService)
        {
           _faqsService = faqsService;
        }

        [HttpPost("addFAQs")]
        public async Task<IActionResult> AddFAQs([FromBody] FAQsViewModel faqsViewModel)
        {
            var faqs = await _faqsService.AddFAQs(faqsViewModel);
            return Ok(faqs);
        }


        [HttpGet("FAQs")]
        public async Task<IActionResult> GetAllFAQs()
        {
            var allFAQS = await _faqsService.GetAllFAQs(); 
            return Ok(allFAQS);
        }

        [HttpPut("updateFAQ/{id}")]
        public async Task<IActionResult> UpdateFAQ(Guid id, [FromForm] FAQsViewModel faqsViewModel)
        {
            var updated = await _faqsService.UpdateFAQ(id, faqsViewModel);

            if (updated == null)
                return NotFound();

            return Ok(updated);
        }


        [HttpPut("softDeleteFAQ/{id}")]
        public async Task<IActionResult> SoftDeleteFAQ(Guid id)
        {
            var faq = await _faqsService.SoftDeleteFAQ(id);
            if (faq == null) return NotFound();
            return Ok(faq);
        }

        [HttpPut("recoverFAQ/{id}")]
        public async Task<IActionResult> RecoverFAQ(Guid id)
        {
            var faq = await _faqsService.RecoverFAQ(id);
            if (faq == null) return NotFound();
            return Ok(faq);
        }

        [HttpGet("FAQsCount")]
        public async Task<IActionResult> GetProductCounts()
        {
            var count = await _faqsService.GetFAQsCount();
            return Ok(count);
        }

    }
}
