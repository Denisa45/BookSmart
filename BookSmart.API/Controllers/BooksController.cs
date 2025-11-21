using BookSmart.Data.Data;
using BookSmart.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookSmart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookSmartContext _context;

        public BooksController(BookSmartContext context) { _context = context; }

        // GET api/books
        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _context.Books.ToListAsync();
            return Ok(books);
        }

        [HttpGet("title/{title}")]
        public async Task<IActionResult> GetByTitle(string title)
        {
            var book = await _context.Books.
                                  FirstOrDefaultAsync(b=> b.Title == title);
            if (book == null) return NotFound();

            return Ok(book);
        }

        // POST api/books
        [HttpPost]
        public async Task<IActionResult> AddBook(Book book)
        {
            book.Id ??= Guid.NewGuid().ToString("N");
            await _context.Books.AddAsync(book); //inserted into the Books table
            await _context.SaveChangesAsync(); // commits all pending changes to the database.
            return CreatedAtAction(nameof(GetByTitle), new { title = book.Title }, book);
        }

        // PUT api/books/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(string id, Book updated)
        {
            var existing = await _context.Books.FindAsync(id);
            if (existing == null) return NotFound();
            existing.Title = updated.Title;
            existing.Author = updated.Author;
            existing.TotalCopies = updated.TotalCopies;
            existing.AvailableCopies = updated.AvailableCopies;

            await _context.SaveChangesAsync();

            await _context.SaveChangesAsync();
            return Ok(existing);
        }

        //DELETE api/books/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(string id)
        {
            var existing = await _context.Books.FindAsync(id);
            if (existing == null) return NotFound();
            _context.Books.Remove(existing);
            await _context.SaveChangesAsync();
            return NoContent();
        }



    }
}
