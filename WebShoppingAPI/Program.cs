using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using WebShoppingAPI.Extensions;
using WebShoppingAPI.Infrastructure.Data.Identity;
using WebShoppingAPI.Infrastructure.Interfaces;
using WebShoppingAPI.Infrastructure.Models.IdentityModels;
using WebShoppingAPI.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddSingleton<IConnectionMultiplexer>(c =>
{
    var config = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"));
    return ConnectionMultiplexer.Connect(config);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name:
      "CorsPolicy",
      policy => policy.SetIsOriginAllowed((host) => true)
      .AllowAnyOrigin()
      .AllowAnyMethod()
      .AllowAnyHeader());
});

builder.Services.AddDbContext<AppIdentityDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("WebShoppingIdentity")));

builder.Services.AddIdentityServices(builder.Configuration);

// Register the global exception handler
builder.Services.AddExceptionHandlers();

var app = builder.Build();
using (var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;

    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var dependencyContext = services.GetRequiredService<AppIdentityDbContext>();
    await dependencyContext.Database.MigrateAsync();
    await AppIdentityDbContextSeed.SeedUsersAsync(userManager);

}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use the global exception handler
app.UseExceptionHandler(opt => { });

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
