using System;

namespace BookSmart.Data.Models
{
    public class Customer
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        public string FullName { get; set; } = "";   
        public string Email { get; set; } = "";     

        public override string ToString() => FullName;
    }
}
