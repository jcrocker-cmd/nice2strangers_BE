using Crud.Data;
using Crud.ViewModel;
using Crud.Contracts; 
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Crud.Models.Entities;

namespace Crud.Service
{
    public class NewsletterService : INewsletterService
    {
        public readonly ApplicationDBContext dbContext;

        public NewsletterService(ApplicationDBContext dbContext)
        {
          this.dbContext = dbContext;
        }
            public async Task<List<NewsletterViewModel>> GetNewsletterAsync()
            {
                return await dbContext.Newsletter
                .AsNoTracking()
                .Select(n => new NewsletterViewModel
                {
                    Id = n.Id, 
                    Name = n.Name,
                    Email = n.Email,
                    CreatedDate = n.CreatedDate.ToString("MMM, dd, yyyy h:mmtt", CultureInfo.InvariantCulture)
                })
                .ToListAsync();
            }

            public async Task<Newsletter> PostNewsletter(NewsletterViewModel newsletterViewModel)
            {
                var newsletter = new Newsletter
                {
                    Name = newsletterViewModel.Name,
                    Email = newsletterViewModel.Email,
                    CreatedDate = DateTime.UtcNow
                };

                dbContext.Newsletter.Add(newsletter);
                await dbContext.SaveChangesAsync();

                return newsletter;
            }

        }

}
