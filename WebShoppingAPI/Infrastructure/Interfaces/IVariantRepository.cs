using WebShoppingAPI.Infrastructure.Models;

namespace WebShoppingAPI.Infrastructure.Interfaces
{
    public interface IVariantRepository
    {
        public  Task<int> CreateVariantAsync(Variant variant);
        public  Task<int> UpdateVariantAsync(Variant variant);
        public  Task<int> DeleteVariantAsync(int variantId);
        Task<IReadOnlyList<Variant>> GetVariantsAsync();
        public  Task<Variant> GetVariantsByIdAsync(int variantId);
        public  Task<string> SaveFileAsync(IFormFile file);
    }
}
