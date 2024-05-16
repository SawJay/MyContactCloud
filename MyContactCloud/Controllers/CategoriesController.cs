using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyContactCloud.Client.Models;
using MyContactCloud.Client.Services.Interfaces;
using MyContactCloud.Data;
using MyContactCloud.Helpers.Extensions;
using MyContactCloud.Services;

namespace MyContactCloud.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;

        private string _userId => _userManager.GetUserId(User)!; // [authorize] means userId cannot be null

        private readonly ICategoryDTOService _categoryDTOService;

        public CategoriesController(ICategoryDTOService categoryDTOService, UserManager<ApplicationUser> userManager)
        {
            _categoryDTOService = categoryDTOService;

            _userManager = userManager;
        }



        // GET: "api/categories" -> returns the users categories
        [HttpGet]
        public async Task<ActionResult<List<CategoryDTO>>> GetCategories()
        {
            try
            {
                IEnumerable<CategoryDTO> categories = await _categoryDTOService.GetCategoriesAsync(_userId);

                return Ok(categories);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }

        // GET: "api/categories/5" -> returns a category or 404
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryDTO?>> GetCategory([FromRoute] int id)
        {
            try
            {
                CategoryDTO? categoryDTO = (await _categoryDTOService.GetCategoryByIdAsync(id, _userId))!;
                return Ok(categoryDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }

        // POST: "api/categories" -> creates a category and returns the created category
        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> CreateCategory([FromBody] CategoryDTO categoryDTO)
        {
            try
            {
                CategoryDTO createdCategoryDTO = await _categoryDTOService.CreateCategoryAsync(categoryDTO, _userId);
                return Ok(createdCategoryDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }

        //// PUT: "api/categories/5" -> updates the selected category and returns Ok
        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateCategory([FromRoute] int id, [FromBody] CategoryDTO category)
        {
            try
            {
                if (id != category.Id)
                {
                    return BadRequest();

                }
                else
                {
                    await _categoryDTOService.UpdateCategoryAsync(category, _userId);
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }

        //// DELETE: "api/categories/5" -> deletes the selected category and returns NoContent
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteCategory([FromRoute] int id)
        {
            try
            {
                await _categoryDTOService.DeleteCategoryAsync(id, _userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }

        //// POST: "api/categories/5/email" -> sends an email to category and returns Ok or BadRequest to indicate success or failure
        [HttpPost("{id:int}/email")]
        public async Task<ActionResult> EmailCategory([FromRoute] int id, [FromBody] EmailData emailData)
        {
            try
            {
                await _categoryDTOService.EmailCategoryAsync(id, emailData, _userId);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }
    }
}
