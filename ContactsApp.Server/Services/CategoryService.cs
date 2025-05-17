using ContactsApp.Server.Data;
using ContactsApp.Server.DTOs;

namespace ContactsApp.Server.Services
{
    /// <summary>
    /// Interface for the category service.
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Retrieves a list of categories.
        /// </summary>
        /// <returns>
        /// A list of CategoryDto containing category information.
        /// </returns>
        Task<List<CategoryDto>> GetCategoriesAsync();


        /// <summary>
        /// Retrieves the ID of a category by its name.
        /// </summary>
        /// <param name="categoryName">
        /// The name of the category to search for. Can be null.
        /// </param>
        /// <returns>
        /// The ID of the category if found, null otherwise.
        /// </returns>
        Task<int?> GetCategoryIdAsync(string? categoryName);
    }


    /// <summary>
    /// Implementation of the category service.
    /// </summary>
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
