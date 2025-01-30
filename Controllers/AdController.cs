using lab7.Models;
using lab7.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdController : ControllerBase
    {
        private readonly IAdService _adService;

        public AdController(IAdService adService)
        {
            _adService = adService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ad>>> GetAds()
        {
           var ads = await _adService.GetAllAdsAsync();
            if (ads == null || ads.Count == 0)
            {
                return NotFound("No ads found");
            }
            return Ok(ads);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetAd(int id)
        {
            var ad = await _adService.GetAdByIdAsync(id);
            if (ad == null)
            {
                return NotFound();
            }
            return Ok(ad);
        }

        [HttpPost]
        public async Task<ActionResult<Ad>> PostAd(Ad ad)
        {
            if (ad == null)
            {
                return BadRequest("Ad is null.");
            }

            var createdAd = await _adService.CreateAdAsync(ad);
            if (createdAd == null)
            {
                return Problem("Failed to create ad.");
            }

            return CreatedAtAction(nameof(GetAd), new { id = createdAd.Id }, createdAd);
        }
    }
}
