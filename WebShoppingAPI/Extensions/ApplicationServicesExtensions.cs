using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using WebShoppingAPI.Infrastructure.Data;
using WebShoppingAPI.Infrastructure.Data.Repositories;
using WebShoppingAPI.Infrastructure.Interfaces;
using WebShoppingAPI.Infrastructure.Services;

namespace WebShoppingAPI.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration config)
        {
            services.AddDbContext<DatabaseContext>(opt => opt.UseSqlServer(config.GetConnectionString("WebShopping")));


            services.AddSingleton<IConnectionMultiplexer>(c =>
            {
                var redisConfig = ConfigurationOptions.Parse(config.GetConnectionString("Redis")!);
                return ConnectionMultiplexer.Connect(redisConfig);
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IMeasureRepository, MeasureRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IVariantRepository, VariantRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));


            services.AddCors(options =>
            {
                options.AddPolicy(name:
                  "CorsPolicy",
                  policy => policy.SetIsOriginAllowed((host) => true)
                  .AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader());
            });

            return services;
        }
    }
}
