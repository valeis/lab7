using AutoMapper;
using lab7.Models;
using lab7.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace lab7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITrackService _trackService;
        private readonly ILogger<TrackController> _logger; 

        public TrackController(IMapper mapper, ITrackService trackService, ILogger<TrackController> logger)
        {
            _mapper = mapper;
            _trackService = trackService;
            _logger = logger; 
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Track>>> GetTracks()
        {
            _logger.LogInformation("Fetching all tracks.");
            var tracks = await _trackService.GetAllTracksAsync();

            if (tracks == null || !tracks.Any())
            {
                _logger.LogWarning("No tracks found.");
                return NotFound("No tracks found");
            }

            _logger.LogInformation("Successfully fetched {TrackCount} tracks.", tracks.Count());
            return Ok(tracks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Track>> GetTrack(int id)
        {
            _logger.LogInformation("Fetching track with ID {TrackId}.", id);
            var track = await _trackService.GetTrackByIdAsync(id);

            if (track == null)
            {
                _logger.LogWarning("Track with ID {TrackId} not found.", id);
                return NotFound("Track not found");
            }

            _logger.LogInformation("Successfully fetched track with ID {TrackId}.", id);
            return Ok(track);
        }

        [HttpPost]
        public async Task<ActionResult<Track>> PostTrack(Track track)
        {
            if (track == null)
            {
                _logger.LogWarning("Received invalid track data.");
                return BadRequest("Track data is invalid.");
            }

            _logger.LogInformation("Creating a new track.");
            var createdTrack = await _trackService.CreateTrackAsync(track);

            if (createdTrack == null)
            {
                _logger.LogError("Error occurred while creating the track.");
                return Problem("Error occurred while creating the track.");
            }

            _logger.LogInformation("Successfully created track with ID {TrackId}.", createdTrack.Id);
            return CreatedAtAction(nameof(GetTrack), new { id = createdTrack.Id }, createdTrack);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrack(int id, Track track)
        {
            if (id != track.Id)
            {
                _logger.LogWarning("Track ID mismatch: received {ReceivedId}, expected {ExpectedId}.", track.Id, id);
                return BadRequest("Track ID mismatch.");
            }

            _logger.LogInformation("Updating track with ID {TrackId}.", id);
            var updatedTrack = await _trackService.UpdateTrackAsync(id, track);

            if (updatedTrack == null)
            {
                _logger.LogWarning("Track with ID {TrackId} not found for update.", id);
                return NotFound("Track not found.");
            }

            _logger.LogInformation("Successfully updated track with ID {TrackId}.", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrack(int id)
        {
            _logger.LogInformation("Deleting track with ID {TrackId}.", id);
            var result = await _trackService.DeleteTrackAsync(id);

            if (result == null)
            {
                _logger.LogWarning("Track with ID {TrackId} not found for deletion.", id);
                return NotFound("Track not found.");
            }

            _logger.LogInformation("Successfully deleted track with ID {TrackId}.", id);
            return NoContent();
        }
    }
}
