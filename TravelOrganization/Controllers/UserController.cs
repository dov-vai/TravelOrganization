using System.Threading.Tasks;
using TravelOrganization.Data.Models.Account;
using TravelOrganization.Data.Repositories.Account;
using TravelOrganization.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TravelOrganization.Controllers.Account
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;

        public UserController(IUserRepository userRepository, IAuthService authService, IEmailService emailService)
        {
            _userRepository = userRepository;
            _authService = authService;
            _emailService = emailService;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            if (!int.TryParse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value, out int userId))
                return Unauthorized();

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound();

            return Ok(new
            {
                user.Id,
                user.Email,
                user.Name,
                user.Surname,
                user.BirthDate,
                user.RegistrationDate,
                user.ProfilePictureLink,
                Role = user.Role.Name
            });
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileModel model)
        {
            if (!int.TryParse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value, out int userId))
                return Unauthorized();

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound();

            user.Email = model.Email;
            user.Name = model.Name;
            user.Surname = model.Surname;
            user.Password = model.Password;

            await _userRepository.UpdateUserAsync(user);
            return Ok(new { message = "Profile updated successfully" });
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(model.Email);
            if (existingUser != null)
                return BadRequest(new { message = "Email already exists" });

            var user = new User
            {
                Email = model.Email,
                Name = model.Name,
                Surname = model.Surname,
                RegistrationDate = DateTime.UtcNow,
                RoleId = 1, // Client Role
                ProfilePictureLink = "default.jpg",
                EmailConfirmed = false,
                ConfirmationToken = Guid.NewGuid().ToString(),
                TokenExpiration = DateTime.UtcNow.AddHours(24) // Token valid for 24 hours
            };

            user.Password = _authService is AuthService authService
                ? authService.HashPassword(user, model.Password)
                : throw new Exception("AuthService not properly configured");

            await _userRepository.AddUserAsync(user);

            var confirmationLink = Url.Action(
                "ConfirmEmail",
                "User",
                new { email = user.Email, token = user.ConfirmationToken },
                Request.Scheme);

            var emailSubject = "Confirm your email";
            var emailBody = $"<p>Please confirm your email by clicking <a href='{confirmationLink}'>here</a>.</p>";
            await _emailService.SendEmailAsync(user.Email, emailSubject, emailBody);

            return Ok(new { message = "User registered successfully. Please check your email to confirm your account." });
        }


        [HttpGet("confirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
                return BadRequest(new { message = "Invalid email" });

            if (user.ConfirmationToken != token)
                return BadRequest(new { message = "Invalid token" });

            if (user.TokenExpiration < DateTime.UtcNow)
                return BadRequest(new { message = "Token expired. Please request a new confirmation email." });

            user.EmailConfirmed = true;
            user.ConfirmationToken = null; // Clear the token after confirmation
            user.TokenExpiration = null;
            await _userRepository.UpdateUserAsync(user);

            return Ok(new { message = "Email confirmed successfully" });
        }




        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAccount()
        {
            if (!int.TryParse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value, out int userId))
                return Unauthorized();

            await _userRepository.DeleteUserAsync(userId);
            await _authService.LogoutAsync(HttpContext);
            return Ok(new { message = "Account deleted successfully" });
        }

        [HttpPost("uploadProfilePicture")]
        public async Task<IActionResult> UploadProfilePicture([FromForm] IFormFile file)
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound();

            if (file != null && file.Length > 0)
            {
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                var fileBytes = ms.ToArray();
                var base64 = Convert.ToBase64String(fileBytes);
                user.ProfilePictureLink = $"data:{file.ContentType};base64,{base64}";
                await _userRepository.UpdateUserAsync(user);
                return Ok(new { message = "Profile picture updated successfully" });
            }

            return BadRequest(new { message = "No file uploaded" });
        }

    }

    public class UpdateProfileModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
    }

    public class RegisterModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
