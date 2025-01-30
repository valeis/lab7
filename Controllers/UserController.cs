using lab7.Models;
using lab7.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace lab7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return Ok(_userService.GetUsers());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user is null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            var createdUser = await _userService.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            var result = await _userService.UpdateUserAsync(id, user);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteUser(int id)
        {
            bool success = await _userService.SoftDeleteUserAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

       [HttpPut("{id}/premium")]
        public async Task<IActionResult> UpdateUserPremiumStatus(int id, [FromBody] bool isPremium)
        {
            if (_userService == null)
            {
                return NotFound("Service not found.");
            }

            var success = await _userService.UpdateUserPremiumStatusAsync(id, isPremium);
            if (!success)
            {
                return NotFound("User not found.");
            }

            return NoContent();
        }
    }
}
