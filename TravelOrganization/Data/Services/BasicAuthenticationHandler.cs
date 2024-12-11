using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TravelOrganization.Data.Repositories.Account;
using Microsoft.AspNetCore.Identity;
using TravelOrganization.Data.Models.Account;
using System.Net.Http.Headers;

namespace TravelOrganization.Data.Services
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher)
            : base(options, logger, encoder, clock)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing Authorization Header");

            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                if (!"Basic".Equals(authHeader.Scheme, StringComparison.OrdinalIgnoreCase))
                    return AuthenticateResult.Fail("Invalid Authorization Scheme");

                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);
                if (credentials.Length != 2)
                    return AuthenticateResult.Fail("Invalid Authorization Header");

                var email = credentials[0];
                var password = credentials[1];

                var user = await _userRepository.GetUserByEmailAsync(email);
                if (user == null)
                    return AuthenticateResult.Fail("Invalid Username or Password");

                var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
                if (verificationResult != PasswordVerificationResult.Success)
                    return AuthenticateResult.Fail("Invalid Username or Password");

                if (!user.EmailConfirmed)
                    return AuthenticateResult.Fail("Email not confirmed");

                string roleName = user.RoleId switch
                {
                    1 => "Klientas",
                    2 => "Kelioniu organizatorius",
                    3 => "Administratorius"
                };

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, $"{user.Name} {user.Surname}"),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, roleName)
                };

                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }
        }
    }
}
