using BookSmart.Data.Data;
using BookSmart.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BookSmart.Data.Services
{
    public class DatabaseStorage : IStorage
    {
        // BOOKS -------------------------------------------------------------

        public async Task<List<Book>> GetBooksAsync()
        {
            using var db = new BookSmartContext();
            return await db.Books.ToListAsync();
        }

        public async Task<Book?> GetBookByTitleAsync(string title)
        {
            using var db = new BookSmartContext();
            return await db.Books
                .FirstOrDefaultAsync(b => b.Title == title);
        }

        public async Task AddBookAsync(Book book)
        {
            using var db = new BookSmartContext();
            await db.Books.AddAsync(book);
            await db.SaveChangesAsync();
        }

        public async Task UpdateBookAsync(Book book)
        {
            using var db = new BookSmartContext();
            db.Books.Update(book);
            await db.SaveChangesAsync();
        }



        // CUSTOMERS ---------------------------------------------------------

        public async Task<List<Customer>> GetCustomersAsync()
        {
            using var db = new BookSmartContext();
            return await db.Customers.ToListAsync();
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            using var db = new BookSmartContext();
            await db.Customers.AddAsync(customer);
            await db.SaveChangesAsync();
        }



        // RENTALS -----------------------------------------------------------

        public async Task<List<Rental>> GetRentalsAsync()
        {
            using var db = new BookSmartContext();
            return await db.Rentals.ToListAsync();
        }

        public async Task AddRentalAsync(Rental rental)
        {
            using var db = new BookSmartContext();
            await db.Rentals.AddAsync(rental);
            await db.SaveChangesAsync();
        }

        public async Task UpdateRentalAsync(Rental rental)
        {
            using var db = new BookSmartContext();
            db.Rentals.Update(rental);
            await db.SaveChangesAsync();
        }



        // ORDERS ------------------------------------------------------------

        public async Task<List<Order>> GetOrdersAsync()
        {
            using var db = new BookSmartContext();
            return await db.Orders.ToListAsync();
        }

        public async Task AddOrderAsync(Order order)
        {
            using var db = new BookSmartContext();
            await db.Orders.AddAsync(order);
            await db.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(Order order)
        {
            using var db = new BookSmartContext();
            db.Orders.Update(order);
            await db.SaveChangesAsync();
        }

        // DELETE METHODS ----------------------------------------------------

        public async Task DeleteBookAsync(string id)
        {
            using var db = new BookSmartContext();
            var book = await db.Books.FindAsync(id);
            if (book == null) return;

            db.Books.Remove(book);
            await db.SaveChangesAsync();
        }

        public async Task DeleteCustomerAsync(string id)
        {
            using var db = new BookSmartContext();
            var customer = await db.Customers.FindAsync(id);
            if (customer == null) return;

            db.Customers.Remove(customer);
            await db.SaveChangesAsync();
        }

        public async Task DeleteRentalAsync(string id)
        {
            using var db = new BookSmartContext();
            var rental = await db.Rentals.FindAsync(id);
            if (rental == null) return;

            db.Rentals.Remove(rental);
            await db.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(string id)
        {
            using var db = new BookSmartContext();
            var order = await db.Orders.FindAsync(id);
            if (order == null) return;

            db.Orders.Remove(order);
            await db.SaveChangesAsync();
        }

    }
}
