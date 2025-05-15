using ContactsApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.Server.Data
{
    public interface ISubcategoryRepository
    {
        Task<int?> GetSubcategoryIdAsync(string? subcategoryName);

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
