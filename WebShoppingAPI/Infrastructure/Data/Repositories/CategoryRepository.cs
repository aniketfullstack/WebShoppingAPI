using Microsoft.EntityFrameworkCore;
using WebShoppingAPI.Infrastructure.Interfaces;
using WebShoppingAPI.Infrastructure.Models;

namespace WebShoppingAPI.Infrastructure.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DatabaseContext _databaseContext;
        public CategoryRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;

        }

        public async Task<int> CreateCategoryAsync(Category category)
        {
            _databaseContext.Category.Add(category);
            return await _databaseContext.SaveChangesAsync();
        }

        public async Task<int> UpdateCategoryAsync(Category category)
        {
            _databaseContext.Entry(category).State = EntityState.Modified;

            return await _databaseContext.SaveChangesAsync();
        }

        public async Task<int> DeleteCategoryAsync(int id)
        {
            var category = await _databaseContext.Category.FindAsync(id);
            if (category == null)
            {
                return 0;
            }

            _databaseContext.Category.Remove(category);

            return await _databaseContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<Category>> GetCategoriesAsync()
        {
            return await _databaseContext.Category
                 .ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _databaseContext.Category
                .FirstOrDefaultAsync(c => c.Id == id);
        }


        public async Task<int> CreateParentCategoryAsync(ParentCategory parentCategory)
        {
            _databaseContext.ParentCategory.Add(parentCategory);
            return await _databaseContext.SaveChangesAsync();
        }

        public async Task<int> UpdateParentCategoryAsync(ParentCategory parentCategory)
        {
            _databaseContext.Entry(parentCategory).State = EntityState.Modified;

            return await _databaseContext.SaveChangesAsync();
        }

        public async Task<int> DeleteParentCategoryAsync(int id)
        {
            var parentCategory = await _databaseContext.ParentCategory.FindAsync(id);
            if (parentCategory == null)
            {
                return 0;
            }

            _databaseContext.ParentCategory.Remove(parentCategory);

            return await _databaseContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<ParentCategory>> GetParentCategoriesAsync()
        {
            return await _databaseContext.ParentCategory.ToListAsync();
        }

        public async Task<ParentCategory> GetParentCategoryByIdAsync(int id)
        {
            return await _databaseContext.ParentCategory
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
