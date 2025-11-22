namespace BookSmart.Data.Models
{
    public class Order
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        // RELATIONAL FIELDS
        public string BookId { get; set; } = "";
        public Book? Book { get; set; }

        public string CustomerId { get; set; } = "";
        public Customer? Customer { get; set; }

        // LEGACY FIELDS (WinForms compatibility)
        public string BookTitle { get; set; } = "";
        public string CustomerName { get; set; } = "";

        // ORDER DATA
        public int Quantity { get; set; }
        public DateTime OrderedAt { get; set; } = DateTime.UtcNow;
        public DateTime EstimatedDelivery { get; set; }

        // Status: Pending / Arrived / Cancelled
        public string Status { get; set; } = "Pending";
    }
}
