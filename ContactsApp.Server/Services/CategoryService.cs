using ContactsApp.Server.Data;
using ContactsApp.Server.DTOs;

namespace ContactsApp.Server.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetCategoriesAsync();

        Task<int?> GetCategoryIdAsync(string? categoryName);
    }

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<CategoryDto>> GetCategoriesAsync()
        {
            var categories = await _categoryRepository.GetCategoriesAsync();
            return categories.Select(category => new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
            }).ToList();
        }

        public async Task<int?> GetCategoryIdAsync(string? categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
            {
                return null;
            }
            var categoryId = await _categoryRepository.GetCategoryIdAsync(categoryName);
            return categoryId;
        }
    }
}
