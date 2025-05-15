using ContactsApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.Server.Data
{
    public interface ICategoryRepository
    {
        Task<int?> GetCategoryIdAsync(string? categoryName);

        Task<List<Category>> GetCategoriesAsync();

    }

    public class CategoryRepository : ICategoryRepository
    {
        private readonly ContactsDbContext _context;

        public CategoryRepository(ContactsDbContext context)
        {
            _context = context;
        }

        public async Task<int?> GetCategoryIdAsync(string? categoryName)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == categoryName);
            return category?.Id;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }
    }
}
