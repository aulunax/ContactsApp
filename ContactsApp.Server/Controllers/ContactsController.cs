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
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null || int.Parse(userId) != contactDto.UserId)
                return Unauthorized("Unauthorized Access");

            System.Console.WriteLine($"Adding contact: {contactDto.Name}, {contactDto.Surname}, {contactDto.Email}, {contactDto.PhoneNumber}, {contactDto.Category}, {contactDto.Subcategory}, {contactDto.BirthDate}");

            var result = await _service.AddContactAsync(contactDto);

            if (result == false)
                return BadRequest("Failed to add contact");

            return Ok();
        }

        // PUT: api/Contacts
        [Authorize]
        [HttpPut("{contactId}")]
        public async Task<ActionResult<bool>> UpdateContact(int contactId, [FromBody] ContactDto contactDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var contact = await _service.GetContactById(contactId);
            var userIdFromContact = contact?.UserId;

            if (userId == null || int.Parse(userId) != contactDto.UserId || int.Parse(userId) != userIdFromContact)
                return Unauthorized("Unauthorized Access");

            var result = await _service.UpdateContactAsync(contactId, contactDto);

            if (result == false)
                return BadRequest("Failed to update contact");

            return Ok();
        }

        // DELETE: api/Contacts/{contactId}
        [Authorize]
        [HttpDelete("{contactId}")]
        public async Task<ActionResult<bool>> DeleteContact(int contactId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var contact = await _service.GetContactById(contactId);
            var userIdFromContact = contact?.UserId;

            if (userId == null || int.Parse(userId) != userIdFromContact)
                return Unauthorized("Unauthorized Access");

            var result = await _service.DeleteContactAsync(contactId);

            if (result == false)
                return BadRequest("Failed to delete contact");

            return Ok();
        }
    }
}
