using ContactsApp.Server.Data;
using ContactsApp.Server.DTOs;
using ContactsApp.Server.Models;

namespace ContactsApp.Server.Services
{
    public interface IContactService
    {
        /// <summary>
        /// Retrieves all contacts for a given user ID.
        /// </summary>
        /// <param name="userId">
        /// The ID of the user whose contacts to retrieve.
        /// </param>
        /// <returns>
        /// A list of ContactDto objects representing the user's contacts.
        /// </returns>
        Task<List<ContactDto>> GetAllContactsForUserIdAsync(int userId);

        /// <summary>
        /// Retrieves all contacts for a given user ID, only containing basic information.
        /// </summary>
        /// <param name="userId">
        /// The ID of the user whose contacts to retrieve.
        /// </param>
        /// <returns>
        /// A list of BasicContactDto objects representing the user's contacts with basic information.
        /// </returns>
        Task<List<BasicContactDto>> GetAllBasicContactsForUserIdAsync(int userId);

        /// <summary>
        /// Checks if a contact with given ID exists.
        /// </summary>
        /// <param name="contactId">
        /// The ID of the contact to check.
        /// </param>
        /// <returns>
        /// True if the contact exists, false otherwise.
        /// </returns>
        Task<bool> ContactExistsAsync(int contactId);

        /// <summary>
        /// Checks if a contact with given ID exists for a specific user.
        /// </summary>
        /// <param name="contactId">
        /// The ID of the contact to check.
        /// </param>
        /// <param name="userId">
        /// The ID of the user to check against.
        /// </param>
        /// <returns>
        /// True if the contact exists for the user, false otherwise.
        /// </returns>
        Task<bool> ContactExistsInUserAsync(int contactId, int userId);


        /// <summary>
        /// Adds a new contact to the database.
        /// </summary>
        /// <param name="contactDto">
        /// The contact data to add. Ignores Id field, auto creates new one.
        /// </param>
        /// <returns>
        /// True if the contact was added successfully, false otherwise.
        /// </returns>
        Task<bool> AddContactAsync(ContactDto contactDto);

        /// <summary>
        /// Updates a contact with contactId.
        /// </summary>
        /// <param name="contactId">
        /// The ID of the contact to update.
        /// </param>
        /// <param name="contactDto">
        /// The contact data to update. Ignores Id field.
        /// </param>
        /// <returns>
        /// True if the contact was updated successfully, false otherwise.
        /// </returns>
        Task<bool> UpdateContactAsync(int contactId, ContactDto contactDto);

        /// <summary>
        /// Deletes a contact with contactId.
        /// </summary>
        /// <param name="contactId">
        /// The ID of the contact to delete.
        /// </param>
        /// <returns>
        /// True if the contact was deleted successfully, false otherwise.
        /// </returns>
        Task<bool> DeleteContactAsync(int contactId);


        /// <summary>
        /// Retrieves a contact by its ID.
        /// </summary>
        /// <param name="contactId">
        /// The ID of the contact to retrieve.
        /// </param>
        /// <returns>
        /// The ContactDto object if found, null otherwise.
        /// </returns>
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

            return new ContactDto
            {
                Id = contact.Id,
                Name = contact.Name,
                Surname = contact.Surname,
                Email = contact.Email,
                PhoneNumber = contact.PhoneNumber,
                Category = contact.Category?.Name,
                // If subcategory is empty, we assume it's a custom subcategory (which should be empty if Category != "Other")
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
