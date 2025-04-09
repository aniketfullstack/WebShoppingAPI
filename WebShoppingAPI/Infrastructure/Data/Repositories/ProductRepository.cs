using Microsoft.EntityFrameworkCore;
using WebShoppingAPI.Infrastructure.Interfaces;
using WebShoppingAPI.Infrastructure.Models;

namespace WebShoppingAPI.Infrastructure.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DatabaseContext _databaseContext;
        public ProductRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;

        }

        public async Task<int> CreateProductAsync(Product product)
        {
            _databaseContext.Product.Add(product);
            return await _databaseContext.SaveChangesAsync();
        }

        public async Task<int> UpdateProductAsync(Product product)
        {
            _databaseContext.Entry(product).State = EntityState.Modified;

            return await _databaseContext.SaveChangesAsync();
        }

        public async Task<int> DeleteProductAsync(int id)
        {
            var product = await _databaseContext.Product.FindAsync(id);
            if (product == null)
            {
                return 0;
            }

            _databaseContext.Product.Remove(product);

            return await _databaseContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
            return await _databaseContext.Product
                 .ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _databaseContext.Product
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
