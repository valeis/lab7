using lab7.Models;
using lab7.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace lab7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        private readonly IDistributedCache _cache;

        public UserController(IUserService userService, ILogger<UserController> logger, IDistributedCache cache)
        {
            _userService = userService;
            _logger = logger;
            _cache = cache;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            _logger.LogInformation("Fetching all users");

            var cachedUsers = await _cache.GetStringAsync("all_users");
            if (cachedUsers != null)
            {
                _logger.LogInformation("Users found in cache.");
                var users = JsonConvert.DeserializeObject<List<User>>(cachedUsers);
                return Ok(users);
            }

            var usersFromDb = _userService.GetUsers();
            if (usersFromDb == null || !usersFromDb.Any())
            {
                return NotFound("No users found");
            }

            await _cache.SetStringAsync("all_users", JsonConvert.SerializeObject(usersFromDb), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
            });

            _logger.LogInformation("Users fetched from database.");
            return Ok(usersFromDb);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            _logger.LogInformation("Fetching user with ID {UserId}", id);

            var cachedUser = await _cache.GetStringAsync($"user_{id}");
            if (cachedUser != null)
            {
                _logger.LogInformation("User found in cache with ID {UserId}", id);
                var user = JsonConvert.DeserializeObject<User>(cachedUser);
                return Ok(user);
            }

            var userFromDb = await _userService.GetUserByIdAsync(id);
            if (userFromDb == null)
            {
                _logger.LogWarning("User with ID {UserId} not found", id);
                return NotFound();
            }

            await _cache.SetStringAsync($"user_{id}", JsonConvert.SerializeObject(userFromDb), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });

            _logger.LogInformation("User fetched from database with ID {UserId}", id);
            return Ok(userFromDb);
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _logger.LogInformation("Creating a new user");
            var createdUser = await _userService.AddUserAsync(user);

            await _cache.RemoveAsync("all_users");

            _logger.LogInformation("User created with ID {UserId}", createdUser.Id);
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, [FromBody] User user)
        {
            _logger.LogInformation("Updating user with ID {UserId}", id);
            var (success, updatedUser, ad) = await _userService.UpdateUserAsync(id, user);

            if (!success || updatedUser == null)
            {
                _logger.LogWarning("Failed to update user with ID {UserId}", id);
                return NotFound();
            }

            await _cache.RemoveAsync($"user_{id}");
            await _cache.RemoveAsync("all_users");

            _logger.LogInformation("User with ID {UserId} updated successfully", id);
            return Ok(new { user = updatedUser, ad = ad });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteUser(int id)
        {
            _logger.LogInformation("Soft deleting user with ID {UserId}", id);
            bool success = await _userService.SoftDeleteUserAsync(id);

            if (!success)
            {
                _logger.LogWarning("User with ID {UserId} not found for deletion", id);
                return NotFound();
            }

            await _cache.RemoveAsync($"user_{id}");
            await _cache.RemoveAsync("all_users");

            _logger.LogInformation("User with ID {UserId} soft deleted successfully", id);
            return NoContent();
        }

        [HttpPut("{id}/premium")]
        public async Task<IActionResult> UpdateUserPremiumStatus(int id, [FromBody] bool isPremium)
        {
            _logger.LogInformation("Updating premium status for user with ID {UserId}", id);

            if (_userService == null)
            {
                _logger.LogError("User service is null. Cannot update premium status.");
                return NotFound("Service not found.");
            }

            var success = await _userService.UpdateUserPremiumStatusAsync(id, isPremium);
            if (!success)
            {
                _logger.LogWarning("User with ID {UserId} not found for premium status update", id);
                return NotFound("User not found.");
            }

            await _cache.RemoveAsync($"user_{id}");
            await _cache.RemoveAsync("all_users");

            _logger.LogInformation("Premium status for user with ID {UserId} updated successfully", id);
            return NoContent();
        }
    }
}
