using lab7.Models;

namespace lab7.Services
{
    public interface IAdService
    {
        Task<List<Ad>> GetAllAdsAsync();
        Task<Ad> GetAdByIdAsync(int id);

        Task<Ad> CreateAdAsync(Ad ad);
    }
}
