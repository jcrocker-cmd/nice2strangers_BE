using Crud.Models.Entities;
using Crud.ViewModel;

namespace Crud.Contracts
{
    public interface INewsletterService
    {
        Task<List<NewsletterViewModel>> GetNewsletterAsync();
        Task<Newsletter> PostNewsletter(NewsletterViewModel newsletterViewModel);
    }
}
