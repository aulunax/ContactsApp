using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContactsApp.Server.Data;
using ContactsApp.Server.Models;
using ContactsApp.Server.Services;
using ContactsApp.Server.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ContactsApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly IContactService _contactService;    

        public UsersController(IUserService userService, IContactService contactService)
        {
            _userService = userService;
            _contactService = contactService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers()
        {
            return await _userService.GetAllUsersAsync();
        }

        // GET: api/Users/{userId}
        [HttpGet("{userId}")]
        public async Task<ActionResult<List<BasicContactDto>>> GetUserContactList(int userId)
        {
            var userExists = await _userService.UserExistsAsync(userId);

            if (userExists == false)
            {
                return NotFound();
            }

            var contactList = await _contactService.GetAllBasicContactsForUserIdAsync(userId);

            return contactList;
        }


        // GET api/Users/me
        [Authorize]
        [HttpGet("me")]
        async public Task<ActionResult<UserDto>> UserMe()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var user = await _userService.GetUserByIdAsync(int.Parse(userId));

            if (user == null)
                return Unauthorized();

            return Ok(user);
        }
    }
}
