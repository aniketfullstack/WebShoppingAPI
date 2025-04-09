using Microsoft.EntityFrameworkCore;
using WebShoppingAPI.Infrastructure.Interfaces;
using WebShoppingAPI.Infrastructure.Models;

namespace WebShoppingAPI.Infrastructure.Data.Repositories
{
    public class VariantRepository : IVariantRepository
    {
        private readonly DatabaseContext _databaseContext;
        public VariantRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;

        }

        public async Task<int> CreateVariantAsync(Variant variant)
        {
            _databaseContext.Variant.Add(variant);
            return await _databaseContext.SaveChangesAsync();
        }

        public async Task<int> UpdateVariantAsync(Variant variant)
        {
            _databaseContext.Entry(variant).State = EntityState.Modified;

            return await _databaseContext.SaveChangesAsync();
        }

        public async Task<int> DeleteVariantAsync(int id)
        {
            var variant = await _databaseContext.Variant.FindAsync(id);
            if (variant == null)
            {
                return 0;
            }

            _databaseContext.Variant.Remove(variant);

            return await _databaseContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<Variant>> GetVariantsAsync()
        {
            return await _databaseContext
                .Variant
                .Include(c => c.Product)
                .ToListAsync();
        }

        public async Task<Variant> GetVariantsByIdAsync(int id)
        {
            return await _databaseContext
                .Variant
                .Include(c => c.Product)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<string> SaveFileAsync(IFormFile file)
        {
            //string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            var uploadsFolder = Path.Combine("Resources", "Images");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), uploadsFolder);
            Directory.CreateDirectory(uploadsFolder); // Ensure directory exists

            string uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            string filePath = Path.Combine(pathToSave, uniqueFileName);
            var dbPath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return dbPath; // Return relative path
        }

    }
}
