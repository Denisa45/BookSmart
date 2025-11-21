using Microsoft.EntityFrameworkCore;
using BookSmart.Data.Models;

namespace BookSmart.Data.Data
{
    public class BookSmartContext : DbContext
    {
        // DI constructor (used by API)
        public BookSmartContext(DbContextOptions<BookSmartContext> options)
            : base(options)
        {
        }

        // Parameterless constructor (used by WinForms)
        public BookSmartContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=booksmart.db");
            }
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Order> Orders { get; set; }
    }

}
