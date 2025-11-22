namespace BookSmart.Data.Models
{
    public class Rental
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        // Relations
        public string BookId { get; set; } = "";
        public Book? Book { get; set; }

        public string CustomerId { get; set; } = "";
        public Customer? Customer { get; set; }

        // Legacy fields (optional but safe to keep for WinForms compatibility)
        public string BookTitle { get; set; } = "";
        public string CustomerName { get; set; } = "";

        // Rental dates
        public DateTime RentedAt { get; set; }
        public DateTime DueAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
    }
}
