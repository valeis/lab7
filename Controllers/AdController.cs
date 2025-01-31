using lab7.Models;
using lab7.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace lab7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdController : ControllerBase
    {
        private readonly IAdService _adService;
        private readonly ILogger<AdController> _logger; 

        public AdController(IAdService adService, ILogger<AdController> logger)
        {
            _adService = adService;
            _logger = logger; 
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ad>>> GetAds()
        {
            _logger.LogInformation("Fetching all ads.");
            var ads = await _adService.GetAllAdsAsync();

            if (ads == null || ads.Count == 0)
            {
                _logger.LogWarning("No ads found.");
                return NotFound("No ads found");
            }

            _logger.LogInformation("Successfully fetched {AdCount} ads.", ads.Count);
            return Ok(ads);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Ad>> GetAd(int id)
        {
            _logger.LogInformation("Fetching ad with ID {AdId}.", id);
            var ad = await _adService.GetAdByIdAsync(id);

            if (ad == null)
            {
                _logger.LogWarning("Ad with ID {AdId} not found.", id);
                return NotFound();
            }

            _logger.LogInformation("Successfully fetched ad with ID {AdId}.", id);
            return Ok(ad);
        }

        [HttpPost]
        public async Task<ActionResult<Ad>> PostAd(Ad ad)
        {
            if (ad == null)
            {
                _logger.LogWarning("Received invalid ad data.");
                return BadRequest("Ad is null.");
            }

            _logger.LogInformation("Creating a new ad.");
            var createdAd = await _adService.CreateAdAsync(ad);

            if (createdAd == null)
            {
                _logger.LogError("Failed to create ad.");
                return Problem("Failed to create ad.");
            }

            _logger.LogInformation("Successfully created ad with ID {AdId}.", createdAd.Id);
            return CreatedAtAction(nameof(GetAd), new { id = createdAd.Id }, createdAd);
        }
    }
}
