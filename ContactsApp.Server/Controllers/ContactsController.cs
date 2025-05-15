using ContactsApp.Server.DTOs;
using ContactsApp.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactsApp.Server.Controllers
{
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
        /// <summary>
        /// Get detailed contact info for contactId.
        /// </summary>
        /// <param name="contactId">Integer representing Id of the Contact</param>
        /// <returns>
        /// Returns a detailed contact info for a contact with contactId.
        /// </returns>
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


        [HttpPost]
        public async Task<ActionResult<bool>> AddContact([FromBody] ContactDto contactDto)
        {
            var result = await _service.AddContactAsync(contactDto);

            if (result == false)
                return BadRequest("Failed to add contact");

            return Ok();
        }

        [HttpPut("{contactId}")]
        public async Task<ActionResult<bool>> UpdateContact(int contactId, [FromBody] ContactDto contactDto)
        {
            var result = await _service.UpdateContactAsync(contactId, contactDto);

            if (result == false)
                return BadRequest("Failed to update contact");

            return Ok();
        }

        [HttpDelete("{contactId}")]
        public async Task<ActionResult<bool>> DeleteContact(int contactId)
        {
            var result = await _service.DeleteContactAsync(contactId);

            if (result == false)
                return BadRequest("Failed to delete contact");

            return Ok();
        }
    }
}
