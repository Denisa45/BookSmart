using BookSmart.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BookSmart.Data.Services
{
    public class ApiStorage : IStorage
    {   
        private readonly HttpClient _http;

        public ApiStorage(string baseUrl)
        {
            _http = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
        }
        public async Task AddBookAsync(Book book)
        {
            await _http.PostAsJsonAsync("/api/books", book);
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            await _http.PostAsJsonAsync("/api/customers", customer);
        }

        public async Task AddOrderAsync(Order order)
        {
            await _http.PostAsJsonAsync("/api/orders", order);
        }

        public async Task AddRentalAsync(Rental rental)
        {
            var response =  await _http.PostAsJsonAsync("/api/rentals", rental);
            response.EnsureSuccessStatusCode();
        }

        public async Task<Book?> GetBookByTitleAsync(string title)
        {
            return await _http.GetFromJsonAsync<Book>($"api/books/title/{title}");
        }

        public async Task<List<Book>> GetBooksAsync()
        {
            return await _http.GetFromJsonAsync<List<Book>>("api/books") ?? new List<Book>();
        }

        public async Task<List<Customer>> GetCustomersAsync()
        {
           return await _http.GetFromJsonAsync<List<Customer>>("api/customers") ?? new List<Customer>();
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            return await _http.GetFromJsonAsync<List<Order>>("api/orders") ?? new List<Order>();
        }

        public async Task<List<Rental>> GetRentalsAsync()
        {
            return await _http.GetFromJsonAsync<List<Rental>>("api/rentals") ?? new List<Rental>();
        }

        public async Task UpdateBookAsync(Book book)
        {
            var response=  await _http.PutAsJsonAsync($"/api/books/{book.Id}", book);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateOrderAsync(Order order)
        {
            var response =  await _http.PutAsJsonAsync($"/api/orders/{order.Id}", order);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateRentalAsync(Rental rental)
        {
            var response =  await _http.PutAsJsonAsync($"/api/rentals/{rental.Id}", rental);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteBookAsync(string id)
        {
            var response = await _http.DeleteAsync($"/api/books/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteCustomerAsync(string id)
        {
            var response = await _http.DeleteAsync($"/api/customers/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteRentalAsync(string id)
        {
            var response = await _http.DeleteAsync($"/api/rentals/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteOrderAsync(string id)
        {
            var response = await _http.DeleteAsync($"/api/orders/{id}");
            response.EnsureSuccessStatusCode();
        }

    }
}
