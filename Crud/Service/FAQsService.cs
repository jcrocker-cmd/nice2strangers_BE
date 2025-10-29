using Crud.Contracts;
using Crud.Data;
using Crud.Models;
using Crud.Models.Entities;
using Crud.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Globalization;


namespace Crud.Service
{
    public class FAQsService : IFAQsService
    {
        public readonly ApplicationDBContext dbContext;

        public FAQsService(ApplicationDBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<List<FAQsViewModel>> GetAllFAQs()
        {
            return await dbContext.FAQs
                .AsNoTracking()
                .Select(p => new FAQsViewModel
                {
                    Id = p.Id,
                    Question = p.Question,
                    Answer = p.Answer,
                    isActive = p.isActive,
                    CreatedDate = p.CreatedDate.ToString("MMM, dd, yyyy h:mmtt", CultureInfo.InvariantCulture)
                })
                .ToListAsync();
        }

        public async Task<FAQsModel> AddFAQs(FAQsViewModel faqsViewModel)
        {
            var faqs = new FAQsModel
            {
                Question = faqsViewModel.Question,
                Answer = faqsViewModel.Answer,
                isActive = true,
                CreatedDate = DateTime.UtcNow
            };

            dbContext.FAQs.Add(faqs);
            await dbContext.SaveChangesAsync();

            // Map back to ViewModel
            return faqs;
        }


        public async Task<FAQsModel?> UpdateFAQ(Guid id, FAQsViewModel faqsViewModel)
        {
            var faq = await dbContext.FAQs.FindAsync(id);
            if (faq == null)
                return null;

            faq.Question = faqsViewModel.Question;
            faq.Answer = faqsViewModel.Answer;
            faq.UpdatedDate = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();

            return faq;
        }


        public async Task<FAQsModel?> SoftDeleteFAQ(Guid id)
        {
            var faq = await dbContext.FAQs.FindAsync(id);
            if (faq == null) return null;

            faq.isActive = false;
            faq.UpdatedDate = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();
            return faq;
        }

        public async Task<FAQsModel?> RecoverFAQ(Guid id)
        {
            var faq = await dbContext.FAQs.FindAsync(id);
            if (faq == null) return null;

            faq.isActive = true;
            faq.UpdatedDate = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();
            return faq;
        }


        public async Task<FAQsCountViewModel> GetFAQsCount()
        {
            var activeCount = await dbContext.FAQs.CountAsync(p => p.isActive == true);
            var inactiveCount = await dbContext.FAQs.CountAsync(p => p.isActive == false);
            var totalCount = await dbContext.FAQs.CountAsync();

            return new FAQsCountViewModel
            {
                Active = activeCount,
                Inactive = inactiveCount,
                Total = totalCount
            };
        }

    }
}
