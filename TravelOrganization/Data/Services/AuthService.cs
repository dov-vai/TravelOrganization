using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TravelOrganization.Data.Models.Account;
using TravelOrganization.Data.Models.Enumerators;
using TravelOrganization.Data.Repositories.Account;
using TravelOrganization.Data.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using TravelOrganization.Components.Pages.Payment;

namespace TravelOrganization.Data.Services
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(string email, string password, HttpContext httpContext);
        Task LogoutAsync(HttpContext httpContext);
        string HashPassword(User user, string password);
    }

    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly DataContext _context;

        public AuthService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher, AuthenticationStateProvider authenticationStateProvider, DataContext context)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _authenticationStateProvider = authenticationStateProvider;
            _context = context;
        }


        // Hashes the user's password
        public string HashPassword(User user, string password)
        {
            return _passwordHasher.HashPassword(user, password);
        }

        // Handles user login
        public async Task<bool> LoginAsync(string email, string password, HttpContext httpContext)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            if (result != PasswordVerificationResult.Success)
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var role = await _context.GetRoleByIdAsync(user.RoleId);
            if (role == null)
            {
                throw new InvalidOperationException($"Role with ID {user.RoleId} not found.");
            }
            user.Role = role;

            if (string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Surname) || string.IsNullOrEmpty(user.Role?.Name))
            {
                throw new InvalidOperationException("User details are incomplete. Cannot create claims.");
            }

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

            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
            );

            if (_authenticationStateProvider is CustomAuthenticationStateProvider customAuthProvider)
            {
                customAuthProvider.NotifyUserAuthentication(new ClaimsPrincipal(claimsIdentity));
            }

            return true;
        }


        // Handles user logout
        public async Task LogoutAsync(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (_authenticationStateProvider is CustomAuthenticationStateProvider customAuthProvider)
            {
                customAuthProvider.NotifyUserLogout();
            }
        }
    }
}
