using lab7.Models;

namespace lab7.Services
{
    public interface IUserService
    {
        List<User> GetUsers();
        Task<User?> GetUserByIdAsync(int id);
        Task<User> AddUserAsync(User user);

        Task<bool> UpdateUserAsync(int id, User user);

        Task<bool> SoftDeleteUserAsync(int id);

        Task<bool> UpdateUserPremiumStatusAsync(int id, bool isPremium);
    }
}
