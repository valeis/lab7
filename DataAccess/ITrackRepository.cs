using lab7.Models;

namespace lab7.DataAccess
{
    public interface ITrackRepository
    {
        Task<List<Track>> GetAllTracksAsync();
        Task<Track> GetTrackByIdAsync(int id);
        Task<Track> AddTrackAsync(Track track);
        Task<Track> UpdateTrackAsync(int id, Track track);
        Task DeleteTrackAsync(Track track);
    }
}
