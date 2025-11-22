using BookSmart.API.DTOs.Orders;
using BookSmart.Data.Data;
using BookSmart.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookSmart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly BookSmartContext _context;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(BookSmartContext context, ILogger<OrdersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ===========================
        //       MAPPING HELPERS
        // ===========================
        private static OrderDto ToDto(Order order) =>
            new OrderDto
            {
                Id = order.Id,
                BookId = order.BookId,
                BookTitle = order.BookTitle,
                CustomerId = order.CustomerId,
                CustomerName = order.CustomerName,
                Quantity = order.Quantity,
                OrderedAt = order.OrderedAt,
                EstimatedDelivery = order.EstimatedDelivery,
                Status = order.Status
            };

        // ===========================
        //            GET ALL
        // ===========================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            var orders = await _context.Orders
                .OrderByDescending(o => o.OrderedAt)
                .ToListAsync();

            return Ok(orders.Select(ToDto));
        }

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetByCustomer(string customerId)
        {
            var exists = await _context.Customers.AnyAsync(c => c.Id == customerId);
            if (!exists)
                return NotFound(new { error = $"Customer with id '{customerId}' not found." });

            var orders = await _context.Orders
                .Where(o => o.CustomerId == customerId)
                .ToListAsync();

            return Ok(orders.Select(ToDto));
        }

        // ===========================
        //           CREATE ORDER
        // ===========================
        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder(CreateOrderDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var book = await _context.Books.FindAsync(dto.BookId);
            if (book == null)
                return NotFound(new { error = $"Book with id '{dto.BookId}' not found." });

            var customer = await _context.Customers.FindAsync(dto.CustomerId);
            if (customer == null)
                return NotFound(new { error = $"Customer with id '{dto.CustomerId}' not found." });

            var order = new Order
            {
                Id = Guid.NewGuid().ToString("N"),
                BookId = book.Id,
                CustomerId = customer.Id,
                BookTitle = book.Title,
                CustomerName = customer.FullName,
                Quantity = dto.Quantity,
                OrderedAt = DateTime.UtcNow,
                EstimatedDelivery = DateTime.UtcNow.AddDays(7), // you can change this logic
                Status = "Pending"
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Order created: {OrderId}", order.Id);

            return CreatedAtAction(nameof(GetOrders), new { id = order.Id }, ToDto(order));
        }

        // ===========================
        //            UPDATE ORDER
        // ===========================
        [HttpPut("{id}")]
        public async Task<ActionResult<OrderDto>> UpdateOrder(string id, UpdateOrderDto dto)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound(new { error = $"Order with id '{id}' not found." });

            order.Quantity = dto.Quantity;
            if (!string.IsNullOrWhiteSpace(dto.Status))
                order.Status = dto.Status;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Order updated: {OrderId}", id);

            return Ok(ToDto(order));
        }

        // ===========================
        //            DELETE
        // ===========================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound(new { error = $"Order with id '{id}' not found." });

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Order deleted: {OrderId}", id);

            return NoContent();
        }
    }
}
