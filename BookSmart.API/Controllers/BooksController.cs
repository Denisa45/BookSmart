using BookSmart.API.DTOs.Books;
using BookSmart.Data.Data;
using BookSmart.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace BookSmart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookSmartContext _context;
        private readonly ILogger<BooksController> _logger;

        public BooksController(BookSmartContext context, ILogger<BooksController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // mapping methods 
        private static BookDto ToDto(Book book) =>
            new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                TotalCopies = book.TotalCopies,
                AvailableCopies = book.AvailableCopies
            };
        private Book FromCreateDto(CreateBookDto dto) =>
            new Book
            {   
                Id=Guid.NewGuid().ToString("N"),
                Title = dto.Title,
                Author = dto.Author,
                TotalCopies = dto.TotalCopies,
                AvailableCopies = dto.TotalCopies
            };
        private void ApplyUpdate(Book book, UpdateBookDto dto)
        {
            book.Title = dto.Title;
            book.Author = dto.Author;
            book.TotalCopies = dto.TotalCopies;

            if (book.AvailableCopies > dto.TotalCopies)
            {
                book.AvailableCopies = dto.TotalCopies;
            }
        }

        [HttpGet]
        // async for calling EF Core methods
        //Task for returning a value asynchronously
        // IactionResult for Returning a valid object OR an error response.”
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
        {
            var books = await _context.Books.ToListAsync();
            _logger.LogInformation("Getting all books");
            return Ok(books);
        }

        [HttpGet("title/{title}")]
        public async Task<ActionResult<BookDto>> GetByTitle(string title)
        {
            var book = await _context.Books
                .Where(b => b.Title.Equals(title))
                .FirstOrDefaultAsync();

            if (book == null)
            {
                _logger.LogWarning("Book not found with title: {Title}", title);
                return NotFound(new { error = $"Book with title '{title}' not found." });
            }

            return Ok(ToDto(book));
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<BookDto>>> SearchResult([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest(new { error = "Query parameter cannot be empty." });

            var normalized = query.Trim().ToLower();

            var results = await _context.Books
                .Where(b =>
                    b.Title.ToLower().Contains(normalized) ||
                    b.Author.ToLower().Contains(normalized)
                )
                .Select(b => ToDto(b))   // Apply static mapping HERE too ✔
                .ToListAsync();

            _logger.LogInformation("Searching books with query: {Query}", query);

            return Ok(results);
        }


        [HttpPost]
        //DTO in → Entity → Save → DTO out
        public async Task<ActionResult<BookDto>> AddBook(CreateBookDto createBookDto)
        {
            if (!ModelState.IsValid) { 
                return BadRequest(ModelState);
            }
            var book= FromCreateDto(createBookDto); // now we have a book 
            await _context.Books.AddAsync(book); // “EF, please insert this when you're ready.”
            await _context.SaveChangesAsync(); // Now the book is officially stored in SQLite.
            var result = ToDto(book);
            _logger.LogInformation("Book created: {Title} ({Id})", book.Title, book.Id);
            return CreatedAtAction(nameof(GetByTitle), new { title = book.Title }, result); //“I have created your book.You can retrieve it later using this GET endpoint.
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BookDto>> UpdateBook(string id, UpdateBookDto updateBookDto)
        {
            if(ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var existing= await _context.Books.FindAsync(id);
            if (existing == null)
            {
                _logger.LogWarning("Delete failed: Book not found with id: {Id}", id);
                return NotFound(new { error = $"Book with id '{id}' not found." });
            }

            ApplyUpdate(existing, updateBookDto);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Book updated: {Id}", id);
            return Ok(ToDto(existing));

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(string id)
        {
            var existing = await _context.Books.FindAsync(id);
            if (existing == null)
            {
                return Accepted(new { message = $"Book with id '{id}' not found. No action taken." });
            }
            _context.Books.Remove(existing);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Book deleted: {Id}", id);
            return NoContent();
        }





    }
}
