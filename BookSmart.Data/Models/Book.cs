using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookSmart.Data.Models
{
    public class Book
    {
        public string Id { get; set; }=Guid.NewGuid().ToString("N"); //unique id string 
        public string Title { get; set; } = "";
        public string Author { get; set; } = "";
        public int TotalCopies { get; set; } = 1;
        public int AvailableCopies { get; set; } = 1;

        public bool InStock()
        {
            return AvailableCopies > 0;
        }

        public override string ToString() => $"{Title} — {Author} (Avail: {AvailableCopies}/{TotalCopies})";

    }
}
