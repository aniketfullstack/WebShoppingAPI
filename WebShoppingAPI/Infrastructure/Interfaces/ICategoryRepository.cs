using WebShoppingAPI.Infrastructure.Models;

namespace WebShoppingAPI.Infrastructure.Interfaces
{
    public interface ICategoryRepository
    {
        public Task<int> CreateCategoryAsync(Category category);
        public Task<int> UpdateCategoryAsync(Category category);
        public Task<int> DeleteCategoryAsync(int id);
        Task<IReadOnlyList<Category>> GetCategoriesAsync();
        public Task<Category> GetCategoryByIdAsync(int id);



        public Task<int> CreateParentCategoryAsync(ParentCategory category);
        public Task<int> UpdateParentCategoryAsync(ParentCategory category);
        public Task<int> DeleteParentCategoryAsync(int id);
        Task<IReadOnlyList<ParentCategory>> GetParentCategoriesAsync();
        public Task<ParentCategory> GetParentCategoryByIdAsync(int id);
    }
}
