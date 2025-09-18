using Algolia.Search.Clients;
using ECommerceAPI.Data;
using ECommerceAPI.Repositories;
using ECommerceAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddScoped<CartRepository>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<OrderDetailRepository>();
builder.Services.AddScoped<AuthService>();

// Or for search-only client:
// services.AddSingleton<ISearchClient>(new SearchClient("YourApplicationID", "YourSearchOnlyAPIKey"));
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<CartSerivce>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<AlgoliaSyncService>();

builder.Services.AddDbContext<ECommerceDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});



builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = "ECommerceAPI",
            ValidAudience = "ECommerceClient",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("ThisIsASuperStrongJwtSecretKey1234567890!"))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
