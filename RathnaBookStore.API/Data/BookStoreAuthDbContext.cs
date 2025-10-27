using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RathnaBookStore.API.Data
{
    public class BookStoreAuthDbContext : IdentityDbContext
    {
        public BookStoreAuthDbContext(DbContextOptions<BookStoreAuthDbContext> options) : base(options)
        {
        }

    }
}
