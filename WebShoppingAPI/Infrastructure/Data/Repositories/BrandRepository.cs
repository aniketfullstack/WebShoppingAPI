using Microsoft.EntityFrameworkCore;
using WebShoppingAPI.Infrastructure.Interfaces;
using WebShoppingAPI.Infrastructure.Models;

namespace WebShoppingAPI.Infrastructure.Data.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly DatabaseContext _databaseContext;
        public BrandRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;

        }

        public async Task<int> CreateBrandAsync(Brand brand)
        {
            _databaseContext.Brand.Add(brand);
            return await _databaseContext.SaveChangesAsync();
        }

        public async Task<int> UpdateBrandAsync(Brand brand)
        {
            _databaseContext.Entry(brand).State = EntityState.Modified;

            return await _databaseContext.SaveChangesAsync();
        }

        public async Task<int> DeleteBrandAsync(int id)
        {
            var brand = await _databaseContext.Brand.FindAsync(id);
            if (brand == null)
            {
                return 0;
            }

            _databaseContext.Brand.Remove(brand);

            return await _databaseContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<Brand>> GetBrandsAsync()
        {
            return await _databaseContext.Brand
                 .ToListAsync();
        }

        public async Task<Brand> GetBrandByIdAsync(int id)
        {
            return await _databaseContext.Brand.FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
