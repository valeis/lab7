using lab7.Models;

namespace lab7.Services
{
    public interface ITrackService
    {
        Task<List<Track>> GetAllTracksAsync();
        Task<Track> GetTrackByIdAsync(int id);

        Task<Track> CreateTrackAsync(Track track);

        Task<Track> UpdateTrackAsync(int id, Track track);

        Task<Track> DeleteTrackAsync(int id);
    }
}
