using lab7.DataAccess;
using lab7.Models;

namespace lab7.Services
{
    public class AdService(IAdRepository repository) : IAdService
    {
        public readonly IAdRepository Repository = repository;

        public async Task<List<Ad>> GetAllAdsAsync()
        {
            return await Repository.GetAllAdsAsync();
        }

        public async Task<Ad> GetAdByIdAsync(int id)
        {
            return await Repository.GetAdByIdAsync(id);
        }
        public async Task<Ad> CreateAdAsync(Ad ad)
        {
            ad.CreatedAt = DateTime.UtcNow;
            ad.UpdatedAt = DateTime.UtcNow;

            return await Repository.AddAdAsync(ad);
        }
    }
}
