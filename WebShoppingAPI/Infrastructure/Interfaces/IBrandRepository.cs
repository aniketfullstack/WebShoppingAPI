using WebShoppingAPI.Infrastructure.Models;

namespace WebShoppingAPI.Infrastructure.Interfaces
{
    public interface IBrandRepository
    {
        public Task<int> CreateBrandAsync(Brand brand);
        public Task<int> UpdateBrandAsync(Brand brand);
        public Task<int> DeleteBrandAsync(int id);
        Task<IReadOnlyList<Brand>> GetBrandsAsync();
        public Task<Brand> GetBrandByIdAsync(int id);

    }
}
