using ContactsApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.Server.Data
{
    public interface ICategoryRepository
    {
        /// <summary>
        /// Retrieves the ID of a category by its name.
        /// </summary>
        /// <param name="categoryName">
        /// The name of the category to search for.
        /// </param>
        /// <returns>
        /// The ID of the category if found, null otherwise.
        /// </returns>
        Task<int?> GetCategoryIdAsync(string? categoryName);

        /// <summary>
        /// Retrieves all categories from the database.
        /// </summary>
        /// <returns>
        /// A list of Category objects.
        /// </returns>
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
