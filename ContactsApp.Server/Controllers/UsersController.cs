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
        /// <summary>
        /// Get a list of all contacts for a user by userId.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        // GET: api/Users/{userId}/{contactId}
        /// <summary>
        /// Get detailed contact info for a user by userId and contactId.
        /// </summary>
        /// <param name="userId">Integer representing Id of the User</param>
        /// <param name="contactId">Integer representing Id of the Contact</param>
        /// <returns>
        /// Returns a detailed contact info for a contact with contactId.
        /// </returns>
        [HttpGet("{userId}/{contactId}")]
        public async Task<ActionResult<ContactDto>> GetContactForUser(int userId, int contactId)
        {

            var contactExists = await _contactService.ContactExistsInUserAsync(contactId, userId);

            if (contactExists == false)
            {
                return NotFound();
            }

            var contact = await _contactService.GetContactById(contactId);

            if (contact == null)
            {
                return NotFound();
            }

            return contact;
        }

        //// PUT: api/Users/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutUser(int id, User user)
        //{
        //    if (id != user.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(user).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Users
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<User>> PostUser(User user)
        //{
        //    _context.Users.Add(user);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetUser", new { id = user.Id }, user);
        //}

        //// DELETE: api/Users/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUser(int id)
        //{
        //    var user = await _context.Users.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Users.Remove(user);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}
    }
}
