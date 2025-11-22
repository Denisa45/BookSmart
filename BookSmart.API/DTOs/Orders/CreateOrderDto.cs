using System.ComponentModel.DataAnnotations;

namespace BookSmart.API.DTOs.Orders
{
    public class CreateOrderDto
    {
        [Required]
        public string BookId { get; set; }

        [Required]
        public string CustomerId { get; set; }

        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100.")]
        public int Quantity { get; set; }
    }
}
