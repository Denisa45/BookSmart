using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookSmart.Data.Models
{
    public class Rental
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N"); //unique id string
        public string BookId { get; set; } = "";
        public string BookTitle { get; set; } = "";
        public string CustomerName { get; set; } = "";

        public DateTime RentedAt { get; set; }

        public DateTime DueAt { get; set; }
        public DateTime? ReturnedAt { get; set; }

        public Book? Book { get; set; }

    }
}
