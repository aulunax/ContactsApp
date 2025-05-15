using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactsApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        //[HttpPost]
        //public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        //{
        //    var result = await _loginService.LoginAsync(dto);
        //    if (result == null)
        //        return Unauthorized("Invalid credentials");

        //    return Ok(result);
        //}
    }
}
