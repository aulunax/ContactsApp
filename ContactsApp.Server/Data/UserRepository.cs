using ContactsApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.Server.Data
{
    /// <summary>
    /// Interface for the user repository.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Retrieves list of all users from the database.
        /// </summary>
        /// <returns>
        /// A list of User objects representing all users in the database.
        /// </returns>
        Task<List<User>> GetAllUsersAsync();

        /// <summary>
        /// Checks if a user with the given ID exists in the database.
        /// </summary>
        /// <param name="id"> 
        /// The ID of the user to check.
        /// </param>
        /// <returns>
        /// True if the user exists, false otherwise.
        /// </returns>
        Task<bool> UserExistsByIdAsync(int id);

        /// <summary>
        /// Retrieves a user by their email address.
        /// </summary>
        /// <param name="email"> 
        /// The email address of the user to retrieve.
        /// </param>
        /// <returns>
        /// The User object if found, null otherwise.
        /// </returns>
        Task<User?> UserByEmailAsync(string email);

    }

    /// <summary>
    /// Implementation of the user repository.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly ContactsDbContext _context;

        public UserRepository(ContactsDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<bool> UserExistsByIdAsync(int id)
        {
            return await _context.Users.AnyAsync(u => u.Id == id);
        }

        public async Task<User?> UserByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
