using BookSmart.Data.Models;

namespace BookSmart.Data.Services
{
    public interface IStorage
    {
        // BOOKS
        Task<List<Book>> GetBooksAsync();
        Task<Book?> GetBookByTitleAsync(string title);
        Task AddBookAsync(Book book);
        Task UpdateBookAsync(Book book);
        Task DeleteBookAsync(string id);

        // CUSTOMERS
        Task<List<Customer>> GetCustomersAsync();
        Task AddCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(string id);

        // RENTALS
        Task<List<Rental>> GetRentalsAsync();
        Task AddRentalAsync(Rental rental);
        Task UpdateRentalAsync(Rental rental);
        Task DeleteRentalAsync(string id);

        // ORDERS
        Task<List<Order>> GetOrdersAsync();
        Task AddOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(string id);
    }

}
