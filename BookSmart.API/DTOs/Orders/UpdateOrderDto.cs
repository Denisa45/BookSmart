using System.ComponentModel.DataAnnotations;

namespace BookSmart.API.DTOs.Orders
{
    public class UpdateOrderDto
    {
        [Range(1, 100)]
        public int Quantity { get; set; }

        public string Status { get; set; }
    }
}
