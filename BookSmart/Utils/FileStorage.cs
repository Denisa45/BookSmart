using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;

using BookSmart.Data.Models;
using BookSmart.Data.Services;   // <--- IMPORTANT
namespace BookSmart.Utils
{
    public class FileStorage 
    {
        //Books

        //File → Memory
        public static async Task<List<Book>> LoadBooksAsync(string path)
        {
            var result = new List<Book>();

            if (!File.Exists(path)) return result;

            var lines = await File.ReadAllLinesAsync(path);

            foreach (var line in lines.Skip(1)) // skip header
            {
                if (string.IsNullOrEmpty(line)) continue;
                var c = SplitCsv(line);

                if (c.Length < 5) continue;
                result.Add(new Book
                {
                    Id = c[0],
                    Title = c[1],
                    Author = c[2],
                    TotalCopies = int.Parse(c[3], CultureInfo.InvariantCulture), //conversion doesn’t depend on your system’s language or region.
                    AvailableCopies = int.Parse(c[4], CultureInfo.InvariantCulture),
                });
            }
            return result;
        }

        //Memory → File
        public static async Task SaveBooksAsync(string path, IEnumerable<Book> books)
        {
            var lines = new List<string> { "Id,Title,Author,TotalCopies,AvailableCopies" };

            lines.AddRange(
                books.Select(b =>
                    string.Join(',',
                        EscapeCsv(b.Id),
                        EscapeCsv(b.Title),
                        EscapeCsv(b.Author), // strings may have commas/quotes
                        b.TotalCopies.ToString(CultureInfo.InvariantCulture), // ensure consistent number format
                        b.AvailableCopies.ToString(CultureInfo.InvariantCulture)
                    )
                )
            );

            await File.WriteAllLinesAsync(path, lines);
        }

        //Customers
        public async static Task<List<Customer>> LoadCustomersAsync(string path)
        {
            var result = new List<Customer>();
            if (!File.Exists(path)) return result;
            var lines = await File.ReadAllLinesAsync(path);
            foreach (var line in lines.Skip(1)) // skip header
            {
                if (string.IsNullOrEmpty(line)) continue;
                var c = SplitCsv(line);
                if (c.Length < 2) continue;
                result.Add(new Customer
                {
                    Id = c[0],
                    FullName = c[1],
                });
            }
            return result;
        }
        public async static Task SaveCustomersAsync(string path, IEnumerable<Customer> customers)
        {
            var lines = new List<string> { "ID,Name" };
            lines.AddRange(
                customers.Select(c =>
                    string.Join(',',
                        EscapeCsv(c.Id),
                        EscapeCsv(c.FullName) // strings may have commas/quotes
                    )
                )
            );
            await File.WriteAllLinesAsync(path, lines);
        }

        // RENTALS
        public static async Task<List<Rental>> LoadRentalsAsync(string path)
        {
            var result = new List<Rental>();
            if (!File.Exists(path)) return result;

            var lines = await File.ReadAllLinesAsync(path);
            foreach (var line in lines.Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var c = SplitCsv(line);
                if (c.Length < 7) continue;
                result.Add(new Rental
                {
                    Id = c[0],
                    BookId = c[1],
                    BookTitle = c[2],
                    CustomerName = c[3],
                    RentedAt = DateTime.Parse(c[4], CultureInfo.InvariantCulture),
                    DueAt = DateTime.Parse(c[5], CultureInfo.InvariantCulture),
                    ReturnedAt = string.IsNullOrWhiteSpace(c[6]) ? null : DateTime.Parse(c[6], CultureInfo.InvariantCulture)
                });
            }
            return result;
        }

        public static async Task SaveRentalsAsync(string path, IEnumerable<Rental> rentals)
        {
            var lines = new List<string> { "Id,BookId,BookTitle,CustomerName,RentedAt,DueAt,ReturnedAt" };
            lines.AddRange(rentals.Select(r =>
                string.Join(',',
                    EscapeCsv(r.Id),
                    EscapeCsv(r.BookId),
                    EscapeCsv(r.BookTitle),
                    EscapeCsv(r.CustomerName),
                    r.RentedAt.ToString(CultureInfo.InvariantCulture),
                    r.DueAt.ToString(CultureInfo.InvariantCulture),
                    r.ReturnedAt?.ToString(CultureInfo.InvariantCulture) ?? "")));
            await File.WriteAllLinesAsync(path, lines);
        }

        // ORDERS
        public static async Task<List<Order>> LoadOrdersAsync(string path)
        {
            var result = new List<Order>();
            if (!File.Exists(path)) return result;

            var lines = await File.ReadAllLinesAsync(path);
            foreach (var line in lines.Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var c = SplitCsv(line);
                if (c.Length < 6) continue;
                result.Add(new Order
                {
                    Id = c[0],
                    BookTitle = c[1],
                    CustomerName = c[2],
                    OrderedAt = DateTime.Parse(c[3], CultureInfo.InvariantCulture),
                    EstimatedDelivery = DateTime.Parse(c[4], CultureInfo.InvariantCulture),
                    Status = c[5]
                });
            }
            return result;
        }

        public static async Task SaveOrdersAsync(string path, IEnumerable<Order> orders)
        {
            var lines = new List<string> { "Id,BookTitle,CustomerName,OrderedAt,EstimatedDelivery,Status" };
            lines.AddRange(orders.Select(o =>
                string.Join(',',
                    EscapeCsv(o.Id),
                    EscapeCsv(o.BookTitle),
                    EscapeCsv(o.CustomerName),
                    o.OrderedAt.ToString(CultureInfo.InvariantCulture),
                    o.EstimatedDelivery.ToString(CultureInfo.InvariantCulture),
                    EscapeCsv(o.Status))));
            await File.WriteAllLinesAsync(path, lines);
        }


        //Prevent commas/quotes from breaking columns
        private static string EscapeCsv(string s) =>
            s.Contains(',') || s.Contains('"') ? $"\"{s.Replace("\"", "\"\"")}\"" : s;

        //Convert back to original text safely
        private static string[] SplitCsv(string line)
        {
            var list = new List<string>();
            bool inQuotes = false;
            var cur = new System.Text.StringBuilder();

            for (int i = 0; i < line.Length; i++)
            {
                char ch = line[i];
                if (ch == '"')
                {
                    if (inQuotes &&
                        i + 1 < line.Length &&
                        line[i + 1] == '"') // actually quote so add one
                    {
                        cur.Append('"');
                        i++; // skip the fake one
                    }
                    else inQuotes = !inQuotes; // end or start quote
                }
                else if (ch == ',' && !inQuotes) //end of one field
                {
                    list.Add(cur.ToString());
                    cur.Clear(); // fresh start for new field
                }
                else cur.Append(ch);
            }

            list.Add(cur.ToString());
            return list.ToArray();
        }
    }
}
