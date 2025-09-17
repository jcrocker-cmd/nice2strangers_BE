using Crud.Models;
using Crud.Models.Entities;
using Crud.ViewModel;

namespace Crud.Contracts
{
    public interface IInquiryService
    {
        Task<string> SendEmailAsync(EmailRequest request);
        Task<ContactUs> ContactUsAsync(EmailRequest request);
        Task<List<EmailRequest>> GetContactusAsync();
        Task<string> SendReplyAsync(SendReplyRequest request);
    }
}
