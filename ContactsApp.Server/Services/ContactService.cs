using ContactsApp.Server.Data;
using ContactsApp.Server.DTOs;

namespace ContactsApp.Server.Services
{
    public interface IContactService
    {
        Task<List<ContactDto>> GetAllContactsForUserIdAsync(int userId);
        Task<List<BasicContactDto>> GetAllBasicContactsForUserIdAsync(int userId);
        Task<bool> ContactExistsAsync(int contactId);
        Task<bool> ContactExistsInUserAsync(int contactId, int userId);

        Task<ContactDto?> GetContactById(int contactId);   
    }

    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<List<ContactDto>> GetAllContactsForUserIdAsync(int userId)
        {
            var contacts = await _contactRepository.GetAllContactsForUserIdAsync(userId);

            return contacts.Select(contact => new ContactDto
            {
                Id = contact.Id,
                Name = contact.Name,
                Surname = contact.Surname,
                Email = contact.Email,
                PhoneNumber = contact.PhoneNumber,
                Category = contact.Category?.Name,
                Subcategory = contact.Subcategory?.Name
            }).ToList();
        }

        public async Task<List<BasicContactDto>> GetAllBasicContactsForUserIdAsync(int userId)
        {
            var contacts = await _contactRepository.GetAllContactsForUserIdAsync(userId);

            return contacts.Select(contact => new BasicContactDto
            {
                Id = contact.Id,
                Name = contact.Name,
                Surname = contact.Surname
            }).ToList();
        }

        public async Task<ContactDto?> GetContactById(int contactId)
        {
            var contact = await _contactRepository.GetContactByIdAsync(contactId);

            if (contact == null)
            {
                return null;
            }

            return new ContactDto
            {
                Id = contact.Id,
                Name = contact.Name,
                Surname = contact.Surname,
                Email = contact.Email,
                PhoneNumber = contact.PhoneNumber,
                Category = contact.Category?.Name,
                Subcategory = contact.Subcategory?.Name,
                BirthDate = contact.BirthDate
            };
        }

        public async Task<bool> ContactExistsAsync(int contactId)
        {
            return await _contactRepository.ContactExistsByIdAsync(contactId);
        }

        public async Task<bool> ContactExistsInUserAsync(int contactId, int userId)
        {
            var contacts = await _contactRepository.GetAllContactsForUserIdAsync(userId);
            return contacts.Any(c => c.Id == contactId);
        }
    }
}
