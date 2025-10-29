using Crud.ViewModel;

namespace Crud.Contracts
{
    public interface ISocialLinksService
    {
        Task<SocialLinksViewModel> GetSocialLinksAsync();
        Task<SocialLinksViewModel> UpdateSocialLinksAsync(SocialLinksViewModel updatedLinks);
    }
}
