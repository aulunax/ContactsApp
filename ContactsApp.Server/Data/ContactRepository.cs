using ContactsApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.Server.Data
{
    public interface IContactRepository
    {
        Task<bool> ContactExistsByIdAsync(int contactId);
        Task<List<Contact>> GetAllContactsForUserIdAsync(int userId);
        Task<Contact?> GetContactByIdAsync(int contactId);

        Task<bool> AddContactAsync(Contact contact);
        Task<bool> UpdateContactAsync(int contactId, Contact contact);
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

        /// <summary>
        /// Update a contact by contactId.
        /// Can't change userId or Id
        /// </summary>
        /// <param name="contactId"></param>
        /// <param name="updatedContact"></param>
        /// <returns></returns>
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
            if (contact == null)
            {
                return false;
            }

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
