using ContactsApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.Server.Data
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsersAsync();
        Task<bool> UserExistsByIdAsync(int id);
        Task<User?> UserByEmailAsync(string email);

    }

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
