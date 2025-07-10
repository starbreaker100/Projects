using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BusinessManagementSystem.Data;
using BusinessManagementSystem.Models;

namespace BusinessManagementSystem.Forms
{
    public partial class OrderHistoryForm : Form
    {
        private DatabaseHelper _dbHelper;
        private BindingSource _bindingSource;

        public OrderHistoryForm()
        {
            InitializeComponent();
            _dbHelper = new DatabaseHelper();
            _bindingSource = new BindingSource();
            LoadOrders();
        }

        private void InitializeComponent()
        {
            this.dgvOrders = new DataGridView();
            this.dgvOrderDetails = new DataGridView();
            this.dtpFromDate = new DateTimePicker();
            this.dtpToDate = new DateTimePicker();
            this.lblFromDate = new Label();
            this.lblToDate = new Label();
            this.btnFilter = new Button();
            this.btnViewDetails = new Button();
            this.btnPrintReceipt = new Button();
            this.btnRefresh = new Button();
            this.btnClose = new Button();
            this.groupBoxFilters = new GroupBox();
            this.groupBoxOrders = new GroupBox();
            this.groupBoxOrderDetails = new GroupBox();
            this.lblTotalOrders = new Label();
            this.lblTotalAmount = new Label();
            this.SuspendLayout();

            // 
            // groupBoxFilters
            // 
            this.groupBoxFilters.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            this.groupBoxFilters.Location = new Point(12, 12);
            this.groupBoxFilters.Name = "groupBoxFilters";
            this.groupBoxFilters.Size = new Size(1180, 80);
            this.groupBoxFilters.TabIndex = 0;
            this.groupBoxFilters.TabStop = false;
            this.groupBoxFilters.Text = "Filters";

            // 
            // lblFromDate
            // 
            this.lblFromDate.AutoSize = true;
            this.lblFromDate.Font = new Font("Microsoft Sans Serif", 9F);
            this.lblFromDate.Location = new Point(30, 35);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Size = new Size(72, 15);
            this.lblFromDate.TabIndex = 1;
            this.lblFromDate.Text = "From Date:";

            // 
            // dtpFromDate
            // 
            this.dtpFromDate.Font = new Font("Microsoft Sans Serif", 9F);
            this.dtpFromDate.Format = DateTimePickerFormat.Short;
            this.dtpFromDate.Location = new Point(110, 32);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new Size(100, 21);
            this.dtpFromDate.TabIndex = 2;
            this.dtpFromDate.Value = DateTime.Now.AddDays(-30);

            // 
            // lblToDate
            // 
            this.lblToDate.AutoSize = true;
            this.lblToDate.Font = new Font("Microsoft Sans Serif", 9F);
            this.lblToDate.Location = new Point(230, 35);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new Size(56, 15);
            this.lblToDate.TabIndex = 3;
            this.lblToDate.Text = "To Date:";

            // 
            // dtpToDate
            // 
            this.dtpToDate.Font = new Font("Microsoft Sans Serif", 9F);
            this.dtpToDate.Format = DateTimePickerFormat.Short;
            this.dtpToDate.Location = new Point(295, 32);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new Size(100, 21);
            this.dtpToDate.TabIndex = 4;
            this.dtpToDate.Value = DateTime.Now;

            // 
            // btnFilter
            // 
            this.btnFilter.BackColor = Color.FromArgb(0, 123, 255);
            this.btnFilter.ForeColor = Color.White;
            this.btnFilter.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            this.btnFilter.Location = new Point(415, 30);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new Size(80, 25);
            this.btnFilter.TabIndex = 5;
            this.btnFilter.Text = "Filter";
            this.btnFilter.UseVisualStyleBackColor = false;
            this.btnFilter.Click += new EventHandler(this.btnFilter_Click);

            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = Color.FromArgb(40, 167, 69);
            this.btnRefresh.ForeColor = Color.White;
            this.btnRefresh.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            this.btnRefresh.Location = new Point(510, 30);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new Size(80, 25);
            this.btnRefresh.TabIndex = 6;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new EventHandler(this.btnRefresh_Click);

            // 
            // groupBoxOrders
            // 
            this.groupBoxOrders.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            this.groupBoxOrders.Location = new Point(12, 100);
            this.groupBoxOrders.Name = "groupBoxOrders";
            this.groupBoxOrders.Size = new Size(1180, 300);
            this.groupBoxOrders.TabIndex = 7;
            this.groupBoxOrders.TabStop = false;
            this.groupBoxOrders.Text = "Orders";

            // 
            // dgvOrders
            // 
            this.dgvOrders.AllowUserToAddRows = false;
            this.dgvOrders.AllowUserToDeleteRows = false;
            this.dgvOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvOrders.BackgroundColor = Color.White;
            this.dgvOrders.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrders.Location = new Point(30, 130);
            this.dgvOrders.MultiSelect = false;
            this.dgvOrders.Name = "dgvOrders";
            this.dgvOrders.ReadOnly = true;
            this.dgvOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvOrders.Size = new Size(1140, 240);
            this.dgvOrders.TabIndex = 8;
            this.dgvOrders.SelectionChanged += new EventHandler(this.dgvOrders_SelectionChanged);

            // Summary Labels
            // 
            // lblTotalOrders
            // 
            this.lblTotalOrders.AutoSize = true;
            this.lblTotalOrders.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            this.lblTotalOrders.Location = new Point(30, 375);
            this.lblTotalOrders.Name = "lblTotalOrders";
            this.lblTotalOrders.Size = new Size(100, 15);
            this.lblTotalOrders.TabIndex = 9;
            this.lblTotalOrders.Text = "Total Orders: 0";

            // 
            // lblTotalAmount
            // 
            this.lblTotalAmount.AutoSize = true;
            this.lblTotalAmount.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            this.lblTotalAmount.ForeColor = Color.FromArgb(220, 53, 69);
            this.lblTotalAmount.Location = new Point(200, 375);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Size = new Size(120, 15);
            this.lblTotalAmount.TabIndex = 10;
            this.lblTotalAmount.Text = "Total Amount: ₹0.00";

            // Action Buttons
            // 
            // btnViewDetails
            // 
            this.btnViewDetails.BackColor = Color.FromArgb(255, 193, 7);
            this.btnViewDetails.ForeColor = Color.Black;
            this.btnViewDetails.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            this.btnViewDetails.Location = new Point(700, 370);
            this.btnViewDetails.Name = "btnViewDetails";
            this.btnViewDetails.Size = new Size(100, 25);
            this.btnViewDetails.TabIndex = 11;
            this.btnViewDetails.Text = "View Details";
            this.btnViewDetails.UseVisualStyleBackColor = false;
            this.btnViewDetails.Click += new EventHandler(this.btnViewDetails_Click);

            // 
            // btnPrintReceipt
            // 
            this.btnPrintReceipt.BackColor = Color.FromArgb(40, 167, 69);
            this.btnPrintReceipt.ForeColor = Color.White;
            this.btnPrintReceipt.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            this.btnPrintReceipt.Location = new Point(810, 370);
            this.btnPrintReceipt.Name = "btnPrintReceipt";
            this.btnPrintReceipt.Size = new Size(100, 25);
            this.btnPrintReceipt.TabIndex = 12;
            this.btnPrintReceipt.Text = "Print Receipt";
            this.btnPrintReceipt.UseVisualStyleBackColor = false;
            this.btnPrintReceipt.Click += new EventHandler(this.btnPrintReceipt_Click);

            // 
            // groupBoxOrderDetails
            // 
            this.groupBoxOrderDetails.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            this.groupBoxOrderDetails.Location = new Point(12, 410);
            this.groupBoxOrderDetails.Name = "groupBoxOrderDetails";
            this.groupBoxOrderDetails.Size = new Size(1180, 200);
            this.groupBoxOrderDetails.TabIndex = 13;
            this.groupBoxOrderDetails.TabStop = false;
            this.groupBoxOrderDetails.Text = "Order Details";

            // 
            // dgvOrderDetails
            // 
            this.dgvOrderDetails.AllowUserToAddRows = false;
            this.dgvOrderDetails.AllowUserToDeleteRows = false;
            this.dgvOrderDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvOrderDetails.BackgroundColor = Color.White;
            this.dgvOrderDetails.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrderDetails.Location = new Point(30, 440);
            this.dgvOrderDetails.MultiSelect = false;
            this.dgvOrderDetails.Name = "dgvOrderDetails";
            this.dgvOrderDetails.ReadOnly = true;
            this.dgvOrderDetails.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvOrderDetails.Size = new Size(1140, 150);
            this.dgvOrderDetails.TabIndex = 14;

            // 
            // btnClose
            // 
            this.btnClose.BackColor = Color.FromArgb(108, 117, 125);
            this.btnClose.ForeColor = Color.White;
            this.btnClose.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            this.btnClose.Location = new Point(1100, 630);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(80, 30);
            this.btnClose.TabIndex = 15;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new EventHandler(this.btnClose_Click);

            // 
            // OrderHistoryForm
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(1204, 680);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.dgvOrderDetails);
            this.Controls.Add(this.groupBoxOrderDetails);
            this.Controls.Add(this.btnPrintReceipt);
            this.Controls.Add(this.btnViewDetails);
            this.Controls.Add(this.lblTotalAmount);
            this.Controls.Add(this.lblTotalOrders);
            this.Controls.Add(this.dgvOrders);
            this.Controls.Add(this.groupBoxOrders);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnFilter);
            this.Controls.Add(this.dtpToDate);
            this.Controls.Add(this.lblToDate);
            this.Controls.Add(this.dtpFromDate);
            this.Controls.Add(this.lblFromDate);
            this.Controls.Add(this.groupBoxFilters);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OrderHistoryForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Order History";
            this.ResumeLayout(false);
            this.PerformLayout();

            // Initialize button states
            btnViewDetails.Enabled = false;
            btnPrintReceipt.Enabled = false;
        }

        private DataGridView dgvOrders;
        private DataGridView dgvOrderDetails;
        private DateTimePicker dtpFromDate;
        private DateTimePicker dtpToDate;
        private Label lblFromDate;
        private Label lblToDate;
        private Button btnFilter;
        private Button btnViewDetails;
        private Button btnPrintReceipt;
        private Button btnRefresh;
        private Button btnClose;
        private GroupBox groupBoxFilters;
        private GroupBox groupBoxOrders;
        private GroupBox groupBoxOrderDetails;
        private Label lblTotalOrders;
        private Label lblTotalAmount;

        private void LoadOrders()
        {
            try
            {
                var orders = _dbHelper.GetAllOrders();
                
                // Filter by date range
                var filteredOrders = orders.Where(o => 
                    o.OrderDate.Date >= dtpFromDate.Value.Date && 
                    o.OrderDate.Date <= dtpToDate.Value.Date).ToList();

                _bindingSource.DataSource = filteredOrders.Select(o => new
                {
                    OrderID = o.OrderID,
                    OrderDate = o.OrderDate.ToString("yyyy-MM-dd HH:mm"),
                    CustomerName = string.IsNullOrEmpty(o.CustomerName) ? "Walk-in Customer" : o.CustomerName,
                    CustomerPhone = o.CustomerPhone,
                    SellerName = o.SellerName,
                    SubTotal = o.SubTotal,
                    GlobalAdjustment = $"{o.GlobalAdjustmentPercentage:0.##}%",
                    TotalAmount = o.TotalAmount,
                    OrderStatus = o.OrderStatus
                }).ToList();

                dgvOrders.DataSource = _bindingSource;

                // Configure columns
                if (dgvOrders.Columns.Count > 0)
                {
                    dgvOrders.Columns["OrderID"].HeaderText = "Order ID";
                    dgvOrders.Columns["OrderID"].Width = 80;
                    dgvOrders.Columns["OrderDate"].HeaderText = "Date & Time";
                    dgvOrders.Columns["OrderDate"].Width = 130;
                    dgvOrders.Columns["CustomerName"].HeaderText = "Customer";
                    dgvOrders.Columns["CustomerName"].Width = 120;
                    dgvOrders.Columns["CustomerPhone"].HeaderText = "Phone";
                    dgvOrders.Columns["CustomerPhone"].Width = 100;
                    dgvOrders.Columns["SellerName"].HeaderText = "Seller";
                    dgvOrders.Columns["SellerName"].Width = 100;
                    dgvOrders.Columns["SubTotal"].HeaderText = "Sub Total";
                    dgvOrders.Columns["SubTotal"].Width = 80;
                    dgvOrders.Columns["GlobalAdjustment"].HeaderText = "Global Adj";
                    dgvOrders.Columns["GlobalAdjustment"].Width = 80;
                    dgvOrders.Columns["TotalAmount"].HeaderText = "Total Amount";
                    dgvOrders.Columns["TotalAmount"].Width = 100;
                    dgvOrders.Columns["OrderStatus"].HeaderText = "Status";
                    dgvOrders.Columns["OrderStatus"].Width = 80;

                    // Format currency columns
                    dgvOrders.Columns["SubTotal"].DefaultCellStyle.Format = "₹0.00";
                    dgvOrders.Columns["TotalAmount"].DefaultCellStyle.Format = "₹0.00";
                    dgvOrders.Columns["SubTotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvOrders.Columns["TotalAmount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                // Update summary
                UpdateSummary(filteredOrders);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading orders: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateSummary(System.Collections.Generic.List<Order> orders)
        {
            lblTotalOrders.Text = $"Total Orders: {orders.Count}";
            lblTotalAmount.Text = $"Total Amount: ₹{orders.Sum(o => o.TotalAmount):0.00}";
        }

        private void dgvOrders_SelectionChanged(object sender, EventArgs e)
        {
            bool hasSelection = dgvOrders.SelectedRows.Count > 0;
            btnViewDetails.Enabled = hasSelection;
            btnPrintReceipt.Enabled = hasSelection;

            if (hasSelection)
            {
                var orderId = Convert.ToInt32(dgvOrders.SelectedRows[0].Cells["OrderID"].Value);
                LoadOrderDetails(orderId);
            }
            else
            {
                dgvOrderDetails.DataSource = null;
            }
        }

        private void LoadOrderDetails(int orderId)
        {
            try
            {
                var order = _dbHelper.GetOrderWithDetails(orderId);
                if (order != null)
                {
                    var orderDetailsData = order.OrderDetails.Select(od => new
                    {
                        ProductName = od.ProductName,
                        Unit = od.Unit,
                        Quantity = od.Quantity,
                        UnitPrice = od.UnitPrice,
                        ProductAdjustment = $"{od.ProductAdjustmentPercentage:0.##}%",
                        FinalPrice = od.FinalPrice,
                        LineTotal = od.LineTotal
                    }).ToList();

                    dgvOrderDetails.DataSource = orderDetailsData;

                    // Configure columns
                    if (dgvOrderDetails.Columns.Count > 0)
                    {
                        dgvOrderDetails.Columns["ProductName"].HeaderText = "Product";
                        dgvOrderDetails.Columns["Unit"].HeaderText = "Unit";
                        dgvOrderDetails.Columns["Unit"].Width = 60;
                        dgvOrderDetails.Columns["Quantity"].HeaderText = "Qty";
                        dgvOrderDetails.Columns["Quantity"].Width = 60;
                        dgvOrderDetails.Columns["UnitPrice"].HeaderText = "Unit Price";
                        dgvOrderDetails.Columns["UnitPrice"].Width = 80;
                        dgvOrderDetails.Columns["ProductAdjustment"].HeaderText = "Adj%";
                        dgvOrderDetails.Columns["ProductAdjustment"].Width = 60;
                        dgvOrderDetails.Columns["FinalPrice"].HeaderText = "Final Price";
                        dgvOrderDetails.Columns["FinalPrice"].Width = 80;
                        dgvOrderDetails.Columns["LineTotal"].HeaderText = "Line Total";
                        dgvOrderDetails.Columns["LineTotal"].Width = 90;

                        // Format currency columns
                        dgvOrderDetails.Columns["UnitPrice"].DefaultCellStyle.Format = "₹0.00";
                        dgvOrderDetails.Columns["FinalPrice"].DefaultCellStyle.Format = "₹0.00";
                        dgvOrderDetails.Columns["LineTotal"].DefaultCellStyle.Format = "₹0.00";
                        dgvOrderDetails.Columns["UnitPrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgvOrderDetails.Columns["FinalPrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgvOrderDetails.Columns["LineTotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading order details: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            LoadOrders();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadOrders();
        }

        private void btnViewDetails_Click(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count == 0) return;

            try
            {
                var orderId = Convert.ToInt32(dgvOrders.SelectedRows[0].Cells["OrderID"].Value);
                var order = _dbHelper.GetOrderWithDetails(orderId);

                if (order != null)
                {
                    var details = $"Order ID: {order.OrderID}\n" +
                                 $"Date: {order.OrderDate:yyyy-MM-dd HH:mm}\n" +
                                 $"Customer: {(string.IsNullOrEmpty(order.CustomerName) ? "Walk-in Customer" : order.CustomerName)}\n" +
                                 $"Phone: {order.CustomerPhone ?? "N/A"}\n" +
                                 $"Seller: {order.SellerName}\n" +
                                 $"Sub Total: ₹{order.SubTotal:0.00}\n" +
                                 $"Global Adjustment: {order.GlobalAdjustmentPercentage:0.##}%\n" +
                                 $"Total Amount: ₹{order.TotalAmount:0.00}\n" +
                                 $"Status: {order.OrderStatus}\n" +
                                 $"Items: {order.OrderDetails.Count}\n";

                    if (!string.IsNullOrEmpty(order.Notes))
                    {
                        details += $"Notes: {order.Notes}";
                    }

                    MessageBox.Show(details, "Order Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error viewing order details: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPrintReceipt_Click(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count == 0) return;

            try
            {
                var orderId = Convert.ToInt32(dgvOrders.SelectedRows[0].Cells["OrderID"].Value);
                var order = _dbHelper.GetOrderWithDetails(orderId);

                if (order != null)
                {
                    PdfGenerator.GenerateOrderReceipt(order);
                    MessageBox.Show("Receipt generated successfully!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating receipt: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}