using lab7.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab7.DataAccess
{
    public class UserRepository(TrackContext trackContext, ILogger<UserRepository> logger) : IUserRepository
    {
        private readonly TrackContext _trackContext = trackContext;
        private readonly ILogger<UserRepository> _logger = logger;

        public List<User> GetUsers()
        {
            return _trackContext.User
                .Where(u => !u.IsDeleted) 
                .ToList(); 
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _trackContext.User.FindAsync(id);
        }

        public async Task<User> AddUserAsync(User user)
        {
            await _trackContext.User.AddAsync(user);
            await _trackContext.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            _trackContext.Entry(user).State = EntityState.Modified;
            _trackContext.Entry(user).Property(x => x.CreatedAt).IsModified = false;

            try
            {
                await _trackContext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> SoftDeleteUserAsync(int id)
        {
            var user = await _trackContext.User.FindAsync(id);
            if (user == null) return false;

            user.IsDeleted = true;
            await _trackContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateUserPremiumStatusAsync(int id, bool isPremium)
        {
            var user = await _trackContext.User.FindAsync(id);
            if (user == null) return false;

            user.IsPremium = isPremium;
            user.UpdatedAt = DateTime.UtcNow;

            _trackContext.Entry(user).Property(x => x.IsPremium).IsModified = true;
            _trackContext.Entry(user).Property(x => x.UpdatedAt).IsModified = true;

            await _trackContext.SaveChangesAsync();
            return true;
        }

        public async Task<Ad?> GetRandomAdAsync()
        {
            return await _trackContext.Ad.OrderBy(r => Guid.NewGuid()).FirstOrDefaultAsync();
        }
    }
}
