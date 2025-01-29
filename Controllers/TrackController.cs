using AutoMapper;
using lab7.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace lab7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackController : ControllerBase
    {
        private readonly TrackContext _trackContext;
        private readonly IMapper _mapper;

        public TrackController(TrackContext trackContext, IMapper mapper)
        {
            _trackContext = trackContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Track>>> GetTracks()
        {
            if (_trackContext.Track == null)
            {
                return NotFound();
            }

            /*return await _trackContext.Track
                .AsNoTracking()
                .ToListAsync();*/

            var tracks = await _trackContext.Track
               .AsNoTracking()
               .ToListAsync();

            var trackDtos = _mapper.Map<List<TrackDTOModel>>(tracks);
            return Ok(trackDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Track>> GetTrack(int id)
        {
            if (_trackContext.Track is null)
            {
                return NotFound();
            }
            var track = await _trackContext.Track.FindAsync(id);
            if (track is null)
            {
                return NotFound();
            }
            var trackDto = _mapper.Map<TrackDTOModel>(track);
            return Ok(trackDto);
        }

        [HttpPost]
        public async Task<ActionResult<Track>> PostTrack(Track track)
        {
            if (_trackContext.Track == null)
            {
                return Problem("Entity set 'TrackContext.Track' is null.");
            }

            track.CreatedAt = DateTime.UtcNow;
            track.UpdatedAt = DateTime.UtcNow;

            _trackContext.Track.Add(track);
            await _trackContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTrack), new { id = track.Id }, track);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrack(int id, Track track)
        {
            if (id != track.Id)
            {
                return BadRequest();
            }

            track.UpdatedAt = DateTime.UtcNow;
            _trackContext.Entry(track).State = EntityState.Modified;
            _trackContext.Entry(track).Property(x => x.CreatedAt).IsModified = false;

            try
            {
                await _trackContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrackExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        private bool TrackExists(int id)
        {
            return (_trackContext.Track?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
