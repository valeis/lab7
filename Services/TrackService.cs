using lab7.DataAccess;
using lab7.Models;

namespace lab7.Services
{
    public class TrackService : ITrackService
    {
        private readonly ITrackRepository _trackRepository;

        public TrackService(ITrackRepository trackRepository)
        {
            _trackRepository = trackRepository;
        }

        public async Task<List<Track>> GetAllTracksAsync()
        {
            return await _trackRepository.GetAllTracksAsync();
        }
        public async Task<Track> GetTrackByIdAsync(int id)
        {
            return await _trackRepository.GetTrackByIdAsync(id);
        }

        public async Task<Track> CreateTrackAsync(Track track)
        {
            track.CreatedAt = DateTime.UtcNow;
            track.UpdatedAt = DateTime.UtcNow;
            return await _trackRepository.AddTrackAsync(track);
        }

        public async Task<Track> UpdateTrackAsync(int id, Track track)
        {
            track.UpdatedAt = DateTime.UtcNow;

            return await _trackRepository.UpdateTrackAsync(id, track);
        }

        public async Task<Track> DeleteTrackAsync(int id)
        {
            var track = await _trackRepository.GetTrackByIdAsync(id);

            if (track == null)
            {
                return null;
            }

            await _trackRepository.DeleteTrackAsync(track);
            return track;
        }
    }
}
