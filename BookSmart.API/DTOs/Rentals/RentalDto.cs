namespace BookSmart.API.DTOs.Rentals
{
    public class RentalDto
    {
        public string Id { get; set; }

        public string BookId { get; set; }
        public string BookTitle { get; set; }

        public string CustomerId { get; set; }
        public string CustomerName { get; set; }

        public DateTime RentedAt { get; set; }
        public DateTime DueAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
    }
}
