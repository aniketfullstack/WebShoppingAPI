using System.Text.Json;
using System.Reflection;
using WebShoppingAPI.Infrastructure.Models;

namespace WebShoppingAPI.Infrastructure.Data
{
    public class DatabaseContextSeed
    {
        public static async Task SeedAsync(DatabaseContext context)
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);


            if (!context.Brand.Any())
            {
                var brandData = File.ReadAllText(path + @"/Infrastructure/Data/SeedData/brand.json");
                var brands = JsonSerializer.Deserialize<List<Brand>>(brandData);
                foreach (var item in brands)
                {
                    context.Brand.Add(item);
                }

                await context.SaveChangesAsync();
            }
            if (context.ChangeTracker.HasChanges()) await context.SaveChangesAsync();
        }
    }
}
