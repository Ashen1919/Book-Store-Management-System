using Microsoft.EntityFrameworkCore;
using RathnaBookStore.API.Models.Domains;

namespace RathnaBookStore.API.Data
{
    public class BookStoreDbContext(DbContextOptions<BookStoreDbContext> dbContextOptions) : DbContext(dbContextOptions)
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
