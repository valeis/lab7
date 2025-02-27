﻿using lab7.DataAccess;
using lab7.Models;

namespace lab7.Services
{
    public class UserService(IUserRepository repository) : IUserService
    {
        public readonly IUserRepository Repository = repository;

        public List<User> GetUsers()
        {
            return Repository.GetUsers();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await Repository.GetUserByIdAsync(id);
        }

        public async Task<User> AddUserAsync(User user)
        {
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;
            return await Repository.AddUserAsync(user);
        }

        //public async Task<bool> UpdateUserAsync(int id, User user)
        //{
        //    if (id != user.Id)
        //    {
        //        return false;
        //    }

        //    user.UpdatedAt = DateTime.UtcNow;
        //    return await Repository.UpdateUserAsync(user);
        //}

        public async Task<(bool Success, User? UpdatedUser, Ad? AdToShow)> UpdateUserAsync(int id, User user)
        {
            if (id != user.Id)
            {
                return (false, null, null);
            }

            user.UpdatedAt = DateTime.UtcNow;
            bool isUpdated = await Repository.UpdateUserAsync(user);

            if (isUpdated && user.TrackCount > 3 && !user.IsPremium)
            {
                Ad? ad = await Repository.GetRandomAdAsync();
                user.TrackCount = 0;
                return (true, user, ad);
            }

            return (isUpdated, user, null);
        }


        public async Task<bool> SoftDeleteUserAsync(int id)
        {
            return await Repository.SoftDeleteUserAsync(id);
        }

        public async Task<bool> UpdateUserPremiumStatusAsync(int id, bool isPremium)
        {
            return await Repository.UpdateUserPremiumStatusAsync(id, isPremium);
        }

    }
}
