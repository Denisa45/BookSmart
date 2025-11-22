using BookSmart.API.DTOs.Customers;
using BookSmart.Data.Data;
using BookSmart.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookSmart.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly BookSmartContext _context;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(BookSmartContext context, ILogger<CustomersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        //mapping 
        private static CustomerDto ToDto(Customer customer) {
            return new CustomerDto
            {
                Id= customer.Id,
                FullName= customer.FullName,
                Email= customer.Email
            };
        }

        private Customer FromCreateDto(CreateCustomerDto dto) {
            return new Customer
            {
                Id= Guid.NewGuid().ToString("N"),
                FullName= dto.FullName,
                Email= dto.Email
            };
        }

        private void ApplyUpdate(Customer customer, UpdateCustomerDto dto) {
            customer.FullName = dto.FullName;
            customer.Email = dto.Email;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
        {
            _logger.LogInformation("Fetching all customers");

            var customers = await _context.Customers.ToListAsync();

            return Ok(customers.Select(ToDto));
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomerById(string id)
        {
            _logger.LogInformation("Fetching customer with id: {Id}", id);

            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                _logger.LogWarning("Customer not found: {Id}", id);
                return NotFound(new { error = $"Customer with id '{id}' not found." });
            }

            return Ok(ToDto(customer));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> AddCustomer(CreateCustomerDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var customer = FromCreateDto(dto);
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Created new customer with id: {Id}", customer.Id);
            return CreatedAtAction(nameof(GetCustomerById), new { id = customer.Id }, ToDto(customer));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerDto>> UpdateCustomer(string id, UpdateCustomerDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                _logger.LogWarning("Update failed: Customer not found {Id}", id);
                return NotFound(new { error = $"Customer with id '{id}' not found." });
            }

            ApplyUpdate(customer, dto);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Customer updated: {Id}", id);

            return Ok(ToDto(customer));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(string id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                _logger.LogWarning("Delete failed: Customer not found {Id}", id);
                return NotFound(new { error = $"Customer with id '{id}' not found." });
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Customer deleted: {Id}", id);

            return NoContent();
        }
    }
}
