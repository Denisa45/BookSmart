using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookSmart.Data.Models
{
    public class Order
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N"); //unique id string
        public string BookTitle { get; set; } = "";
        public string CustomerName { get; set; } = "";
        public DateTime OrderedAt { get; set; }

        public DateTime EstimatedDelivery { get; set; }

        public string Status { get; set; } = "Pending";  // Pending / Arrived / Cancelled
    }
}
