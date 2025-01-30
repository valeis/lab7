using lab7.Models;

namespace lab7.DataAccess
{
    public interface IUserRepository
    {
        List<User> GetUsers();

        Task<User?> GetUserByIdAsync(int id);

        Task<User> AddUserAsync(User user);

        Task<bool> UpdateUserAsync(User user);

        Task<bool> SoftDeleteUserAsync(int id);

        Task<bool> UpdateUserPremiumStatusAsync(int id, bool isPremium);

    }
}
