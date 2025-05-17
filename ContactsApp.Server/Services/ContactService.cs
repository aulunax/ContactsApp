using ContactsApp.Server.Data;
using ContactsApp.Server.DTOs;
using ContactsApp.Server.Models;

namespace ContactsApp.Server.Services
{
    public interface IContactService
    {
        Task<List<ContactDto>> GetAllContactsForUserIdAsync(int userId);
        Task<List<BasicContactDto>> GetAllBasicContactsForUserIdAsync(int userId);
        Task<bool> ContactExistsAsync(int contactId);
        Task<bool> ContactExistsInUserAsync(int contactId, int userId);

        Task<bool> AddContactAsync(ContactDto contactDto);
        Task<bool> UpdateContactAsync(int contactId, ContactDto contactDto);
        Task<bool> DeleteContactAsync(int contactId);

        Task<ContactDto?> GetContactById(int contactId);   
    }

    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly ISubcategoryRepository _subcategoryRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ContactService(IContactRepository contactRepository, ISubcategoryRepository subcategoryRepository, ICategoryRepository categoryRepository)
        {
            _contactRepository = contactRepository;
            _subcategoryRepository = subcategoryRepository;
            _categoryRepository = categoryRepository;
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


            System.Diagnostics.Debug.WriteLine($"Contact: {contact.Name}, {contact.Surname}, {contact.Email}, {contact.PhoneNumber}, {contact.Category?.Name}, {contact.Subcategory?.Name}");

            return new ContactDto
            {
                Id = contact.Id,
                Name = contact.Name,
                Surname = contact.Surname,
                Email = contact.Email,
                PhoneNumber = contact.PhoneNumber,
                Category = contact.Category?.Name,
                Subcategory = string.IsNullOrEmpty(contact.Subcategory?.Name) ? contact.CustomSubcategory : contact.Subcategory?.Name,
                BirthDate = contact.BirthDate,
                UserId = contact.UserId
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

        public async Task<bool> AddContactAsync(ContactDto contactDto)
        {
            var categoryId = await _categoryRepository.GetCategoryIdAsync(contactDto.Category);
            var subcategoryId = await _subcategoryRepository.GetSubcategoryIdAsync(contactDto.Subcategory);

            System.Diagnostics.Debug.WriteLine($"CategoryId: {categoryId}, SubcategoryId: {subcategoryId}");

            // If the subcategory is not found in dictionary, but has content, we assume it's a custom subcategory 
            var customSubcategory = (!string.IsNullOrEmpty(contactDto.Subcategory) && subcategoryId == null) ? contactDto.Subcategory : null;

            System.Diagnostics.Debug.WriteLine($"CustomSubcategory: {customSubcategory}");

            var contact = new Contact
            {
                Name = contactDto.Name,
                Surname = contactDto.Surname,
                Email = contactDto.Email,
                PhoneNumber = contactDto.PhoneNumber,
                CategoryId = categoryId,
                SubcategoryId = subcategoryId,
                CustomSubcategory = customSubcategory,
                BirthDate = contactDto.BirthDate,
                UserId = contactDto.UserId
            };

            return await _contactRepository.AddContactAsync(contact);
        }

        public async Task<bool> UpdateContactAsync(int contactId, ContactDto contactDto)
        {
            var categoryId = await _categoryRepository.GetCategoryIdAsync(contactDto.Category);
            var subcategoryId = await _subcategoryRepository.GetSubcategoryIdAsync(contactDto.Subcategory);

            // If the subcategory is not found in dictionary, but has content, we assume it's a custom subcategory 
            var customSubcategory = (!string.IsNullOrEmpty(contactDto.Subcategory) && subcategoryId == null) ? contactDto.Subcategory : null;
            var contact = new Contact
            {
                Name = contactDto.Name,
                Surname = contactDto.Surname,
                Email = contactDto.Email,
                PhoneNumber = contactDto.PhoneNumber,
                CategoryId = categoryId,
                SubcategoryId = subcategoryId,
                CustomSubcategory = customSubcategory,
                BirthDate = contactDto.BirthDate,
            };
            return await _contactRepository.UpdateContactAsync(contactId, contact);
        }

        public async Task<bool> DeleteContactAsync(int contactId)
        {
            return await _contactRepository.DeleteContactAsync(contactId);
        }
    }
}
