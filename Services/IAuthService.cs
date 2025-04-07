using MillyBeads.Models.ViewModels;
using MillyBeads.Models.Entities;

namespace MillyBeads.Services
{
    public interface IAuthService
    {
        Task<AuthResponseViewModel> LoginAsync(LoginViewModel loginViewModel);
        Task<AuthResponseViewModel> RegisterAsync(RegisterViewModel registerViewModel);
        Task<bool> ValidateUserAsync(string email, string password);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> UserExistsAsync(string email);
        string GenerateJwtToken(User user);
        (string hash, string salt) HashPassword(string password);
        bool VerifyPassword(string password, string hash, string salt);
    }
} 