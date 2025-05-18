using ContactsApp.Server.Data;
using ContactsApp.Server.DTOs;
using ContactsApp.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactsApp.Server.Controllers
{
    /// <summary>
    /// Controller for handling category-related actions.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<List<CategoryDto>>> GetCategories()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            return Ok(categories);
        }

        // GET: api/Categories/{categoryName}
        [HttpGet("{categoryName}")]
        public async Task<ActionResult<int?>> GetCategoryId(string? categoryName)
        {
            // Validate the input
            if (string.IsNullOrEmpty(categoryName))
            {
                return BadRequest("Category name cannot be null or empty.");
            }

            var categoryId = await _categoryService.GetCategoryIdAsync(categoryName);

            // Check if the category ID was found
            if (categoryId == null)
            {
                return NotFound($"Category '{categoryName}' not found.");
            }

            return Ok(categoryId);
        }

    }
}
