using lab7.Models;
using Microsoft.EntityFrameworkCore;

namespace lab7.DataAccess
{
    public class AdRepository : IAdRepository
    {
        private readonly TrackContext _context;

        public AdRepository(TrackContext context)
        {
            _context = context;
        }

        public async Task<List<Ad>> GetAllAdsAsync()
        {
            if (_context.Ad == null)
            {
                return null;
            }
            return await _context.Ad.AsNoTracking().ToListAsync();
        }

        public async Task<Ad> GetAdByIdAsync(int id)
        {
            return await _context.Ad.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Ad> AddAdAsync(Ad ad)
        {
            if (_context.Ad == null)
            {
                return null;
            }
            _context.Ad.Add(ad);
            await _context.SaveChangesAsync();
            return ad;
        }
    }
}
