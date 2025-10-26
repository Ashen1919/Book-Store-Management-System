using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RathnaBookStore.API.Data;
using RathnaBookStore.API.Models.Domains;

namespace RathnaBookStore.API.Repositories.BookRepository
{
    public class SQLBookRepository : IBookRepository
    {
        private readonly BookStoreDbContext dbContext;

        public SQLBookRepository(BookStoreDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //Create Book
        public async Task<Book> CreateBookAsync(Book book)
        {
            await dbContext.Books.AddAsync(book);
            await dbContext.SaveChangesAsync();
            return book;
        }

        public async Task<Book?> DeleteBookAsync(Guid id)
        {
            var bookDomainModel = await dbContext.Books.FirstOrDefaultAsync(x => x.Id == id);

            if(bookDomainModel == null)
            {
                return null;
            } 

            dbContext.Books.Remove(bookDomainModel);
            await dbContext.SaveChangesAsync();

            return bookDomainModel;

        }

        //Get all Books
        public async Task<List<Book>> GetAllBooksAsync(string? filterOn = null, string? filterQuery = null,
            string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            //Filtering
            var books = dbContext.Books.AsQueryable();

            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if(filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    books = books.Where(x => x.Name.Contains(filterQuery));
                }
                else if(filterOn.Equals("ISBN", StringComparison.OrdinalIgnoreCase))
                {
                    books = books.Where(x => x.ISBN.Contains(filterQuery));
                }
                else if (filterOn.Equals("Author", StringComparison.OrdinalIgnoreCase))
                {
                    books = books.Where(x => x.Author.Contains(filterQuery));
                }
                else if (filterOn.Equals("Category", StringComparison.OrdinalIgnoreCase))
                {
                    books = books.Where(x => x.Category.Contains(filterQuery));
                }
            }

            //sorting
            if(string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if(sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    books = isAscending ? books.OrderBy(x => x.Name) : books.OrderByDescending(x => x.Name);
                }
            }

            //Pagination
            var skipResults = (pageNumber - 1) * pageSize;

            return await books.Skip(skipResults).Take(pageSize).ToListAsync();
        }

        public async Task<Book?> GetBookByIdAsync(Guid id)
        {
            return await dbContext.Books.FirstOrDefaultAsync(x => x.Id == id);
        }

        //Update books
        public async Task<Book?> UpdateBooksAsync(Guid id, Book book)
        {
            var existingBook = await dbContext.Books.FirstOrDefaultAsync(x => x.Id == id);

            if (existingBook == null)
            {
                return null;
            }

            existingBook.ISBN = book.ISBN;
            existingBook.Author = book.Author;
            existingBook.ImageUrl = book.ImageUrl;
            existingBook.Name = book.Name;
            existingBook.Category = book.Category;
            existingBook.Price = book.Price;
            existingBook.Quantity = book.Quantity;

            await dbContext.SaveChangesAsync();
            return existingBook;
        }
    }
}
