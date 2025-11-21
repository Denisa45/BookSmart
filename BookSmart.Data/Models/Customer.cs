using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookSmart.Data.Models
{
    public class Customer
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N"); //unique id string
        public string Name { get; set; } = "";

        public override string ToString() => Name;
    }
}
