using System.ComponentModel.DataAnnotations;

namespace BookSmart.API.DTOs.Rentals
{
    public class CreateRentalDto
    {
        [Required]
        public string BookId { get; set; }

        [Required]
        public string CustomerId { get; set; }

        [Range(1, 60)]
        public int Days { get; set; }
    }
}
