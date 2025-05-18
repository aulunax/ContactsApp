using ContactsApp.Server.DTOs;
using ContactsApp.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

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
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            var jwtToken = await _authService.LoginAsync(loginDto);

            if (jwtToken == null)
                return Unauthorized("Invalid credentials");

            Response.Cookies.Append("accessToken", jwtToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(30)
            });

            return Ok();
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

        // POST api/auth/logout
        [HttpPost("logout")]
        public ActionResult Logout()
        {
            Response.Cookies.Delete("accessToken");
            return Ok();
        }

    }
}
