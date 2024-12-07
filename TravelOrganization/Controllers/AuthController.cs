using System.Threading.Tasks;
using TravelOrganization.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TravelOrganization.Data.Models.Account;
using TravelOrganization.Data.Repositories.Account;
using TravelOrganization.Data;

namespace TravelOrganization.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly DataContext _context;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthController(
            IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher,
            DataContext context,
            AuthenticationStateProvider authenticationStateProvider,
            IAuthService authService,
            ILogger<AuthController> logger)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _context = context;
            _authenticationStateProvider = authenticationStateProvider;
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel model)
        {
            try
            {
                var user = await _userRepository.GetUserByEmailAsync(model.Email);
                if (user == null)
                {
                    return Unauthorized(new { message = "User not found" });
                }

                if (!user.EmailConfirmed)
                {
                    return Unauthorized(new { message = "Email not confirmed. Please confirm your email to log in." });
                }

                var result = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);
                if (result != PasswordVerificationResult.Success)
                {
                    return Unauthorized(new { message = "Invalid credentials" });
                }

                var role = await _context.GetRoleByIdAsync(user.RoleId);
                if (role == null)
                {
                    return StatusCode(500, new { message = $"Role with ID {user.RoleId} not found." });
                }
                user.Role = role;

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, $"{user.Name} {user.Surname}"),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.Name)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties
                );

                if (_authenticationStateProvider is CustomAuthenticationStateProvider customAuthProvider)
                {
                    customAuthProvider.NotifyUserAuthentication(new ClaimsPrincipal(claimsIdentity));
                }

                return Ok(new { message = "Login successful" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login.");
                return StatusCode(500, new { message = "An error occurred during login. Please try again later." });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync(HttpContext);
            return Ok(new { message = "Logged out successfully" });
        }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
