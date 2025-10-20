//using Crud.Contracts;
//using Crud.ViewModel.Auth;
//using Microsoft.AspNetCore.Mvc;

//[ApiController]
//[Route("api/[controller]")]
//public class AuthController : ControllerBase
//{
//    private readonly IAuthService _authService;

//    public AuthController(IAuthService authService)
//    {
//        _authService = authService;
//    }

//    [HttpPost("register")]
//    public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
//    {
//        var (succeeded, errors) = await _authService.RegisterUserAsync(model);
//        if (!succeeded) return BadRequest(errors);
//        return Ok(new { message = "User registered successfully" });
//    }

//    [HttpPost("register-admin")]
//    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterViewModel model)
//    {
//        var (succeeded, errors) = await _authService.RegisterAdminAsync(model);
//        if (!succeeded) return BadRequest(errors);
//        return Ok(new { message = "Admin registered successfully" });
//    }

//    [HttpPost("login")]
//    public async Task<IActionResult> Login([FromBody] LoginViewModel model)
//    {
//        var result = await _authService.LoginAsync(model);
//        if (result == null) return Unauthorized(new { message = "Invalid email or password" });

//        return Ok(result);
//    }

//    [HttpPost("forgot-password")]
//    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel model)
//    {
//        await _authService.ForgotPasswordAsync(model);
//        return Ok(new { message = "Reset link sent if account exists." });
//    }

//    [HttpPost("reset-password")]
//    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel model)
//    {
//        var (succeeded, errors) = await _authService.ResetPasswordAsync(model);
//        if (!succeeded) return BadRequest(errors);
//        return Ok(new { message = "Password reset successfully" });
//    }
//}


using Crud.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Crud.Contracts;
using Crud.ViewModel.Auth;


namespace Crud.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthController(UserManager<ApplicationUser> userManager, IEmailService emailService, IConfiguration config, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _config = config;
            _roleManager = roleManager;
            _emailService = emailService;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);


            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(e => e.Description));
            }

            // Assign role
            if (!await _roleManager.RoleExistsAsync("User"))
            {
                await _roleManager.CreateAsync(new IdentityRole("User"));
            }
            await _userManager.AddToRoleAsync(user, "User");

            return Ok(new { message = "Account created successfully" });    
        }

        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterViewModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => e.Description));

            // Ensure Admin role exists
            if (!await _roleManager.RoleExistsAsync("Admin"))
                await _roleManager.CreateAsync(new IdentityRole("Admin"));

            // Assign Admin role
            await _userManager.AddToRoleAsync(user, "Admin");

            return Ok(new { message = "Admin account created successfully" });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized(new { message = "Invalid email or password" });

            if (!await _userManager.CheckPasswordAsync(user, model.Password))
                return Unauthorized(new { message = "Invalid email or password" });

            // get roles from Identity
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault(); // assuming single role

            // Generate JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
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

            return Ok(new
            {
                token = jwt,
                email = user.Email,
                role,
                firstName = user.FirstName,
                lastName = user.LastName,
                userId = user.Id
            });
        }


        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok(new { message = "Logged out successfully" });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Ok(new { message = "If an account exists with that email, a reset link has been sent." });

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var frontendUrl = "http://localhost:5173/reset-password";
            var encodedToken = System.Web.HttpUtility.UrlEncode(token);
            var resetLink = $"{frontendUrl}?token={encodedToken}&email={user.Email}";

            var body = Constants.GetResetPasswordEmailBody(resetLink, user.FirstName ?? user.Email);

            await _emailService.SendEmailAsync(user.Email, Constants.Subject.ResetPassword, body);
            return Ok(new { message = "Reset link sent to your email." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest(new { message = "Invalid email" });

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => e.Description));

            return Ok(new { message = "Password has been reset successfully" });
        }

        [HttpGet("google-login")]
        public IActionResult GoogleLogin()
        {
            var redirectUrl = Url.Action("GoogleCallback");
            //var redirectUrl = Url.Action("GoogleCallback", "Auth", null, Request.Scheme);
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return Challenge(properties, "Google");
        }

        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null) return BadRequest("Error loading external login information.");

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            var firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName);
            var lastName = info.Principal.FindFirstValue(ClaimTypes.Surname);

            var roleExists = await _roleManager.RoleExistsAsync("User");
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole("User"));
            }

            if (user == null)
            {
                user = new ApplicationUser
                {
                    Email = email,
                    UserName = email,
                    FirstName = firstName,
                    LastName = lastName,
                };
                await _userManager.CreateAsync(user);
                await _userManager.AddToRoleAsync(user, "User");
            }

            // generate JWT token same as your login method
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "User";

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, role),
            new Claim("firstName", user.FirstName ?? firstName),
            new Claim("lastName", user.LastName ?? lastName)
                }),
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            // redirect to frontend with token
            return Redirect($"http://localhost:5173/login?token={jwt}");
           // return Redirect($"https://nice2strangers.org/login?token={jwt}");
        }


    }


}
