using MillyBeads.Models.Entities;

namespace MillyBeads.Models.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<bool> ExistsAsync(string email);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
    }
} 