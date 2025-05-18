using ContactsApp.Server.DTOs;
using ContactsApp.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ContactsApp.Server.Controllers
{
    /// <summary>
    /// Controller for handling contact-related actions.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _service;

        public ContactsController(IContactService service)
        {
            _service = service;
        }

        // GET: api/Contacts/{contactId}
        [HttpGet("{contactId}")]
        public async Task<ActionResult<ContactDto>> GetContactForUser(int contactId)
        {

            var contact = await _service.GetContactById(contactId);

            // Check if the contact exists
            if (contact == null)
            {
                return NotFound();
            }

            return contact;
        }


        // Post: api/Contacts
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<bool>> AddContact([FromBody] ContactDto contactDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(false);

            // Get the user ID from the JWT token
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Check if the user ID from the token exists and matches the user ID in the contact DTO
            if (userId == null || int.Parse(userId) != contactDto.UserId)
                return Unauthorized("Unauthorized Access");


            var result = await _service.AddContactAsync(contactDto);

            // Check if the contact was added successfully
            if (result == false)
                return BadRequest("Failed to add contact");

            return Ok(true);
        }

        // PUT: api/Contacts
        [Authorize]
        [HttpPut("{contactId}")]
        public async Task<ActionResult<bool>> UpdateContact(int contactId, [FromBody] ContactDto contactDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(false);

            // Get the user ID from the JWT token
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var contact = await _service.GetContactById(contactId);

            // Get user ID from the contact
            var userIdFromContact = contact?.UserId;

            // Check if the user ID from the token exists and matches the user ID in both the contact DTO and the contact from the database
            if (userId == null || int.Parse(userId) != contactDto.UserId || int.Parse(userId) != userIdFromContact)
                return Unauthorized("Unauthorized Access");

            var result = await _service.UpdateContactAsync(contactId, contactDto);

            // Check if the contact was updated successfully
            if (result == false)
                return BadRequest("Failed to update contact");

            return Ok(true);
        }

        // DELETE: api/Contacts/{contactId}
        [Authorize]
        [HttpDelete("{contactId}")]
        public async Task<ActionResult<bool>> DeleteContact(int contactId)
        {
            // Get the user ID from the JWT token
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var contact = await _service.GetContactById(contactId);
            var userIdFromContact = contact?.UserId;

            // Check if the user ID from the token exists and matches the user ID in the contact DTO
            if (userId == null || int.Parse(userId) != userIdFromContact)
                return Unauthorized("Unauthorized Access");

            var result = await _service.DeleteContactAsync(contactId);

            // Check if the contact was deleted successfully
            if (result == false)
                return BadRequest("Failed to delete contact");

            return Ok(true);
        }
    }
}
