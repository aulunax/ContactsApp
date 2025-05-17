using ContactsApp.Server.Data;
using ContactsApp.Server.DTOs;

namespace ContactsApp.Server.Services
{

    /// <summary>
    /// Interface for the subcategory service.
    /// </summary>
    public interface ISubcategoryService
    {
        /// <summary>
        /// Retrieves a list of subcategories for a given category ID.
        /// </summary>
        /// <param name="categoryId"> The ID of the category for which to retrieve subcategories from. Can be null. </param>
        /// <returns> A list of SubcategoryDto containing subcategory information.</returns> 
        Task<List<SubcategoryDto>> GetSubcategoriesForCategoryByIdAsync(int? categoryId);
    }

    /// <summary>
    /// Implementation of the subcategory service.
    /// </summary>
    public class SubcategoryService : ISubcategoryService
    {
        private readonly ISubcategoryRepository _subcategoryRepository;

        public SubcategoryService(ISubcategoryRepository subcategoryRepository)
        {
            _subcategoryRepository = subcategoryRepository;
        }

        public async Task<List<SubcategoryDto>> GetSubcategoriesForCategoryByIdAsync(int? categoryId)
        {
            var subcategories = await _subcategoryRepository.GetSubcategoriesForCategoryByIdAsync(categoryId);
            return subcategories.Select(subcategory => new SubcategoryDto
            {
                Id = subcategory.Id,
                Name = subcategory.Name,
                CategoryId = subcategory.CategoryId
            }).ToList();
        }
    }
}
