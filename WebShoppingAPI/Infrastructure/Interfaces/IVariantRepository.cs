using WebShoppingAPI.Infrastructure.Models;

namespace WebShoppingAPI.Infrastructure.Interfaces
{
    public interface IVariantRepository
    {
        public  Task<int> CreateVariantAsync(Variant variant);
        public  Task<int> UpdateVariantAsync(Variant variant);
        public  Task<int> DeleteVariantAsync(int variantId);
        Task<IReadOnlyList<Variant>> GetVariantsAsync();

        Task<IReadOnlyList<Variant>> GetVariantsByCategoryIdAsync(int categoryId);

        Task<IReadOnlyList<Variant>> GetVariantsByProductIdAsync(int categoryId);

        public  Task<Variant> GetVariantsByIdAsync(int variantId);
        public  Task<string> SaveFileAsync(IFormFile file);
    }
}
