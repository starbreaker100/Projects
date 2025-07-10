using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BusinessManagementSystem.Data;
using BusinessManagementSystem.Models;

namespace BusinessManagementSystem.Forms
{
    public partial class OrderForm : Form
    {
        private DatabaseHelper _dbHelper;
        private User _currentUser;
        private List<Product> _products;
        private List<OrderDetail> _orderDetails;
        private BindingSource _orderDetailsBindingSource;
        private BindingSource _productsBindingSource;

        public OrderForm(User currentUser)
        {
            _currentUser = currentUser;
            InitializeComponent();
            _dbHelper = new DatabaseHelper();
            _products = new List<Product>();
            _orderDetails = new List<OrderDetail>();
            _orderDetailsBindingSource = new BindingSource();
            _productsBindingSource = new BindingSource();
            
            LoadProducts();
            SetupOrderDetailsGrid();
            UpdateTotals();
        }

        private void InitializeComponent()
        {
            this.txtCustomerName = new TextBox();
            this.txtCustomerPhone = new TextBox();
            this.txtProductSearch = new TextBox();
            this.dgvProducts = new DataGridView();
            this.dgvOrderDetails = new DataGridView();
            this.nudQuantity = new NumericUpDown();
            this.nudProductAdjustment = new NumericUpDown();
            this.nudGlobalAdjustment = new NumericUpDown();
            this.txtNotes = new TextBox();
            this.lblCustomerName = new Label();
            this.lblCustomerPhone = new Label();
            this.lblProductSearch = new Label();
            this.lblQuantity = new Label();
            this.lblProductAdjustment = new Label();
            this.lblGlobalAdjustment = new Label();
            this.lblNotes = new Label();
            this.lblSubTotal = new Label();
            this.lblTotalAmount = new Label();
            this.lblSubTotalValue = new Label();
            this.lblTotalAmountValue = new Label();
            this.btnAddProduct = new Button();
            this.btnRemoveProduct = new Button();
            this.btnSaveOrder = new Button();
            this.btnPrintReceipt = new Button();
            this.btnClose = new Button();
            this.groupBoxCustomer = new GroupBox();
            this.groupBoxProducts = new GroupBox();
            this.groupBoxOrderItems = new GroupBox();
            this.groupBoxTotals = new GroupBox();
            this.SuspendLayout();

            // 
            // groupBoxCustomer
            // 
            this.groupBoxCustomer.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            this.groupBoxCustomer.Location = new Point(12, 12);
            this.groupBoxCustomer.Name = "groupBoxCustomer";
            this.groupBoxCustomer.Size = new Size(1200, 80);
            this.groupBoxCustomer.TabIndex = 0;
            this.groupBoxCustomer.TabStop = false;
            this.groupBoxCustomer.Text = "Customer Information";

            // 
            // lblCustomerName
            // 
            this.lblCustomerName.AutoSize = true;
            this.lblCustomerName.Font = new Font("Microsoft Sans Serif", 9F);
            this.lblCustomerName.Location = new Point(30, 35);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new Size(103, 15);
            this.lblCustomerName.TabIndex = 1;
            this.lblCustomerName.Text = "Customer Name:";

            // 
            // txtCustomerName
            // 
            this.txtCustomerName.Font = new Font("Microsoft Sans Serif", 9F);
            this.txtCustomerName.Location = new Point(145, 32);
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.Size = new Size(200, 21);
            this.txtCustomerName.TabIndex = 2;

            // 
            // lblCustomerPhone
            // 
            this.lblCustomerPhone.AutoSize = true;
            this.lblCustomerPhone.Font = new Font("Microsoft Sans Serif", 9F);
            this.lblCustomerPhone.Location = new Point(370, 35);
            this.lblCustomerPhone.Name = "lblCustomerPhone";
            this.lblCustomerPhone.Size = new Size(101, 15);
            this.lblCustomerPhone.TabIndex = 3;
            this.lblCustomerPhone.Text = "Customer Phone:";

            // 
            // txtCustomerPhone
            // 
            this.txtCustomerPhone.Font = new Font("Microsoft Sans Serif", 9F);
            this.txtCustomerPhone.Location = new Point(485, 32);
            this.txtCustomerPhone.Name = "txtCustomerPhone";
            this.txtCustomerPhone.Size = new Size(150, 21);
            this.txtCustomerPhone.TabIndex = 4;

            // 
            // groupBoxProducts
            // 
            this.groupBoxProducts.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            this.groupBoxProducts.Location = new Point(12, 100);
            this.groupBoxProducts.Name = "groupBoxProducts";
            this.groupBoxProducts.Size = new Size(600, 350);
            this.groupBoxProducts.TabIndex = 5;
            this.groupBoxProducts.TabStop = false;
            this.groupBoxProducts.Text = "Available Products";

            // 
            // lblProductSearch
            // 
            this.lblProductSearch.AutoSize = true;
            this.lblProductSearch.Font = new Font("Microsoft Sans Serif", 9F);
            this.lblProductSearch.Location = new Point(30, 130);
            this.lblProductSearch.Name = "lblProductSearch";
            this.lblProductSearch.Size = new Size(48, 15);
            this.lblProductSearch.TabIndex = 6;
            this.lblProductSearch.Text = "Search:";

            // 
            // txtProductSearch
            // 
            this.txtProductSearch.Font = new Font("Microsoft Sans Serif", 9F);
            this.txtProductSearch.Location = new Point(85, 127);
            this.txtProductSearch.Name = "txtProductSearch";
            this.txtProductSearch.Size = new Size(200, 21);
            this.txtProductSearch.TabIndex = 7;
            this.txtProductSearch.TextChanged += new EventHandler(this.txtProductSearch_TextChanged);

            // 
            // dgvProducts
            // 
            this.dgvProducts.AllowUserToAddRows = false;
            this.dgvProducts.AllowUserToDeleteRows = false;
            this.dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProducts.BackgroundColor = Color.White;
            this.dgvProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProducts.Location = new Point(30, 155);
            this.dgvProducts.MultiSelect = false;
            this.dgvProducts.Name = "dgvProducts";
            this.dgvProducts.ReadOnly = true;
            this.dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvProducts.Size = new Size(550, 200);
            this.dgvProducts.TabIndex = 8;
            this.dgvProducts.SelectionChanged += new EventHandler(this.dgvProducts_SelectionChanged);

            // Product Addition Controls
            // 
            // lblQuantity
            // 
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Font = new Font("Microsoft Sans Serif", 9F);
            this.lblQuantity.Location = new Point(30, 370);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new Size(58, 15);
            this.lblQuantity.TabIndex = 9;
            this.lblQuantity.Text = "Quantity:";

            // 
            // nudQuantity
            // 
            this.nudQuantity.DecimalPlaces = 2;
            this.nudQuantity.Font = new Font("Microsoft Sans Serif", 9F);
            this.nudQuantity.Location = new Point(95, 368);
            this.nudQuantity.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            this.nudQuantity.Minimum = new decimal(new int[] { 1, 0, 0, 131072 });
            this.nudQuantity.Name = "nudQuantity";
            this.nudQuantity.Size = new Size(80, 21);
            this.nudQuantity.TabIndex = 10;
            this.nudQuantity.Value = new decimal(new int[] { 1, 0, 0, 0 });

            // 
            // lblProductAdjustment
            // 
            this.lblProductAdjustment.AutoSize = true;
            this.lblProductAdjustment.Font = new Font("Microsoft Sans Serif", 9F);
            this.lblProductAdjustment.Location = new Point(190, 370);
            this.lblProductAdjustment.Name = "lblProductAdjustment";
            this.lblProductAdjustment.Size = new Size(99, 15);
            this.lblProductAdjustment.TabIndex = 11;
            this.lblProductAdjustment.Text = "Price Adjust (%):";

            // 
            // nudProductAdjustment
            // 
            this.nudProductAdjustment.DecimalPlaces = 2;
            this.nudProductAdjustment.Font = new Font("Microsoft Sans Serif", 9F);
            this.nudProductAdjustment.Location = new Point(300, 368);
            this.nudProductAdjustment.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            this.nudProductAdjustment.Minimum = new decimal(new int[] { 1000, 0, 0, -2147483648 });
            this.nudProductAdjustment.Name = "nudProductAdjustment";
            this.nudProductAdjustment.Size = new Size(80, 21);
            this.nudProductAdjustment.TabIndex = 12;

            // 
            // btnAddProduct
            // 
            this.btnAddProduct.BackColor = Color.FromArgb(40, 167, 69);
            this.btnAddProduct.ForeColor = Color.White;
            this.btnAddProduct.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            this.btnAddProduct.Location = new Point(400, 365);
            this.btnAddProduct.Name = "btnAddProduct";
            this.btnAddProduct.Size = new Size(80, 27);
            this.btnAddProduct.TabIndex = 13;
            this.btnAddProduct.Text = "Add Item";
            this.btnAddProduct.UseVisualStyleBackColor = false;
            this.btnAddProduct.Click += new EventHandler(this.btnAddProduct_Click);

            // 
            // groupBoxOrderItems
            // 
            this.groupBoxOrderItems.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            this.groupBoxOrderItems.Location = new Point(630, 100);
            this.groupBoxOrderItems.Name = "groupBoxOrderItems";
            this.groupBoxOrderItems.Size = new Size(582, 350);
            this.groupBoxOrderItems.TabIndex = 14;
            this.groupBoxOrderItems.TabStop = false;
            this.groupBoxOrderItems.Text = "Order Items";

            // 
            // dgvOrderDetails
            // 
            this.dgvOrderDetails.AllowUserToAddRows = false;
            this.dgvOrderDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvOrderDetails.BackgroundColor = Color.White;
            this.dgvOrderDetails.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrderDetails.Location = new Point(650, 130);
            this.dgvOrderDetails.MultiSelect = false;
            this.dgvOrderDetails.Name = "dgvOrderDetails";
            this.dgvOrderDetails.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvOrderDetails.Size = new Size(542, 280);
            this.dgvOrderDetails.TabIndex = 15;

            // 
            // btnRemoveProduct
            // 
            this.btnRemoveProduct.BackColor = Color.FromArgb(220, 53, 69);
            this.btnRemoveProduct.ForeColor = Color.White;
            this.btnRemoveProduct.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            this.btnRemoveProduct.Location = new Point(650, 420);
            this.btnRemoveProduct.Name = "btnRemoveProduct";
            this.btnRemoveProduct.Size = new Size(100, 25);
            this.btnRemoveProduct.TabIndex = 16;
            this.btnRemoveProduct.Text = "Remove Item";
            this.btnRemoveProduct.UseVisualStyleBackColor = false;
            this.btnRemoveProduct.Click += new EventHandler(this.btnRemoveProduct_Click);

            // 
            // groupBoxTotals
            // 
            this.groupBoxTotals.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            this.groupBoxTotals.Location = new Point(12, 470);
            this.groupBoxTotals.Name = "groupBoxTotals";
            this.groupBoxTotals.Size = new Size(1200, 120);
            this.groupBoxTotals.TabIndex = 17;
            this.groupBoxTotals.TabStop = false;
            this.groupBoxTotals.Text = "Order Totals & Adjustments";

            // 
            // lblGlobalAdjustment
            // 
            this.lblGlobalAdjustment.AutoSize = true;
            this.lblGlobalAdjustment.Font = new Font("Microsoft Sans Serif", 9F);
            this.lblGlobalAdjustment.Location = new Point(30, 500);
            this.lblGlobalAdjustment.Name = "lblGlobalAdjustment";
            this.lblGlobalAdjustment.Size = new Size(133, 15);
            this.lblGlobalAdjustment.TabIndex = 18;
            this.lblGlobalAdjustment.Text = "Global Adjustment (%):";

            // 
            // nudGlobalAdjustment
            // 
            this.nudGlobalAdjustment.DecimalPlaces = 2;
            this.nudGlobalAdjustment.Font = new Font("Microsoft Sans Serif", 9F);
            this.nudGlobalAdjustment.Location = new Point(175, 498);
            this.nudGlobalAdjustment.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            this.nudGlobalAdjustment.Minimum = new decimal(new int[] { 1000, 0, 0, -2147483648 });
            this.nudGlobalAdjustment.Name = "nudGlobalAdjustment";
            this.nudGlobalAdjustment.Size = new Size(80, 21);
            this.nudGlobalAdjustment.TabIndex = 19;
            this.nudGlobalAdjustment.ValueChanged += new EventHandler(this.nudGlobalAdjustment_ValueChanged);

            // 
            // lblSubTotal
            // 
            this.lblSubTotal.AutoSize = true;
            this.lblSubTotal.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            this.lblSubTotal.Location = new Point(400, 500);
            this.lblSubTotal.Name = "lblSubTotal";
            this.lblSubTotal.Size = new Size(75, 17);
            this.lblSubTotal.TabIndex = 20;
            this.lblSubTotal.Text = "Sub Total:";

            // 
            // lblSubTotalValue
            // 
            this.lblSubTotalValue.AutoSize = true;
            this.lblSubTotalValue.Font = new Font("Microsoft Sans Serif", 10F);
            this.lblSubTotalValue.Location = new Point(485, 500);
            this.lblSubTotalValue.Name = "lblSubTotalValue";
            this.lblSubTotalValue.Size = new Size(40, 17);
            this.lblSubTotalValue.TabIndex = 21;
            this.lblSubTotalValue.Text = "₹0.00";

            // 
            // lblTotalAmount
            // 
            this.lblTotalAmount.AutoSize = true;
            this.lblTotalAmount.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            this.lblTotalAmount.ForeColor = Color.FromArgb(220, 53, 69);
            this.lblTotalAmount.Location = new Point(600, 498);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Size = new Size(111, 20);
            this.lblTotalAmount.TabIndex = 22;
            this.lblTotalAmount.Text = "Total Amount:";

            // 
            // lblTotalAmountValue
            // 
            this.lblTotalAmountValue.AutoSize = true;
            this.lblTotalAmountValue.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            this.lblTotalAmountValue.ForeColor = Color.FromArgb(220, 53, 69);
            this.lblTotalAmountValue.Location = new Point(720, 498);
            this.lblTotalAmountValue.Name = "lblTotalAmountValue";
            this.lblTotalAmountValue.Size = new Size(49, 20);
            this.lblTotalAmountValue.TabIndex = 23;
            this.lblTotalAmountValue.Text = "₹0.00";

            // 
            // lblNotes
            // 
            this.lblNotes.AutoSize = true;
            this.lblNotes.Font = new Font("Microsoft Sans Serif", 9F);
            this.lblNotes.Location = new Point(30, 535);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new Size(42, 15);
            this.lblNotes.TabIndex = 24;
            this.lblNotes.Text = "Notes:";

            // 
            // txtNotes
            // 
            this.txtNotes.Font = new Font("Microsoft Sans Serif", 9F);
            this.txtNotes.Location = new Point(80, 532);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new Size(400, 40);
            this.txtNotes.TabIndex = 25;

            // Action Buttons
            // 
            // btnSaveOrder
            // 
            this.btnSaveOrder.BackColor = Color.FromArgb(0, 123, 255);
            this.btnSaveOrder.ForeColor = Color.White;
            this.btnSaveOrder.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            this.btnSaveOrder.Location = new Point(30, 610);
            this.btnSaveOrder.Name = "btnSaveOrder";
            this.btnSaveOrder.Size = new Size(100, 35);
            this.btnSaveOrder.TabIndex = 26;
            this.btnSaveOrder.Text = "Save Order";
            this.btnSaveOrder.UseVisualStyleBackColor = false;
            this.btnSaveOrder.Click += new EventHandler(this.btnSaveOrder_Click);

            // 
            // btnPrintReceipt
            // 
            this.btnPrintReceipt.BackColor = Color.FromArgb(40, 167, 69);
            this.btnPrintReceipt.ForeColor = Color.White;
            this.btnPrintReceipt.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            this.btnPrintReceipt.Location = new Point(150, 610);
            this.btnPrintReceipt.Name = "btnPrintReceipt";
            this.btnPrintReceipt.Size = new Size(120, 35);
            this.btnPrintReceipt.TabIndex = 27;
            this.btnPrintReceipt.Text = "Save & Print";
            this.btnPrintReceipt.UseVisualStyleBackColor = false;
            this.btnPrintReceipt.Click += new EventHandler(this.btnPrintReceipt_Click);

            // 
            // btnClose
            // 
            this.btnClose.BackColor = Color.FromArgb(108, 117, 125);
            this.btnClose.ForeColor = Color.White;
            this.btnClose.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            this.btnClose.Location = new Point(1110, 610);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(80, 35);
            this.btnClose.TabIndex = 28;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new EventHandler(this.btnClose_Click);

            // 
            // OrderForm
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(1224, 670);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnPrintReceipt);
            this.Controls.Add(this.btnSaveOrder);
            this.Controls.Add(this.txtNotes);
            this.Controls.Add(this.lblNotes);
            this.Controls.Add(this.lblTotalAmountValue);
            this.Controls.Add(this.lblTotalAmount);
            this.Controls.Add(this.lblSubTotalValue);
            this.Controls.Add(this.lblSubTotal);
            this.Controls.Add(this.nudGlobalAdjustment);
            this.Controls.Add(this.lblGlobalAdjustment);
            this.Controls.Add(this.groupBoxTotals);
            this.Controls.Add(this.btnRemoveProduct);
            this.Controls.Add(this.dgvOrderDetails);
            this.Controls.Add(this.groupBoxOrderItems);
            this.Controls.Add(this.btnAddProduct);
            this.Controls.Add(this.nudProductAdjustment);
            this.Controls.Add(this.lblProductAdjustment);
            this.Controls.Add(this.nudQuantity);
            this.Controls.Add(this.lblQuantity);
            this.Controls.Add(this.dgvProducts);
            this.Controls.Add(this.txtProductSearch);
            this.Controls.Add(this.lblProductSearch);
            this.Controls.Add(this.groupBoxProducts);
            this.Controls.Add(this.txtCustomerPhone);
            this.Controls.Add(this.lblCustomerPhone);
            this.Controls.Add(this.txtCustomerName);
            this.Controls.Add(this.lblCustomerName);
            this.Controls.Add(this.groupBoxCustomer);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OrderForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "New Order";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private TextBox txtCustomerName;
        private TextBox txtCustomerPhone;
        private TextBox txtProductSearch;
        private DataGridView dgvProducts;
        private DataGridView dgvOrderDetails;
        private NumericUpDown nudQuantity;
        private NumericUpDown nudProductAdjustment;
        private NumericUpDown nudGlobalAdjustment;
        private TextBox txtNotes;
        private Label lblCustomerName;
        private Label lblCustomerPhone;
        private Label lblProductSearch;
        private Label lblQuantity;
        private Label lblProductAdjustment;
        private Label lblGlobalAdjustment;
        private Label lblNotes;
        private Label lblSubTotal;
        private Label lblTotalAmount;
        private Label lblSubTotalValue;
        private Label lblTotalAmountValue;
        private Button btnAddProduct;
        private Button btnRemoveProduct;
        private Button btnSaveOrder;
        private Button btnPrintReceipt;
        private Button btnClose;
        private GroupBox groupBoxCustomer;
        private GroupBox groupBoxProducts;
        private GroupBox groupBoxOrderItems;
        private GroupBox groupBoxTotals;

        private void LoadProducts()
        {
            try
            {
                _products = _dbHelper.GetAllProducts();
                _productsBindingSource.DataSource = _products.Select(p => new
                {
                    ProductID = p.ProductID,
                    ProductName = p.ProductName,
                    CategoryName = p.CategoryName,
                    Unit = p.Unit,
                    BasePrice = p.BasePrice,
                    Stock = p.Quantity,
                    CategoryIncrease = p.PriceIncreasePercentage
                }).ToList();

                dgvProducts.DataSource = _productsBindingSource;

                if (dgvProducts.Columns.Count > 0)
                {
                    dgvProducts.Columns["ProductID"].Visible = false;
                    dgvProducts.Columns["ProductName"].HeaderText = "Product";
                    dgvProducts.Columns["CategoryName"].HeaderText = "Category";
                    dgvProducts.Columns["CategoryName"].Width = 80;
                    dgvProducts.Columns["Unit"].HeaderText = "Unit";
                    dgvProducts.Columns["Unit"].Width = 50;
                    dgvProducts.Columns["BasePrice"].HeaderText = "Price";
                    dgvProducts.Columns["BasePrice"].Width = 70;
                    dgvProducts.Columns["Stock"].HeaderText = "Stock";
                    dgvProducts.Columns["Stock"].Width = 60;
                    dgvProducts.Columns["CategoryIncrease"].HeaderText = "Cat%";
                    dgvProducts.Columns["CategoryIncrease"].Width = 50;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupOrderDetailsGrid()
        {
            _orderDetailsBindingSource.DataSource = _orderDetails;
            dgvOrderDetails.DataSource = _orderDetailsBindingSource;

            // Configure columns
            dgvOrderDetails.Columns.Clear();
            dgvOrderDetails.Columns.Add("ProductName", "Product");
            dgvOrderDetails.Columns.Add("Unit", "Unit");
            dgvOrderDetails.Columns.Add("Quantity", "Qty");
            dgvOrderDetails.Columns.Add("UnitPrice", "Unit Price");
            dgvOrderDetails.Columns.Add("ProductAdjustmentPercentage", "Adj%");
            dgvOrderDetails.Columns.Add("FinalPrice", "Final Price");
            dgvOrderDetails.Columns.Add("LineTotal", "Total");

            dgvOrderDetails.Columns["Unit"].Width = 60;
            dgvOrderDetails.Columns["Quantity"].Width = 60;
            dgvOrderDetails.Columns["UnitPrice"].Width = 80;
            dgvOrderDetails.Columns["ProductAdjustmentPercentage"].Width = 60;
            dgvOrderDetails.Columns["FinalPrice"].Width = 80;
            dgvOrderDetails.Columns["LineTotal"].Width = 80;

            // Make columns read-only
            foreach (DataGridViewColumn column in dgvOrderDetails.Columns)
            {
                column.ReadOnly = true;
            }
        }

        private void RefreshOrderDetailsGrid()
        {
            dgvOrderDetails.Rows.Clear();
            foreach (var detail in _orderDetails)
            {
                dgvOrderDetails.Rows.Add(
                    detail.ProductName,
                    detail.Unit,
                    detail.Quantity.ToString("0.##"),
                    $"₹{detail.UnitPrice:0.00}",
                    $"{detail.ProductAdjustmentPercentage:0.##}%",
                    $"₹{detail.FinalPrice:0.00}",
                    $"₹{detail.LineTotal:0.00}"
                );
            }
        }

        private void txtProductSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtProductSearch.Text))
                {
                    _productsBindingSource.DataSource = _products.Select(p => new
                    {
                        ProductID = p.ProductID,
                        ProductName = p.ProductName,
                        CategoryName = p.CategoryName,
                        Unit = p.Unit,
                        BasePrice = p.BasePrice,
                        Stock = p.Quantity,
                        CategoryIncrease = p.PriceIncreasePercentage
                    }).ToList();
                }
                else
                {
                    var filteredProducts = _products.Where(p => 
                        p.ProductName.ToLower().Contains(txtProductSearch.Text.ToLower()) ||
                        p.CategoryName.ToLower().Contains(txtProductSearch.Text.ToLower())).Select(p => new
                    {
                        ProductID = p.ProductID,
                        ProductName = p.ProductName,
                        CategoryName = p.CategoryName,
                        Unit = p.Unit,
                        BasePrice = p.BasePrice,
                        Stock = p.Quantity,
                        CategoryIncrease = p.PriceIncreasePercentage
                    }).ToList();

                    _productsBindingSource.DataSource = filteredProducts;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error filtering products: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvProducts_SelectionChanged(object sender, EventArgs e)
        {
            btnAddProduct.Enabled = dgvProducts.SelectedRows.Count > 0;
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0) return;

            try
            {
                var selectedRow = dgvProducts.SelectedRows[0];
                var productId = Convert.ToInt32(selectedRow.Cells["ProductID"].Value);
                var product = _products.FirstOrDefault(p => p.ProductID == productId);

                if (product == null) return;

                // Check stock
                if (nudQuantity.Value > product.Quantity)
                {
                    MessageBox.Show($"Insufficient stock. Available: {product.Quantity}", "Stock Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Check if product already exists in order
                var existingItem = _orderDetails.FirstOrDefault(od => od.ProductID == productId);
                if (existingItem != null)
                {
                    // Update existing item
                    existingItem.Quantity += nudQuantity.Value;
                    existingItem.ProductAdjustmentPercentage = nudProductAdjustment.Value;
                    CalculateOrderDetailPrices(existingItem, product);
                }
                else
                {
                    // Add new item
                    var orderDetail = new OrderDetail
                    {
                        ProductID = productId,
                        ProductName = product.ProductName,
                        Unit = product.Unit,
                        Quantity = nudQuantity.Value,
                        UnitPrice = product.BasePrice,
                        ProductAdjustmentPercentage = nudProductAdjustment.Value
                    };

                    CalculateOrderDetailPrices(orderDetail, product);
                    _orderDetails.Add(orderDetail);
                }

                RefreshOrderDetailsGrid();
                UpdateTotals();
                
                // Reset controls
                nudQuantity.Value = 1;
                nudProductAdjustment.Value = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding product: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CalculateOrderDetailPrices(OrderDetail orderDetail, Product product)
        {
            // Apply category increase first
            decimal priceWithCategoryIncrease = orderDetail.UnitPrice * (1 + product.PriceIncreasePercentage / 100);
            
            // Apply product-specific adjustment
            orderDetail.FinalPrice = priceWithCategoryIncrease * (1 + orderDetail.ProductAdjustmentPercentage / 100);
            
            // Calculate line total
            orderDetail.LineTotal = orderDetail.FinalPrice * orderDetail.Quantity;
        }

        private void btnRemoveProduct_Click(object sender, EventArgs e)
        {
            if (dgvOrderDetails.SelectedRows.Count == 0) return;

            try
            {
                var selectedIndex = dgvOrderDetails.SelectedRows[0].Index;
                _orderDetails.RemoveAt(selectedIndex);
                RefreshOrderDetailsGrid();
                UpdateTotals();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error removing product: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void nudGlobalAdjustment_ValueChanged(object sender, EventArgs e)
        {
            UpdateTotals();
        }

        private void UpdateTotals()
        {
            try
            {
                decimal subTotal = _orderDetails.Sum(od => od.LineTotal);
                decimal totalAmount = subTotal * (1 + nudGlobalAdjustment.Value / 100);

                lblSubTotalValue.Text = $"₹{subTotal:0.00}";
                lblTotalAmountValue.Text = $"₹{totalAmount:0.00}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating totals: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSaveOrder_Click(object sender, EventArgs e)
        {
            SaveOrder(false);
        }

        private void btnPrintReceipt_Click(object sender, EventArgs e)
        {
            SaveOrder(true);
        }

        private void SaveOrder(bool printReceipt)
        {
            if (_orderDetails.Count == 0)
            {
                MessageBox.Show("Please add at least one product to the order.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var order = new Order
                {
                    OrderDate = DateTime.Now,
                    SellerID = _currentUser.UserID,
                    SellerName = _currentUser.FullName,
                    CustomerName = txtCustomerName.Text.Trim(),
                    CustomerPhone = txtCustomerPhone.Text.Trim(),
                    GlobalAdjustmentPercentage = nudGlobalAdjustment.Value,
                    SubTotal = _orderDetails.Sum(od => od.LineTotal),
                    Notes = txtNotes.Text.Trim(),
                    OrderDetails = new List<OrderDetail>(_orderDetails)
                };

                order.TotalAmount = order.SubTotal * (1 + order.GlobalAdjustmentPercentage / 100);

                var orderId = _dbHelper.InsertOrder(order);
                order.OrderID = orderId;

                MessageBox.Show($"Order saved successfully! Order ID: {orderId}", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (printReceipt)
                {
                    try
                    {
                        PdfGenerator.GenerateOrderReceipt(order);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Order saved but failed to generate receipt: {ex.Message}", 
                            "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                // Clear form for new order
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving order: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearForm()
        {
            txtCustomerName.Clear();
            txtCustomerPhone.Clear();
            txtNotes.Clear();
            nudGlobalAdjustment.Value = 0;
            nudQuantity.Value = 1;
            nudProductAdjustment.Value = 0;
            _orderDetails.Clear();
            RefreshOrderDetailsGrid();
            UpdateTotals();
            LoadProducts(); // Refresh product stock
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (_orderDetails.Count > 0)
            {
                var result = MessageBox.Show("You have unsaved changes. Are you sure you want to close?", 
                    "Confirm Close", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result == DialogResult.No) return;
            }

            this.Close();
        }
    }
}