using lab7.Models;

namespace lab7.DataAccess
{
    public interface IAdRepository
    {
        Task<List<Ad>> GetAllAdsAsync();
        Task<Ad> GetAdByIdAsync(int id);

        Task<Ad> AddAdAsync(Ad ad);
    }
}
