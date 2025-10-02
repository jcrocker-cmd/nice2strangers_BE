using Crud.Contracts; 
using Crud.Data;
using Crud.Models.Entities;
using Crud.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Crud.Service
{
    public class NewsletterService : INewsletterService
    {
        private readonly IEmailService _emailService;
        public readonly ApplicationDBContext dbContext;

        public NewsletterService(ApplicationDBContext dbContext, IEmailService _emailService)
        {
          this.dbContext = dbContext;
          this._emailService = _emailService;
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
                    Id = Guid.NewGuid(),
                    Name = newsletterViewModel.Name,
                    Email = newsletterViewModel.Email,
                    CreatedDate = DateTime.UtcNow
                };

                dbContext.Newsletter.Add(newsletter);
                await dbContext.SaveChangesAsync();

                return newsletter;
            }

        public async Task<bool> SendNewsletterAsync(SendNewsletterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Subject) || string.IsNullOrWhiteSpace(request.Body))
                return false;

            var subscribers = await dbContext.Newsletter.ToListAsync();

            foreach (var subscriber in subscribers)
            {
                var personalizedBody = request.Body.Replace("{{Name}}", subscriber.Name ?? "Subscriber");

                try
                {
                    await _emailService.SendEmailAsync(
                        subscriber.Email,
                        request.Subject,
                        personalizedBody
                    );
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send to {subscriber.Email}: {ex.Message}");
                    continue;
                }
            }

            return true;
        }

    }

}
