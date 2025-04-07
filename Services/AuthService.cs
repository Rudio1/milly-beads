using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MillyBeads.Models.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using MillyBeads.Models.ViewModels;
using MillyBeads.Models.Repositories;

namespace MillyBeads.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(
            IUserRepository userRepository,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AuthResponseViewModel> LoginAsync(LoginViewModel loginViewModel)
        {
            var user = await _userRepository.GetByEmailAsync(loginViewModel.Email);
            if (user == null || !VerifyPassword(loginViewModel.Password, user.PasswordHash, user.Salt))
            {
                throw new UnauthorizedAccessException("Email ou senha inválidos");
            }

            user.LastLogin = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);

            var token = GenerateJwtToken(user);
            await SignInAsync(user);

            return new AuthResponseViewModel
            {
                Token = token,
                Name = user.Name,
                Email = user.Email
            };
        }

        public async Task<AuthResponseViewModel> RegisterAsync(RegisterViewModel registerViewModel)
        {
            if (await UserExistsAsync(registerViewModel.Email))
            {
                throw new InvalidOperationException("Email já está em uso");
            }

            var (hash, salt) = HashPassword(registerViewModel.Password);

            var user = new User
            {
                Name = registerViewModel.Name,
                Email = registerViewModel.Email,
                PasswordHash = hash,
                Salt = salt,
                Role = "User",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _userRepository.AddAsync(user);

            var token = GenerateJwtToken(user);
            await SignInAsync(user);

            return new AuthResponseViewModel
            {
                Token = token,
                Name = user.Name,
                Email = user.Email
            };
        }

        public async Task<bool> ValidateUserAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                return false;
            }

            return VerifyPassword(password, user.PasswordHash, user.Salt);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _userRepository.ExistsAsync(email);
        }

        private async Task SignInAsync(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
            };

            await _httpContextAccessor.HttpContext!.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        public string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public (string hash, string salt) HashPassword(string password)
        {
            var salt = new byte[16];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            var saltString = Convert.ToBase64String(salt);

            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var saltedPassword = password + saltString;
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
            var hashString = Convert.ToBase64String(hashedBytes);

            return (hashString, saltString);
        }

        public bool VerifyPassword(string password, string storedHash, string salt)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var saltedPassword = password + salt;
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
            var hashString = Convert.ToBase64String(hashedBytes);

            return hashString == storedHash;
        }
    }
} 