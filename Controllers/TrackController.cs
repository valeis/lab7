using AutoMapper;
using lab7.Models;
using lab7.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace lab7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITrackService _trackService;

        public TrackController(IMapper mapper, ITrackService trackService)
        {
            _mapper = mapper;
            _trackService = trackService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Track>>> GetTracks()
        {
            var tracks = await _trackService.GetAllTracksAsync();
            if (tracks == null || !tracks.Any())
            {
                return NotFound("No tracks found");
            }

            return Ok(tracks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Track>> GetTrack(int id)
        {
            var track = await _trackService.GetTrackByIdAsync(id);
            if (track == null)
            {
                return NotFound("Track not found");
            }
            return Ok(track);
        }

        [HttpPost]
        public async Task<ActionResult<Track>> PostTrack(Track track)
        {
            if (track == null)
            {
                return BadRequest("Track data is invalid.");
            }

            var createdTrack = await _trackService.CreateTrackAsync(track);
            if (createdTrack == null)
            {
                return Problem("Error occurred while creating the track.");
            }

            return CreatedAtAction(nameof(GetTrack), new { id = createdTrack.Id }, createdTrack);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrack(int id, Track track)
        {
            if (id != track.Id)
            {
                return BadRequest("Track ID mismatch.");
            }

            var updatedTrack = await _trackService.UpdateTrackAsync(id, track);

            if (updatedTrack == null)
            {
                return NotFound("Track not found.");
            }

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrack(int id)
        {
            var result = await _trackService.DeleteTrackAsync(id);

            if (result == null)
            {
                return NotFound("Track not found.");
            }

            return NoContent();
        }
    }
}
