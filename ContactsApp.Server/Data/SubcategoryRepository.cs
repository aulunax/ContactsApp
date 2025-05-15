using Microsoft.EntityFrameworkCore;

namespace ContactsApp.Server.Data
{
    public interface ISubcategoryRepository
    {
        Task<int?> GetSubcategoryIdAsync(string? subcategoryName);

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

    }
}
