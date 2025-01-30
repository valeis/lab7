using lab7.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace lab7.DataAccess
{
    public class TrackRepository(TrackContext trackContext, ILogger<TrackRepository> logger) : ITrackRepository
    {
        private readonly TrackContext _trackContext = trackContext;
        private readonly ILogger<TrackRepository> _logger = logger;

        public async Task<List<Track>> GetAllTracksAsync()
        {
            if (_trackContext.Track == null)
            {
                return null;
            }

            return await _trackContext.Track
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Track> GetTrackByIdAsync(int id)
        {
            if (_trackContext.Track == null)
            {
                return null;
            }

            return await _trackContext.Track
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Track> AddTrackAsync(Track track)
        {
            if (_trackContext.Track == null)
            {
                return null;
            }

            _trackContext.Track.Add(track);
            await _trackContext.SaveChangesAsync();

            return track;
        }

        public async Task<Track> UpdateTrackAsync(int id, Track track)
        {
            var existingTrack = await _trackContext.Track.FindAsync(id);
            if (existingTrack == null)
            {
                return null;
            }

            existingTrack.Title = track.Title;
            existingTrack.Artist = track.Artist;
            existingTrack.Album = track.Album;
            existingTrack.Genre = track.Genre;
            existingTrack.UpdatedAt = track.UpdatedAt;

            _trackContext.Entry(existingTrack).Property(x => x.CreatedAt).IsModified = false;

            await _trackContext.SaveChangesAsync();

            return existingTrack;
        }

        public async Task DeleteTrackAsync(Track track)
        {
            _trackContext.Track.Remove(track);
            await _trackContext.SaveChangesAsync();
        }
    }
}
