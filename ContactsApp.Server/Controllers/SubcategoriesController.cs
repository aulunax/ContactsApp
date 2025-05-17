using ContactsApp.Server.DTOs;
using ContactsApp.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactsApp.Server.Controllers
{
    /// <summary>
    /// Controller for handling subcategory-related actions.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SubcategoriesController : ControllerBase
    {

        private readonly ISubcategoryService _subcategoryService;

        public SubcategoriesController(ISubcategoryService subcategoryService)
        {
            _subcategoryService = subcategoryService;
        }


        // GET api/Subcategories/{categoryId}
        [HttpGet("{categoryId}")]
        public async Task<ActionResult<SubcategoryDto>> GetSubcategoriesFromCategoryById(int categoryId)
        {
            var subcategory = await _subcategoryService.GetSubcategoriesForCategoryByIdAsync(categoryId);

            if (subcategory == null)
            {
                return NotFound();
            }

            return Ok(subcategory);
        }
    }
}
