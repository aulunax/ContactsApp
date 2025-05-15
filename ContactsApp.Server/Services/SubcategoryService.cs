using ContactsApp.Server.Data;
using ContactsApp.Server.DTOs;

namespace ContactsApp.Server.Services
{
    public interface ISubcategoryService
    {
        Task<List<SubcategoryDto>> GetSubcategoriesForCategoryByIdAsync(int? categoryId);
    }

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
