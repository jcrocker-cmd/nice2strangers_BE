using Crud.Contracts;
using Crud.Data;
using Crud.Models;
using Crud.Models.Entities;
using Crud.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using static Crud.Service.InquiryService;

namespace Crud.Service
{

    public class InquiryService : IInquiryService
    {

        private readonly IEmailService _emailService;
        private readonly ApplicationDBContext dbContext;

        public InquiryService(IEmailService emailService, ApplicationDBContext dbContext)
        {
            _emailService = emailService;
            this.dbContext = dbContext;
        }

        public async Task<string> SendEmailAsync(EmailRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ToEmail))
                throw new ArgumentException("ToEmail is required.");

            await _emailService.SendEmailAsync(request.ToEmail, request.Subject, request.Body);
            return "Email Sent";
        }

        public async Task<ContactUs> ContactUsAsync(EmailRequest request)
        {
            var subject = request.Subject;
            var message = request.Body;
            var name = request.Name;
            var email = request.ToEmail;

            var body = Constants.ContactUsClient(name);
            var adminBody = Constants.ContactUs(name, message, email);

            if (!string.IsNullOrEmpty(email))
            {
                try
                {
                    await _emailService.SendEmailAsync(email, subject, body);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Client email failed: {ex.Message}");
                }
            }

            await _emailService.SendEmailAsync(Constants.AdminEmail, $"Admin Notification - {subject}", adminBody);

            var contactus = new ContactUs
            {
                Name = request.Name,
                Subject = request.Subject,
                Email = request.ToEmail,
                Message = request.Body,
                CreatedDate = DateTime.UtcNow
            };

            dbContext.ContactUs.Add(contactus);
            await dbContext.SaveChangesAsync();

            return contactus;
        }

        public async Task<List<EmailRequest>> GetContactusAsync()
        {
            return await dbContext.ContactUs
                .AsNoTracking()
                .Select(c => new EmailRequest
                {
                    Id = c.Id,
                    Name = c.Name,
                    ToEmail = c.Email,
                    Subject = c.Subject,
                    Body = c.Message,
                    IsReplied = c.IsReplied,
                    CreatedDate = c.CreatedDate.ToString("MMM, dd, yyyy h:mmtt", CultureInfo.InvariantCulture)
                })
                .ToListAsync();
        }

        public async Task<string> SendReplyAsync(SendReplyRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Subject) ||
                string.IsNullOrWhiteSpace(request.Body))
            {
                throw new ArgumentException("Email, subject, and body are required.");
            }

            try
            {
                await _emailService.SendEmailAsync(request.Email, request.Subject, request.Body);

                var contact = await dbContext.ContactUs.FirstOrDefaultAsync(c => c.Id == request.Id);
                if (contact != null)
                {
                    contact.IsReplied = true;
                    await dbContext.SaveChangesAsync();
                }

                return "Reply sent and marked as replied!";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send to {request.Email}: {ex.Message}");
                throw new InvalidOperationException("Failed to send reply.", ex);
            }
        }

    }
}
