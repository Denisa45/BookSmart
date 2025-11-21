using BookSmart.Data.Models;
using BookSmart.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookSmart.Utils
{
    public class FileStorageWrapper : IStorage
    {
        public Task<List<Book>> GetBooksAsync()
            => FileStorage.LoadBooksAsync("books.txt");

        public Task<List<Customer>> GetCustomersAsync()
            => FileStorage.LoadCustomersAsync("customers.txt");

        public Task<List<Order>> GetOrdersAsync()
            => FileStorage.LoadOrdersAsync("orders.txt");

        public Task<List<Rental>> GetRentalsAsync()
            => FileStorage.LoadRentalsAsync("rentals.txt");

        // WRITE SUPPORT
        public async Task AddBookAsync(Book b)
        {
            var books = await GetBooksAsync();
            books.Add(b);
            await FileStorage.SaveBooksAsync("books.txt", books);
        }

        public async Task AddCustomerAsync(Customer c)
        {
            var customers = await GetCustomersAsync();
            customers.Add(c);
            await FileStorage.SaveCustomersAsync("customers.txt", customers);
        }

        public async Task AddOrderAsync(Order o)
        {
            var orders = await GetOrdersAsync();
            orders.Add(o);
            await FileStorage.SaveOrdersAsync("orders.txt", orders);
        }

        public async Task AddRentalAsync(Rental r)
        {
            var rentals = await GetRentalsAsync();
            rentals.Add(r);
            await FileStorage.SaveRentalsAsync("rentals.txt", rentals);
        }

        public async Task UpdateBookAsync(Book b)
        {
            var books = await GetBooksAsync();
            var index = books.FindIndex(x => x.Id == b.Id);
            if (index >= 0) books[index] = b;
            await FileStorage.SaveBooksAsync("books.txt", books);
        }

        public async Task UpdateRentalAsync(Rental r)
        {
            var rentals = await GetRentalsAsync();
            var index = rentals.FindIndex(x => x.Id == r.Id);
            if (index >= 0) rentals[index] = r;
            await FileStorage.SaveRentalsAsync("rentals.txt", rentals);
        }

        public async Task UpdateOrderAsync(Order o)
        {
            var orders = await GetOrdersAsync();
            var index = orders.FindIndex(x => x.Id == o.Id);
            if (index >= 0) orders[index] = o;
            await FileStorage.SaveOrdersAsync("orders.txt", orders);
        }

        // DELETE
        public async Task DeleteBookAsync(string id)
        {
            var books = await GetBooksAsync();
            books.RemoveAll(x => x.Id == id);
            await FileStorage.SaveBooksAsync("books.txt", books);
        }

        public async Task DeleteCustomerAsync(string id)
        {
            var customers = await GetCustomersAsync();
            customers.RemoveAll(x => x.Id == id);
            await FileStorage.SaveCustomersAsync("customers.txt", customers);
        }

        public async Task DeleteRentalAsync(string id)
        {
            var rentals = await GetRentalsAsync();
            rentals.RemoveAll(x => x.Id == id);
            await FileStorage.SaveRentalsAsync("rentals.txt", rentals);
        }

        public async Task DeleteOrderAsync(string id)
        {
            var orders = await GetOrdersAsync();
            orders.RemoveAll(x => x.Id == id);
            await FileStorage.SaveOrdersAsync("orders.txt", orders);
        }

        public async Task<Book?> GetBookByTitleAsync(string title)
        {
            var books = await GetBooksAsync();
            return books.FirstOrDefault(b =>
                b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }

    }
}
