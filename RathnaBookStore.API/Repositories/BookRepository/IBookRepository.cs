using RathnaBookStore.API.Models.Domains;

namespace RathnaBookStore.API.Repositories.BookRepository
{
    public interface IBookRepository
    {
        Task<Book> CreateBookAsync(Book book);
        Task<List<Book>> GetAllBooksAsync(string? filterOn = null, string? filterQuery = null,
            string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000);

        Task<Book?> UpdateBooksAsync(Guid id, Book book);

        Task<Book?> DeleteBookAsync(Guid id);

        Task<Book?> GetBookByIdAsync(Guid id);
    }
}
