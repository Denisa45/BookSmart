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

        public RentalsController(BookSmartContext context) { _context = context; }

        // GET api/rentals
        [HttpGet]
        public async Task<IActionResult> GetRentals()
        {
            var rentals = await _context.Rentals.ToListAsync();
            return Ok(rentals);
        }

        [HttpPost]
        public async Task<IActionResult> AddRental(Rental rental)
        {
            rental.Id ??= Guid.NewGuid().ToString("N");
            await _context.Rentals.AddAsync(rental); //inserted into the Rentals table
            await _context.SaveChangesAsync(); // commits all pending changes to the database.
            return CreatedAtAction(nameof(GetRentals), new { id = rental.Id }, rental);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRental(string id, Rental rental)
        {
            var existing = await _context.Rentals.FindAsync(id);
            if (existing == null) return NotFound();
            _context.Entry(existing).CurrentValues.SetValues(rental);
            await _context.SaveChangesAsync();
            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRental(string id)
        {
            var existing = await _context.Rentals.FindAsync(id);
            if (existing == null) return NotFound();
            _context.Rentals.Remove(existing);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
