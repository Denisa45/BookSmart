using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using BookSmart.Data.Models;
using BookSmart.Utils;             
using BookSmart.Data.Services;   

namespace BookSmart
{
    public partial class MainForm : Form
    {
        // -------------------- DATA STORAGE ------------------------
        private AppConfig _config = null!;
        //private readonly DatabaseStorage storage = new();

        private IStorage storage;
        // In-memory cache of DB data (kept in sync with DB)
        private List<Book> _books = new();
        private List<Customer> _customers = new();
        private List<Rental> _rentals = new();
        private List<Order> _orders = new();

        private Order? ActiveOrder;

        // -------------------- SIMULATED CLOCK ------------------------
        private DateTime simulatedNow = DateTime.Now;
        private System.Windows.Forms.Timer? dayTimer;

        public MainForm()
        {
            InitializeComponent();

            txtBookTitle.PlaceholderText = "Book Title...";
            cmbCustomer.Text = "";

            storage = new DatabaseStorage(); // default to local DB


            // Hide UI at startup
            ToggleUI(false);

            // Run async init after the form is shown (so UI doesn't freeze)
            Shown += async (_, __) => await InitAsync();
        }

        // -------------------- INITIALIZATION ------------------------
        private async Task InitAsync()
        {
            try
            {
                // 1) Try API first
                var api = new ApiStorage("https://localhost:7223");
                await api.GetBooksAsync(); // test
                storage = api;
                MessageBox.Show("Connected using API.");
            }
            catch
            {
                try
                {
                    // 2) Try SQLite database
                    var db = new DatabaseStorage();
                    await db.GetBooksAsync(); // test
                    storage = db;
                    MessageBox.Show("Using local SQLite database.");
                }
                catch
                {
                    // 3) Fall back to local text files
                    storage = new FileStorageWrapper();
                    MessageBox.Show("Using FILES (txt mode).");
                }
            }

            // Load configuration from file (still using your existing config system)
            _config = await AppConfig.LoadAsync(Paths.Config);

            // Load data from database asynchronously
            _books = await storage.GetBooksAsync();
            _customers = await storage.GetCustomersAsync();
            _rentals = await storage.GetRentalsAsync();
            _orders = await storage.GetOrdersAsync();

            // Start simulated days (5 seconds = 1 day)
            dayTimer = new System.Windows.Forms.Timer();
            dayTimer.Interval = 5000;
            dayTimer.Tick += DayTimer_Tick;
            dayTimer.Start();

            WriteLine($"Simulated time started at {simulatedNow:d}");

            if (!_customers.Any())
            {
                var seed = new List<Customer>
                    {
                        new Customer { Name = "Ion" },
                        new Customer { Name = "Denisa" },
                        new Customer { Name = "Mara" }
                    };

                foreach (var c in seed)
                    await storage.AddCustomerAsync(c);

                _customers = await storage.GetCustomersAsync();
            }


            if (!_books.Any())
            {
                var seed = new List<Book>
                    {
                        new Book { Id = Guid.NewGuid().ToString("N"), Title = "Harry Potter", Author = "Rowling", TotalCopies = 5, AvailableCopies = 5 },
                        new Book { Id = Guid.NewGuid().ToString("N"), Title = "Dune", Author = "Herbert", TotalCopies = 3, AvailableCopies = 3 },
                        new Book { Id = Guid.NewGuid().ToString("N"), Title = "Inferno", Author = "Dan Brown", TotalCopies = 4, AvailableCopies = 4 }
                    };

                foreach (var b in seed)
                    await storage.AddBookAsync(b);

                _books = await storage.GetBooksAsync();
            }




            // Normalize missing IDs (if old data was migrated from txt/JSON)
            foreach (var b in _books.Where(b => string.IsNullOrWhiteSpace(b.Id)))
            {
                b.Id = Guid.NewGuid().ToString("N");
                await storage.UpdateBookAsync(b);
            }

            // Fill UI data
            cmbCustomer.DataSource = _customers.Select(c => c.Name).ToList();
            RefreshBookDropdown();

            WriteLine("Loaded data.");
        }

        // -------------------- TIMER (SIMULATED DAYS) ------------------------
        private async void DayTimer_Tick(object? sender, EventArgs e)
        {
            simulatedNow = simulatedNow.AddDays(1);
            monthCalendar1.SetDate(simulatedNow);
            WriteLine($"Simulated day advanced → {simulatedNow:d}");

            // update progress bar for active order
            if (ActiveOrder != null && ActiveOrder.Status == "Pending")
            {
                double totalDays = (ActiveOrder.EstimatedDelivery - ActiveOrder.OrderedAt).TotalDays;
                double passedDays = (simulatedNow - ActiveOrder.OrderedAt).TotalDays;
                int percent = (int)((passedDays / totalDays) * 100);
                if (percent > 100) percent = 100;

                progressBar1.Value = percent;

                if (percent == 100)
                {
                    MessageBox.Show($"Order '{ActiveOrder.BookTitle}' has arrived!");
                    ActiveOrder = null; // stop updating
                }
            }

            bool changed = false;

            // Check orders that should arrive
            foreach (var o in _orders.Where(o => o.Status == "Pending" && o.EstimatedDelivery <= simulatedNow))
            {
                o.Status = "Arrived";
                WriteLine($"📦 Order for '{o.BookTitle}' arrived!");

                var existing = _books.FirstOrDefault(b =>
                    b.Title.Equals(o.BookTitle, StringComparison.OrdinalIgnoreCase));

                if (existing != null)
                {
                    existing.TotalCopies++;
                    existing.AvailableCopies++;
                    await storage.UpdateBookAsync(existing);
                }
                else
                {
                    var newBook = new Book
                    {
                        Id = Guid.NewGuid().ToString("N"),
                        Title = o.BookTitle,
                        Author = "Unknown",
                        TotalCopies = 1,
                        AvailableCopies = 1
                    };

                    _books.Add(newBook);
                    await storage.AddBookAsync(newBook);
                }

                await storage.UpdateOrderAsync(o);
                changed = true;
            }

            if (changed)
            {
                RefreshBookDropdown();
                RefreshBookGrid();
            }
        }

        // -------------------- UI Helpers ------------------------
        private void RefreshBookGrid()
        {
            if (!dgvBooks.Visible)
                return; // ⛔ Do NOT show grid until ToggleUI(true)

            dgvBooks.DataSource = _books.Select(b => new
            {
                b.Title,
                b.Author,
                b.TotalCopies,
                b.AvailableCopies,
                Status = b.AvailableCopies > 0 ? "Available" : "Rented Out"
            }).ToList();
        }

        private void RefreshBookDropdown()
        {
            cmbBook.DataSource = _books
                .Where(b => b.AvailableCopies > 0)
                .Select(b => b.Title)
                .ToList();
        }

        private void WriteLine(string msg)
        {
            lstOutput.Items.Add($"{DateTime.Now:HH:mm:ss}  {msg}");
        }

        // -------------------- SEARCH ------------------------
        private void btnSearch_Click(object sender, EventArgs e)
        {
            var query = txtBookTitle.Text.Trim();
            if (string.IsNullOrEmpty(query))
            {
                lblStatus.Text = "Enter a title or author.";
                return;
            }

            var matches = _books
                .Where(b => b.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                            b.Author.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (matches.Count == 0)
            {
                lblStatus.Text = $"No matches found for '{query}'.";
                WriteLine(lblStatus.Text);
                return;
            }

            dgvBooks.DataSource = matches.Select(b => new
            {
                b.Title,
                b.Author,
                b.TotalCopies,
                b.AvailableCopies,
                Status = b.AvailableCopies > 0 ? "Available" : "Rented Out"
            }).ToList();

            lblStatus.Text = $"Found {matches.Count} matching books.";
            WriteLine(lblStatus.Text);
            ToggleUI(true); // reveal advanced UI
        }

        // -------------------- RENT ------------------------
        private async void btnRent_Click(object sender, EventArgs e)
        {
            var customerName = cmbCustomer.Text.Trim();
            var title = cmbBook.Text.Trim();

            if (string.IsNullOrWhiteSpace(customerName) || string.IsNullOrWhiteSpace(title))
            {
                lblStatus.Text = "Select customer and book.";
                return;
            }

            var book = _books.FirstOrDefault(b =>
                b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

            if (book == null || !book.InStock())
            {
                lblStatus.Text = "Book unavailable.";
                return;
            }

            // Create customer if missing
            var customer = _customers.FirstOrDefault(c =>
                c.Name.Equals(customerName, StringComparison.OrdinalIgnoreCase));

            if (customer == null)
            {
                customer = new Customer { Name = customerName };
                _customers.Add(customer);
                await storage.AddCustomerAsync(customer);
            }

            var rental = new Rental
            {
                BookId = book.Id,
                BookTitle = book.Title,
                CustomerName = customerName,
                RentedAt = simulatedNow,
                DueAt = simulatedNow.AddDays(_config.DefaultRentalDays)
            };

            _rentals.Add(rental);
            await storage.AddRentalAsync(rental);

            book.AvailableCopies--;
            await storage.UpdateBookAsync(book);

            lblStatus.Text = $"Rented '{book.Title}' to {customerName}.";
            WriteLine(lblStatus.Text);

            RefreshBookGrid();
            RefreshBookDropdown();
            ToggleUI(true); // reveal advanced UI
        }

        // -------------------- ORDER ------------------------
        private async void btnOrder_Click(object sender, EventArgs e)
        {
            var customer = cmbCustomer.Text.Trim();
            var title = txtBookTitle.Text.Trim();

            if (string.IsNullOrWhiteSpace(customer) || string.IsNullOrWhiteSpace(title))
            {
                lblStatus.Text = "Enter customer and book title.";
                return;
            }

            int days = 5;

            using (var prompt = new Form())
            {
                prompt.Width = 250;
                prompt.Height = 140;

                var tb = new TextBox { Left = 20, Top = 20, Width = 180, Text = "5" };
                var ok = new Button { Left = 70, Top = 60, Width = 80, Text = "OK", DialogResult = DialogResult.OK };

                prompt.Controls.Add(tb);
                prompt.Controls.Add(ok);
                prompt.AcceptButton = ok;

                if (prompt.ShowDialog() == DialogResult.OK && int.TryParse(tb.Text, out var d))
                    days = d;
            }

            var order = new Order
            {
                BookTitle = title,
                CustomerName = customer,
                OrderedAt = simulatedNow,
                EstimatedDelivery = simulatedNow.AddDays(days),
                Status = "Pending"
            };
            ActiveOrder = order;
            progressBar1.Value = 0;
            progressBar1.Visible = true;

            _orders.Add(order);
            await storage.AddOrderAsync(order);

            lblStatus.Text = $"Order placed for '{title}'. Delivery: {days} days.";
            WriteLine(lblStatus.Text);
        }

        // -------------------- CHECK FEES ------------------------
        private void btnCheckFees_Click(object sender, EventArgs e)
        {
            lstOutput.Items.Clear();

            var now = simulatedNow;
            decimal total = 0;
            int count = 0;

            foreach (var r in _rentals.Where(r => r.ReturnedAt == null && r.DueAt < now))
            {
                var fee = FeeCalculator.CalculateLateFee(r.DueAt, now, _config.FeePerDay);
                total += fee;
                count++;

                WriteLine($"{r.CustomerName} owes {fee:C} for '{r.BookTitle}'.");
            }

            lblStatus.Text = count == 0
                ? "No overdue rentals."
                : $"{count} overdue, total {total:C}";
        }

        // -------------------- RETURN ------------------------
        private async void btnReturn_Click(object sender, EventArgs e)
        {
            var customer = cmbCustomer.Text.Trim();
            var title = txtBookTitle.Text.Trim();

            var rental = _rentals.FirstOrDefault(r =>
                r.CustomerName.Equals(customer, StringComparison.OrdinalIgnoreCase) &&
                r.BookTitle.Equals(title, StringComparison.OrdinalIgnoreCase) &&
                r.ReturnedAt == null);

            if (rental == null)
            {
                lblStatus.Text = "No matching rental.";
                return;
            }

            rental.ReturnedAt = simulatedNow;
            await storage.UpdateRentalAsync(rental);

            var book = _books.First(b => b.Title == title);
            book.AvailableCopies++;
            await storage.UpdateBookAsync(book);

            var fee = FeeCalculator.CalculateLateFee(rental.DueAt, simulatedNow, _config.FeePerDay);

            lblStatus.Text = fee == 0 ? "Returned with no fee." : $"Late fee: {fee:C}";
            WriteLine(lblStatus.Text);

            RefreshBookGrid();
            RefreshBookDropdown();
        }

        // -------------------- SHOW ALL ------------------------
        private void btnShowAll_Click(object sender, EventArgs e)
        {
            RefreshBookGrid();
            lblStatus.Text = "Showing all books.";
        }

        private void cmbBook_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBook.SelectedItem != null)
                txtBookTitle.Text = cmbBook.SelectedItem.ToString();
        }

        private void ToggleUI(bool showExtra)
        {
            cmbBook.Visible = showExtra;

            btnRent.Visible = showExtra;
            btnOrder.Visible = showExtra;
            btnReturn.Visible = showExtra;
            btnCheckFees.Visible = showExtra;
            btnShowAll.Visible = showExtra;

            if (!showExtra)
            {
                dgvBooks.DataSource = null; // <--- THIS REMOVES THE GRAY PANEL
            }
            dgvBooks.Height = showExtra ? 384 : 0;   // <--- hide visual area
            dgvBooks.Visible = showExtra;
        }

        private void cmbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckRevealCondition();
        }

        private void txtBookTitle_TextChanged(object sender, EventArgs e)
        {
            CheckRevealCondition();
        }

        private void CheckRevealCondition()
        {
            if (!string.IsNullOrWhiteSpace(cmbCustomer.Text) &&
                !string.IsNullOrWhiteSpace(txtBookTitle.Text))
            {
                ToggleUI(true);
            }
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            simulatedNow = e.Start;
            WriteLine($"Simulated date set to {simulatedNow:d}");
        }

        private void dgvBooks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
