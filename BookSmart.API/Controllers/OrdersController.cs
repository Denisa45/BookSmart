using BookSmart.Data.Data;
using BookSmart.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookSmart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly BookSmartContext _context;
        public OrdersController(BookSmartContext context) { _context = context; }

        // GET api/orders
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _context.Orders.ToListAsync();
            return Ok(orders);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder(Order order)
        {
            order.Id ??= Guid.NewGuid().ToString("N");
            await _context.Orders.AddAsync(order); //inserted into the Orders table
            await _context.SaveChangesAsync(); // commits all pending changes to the database.
            return CreatedAtAction(nameof(GetOrders), new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(string id, Order order)
        {
            var existing = await _context.Orders.FindAsync(id);
            if (existing == null) return NotFound();
            _context.Entry(existing).CurrentValues.SetValues(order);
            await _context.SaveChangesAsync();
            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            var existing = await _context.Orders.FindAsync(id);
            if (existing == null) return NotFound();
            _context.Orders.Remove(existing);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

}
