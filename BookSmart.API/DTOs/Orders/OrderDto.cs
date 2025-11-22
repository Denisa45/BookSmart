namespace BookSmart.API.DTOs.Orders
{
    public class OrderDto
    {
        public string Id { get; set; }

        public string BookId { get; set; }
        public string BookTitle { get; set; }

        public string CustomerId { get; set; }
        public string CustomerName { get; set; }

        public int Quantity { get; set; }

        public DateTime OrderedAt { get; set; }
        public DateTime EstimatedDelivery { get; set; }

        public string Status { get; set; }
    }
}
