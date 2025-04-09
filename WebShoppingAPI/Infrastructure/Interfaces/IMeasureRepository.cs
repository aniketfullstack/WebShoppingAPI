using WebShoppingAPI.Infrastructure.Models;

namespace WebShoppingAPI.Infrastructure.Interfaces
{
    public interface IMeasureRepository
    {
        public Task<int> CreateMeasureAsync(Measure measure);
        public Task<int> UpdateMeasureAsync(Measure measure);
        public Task<int> DeleteMeasureAsync(int id);
        Task<IReadOnlyList<Measure>> GetMeasuresAsync();
        public Task<Measure> GetMeasureByIdAsync(int id);

    }
}
