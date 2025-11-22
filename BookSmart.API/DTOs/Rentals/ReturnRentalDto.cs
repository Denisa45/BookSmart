using System.ComponentModel.DataAnnotations;

namespace BookSmart.API.DTOs.Rentals
{
    public class ReturnRentalDto
    {
        [Required]
        public DateTime ReturnedAt { get; set; }
    }
}
