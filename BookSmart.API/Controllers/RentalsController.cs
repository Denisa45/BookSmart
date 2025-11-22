using BookSmart.API.DTOs.Rentals;
using BookSmart.Data.Data;
using BookSmart.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookSmart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly BookSmartContext _context;
        private readonly ILogger<RentalsController> _logger;

        public RentalsController(BookSmartContext context, ILogger<RentalsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        private static RentalDto ToDto(Rental rental) =>
            new RentalDto
            {
                Id = rental.Id,
                BookId = rental.BookId,
                BookTitle = rental.BookTitle,
                CustomerId = rental.CustomerId,
                CustomerName = rental.CustomerName,
                RentedAt = rental.RentedAt,
                DueAt = rental.DueAt,
                ReturnedAt = rental.ReturnedAt
            };

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RentalDto>>> GetRentals()
        {
            _logger.LogInformation("Fetching all rentals");

            var rentals = await _context.Rentals
                .OrderByDescending(r => r.RentedAt)
                .ToListAsync();

            return Ok(rentals.Select(ToDto));
        }

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<RentalDto>>> GetByCustomer(string customerId)
        {
            var exists = await _context.Customers.AnyAsync(c => c.Id == customerId);
            if (!exists)
                return NotFound(new { error = $"Customer with id '{customerId}' not found." });

            _logger.LogInformation("Fetching rentals for customer: {CustomerId}", customerId);

            var rentals = await _context.Rentals
                .Where(r => r.CustomerId == customerId)
                .OrderByDescending(r => r.RentedAt)
                .ToListAsync();

            return Ok(rentals.Select(ToDto));
        }

        [HttpPost]
        public async Task<ActionResult<RentalDto>> CreateRental(CreateRentalDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var book = await _context.Books.FindAsync(dto.BookId);
            if (book == null)
                return NotFound(new { error = $"Book with id '{dto.BookId}' not found." });

            if (book.AvailableCopies <= 0)
                return BadRequest(new { error = $"Book '{book.Title}' is not available for rent." });

            var customer = await _context.Customers.FindAsync(dto.CustomerId);
            if (customer == null)
                return NotFound(new { error = $"Customer with id '{dto.CustomerId}' not found." });

            var rental = new Rental
            {
                Id = Guid.NewGuid().ToString("N"),
                BookId = book.Id,
                CustomerId = customer.Id,
                BookTitle = book.Title,
                CustomerName = customer.FullName,
                RentedAt = DateTime.UtcNow,
                DueAt = DateTime.UtcNow.AddDays(dto.Days),
                ReturnedAt = null
            };

            book.AvailableCopies--;

            await _context.Rentals.AddAsync(rental);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Book rented: {BookId} by customer {CustomerId}", dto.BookId, dto.CustomerId);

            return CreatedAtAction(nameof(GetRentals), new { id = rental.Id }, ToDto(rental));
        }

        [HttpPut("return/{id}")]
        public async Task<ActionResult<RentalDto>> ReturnRental(string id, ReturnRentalDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var rental = await _context.Rentals.FindAsync(id);
            if (rental == null)
                return NotFound(new { error = $"Rental with id '{id}' not found." });

            if (rental.ReturnedAt != null)
                return BadRequest(new { error = "This rental is already returned." });

            rental.ReturnedAt = dto.ReturnedAt;

            var book = await _context.Books.FindAsync(rental.BookId);
            if (book != null)
                book.AvailableCopies++;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Rental returned: {RentalId}", id);

            return Ok(ToDto(rental));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRental(string id)
        {
            var rental = await _context.Rentals.FindAsync(id);
            if (rental == null)
            {
                _logger.LogWarning("Delete failed: Rental not found {Id}", id);
                return NotFound(new { error = $"Rental with id '{id}' not found." });
            }

            if (rental.ReturnedAt == null)
            {
                var book = await _context.Books.FindAsync(rental.BookId);
                if (book != null)
                    book.AvailableCopies++;
            }

            _context.Rentals.Remove(rental);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Rental deleted: {Id}", id);

            return NoContent();
        }
    }
}
