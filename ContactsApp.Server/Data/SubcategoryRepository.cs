using ContactsApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.Server.Data
{
    public interface ISubcategoryRepository
    {
        /// <summary>
        /// Retrieves the ID of a subcategory by its name from database.
        /// </summary>
        /// <param name="subcategoryName">
        /// The name of the subcategory to search for. Can be null.
        /// </param>
        /// <returns>
        /// The ID of the subcategory if found, null otherwise.
        /// </returns>
        Task<int?> GetSubcategoryIdAsync(string? subcategoryName);

        /// <summary>
        /// Retrieves all subcategories for a given category ID from database.
        /// </summary>
        /// <param name="categoryId">
        /// The ID of the category whose subcategories to retrieve. Can be null.
        /// </param>
        /// <returns>
        /// A list of Subcategory objects associated with the category ID.
        /// </returns>
        Task<List<Subcategory>> GetSubcategoriesForCategoryByIdAsync(int? categoryId);

    }

    public class SubcategoryRepository : ISubcategoryRepository
    {
        private readonly ContactsDbContext _context;

        public SubcategoryRepository(ContactsDbContext context)
        {
            _context = context;
        }

        public async Task<int?> GetSubcategoryIdAsync(string? subcategoryName)
        {
            var subcategory = await _context.Subcategories.FirstOrDefaultAsync(c => c.Name == subcategoryName);
            return subcategory?.Id;
        }

        public async Task<List<Subcategory>> GetSubcategoriesForCategoryByIdAsync(int? categoryId)
        {
            return await _context.Subcategories
                .Where(c => c.CategoryId == categoryId)
                .ToListAsync();
        }

    }
}
