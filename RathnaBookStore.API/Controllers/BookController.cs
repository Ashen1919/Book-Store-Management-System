using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RathnaBookStore.API.Data;
using RathnaBookStore.API.Models.Domains;
using RathnaBookStore.API.Models.DTO.BookDto;
using RathnaBookStore.API.Repositories.BookRepository;

namespace RathnaBookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BookStoreDbContext dbContext;
        private readonly IBookRepository bookRepository;
        private readonly IMapper mapper;

        public BookController(BookStoreDbContext dbContext, IBookRepository bookRepository, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.bookRepository = bookRepository;
            this.mapper = mapper;
        }

        //Create a book
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateBook([FromBody] AddBookRequestDto addBookRequestDto)
        {
            //Map Dto to domain model
            var bookDomainModel = mapper.Map<Book>(addBookRequestDto);

            //Use domain model to create Book
            bookDomainModel = await bookRepository.CreateBookAsync(bookDomainModel);

            //Map domain model to Dto
            var bookDto = mapper.Map<BookDto>(bookDomainModel);

            return Ok(bookDto);
        }

        //Get All Books
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllBooks([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            //Get data from database to Domain model
            var bookDomainModel = await bookRepository.GetAllBooksAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);

            //Map Domain Model to Dto
            var bookDto = mapper.Map<List<BookDto>>(bookDomainModel);

            return Ok(bookDto);
        }

        //Update Books
        [HttpPut]
        [Authorize]
        [Route("{id}")]
        public async Task<IActionResult> UpdateBooks([FromRoute] Guid id, UpdateBookRequestDto updateBookRequestDto)
        {
            var bookDomainModel = mapper.Map<Book>(updateBookRequestDto);

            bookDomainModel = await bookRepository.UpdateBooksAsync(id, bookDomainModel);

            if(bookDomainModel == null)
            {
                return NotFound();
            }

            var bookDto = mapper.Map<BookDto>(bookDomainModel);

            return Ok(bookDto);
        }

        //Delete Book
        [HttpDelete]
        [Authorize]
        [Route("{id}")]
        public async Task<IActionResult> DeleteBook([FromRoute] Guid id)
        {
            var bookDomainModel = await bookRepository.DeleteBookAsync(id);

            if( bookDomainModel == null)
            {
                return NotFound();
            }

            var bookDto = mapper.Map<BookDto>(bookDomainModel);

            return Ok(bookDto);
        }

        //Get Book by ID
        [HttpGet]
        [Authorize]
        [Route("{id}")]
        public async Task<IActionResult> GetBooksById([FromRoute] Guid id)
        {
            var bookDomainModel = await bookRepository.GetBookByIdAsync(id);

            if(bookDomainModel == null)
            {
                return NotFound();
            }

            var bookDto = mapper.Map<BookDto>(bookDomainModel);

            return Ok(bookDto);
        }
    }
}
