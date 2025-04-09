using WebShoppingAPI.Infrastructure.Models;

namespace WebShoppingAPI.Infrastructure.Interfaces
{
    public interface IProductRepository
    {
        public Task<int> CreateProductAsync(Product productGroup);
        public Task<int> UpdateProductAsync(Product productGroup);
        public Task<int> DeleteProductAsync(int id);
        Task<IReadOnlyList<Product>> GetProductsAsync();
        public Task<Product> GetProductByIdAsync(int id);

    }
}
