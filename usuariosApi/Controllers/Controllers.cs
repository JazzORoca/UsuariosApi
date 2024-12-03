using Microsoft.AspNetCore.Mvc;
using UserManagement.Data;
using UsuariosApi.Services;

namespace UsuariosApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController()
        {
            var connectionString = "Server=localhost;Database=UserManagement;User=root;Password=your_password;";
            _userService = new UserService(connectionString);
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            return user != null ? Ok(user) : NotFound("User not found");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }


        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User user, [FromQuery] string password)
        {
            try
            {
                await _userService.AddUserAsync(user, password);
                return CreatedAtAction(nameof(GetUserByEmail), new { email = user.Email }, user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("verify-password")]
        public async Task<IActionResult> VerifyPassword([FromQuery] string email, [FromQuery] string password)
        {
            var isValid = await _userService.VerifyPasswordAsync(email, password);
            return isValid ? Ok("Password is correct") : Unauthorized("Invalid credentials");
        }



        [HttpPut("{email}")]
        public async Task<IActionResult> UpdateUser(string email, [FromBody] User user)
        {
            if (email != user.Email)
            {
                return BadRequest("Email mismatch");
            }

            try
            {
                await _userService.UpdateUserAsync(user);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
