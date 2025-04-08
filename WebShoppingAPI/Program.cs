using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using WebShoppingAPI.Extensions;
using WebShoppingAPI.Infrastructure.Data;
using WebShoppingAPI.Infrastructure.Data.Identity;
using WebShoppingAPI.Infrastructure.Models.IdentityModels;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentityServices(builder.Configuration);

builder.Services.AddApplicationServices(builder.Configuration);

// Register the global exception handler
builder.Services.AddExceptionHandlers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("CorsPolicy");



// Use the global exception handler
app.UseExceptionHandler(opt => { });

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
         Path.Combine(Directory.GetCurrentDirectory(), "Resources")),
    RequestPath = "/Resources"
});


app.MapControllers();


using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var userManager = services.GetRequiredService<UserManager<AppUser>>();
var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
var dBContext = services.GetRequiredService<DatabaseContext>();
var dependencyContext = services.GetRequiredService<AppIdentityDbContext>();
var logger = services.GetRequiredService<ILogger<Program>>();
try
{
    await dBContext.Database.MigrateAsync();
    await dependencyContext.Database.MigrateAsync();
    await DatabaseContextSeed.SeedAsync(dBContext);
    await AppIdentityDbContextSeed.SeedUsersAsync(userManager, roleManager);
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occured during migration");
}

app.Run();
