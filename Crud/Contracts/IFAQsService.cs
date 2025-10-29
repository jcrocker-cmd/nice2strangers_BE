using Crud.Models.Entities;
using Crud.Models;
using Crud.ViewModel;

namespace Crud.Contracts
{
    public interface IFAQsService
    {
        Task<List<FAQsViewModel>> GetAllFAQs();
        Task<FAQsModel> AddFAQs(FAQsViewModel faqsViewModel);
        Task<FAQsModel?> UpdateFAQ(Guid id, FAQsViewModel faqsViewModel);
        Task<FAQsModel?> SoftDeleteFAQ(Guid id);
        Task<FAQsModel?> RecoverFAQ(Guid id);
        Task<FAQsCountViewModel> GetFAQsCount();

    }
}
