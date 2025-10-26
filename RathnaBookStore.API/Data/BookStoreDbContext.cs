using Microsoft.EntityFrameworkCore;
using RathnaBookStore.API.Models.Domains;

namespace RathnaBookStore.API.Data
{
    public class BookStoreDbContext(DbContextOptions<BookStoreDbContext> dbContextOptions) : DbContext(dbContextOptions)
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //configure relationship between Order and Orderitems
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(o => o.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            //configure relationship between orderitems and Book
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Book)
                .WithMany()
                .HasForeignKey(oi => oi.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            //config decimal precision
            modelBuilder.Entity<Order>()
                .Property(o => o.SubTotal)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.Discount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(o => o.UnitPrice)
                .HasPrecision(18, 2);
        }
    }
}
