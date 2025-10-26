using Microsoft.EntityFrameworkCore;
using RathnaBookStore.API.Data;
using RathnaBookStore.API.Mappings;
using RathnaBookStore.API.Repositories.BookRepository;
using RathnaBookStore.API.Repositories.CategoryRepository;
using RathnaBookStore.API.Repositories.OrderRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//dependency injection
builder.Services.AddDbContext<BookStoreDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookStoreConnectionString")));

builder.Services.AddScoped<ICategoryRepository, SQLCategoryRepository>();
builder.Services.AddScoped<IBookRepository, SQLBookRepository>();
builder.Services.AddScoped<IOrderRepository, SQLOrderRepository>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
