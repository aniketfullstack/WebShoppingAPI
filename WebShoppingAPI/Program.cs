using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using WebShoppingAPI.Extensions;
using WebShoppingAPI.Infrastructure.Data.Identity;
using WebShoppingAPI.Infrastructure.Interfaces;
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
    opt.UseSqlServer(builder.Configuration.GetConnectionString("EveriSeasonIdentity")));

builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
