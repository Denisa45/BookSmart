using System.Windows.Forms;

namespace BookSmart
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            cmbCustomer = new ComboBox();
            cmbBook = new ComboBox();
            txtBookTitle = new TextBox();
            btnSearch = new Button();
            btnRent = new Button();
            btnOrder = new Button();
            btnCheckFees = new Button();
            btnReturn = new Button();
            btnShowAll = new Button();
            lstOutput = new ListBox();
            lblStatus = new Label();
            dgvBooks = new DataGridView();
            monthCalendar1 = new MonthCalendar();
            progressBar1 = new ProgressBar();
            ((System.ComponentModel.ISupportInitialize)dgvBooks).BeginInit();
            SuspendLayout();
            // 
            // cmbCustomer
            // 
            cmbCustomer.Location = new Point(12, 117);
            cmbCustomer.Name = "cmbCustomer";
            cmbCustomer.Size = new Size(200, 28);
            cmbCustomer.TabIndex = 0;
            cmbCustomer.SelectedIndexChanged += cmbCustomer_SelectedIndexChanged;
            // 
            // cmbBook
            // 
            cmbBook.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBook.Location = new Point(12, 50);
            cmbBook.Name = "cmbBook";
            cmbBook.Size = new Size(200, 28);
            cmbBook.TabIndex = 1;
            cmbBook.Visible = false;
            cmbBook.SelectedIndexChanged += cmbBook_SelectedIndexChanged;
            // 
            // txtBookTitle
            // 
            txtBookTitle.Location = new Point(12, 84);
            txtBookTitle.Name = "txtBookTitle";
            txtBookTitle.Size = new Size(200, 27);
            txtBookTitle.TabIndex = 2;
            txtBookTitle.TextChanged += txtBookTitle_TextChanged;
            // 
            // btnSearch
            // 
            btnSearch.Location = new Point(12, 146);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(94, 29);
            btnSearch.TabIndex = 3;
            btnSearch.Text = "Search";
            btnSearch.Click += btnSearch_Click;
            // 
            // btnRent
            // 
            btnRent.Location = new Point(12, 316);
            btnRent.Name = "btnRent";
            btnRent.Size = new Size(94, 29);
            btnRent.TabIndex = 7;
            btnRent.Text = "Rent";
            btnRent.Visible = false;
            btnRent.Click += btnRent_Click;
            // 
            // btnOrder
            // 
            btnOrder.Location = new Point(12, 228);
            btnOrder.Name = "btnOrder";
            btnOrder.Size = new Size(94, 29);
            btnOrder.TabIndex = 5;
            btnOrder.Text = "Order";
            btnOrder.Visible = false;
            btnOrder.Click += btnOrder_Click;
            // 
            // btnCheckFees
            // 
            btnCheckFees.Location = new Point(12, 263);
            btnCheckFees.Name = "btnCheckFees";
            btnCheckFees.Size = new Size(110, 29);
            btnCheckFees.TabIndex = 6;
            btnCheckFees.Text = "Check Fees";
            btnCheckFees.Visible = false;
            btnCheckFees.Click += btnCheckFees_Click;
            // 
            // btnReturn
            // 
            btnReturn.Location = new Point(12, 181);
            btnReturn.Name = "btnReturn";
            btnReturn.Size = new Size(94, 29);
            btnReturn.TabIndex = 4;
            btnReturn.Text = "Return";
            btnReturn.Visible = false;
            btnReturn.Click += btnReturn_Click;
            // 
            // btnShowAll
            // 
            btnShowAll.Location = new Point(12, 351);
            btnShowAll.Name = "btnShowAll";
            btnShowAll.Size = new Size(110, 29);
            btnShowAll.TabIndex = 8;
            btnShowAll.Text = "Show All";
            btnShowAll.Visible = false;
            btnShowAll.Click += btnShowAll_Click;
            // 
            // lstOutput
            // 
            lstOutput.Location = new Point(283, 12);
            lstOutput.Name = "lstOutput";
            lstOutput.Size = new Size(521, 384);
            lstOutput.TabIndex = 9;
            // 
            // lblStatus
            // 
            lblStatus.Location = new Point(416, 409);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(100, 23);
            lblStatus.TabIndex = 10;
            lblStatus.Text = "Status";
            // 
            // dgvBooks
            // 
            dgvBooks.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvBooks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBooks.ColumnHeadersHeight = 29;
            dgvBooks.Cursor = Cursors.SizeNWSE;
            dgvBooks.Location = new Point(830, 12);
            dgvBooks.Name = "dgvBooks";
            dgvBooks.ReadOnly = true;
            dgvBooks.RowHeadersVisible = false;
            dgvBooks.RowHeadersWidth = 51;
            dgvBooks.Size = new Size(600, 384);
            dgvBooks.TabIndex = 11;
            dgvBooks.Visible = false;
            dgvBooks.CellContentClick += dgvBooks_CellContentClick;
            // 
            // monthCalendar1
            // 
            monthCalendar1.Location = new Point(292, 441);
            monthCalendar1.Name = "monthCalendar1";
            monthCalendar1.TabIndex = 12;
            monthCalendar1.DateChanged += monthCalendar1_DateChanged;
            // 
            // progressBar1
            // 
            progressBar1.ForeColor = Color.LimeGreen;
            progressBar1.Location = new Point(12, 403);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(125, 29);
            progressBar1.TabIndex = 13;
            progressBar1.Click += progressBar1_Click;
            // 
            // MainForm
            // 
            ClientSize = new Size(1450, 650);
            Controls.Add(progressBar1);
            Controls.Add(monthCalendar1);
            Controls.Add(cmbCustomer);
            Controls.Add(cmbBook);
            Controls.Add(txtBookTitle);
            Controls.Add(btnSearch);
            Controls.Add(btnReturn);
            Controls.Add(btnOrder);
            Controls.Add(btnCheckFees);
            Controls.Add(btnRent);
            Controls.Add(btnShowAll);
            Controls.Add(lstOutput);
            Controls.Add(lblStatus);
            Controls.Add(dgvBooks);
            Name = "MainForm";
            Text = "BookSmart";
            ((System.ComponentModel.ISupportInitialize)dgvBooks).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }


        #endregion

        private ComboBox cmbCustomer;
        private ComboBox cmbBook;
        private TextBox txtBookTitle;
        private Button btnSearch;
        private Button btnRent;
        private Button btnOrder;
        private Button btnCheckFees;
        private Button btnReturn;
        private Button btnShowAll;
        private ListBox lstOutput;
        private Label lblStatus;
        private DataGridView dgvBooks;
        private MonthCalendar monthCalendar1;
        private ProgressBar progressBar1;
    }
}
