using ContactsApp.Server.DTOs;
using ContactsApp.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactsApp.Server.Controllers
{

    /// <summary>
    /// Controller for handling authentication-related actions.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        // POST api/auth/login
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginDto loginDto)
        {
            var jwtToken = await _authService.LoginAsync(loginDto);

            if (jwtToken == null)
                return Unauthorized("Invalid credentials");

            return Ok(jwtToken);
        }

        // POST api/auth/register
        [HttpPost("register")]
        public async Task<ActionResult<AuthResultDto>> Register([FromBody] RegisterDto registerDto)
        {
            var result = await _authService.RegisterAsync(registerDto);


            var authResult = new AuthResultDto
            {
                Succeeded = result.Succeeded,
                Errors = result.Errors.Select(e => e.Description).ToList()
            };

            if (result.Succeeded)
                return Ok(authResult);

            return BadRequest(authResult);
        }
    }
}
