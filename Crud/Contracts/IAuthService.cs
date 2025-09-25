using Crud.ViewModel.Auth;

namespace Crud.Contracts
{
    public interface IAuthService
    {
        Task<(bool Succeeded, IEnumerable<string> Errors)> RegisterUserAsync(RegisterViewModel model);
        Task<(bool Succeeded, IEnumerable<string> Errors)> RegisterAdminAsync(RegisterViewModel model);
        Task<(string Token, string Email, string Role, string FirstName, string LastName)?> LoginAsync(LoginViewModel model);
        Task<bool> ForgotPasswordAsync(ForgotPasswordViewModel model);
        Task<(bool Succeeded, IEnumerable<string> Errors)> ResetPasswordAsync(ResetPasswordViewModel model);
    }
}
