using Crud.Contracts;
using Crud.Models.Auth;
using Crud.Service;
using Crud.ViewModel.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Crud.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IEmailService emailService, IConfiguration config)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _config = config;
        }

        public async Task<(bool Succeeded, IEnumerable<string> Errors)> RegisterUserAsync(RegisterViewModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) return (false, result.Errors.Select(e => e.Description));

            if (!await _roleManager.RoleExistsAsync("User"))
                await _roleManager.CreateAsync(new IdentityRole("User"));

            await _userManager.AddToRoleAsync(user, "User");

            return (true, Array.Empty<string>());
        }

        public async Task<(bool Succeeded, IEnumerable<string> Errors)> RegisterAdminAsync(RegisterViewModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) return (false, result.Errors.Select(e => e.Description));

            if (!await _roleManager.RoleExistsAsync("Admin"))
                await _roleManager.CreateAsync(new IdentityRole("Admin"));

            await _userManager.AddToRoleAsync(user, "Admin");

            return (true, Array.Empty<string>());
        }

        public async Task<(string Token, string Email, string Role, string FirstName, string LastName)?> LoginAsync(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return null;

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "";

            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return (jwt, user.Email, role, user.FirstName, user.LastName);
        }

        public async Task<bool> ForgotPasswordAsync(ForgotPasswordViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return true; // Don't reveal if user exists

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var frontendUrl = "http://localhost:5173/reset-password";
            var encodedToken = System.Web.HttpUtility.UrlEncode(token);
            var resetLink = $"{frontendUrl}?token={encodedToken}&email={user.Email}";

            var body = Constants.GetResetPasswordEmailBody(resetLink, user.FirstName ?? user.Email);

            await _emailService.SendEmailAsync(user.Email, Constants.Subject.ResetPassword, body);

            return true;
        }

        public async Task<(bool Succeeded, IEnumerable<string> Errors)> ResetPasswordAsync(ResetPasswordViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return (false, new[] { "Invalid email" });

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

            return (result.Succeeded, result.Errors.Select(e => e.Description));
        }
    }
}
