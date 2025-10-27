using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RathnaBookStore.API.Data;
using RathnaBookStore.API.Mappings;
using RathnaBookStore.API.Repositories.Auth_Repository;
using RathnaBookStore.API.Repositories.BookRepository;
using RathnaBookStore.API.Repositories.CategoryRepository;
using RathnaBookStore.API.Repositories.OrderRepository;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//dependency injection
builder.Services.AddDbContext<BookStoreDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookStoreConnectionString")));

builder.Services.AddDbContext<BookStoreAuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookStoreAuthConnectionString")));

builder.Services.AddScoped<ICategoryRepository, SQLCategoryRepository>();
builder.Services.AddScoped<IBookRepository, SQLBookRepository>();
builder.Services.AddScoped<IOrderRepository, SQLOrderRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();

//Authentication 
builder.Services.AddIdentityCore<IdentityUser>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("BookStore")
    .AddEntityFrameworkStores<BookStoreAuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            AuthenticationType = "Jwt",
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudiences = new[] { builder.Configuration["Jwt:Audience"] },
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

//config automapper
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<AutoMapperProfiles>();
});

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
