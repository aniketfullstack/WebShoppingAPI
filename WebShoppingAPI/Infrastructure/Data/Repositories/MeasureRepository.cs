using Microsoft.EntityFrameworkCore;
using WebShoppingAPI.Infrastructure.Interfaces;
using WebShoppingAPI.Infrastructure.Models;

namespace WebShoppingAPI.Infrastructure.Data.Repositories
{
    public class MeasureRepository: IMeasureRepository
    {
        private readonly DatabaseContext _databaseContext;
        public MeasureRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;

        }

        public async Task<int> CreateMeasureAsync(Measure measure)
        {
            _databaseContext.Measure.Add(measure);
            return await _databaseContext.SaveChangesAsync();
        }

        public async Task<int> UpdateMeasureAsync(Measure measure)
        {
            _databaseContext.Entry(measure).State = EntityState.Modified;

            return await _databaseContext.SaveChangesAsync();
        }

        public async Task<int> DeleteMeasureAsync(int id)
        {
            var brand = await _databaseContext.Measure.FindAsync(id);
            if (brand == null)
            {
                return 0;
            }

            _databaseContext.Measure.Remove(brand);

            return await _databaseContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<Measure>> GetMeasuresAsync()
        {
            return await _databaseContext.Measure
                 .ToListAsync();
        }

        public async Task<Measure> GetMeasureByIdAsync(int id)
        {
            return await _databaseContext.Measure
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
