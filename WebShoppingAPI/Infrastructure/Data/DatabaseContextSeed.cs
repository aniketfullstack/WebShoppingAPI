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

            if (!context.Status.Any())
            {
                var statusData = File.ReadAllText(path +@"/Infrastructure/Data/SeedData/status.json");
                var status = JsonSerializer.Deserialize<List<Status>>(statusData);
                foreach (var item in status)
                {
                    context.Status.Add(item);
                }

                await context.SaveChangesAsync();
            }

            if (!context.Measure.Any())
            {
                var measureData = File.ReadAllText(path +@"/Infrastructure/Data/SeedData/measure.json");
                var measures = JsonSerializer.Deserialize<List<Measure>>(measureData);
                foreach (var item in measures)
                {
                    context.Measure.Add(item);
                }

                await context.SaveChangesAsync();
            }

            if (!context.ParentCategory.Any())
            {
                var parentCategoriesData = File.ReadAllText(path +@"/Infrastructure/Data/SeedData/parent-categories.json");
                var parentCategories = JsonSerializer.Deserialize<List<ParentCategory>>(parentCategoriesData);
                foreach (var item in parentCategories)
                {
                    context.ParentCategory.Add(item);
                }

                await context.SaveChangesAsync();
            }

            if (!context.Category.Any())
            {
                var categoryData = File.ReadAllText(path +@"/Infrastructure/Data/SeedData/categories.json");
                var categories = JsonSerializer.Deserialize<List<Category>>(categoryData);
                foreach (var item in categories)
                {
                    context.Category.Add(item);
                }
                await context.SaveChangesAsync();
            }

            if (!context.Product.Any())
            {
                var productsData = File.ReadAllText(path +@"/Infrastructure/Data/SeedData/product.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                foreach (var item in products)
                {
                    context.Product.Add(item);
                }
                await context.SaveChangesAsync();
            }

            if (!context.Variant.Any())
            {
                var variantData = File.ReadAllText(path +@"/Infrastructure/Data/SeedData/variant.json");
                var variants = JsonSerializer.Deserialize<List<Variant>>(variantData);
                foreach (var item in variants)
                {
                    context.Variant.Add(item);
                }

                await context.SaveChangesAsync();
            }

            if (context.ChangeTracker.HasChanges()) await context.SaveChangesAsync();
        }
    }
}
