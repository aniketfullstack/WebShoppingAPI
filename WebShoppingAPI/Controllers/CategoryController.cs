using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShoppingAPI.Infrastructure.Data;
using WebShoppingAPI.Infrastructure.Interfaces;
using WebShoppingAPI.Infrastructure.Models;

namespace WebShoppingAPI.Controllers
{
    public class CategoryController : BaseAPIController
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly DatabaseContext _databaseContext;

        public CategoryController(ICategoryRepository categoryRepository, DatabaseContext databaseContext)
        {
            _categoryRepository = categoryRepository;
            _databaseContext = databaseContext;
        }

        [HttpPost]
        [Route("CreateCategory")]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            var i = await _categoryRepository.CreateCategoryAsync(category);
            if (i > 0)
            {
                return Ok("Category added successfully");
            }
            else
            {
                return BadRequest("Unable to create category");
            }
        }

        [HttpPut]
        [Route("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory(int categoryId, Category category)
        {
            if (categoryId != category.Id)
            {
                return BadRequest();
            }
            try
            {
                int m = await _categoryRepository.UpdateCategoryAsync(category);
                if (m > 0)
                {
                    return Ok("Category updated successfully");
                }
                else
                {
                    return BadRequest("Category update failed");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(categoryId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpDelete]
        [Route("DeleteCategory")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            int d = await _categoryRepository.DeleteCategoryAsync(categoryId);
            if (d > 0)
            {
                return Ok("Category deleted successfully");
            }
            else
            {
                return BadRequest("Category delete failed");
            }
        }

        [HttpGet]
        [Route("GetCategories")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _categoryRepository.GetCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet]
        [Route("GetCategoryById")]
        public async Task<ActionResult<Category>> GetCategoryById(int categoryId)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        [HttpPost]
        [Route("CreateParentCategory")]
        public async Task<IActionResult> CreateParentCategory(ParentCategory parentCategory)
        {
            var i = await _categoryRepository.CreateParentCategoryAsync(parentCategory);
            if (i > 0)
            {
                return Ok("Parent category added successfully");
            }
            else
            {
                return BadRequest("Unable to create parent category");
            }
        }

        [HttpPut]
        [Route("UpdateParentCategory")]
        public async Task<IActionResult> UpdateParentCategory(int parentCategoryId, ParentCategory parentCategory)
        {
            if (parentCategoryId != parentCategory.Id)
            {
                return BadRequest();
            }
            try
            {
                int m = await _categoryRepository.UpdateParentCategoryAsync(parentCategory);
                if (m > 0)
                {
                    return Ok("Parent category updated");
                }
                else
                {
                    return BadRequest("Parent category update failed");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(parentCategoryId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpDelete]
        [Route("DeleteParentCategory")]
        public async Task<IActionResult> DeleteParentCategory(int parentCategoryId)
        {
            int d = await _categoryRepository.DeleteParentCategoryAsync(parentCategoryId);
            if (d > 0)
            {
                return Ok("Parent category deleted successfully");
            }
            else
            {
                return BadRequest("Parent category delete failed");
            }
        }

        [HttpGet]
        [Route("GetParentCategories")]
        public async Task<ActionResult<IEnumerable<ParentCategory>>> GetParentCategories()
        {
            var categories = await _categoryRepository.GetParentCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet]
        [Route("GetParentCategoryById")]
        public async Task<ActionResult<ParentCategory>> GetParentCategoryById(int parentCategoryId)
        {
            var category = await _categoryRepository.GetParentCategoryByIdAsync(parentCategoryId);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        private bool CategoryExists(int id)
        {
            return _databaseContext.Category.Any(e => e.Id == id);
        }
    }
}
