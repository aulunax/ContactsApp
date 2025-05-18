using ContactsApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.Server.Data
{
    public interface IContactRepository
    {
        /// <summary>
        /// Checks if a contact with given ID exists in the database.
        /// </summary>
        /// <param name="contactId">
        /// The ID of the contact to check.
        /// </param>
        /// <returns>
        /// True if the contact exists, false otherwise.
        /// </returns>
        Task<bool> ContactExistsByIdAsync(int contactId);

        /// <summary>
        /// Retrieves all contacts for a given user ID from database.
        /// </summary>
        /// <param name="userId">
        /// The ID of the user whose contacts to retrieve.
        /// </param>
        /// <returns>
        /// A list of Contact objects associated with the user ID.
        /// </returns>
        Task<List<Contact>> GetAllContactsForUserIdAsync(int userId);

        /// <summary>
        /// Retrieves a contact by its ID from database.
        /// </summary>
        /// <param name="contactId">
        /// The ID of the contact to retrieve.
        /// </param>
        /// <returns>
        /// The Contact object if found, null otherwise.
        /// </returns>
        Task<Contact?> GetContactByIdAsync(int contactId);


        /// <summary>
        /// Adds a new contact to the database.
        /// </summary>
        /// <param name="contact">
        /// Contact data to add.
        /// Ignores Id, auto creates new one.
        /// </param>
        /// <returns>
        /// True if the contact was added successfully, false otherwise.
        /// </returns>
        Task<bool> AddContactAsync(Contact contact);

        /// <summary>
        /// Updates a contact in database by contactId.
        /// Doesn't change userId or Id
        /// </summary>
        /// <param name="contactId">
        /// Id of the contact to update.
        /// </param>
        /// <param name="contact">
        /// TContact data to update the contact with contactId with.
        /// </param>
        /// <returns>
        /// True if the contact was updated successfully, false otherwise.
        /// </returns>
        Task<bool> UpdateContactAsync(int contactId, Contact contact);

        /// <summary>
        /// Deletes a contact from database by contactId.
        /// </summary>
        /// <param name="contactId">
        /// Id of the contact to delete.
        /// </param>
        /// <returns>
        /// True if the contact was deleted successfully, false otherwise.
        /// </returns>
        Task<bool> DeleteContactAsync(int contactId);

    }

    public class ContactRepository : IContactRepository
    {
        private readonly ContactsDbContext _context;

        public ContactRepository(ContactsDbContext context)
        {
            _context = context;
        }

        public async Task<List<Contact>> GetAllContactsForUserIdAsync(int userId)
        {
            return await _context.Contacts
                .Where(c => c.UserId == userId)
                .Include(c => c.Category)
                .Include(c => c.Subcategory)
                .ToListAsync();
        }

        public async Task<bool> ContactExistsByIdAsync(int contactId)
        {
            return await _context.Contacts.AnyAsync(c => c.Id == contactId);
        }

        public async Task<Contact?> GetContactByIdAsync(int contactId)
        {
            return await _context.Contacts
                .Include(c => c.Category)
                .Include(c => c.Subcategory)
                .FirstOrDefaultAsync(c => c.Id == contactId);
        }


        public async Task<bool> AddContactAsync(Contact contact)
        {
            try
            {
                Console.WriteLine($"Adding contact: Name={contact.Name}, Email={contact.Email}, CategoryId={contact.CategoryId}, SubcategoryId={contact.SubcategoryId}, UserId={contact.UserId}");

                await _context.Contacts.AddAsync(contact);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to add contact: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> UpdateContactAsync(int contactId, Contact updatedContact)
        {
            var existingContact = await GetContactByIdAsync(contactId);

            if (existingContact == null)
            {
                return false;
            }

            existingContact.Name = updatedContact.Name;
            existingContact.Surname = updatedContact.Surname;
            existingContact.Email = updatedContact.Email;
            existingContact.PhoneNumber = updatedContact.PhoneNumber;
            existingContact.CategoryId = updatedContact.CategoryId;
            existingContact.SubcategoryId = updatedContact.SubcategoryId;
            existingContact.BirthDate = updatedContact.BirthDate;
            existingContact.CustomSubcategory = updatedContact.CustomSubcategory;

            try
            {
                Console.WriteLine($"Updating contact: Id={contactId}, Name={existingContact.Name}, Email={existingContact.Email}, CategoryId={existingContact.CategoryId}, SubcategoryId={existingContact.SubcategoryId}");
                _context.Contacts.Update(existingContact);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to update contact: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteContactAsync(int contactId)
        {


            var contact = await GetContactByIdAsync(contactId);

            Console.WriteLine($"Deleting contact: Id={contactId}");
            if (contact == null)
            {
                return false;
            }
            Console.WriteLine($"After Deleting contact: Id={contactId}");


            try
            {
                _context.Contacts.Remove(contact);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to delete contact: {ex.Message}");
                return false;
            }


        }
    }
}
