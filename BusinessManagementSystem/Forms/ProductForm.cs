using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BusinessManagementSystem.Data;
using BusinessManagementSystem.Models;

namespace BusinessManagementSystem.Forms
{
    public partial class ProductForm : Form
    {
        private DatabaseHelper _dbHelper;
        private BindingSource _bindingSource;
        private bool _isEditing = false;
        private int _editingProductId = 0;

        public ProductForm()
        {
            InitializeComponent();
            _dbHelper = new DatabaseHelper();
            _bindingSource = new BindingSource();
            LoadProducts();
            LoadCategories();
        }

        private void InitializeComponent()
        {
            this.dgvProducts = new DataGridView();
            this.txtProductName = new TextBox();
            this.txtDescription = new TextBox();
            this.cmbCategory = new ComboBox();
            this.cmbUnit = new ComboBox();
            this.nudBasePrice = new NumericUpDown();
            this.nudQuantity = new NumericUpDown();
            this.nudMinimumStock = new NumericUpDown();
            this.lblProductName = new Label();
            this.lblCategory = new Label();
            this.lblUnit = new Label();
            this.lblBasePrice = new Label();
            this.lblQuantity = new Label();
            this.lblMinimumStock = new Label();
            this.lblDescription = new Label();
            this.btnAdd = new Button();
            this.btnEdit = new Button();
            this.btnDelete = new Button();
            this.btnSave = new Button();
            this.btnCancel = new Button();
            this.btnClose = new Button();
            this.txtSearch = new TextBox();
            this.btnSearch = new Button();
            this.lblSearch = new Label();
            this.groupBoxForm = new GroupBox();
            this.groupBoxList = new GroupBox();
            this.SuspendLayout();

            // 
            // dgvProducts
            // 
            this.dgvProducts.AllowUserToAddRows = false;
            this.dgvProducts.AllowUserToDeleteRows = false;
            this.dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProducts.BackgroundColor = Color.White;
            this.dgvProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProducts.Location = new Point(20, 60);
            this.dgvProducts.MultiSelect = false;
            this.dgvProducts.Name = "dgvProducts";
            this.dgvProducts.ReadOnly = true;
            this.dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvProducts.Size = new Size(950, 320);
            this.dgvProducts.TabIndex = 0;
            this.dgvProducts.SelectionChanged += new EventHandler(this.dgvProducts_SelectionChanged);

            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new Font("Microsoft Sans Serif", 9F);
            this.lblSearch.Location = new Point(20, 35);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new Size(48, 15);
            this.lblSearch.TabIndex = 1;
            this.lblSearch.Text = "Search:";

            // 
            // txtSearch
            // 
            this.txtSearch.Font = new Font("Microsoft Sans Serif", 9F);
            this.txtSearch.Location = new Point(75, 32);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new Size(200, 21);
            this.txtSearch.TabIndex = 2;

            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = Color.FromArgb(0, 123, 255);
            this.btnSearch.ForeColor = Color.White;
            this.btnSearch.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Bold);
            this.btnSearch.Location = new Point(285, 30);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new Size(60, 25);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);

            // 
            // groupBoxList
            // 
            this.groupBoxList.Controls.Add(this.dgvProducts);
            this.groupBoxList.Controls.Add(this.lblSearch);
            this.groupBoxList.Controls.Add(this.txtSearch);
            this.groupBoxList.Controls.Add(this.btnSearch);
            this.groupBoxList.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            this.groupBoxList.Location = new Point(12, 12);
            this.groupBoxList.Name = "groupBoxList";
            this.groupBoxList.Size = new Size(990, 400);
            this.groupBoxList.TabIndex = 4;
            this.groupBoxList.TabStop = false;
            this.groupBoxList.Text = "Products List";

            // 
            // groupBoxForm
            // 
            this.groupBoxForm.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            this.groupBoxForm.Location = new Point(12, 430);
            this.groupBoxForm.Name = "groupBoxForm";
            this.groupBoxForm.Size = new Size(990, 170);
            this.groupBoxForm.TabIndex = 5;
            this.groupBoxForm.TabStop = false;
            this.groupBoxForm.Text = "Product Details";

            // First Row
            // 
            // lblProductName
            // 
            this.lblProductName.AutoSize = true;
            this.lblProductName.Font = new Font("Microsoft Sans Serif", 9F);
            this.lblProductName.Location = new Point(30, 460);
            this.lblProductName.Name = "lblProductName";
            this.lblProductName.Size = new Size(88, 15);
            this.lblProductName.TabIndex = 6;
            this.lblProductName.Text = "Product Name:";

            // 
            // txtProductName
            // 
            this.txtProductName.Font = new Font("Microsoft Sans Serif", 9F);
            this.txtProductName.Location = new Point(130, 457);
            this.txtProductName.Name = "txtProductName";
            this.txtProductName.Size = new Size(180, 21);
            this.txtProductName.TabIndex = 7;

            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.Font = new Font("Microsoft Sans Serif", 9F);
            this.lblCategory.Location = new Point(330, 460);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new Size(61, 15);
            this.lblCategory.TabIndex = 8;
            this.lblCategory.Text = "Category:";

            // 
            // cmbCategory
            // 
            this.cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbCategory.Font = new Font("Microsoft Sans Serif", 9F);
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new Point(400, 457);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new Size(150, 23);
            this.cmbCategory.TabIndex = 9;

            // 
            // lblUnit
            // 
            this.lblUnit.AutoSize = true;
            this.lblUnit.Font = new Font("Microsoft Sans Serif", 9F);
            this.lblUnit.Location = new Point(570, 460);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.Size = new Size(31, 15);
            this.lblUnit.TabIndex = 10;
            this.lblUnit.Text = "Unit:";

            // 
            // cmbUnit
            // 
            this.cmbUnit.Font = new Font("Microsoft Sans Serif", 9F);
            this.cmbUnit.FormattingEnabled = true;
            this.cmbUnit.Items.AddRange(new object[] { "pcs", "kg", "ltr", "mtr", "box", "set", "dozen" });
            this.cmbUnit.Location = new Point(610, 457);
            this.cmbUnit.Name = "cmbUnit";
            this.cmbUnit.Size = new Size(80, 23);
            this.cmbUnit.TabIndex = 11;

            // Second Row
            // 
            // lblBasePrice
            // 
            this.lblBasePrice.AutoSize = true;
            this.lblBasePrice.Font = new Font("Microsoft Sans Serif", 9F);
            this.lblBasePrice.Location = new Point(30, 495);
            this.lblBasePrice.Name = "lblBasePrice";
            this.lblBasePrice.Size = new Size(69, 15);
            this.lblBasePrice.TabIndex = 12;
            this.lblBasePrice.Text = "Base Price:";

            // 
            // nudBasePrice
            // 
            this.nudBasePrice.DecimalPlaces = 2;
            this.nudBasePrice.Font = new Font("Microsoft Sans Serif", 9F);
            this.nudBasePrice.Location = new Point(130, 493);
            this.nudBasePrice.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
            this.nudBasePrice.Name = "nudBasePrice";
            this.nudBasePrice.Size = new Size(100, 21);
            this.nudBasePrice.TabIndex = 13;

            // 
            // lblQuantity
            // 
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Font = new Font("Microsoft Sans Serif", 9F);
            this.lblQuantity.Location = new Point(250, 495);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new Size(58, 15);
            this.lblQuantity.TabIndex = 14;
            this.lblQuantity.Text = "Quantity:";

            // 
            // nudQuantity
            // 
            this.nudQuantity.DecimalPlaces = 2;
            this.nudQuantity.Font = new Font("Microsoft Sans Serif", 9F);
            this.nudQuantity.Location = new Point(320, 493);
            this.nudQuantity.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
            this.nudQuantity.Name = "nudQuantity";
            this.nudQuantity.Size = new Size(100, 21);
            this.nudQuantity.TabIndex = 15;

            // 
            // lblMinimumStock
            // 
            this.lblMinimumStock.AutoSize = true;
            this.lblMinimumStock.Font = new Font("Microsoft Sans Serif", 9F);
            this.lblMinimumStock.Location = new Point(440, 495);
            this.lblMinimumStock.Name = "lblMinimumStock";
            this.lblMinimumStock.Size = new Size(95, 15);
            this.lblMinimumStock.TabIndex = 16;
            this.lblMinimumStock.Text = "Minimum Stock:";

            // 
            // nudMinimumStock
            // 
            this.nudMinimumStock.DecimalPlaces = 2;
            this.nudMinimumStock.Font = new Font("Microsoft Sans Serif", 9F);
            this.nudMinimumStock.Location = new Point(550, 493);
            this.nudMinimumStock.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
            this.nudMinimumStock.Name = "nudMinimumStock";
            this.nudMinimumStock.Size = new Size(100, 21);
            this.nudMinimumStock.TabIndex = 17;

            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new Font("Microsoft Sans Serif", 9F);
            this.lblDescription.Location = new Point(30, 530);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new Size(70, 15);
            this.lblDescription.TabIndex = 18;
            this.lblDescription.Text = "Description:";

            // 
            // txtDescription
            // 
            this.txtDescription.Font = new Font("Microsoft Sans Serif", 9F);
            this.txtDescription.Location = new Point(130, 527);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new Size(400, 50);
            this.txtDescription.TabIndex = 19;

            // Buttons
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = Color.FromArgb(40, 167, 69);
            this.btnAdd.ForeColor = Color.White;
            this.btnAdd.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            this.btnAdd.Location = new Point(30, 620);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new Size(80, 30);
            this.btnAdd.TabIndex = 20;
            this.btnAdd.Text = "Add New";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);

            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = Color.FromArgb(255, 193, 7);
            this.btnEdit.ForeColor = Color.Black;
            this.btnEdit.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            this.btnEdit.Location = new Point(120, 620);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new Size(80, 30);
            this.btnEdit.TabIndex = 21;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new EventHandler(this.btnEdit_Click);

            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = Color.FromArgb(220, 53, 69);
            this.btnDelete.ForeColor = Color.White;
            this.btnDelete.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            this.btnDelete.Location = new Point(210, 620);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new Size(80, 30);
            this.btnDelete.TabIndex = 22;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);

            // 
            // btnSave
            // 
            this.btnSave.BackColor = Color.FromArgb(0, 123, 255);
            this.btnSave.ForeColor = Color.White;
            this.btnSave.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            this.btnSave.Location = new Point(320, 620);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new Size(80, 30);
            this.btnSave.TabIndex = 23;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Visible = false;
            this.btnSave.Click += new EventHandler(this.btnSave_Click);

            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = Color.FromArgb(108, 117, 125);
            this.btnCancel.ForeColor = Color.White;
            this.btnCancel.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            this.btnCancel.Location = new Point(410, 620);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(80, 30);
            this.btnCancel.TabIndex = 24;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);

            // 
            // btnClose
            // 
            this.btnClose.BackColor = Color.FromArgb(108, 117, 125);
            this.btnClose.ForeColor = Color.White;
            this.btnClose.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            this.btnClose.Location = new Point(900, 620);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(80, 30);
            this.btnClose.TabIndex = 25;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new EventHandler(this.btnClose_Click);

            // 
            // ProductForm
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(1014, 670);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.nudMinimumStock);
            this.Controls.Add(this.lblMinimumStock);
            this.Controls.Add(this.nudQuantity);
            this.Controls.Add(this.lblQuantity);
            this.Controls.Add(this.nudBasePrice);
            this.Controls.Add(this.lblBasePrice);
            this.Controls.Add(this.cmbUnit);
            this.Controls.Add(this.lblUnit);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.txtProductName);
            this.Controls.Add(this.lblProductName);
            this.Controls.Add(this.groupBoxForm);
            this.Controls.Add(this.groupBoxList);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProductForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Product Management";
            this.ResumeLayout(false);
            this.PerformLayout();

            SetFormMode(false);
        }

        private DataGridView dgvProducts;
        private TextBox txtProductName;
        private TextBox txtDescription;
        private ComboBox cmbCategory;
        private ComboBox cmbUnit;
        private NumericUpDown nudBasePrice;
        private NumericUpDown nudQuantity;
        private NumericUpDown nudMinimumStock;
        private Label lblProductName;
        private Label lblCategory;
        private Label lblUnit;
        private Label lblBasePrice;
        private Label lblQuantity;
        private Label lblMinimumStock;
        private Label lblDescription;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnSave;
        private Button btnCancel;
        private Button btnClose;
        private TextBox txtSearch;
        private Button btnSearch;
        private Label lblSearch;
        private GroupBox groupBoxForm;
        private GroupBox groupBoxList;

        private void LoadProducts()
        {
            try
            {
                var products = _dbHelper.GetAllProducts();
                _bindingSource.DataSource = products.Select(p => new
                {
                    ProductID = p.ProductID,
                    ProductName = p.ProductName,
                    CategoryName = p.CategoryName,
                    Unit = p.Unit,
                    BasePrice = p.BasePrice,
                    Quantity = p.Quantity,
                    MinimumStock = p.MinimumStock,
                    Description = p.Description,
                    PriceIncrease = p.PriceIncreasePercentage,
                    CreatedDate = p.CreatedDate.ToString("yyyy-MM-dd")
                }).ToList();

                dgvProducts.DataSource = _bindingSource;

                // Configure columns
                if (dgvProducts.Columns.Count > 0)
                {
                    dgvProducts.Columns["ProductID"].HeaderText = "ID";
                    dgvProducts.Columns["ProductID"].Width = 50;
                    dgvProducts.Columns["ProductName"].HeaderText = "Product Name";
                    dgvProducts.Columns["CategoryName"].HeaderText = "Category";
                    dgvProducts.Columns["CategoryName"].Width = 100;
                    dgvProducts.Columns["Unit"].HeaderText = "Unit";
                    dgvProducts.Columns["Unit"].Width = 60;
                    dgvProducts.Columns["BasePrice"].HeaderText = "Base Price";
                    dgvProducts.Columns["BasePrice"].Width = 80;
                    dgvProducts.Columns["Quantity"].HeaderText = "Stock";
                    dgvProducts.Columns["Quantity"].Width = 70;
                    dgvProducts.Columns["MinimumStock"].HeaderText = "Min Stock";
                    dgvProducts.Columns["MinimumStock"].Width = 80;
                    dgvProducts.Columns["Description"].HeaderText = "Description";
                    dgvProducts.Columns["PriceIncrease"].HeaderText = "Cat %";
                    dgvProducts.Columns["PriceIncrease"].Width = 60;
                    dgvProducts.Columns["CreatedDate"].HeaderText = "Created";
                    dgvProducts.Columns["CreatedDate"].Width = 80;

                    // Color rows with low stock
                    foreach (DataGridViewRow row in dgvProducts.Rows)
                    {
                        var quantity = Convert.ToDecimal(row.Cells["Quantity"].Value ?? 0);
                        var minStock = Convert.ToDecimal(row.Cells["MinimumStock"].Value ?? 0);
                        
                        if (quantity <= minStock)
                        {
                            row.DefaultCellStyle.BackColor = Color.FromArgb(255, 235, 238); // Light red
                            row.DefaultCellStyle.ForeColor = Color.FromArgb(155, 0, 0); // Dark red
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCategories()
        {
            try
            {
                var categories = _dbHelper.GetAllCategories();
                cmbCategory.DataSource = categories;
                cmbCategory.DisplayMember = "CategoryName";
                cmbCategory.ValueMember = "CategoryID";
                cmbCategory.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvProducts_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count > 0 && !_isEditing)
            {
                var selectedRow = dgvProducts.SelectedRows[0];
                txtProductName.Text = selectedRow.Cells["ProductName"].Value?.ToString() ?? "";
                txtDescription.Text = selectedRow.Cells["Description"].Value?.ToString() ?? "";
                cmbCategory.Text = selectedRow.Cells["CategoryName"].Value?.ToString() ?? "";
                cmbUnit.Text = selectedRow.Cells["Unit"].Value?.ToString() ?? "";
                nudBasePrice.Value = Convert.ToDecimal(selectedRow.Cells["BasePrice"].Value ?? 0);
                nudQuantity.Value = Convert.ToDecimal(selectedRow.Cells["Quantity"].Value ?? 0);
                nudMinimumStock.Value = Convert.ToDecimal(selectedRow.Cells["MinimumStock"].Value ?? 0);
                
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                var products = string.IsNullOrWhiteSpace(txtSearch.Text) 
                    ? _dbHelper.GetAllProducts() 
                    : _dbHelper.SearchProducts(txtSearch.Text.Trim());

                _bindingSource.DataSource = products.Select(p => new
                {
                    ProductID = p.ProductID,
                    ProductName = p.ProductName,
                    CategoryName = p.CategoryName,
                    Unit = p.Unit,
                    BasePrice = p.BasePrice,
                    Quantity = p.Quantity,
                    MinimumStock = p.MinimumStock,
                    Description = p.Description,
                    PriceIncrease = p.PriceIncreasePercentage,
                    CreatedDate = p.CreatedDate.ToString("yyyy-MM-dd")
                }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching products: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            _isEditing = false;
            _editingProductId = 0;
            ClearForm();
            SetFormMode(true);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0) return;

            _isEditing = true;
            _editingProductId = Convert.ToInt32(dgvProducts.SelectedRows[0].Cells["ProductID"].Value);
            SetFormMode(true);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0) return;

            var productName = dgvProducts.SelectedRows[0].Cells["ProductName"].Value?.ToString();
            var result = MessageBox.Show($"Are you sure you want to delete product '{productName}'?", 
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var productId = Convert.ToInt32(dgvProducts.SelectedRows[0].Cells["ProductID"].Value);
                    if (_dbHelper.DeleteProduct(productId))
                    {
                        MessageBox.Show("Product deleted successfully!", "Success", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadProducts();
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete product.", "Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting product: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                MessageBox.Show("Please enter a product name.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProductName.Focus();
                return;
            }

            if (cmbCategory.SelectedValue == null)
            {
                MessageBox.Show("Please select a category.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCategory.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(cmbUnit.Text))
            {
                MessageBox.Show("Please select or enter a unit.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbUnit.Focus();
                return;
            }

            try
            {
                var product = new Product
                {
                    ProductID = _editingProductId,
                    ProductName = txtProductName.Text.Trim(),
                    CategoryID = Convert.ToInt32(cmbCategory.SelectedValue),
                    Unit = cmbUnit.Text.Trim(),
                    BasePrice = nudBasePrice.Value,
                    Quantity = nudQuantity.Value,
                    MinimumStock = nudMinimumStock.Value,
                    Description = txtDescription.Text.Trim(),
                    IsActive = true
                };

                bool success;
                string message;

                if (_isEditing)
                {
                    success = _dbHelper.UpdateProduct(product);
                    message = success ? "Product updated successfully!" : "Failed to update product.";
                }
                else
                {
                    var productId = _dbHelper.InsertProduct(product);
                    success = productId > 0;
                    message = success ? "Product added successfully!" : "Failed to add product.";
                }

                if (success)
                {
                    MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProducts();
                    SetFormMode(false);
                    ClearForm();
                }
                else
                {
                    MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving product: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetFormMode(false);
            ClearForm();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SetFormMode(bool isFormMode)
        {
            // Enable/disable form fields
            txtProductName.Enabled = isFormMode;
            txtDescription.Enabled = isFormMode;
            cmbCategory.Enabled = isFormMode;
            cmbUnit.Enabled = isFormMode;
            nudBasePrice.Enabled = isFormMode;
            nudQuantity.Enabled = isFormMode;
            nudMinimumStock.Enabled = isFormMode;

            // Show/hide buttons
            btnAdd.Visible = !isFormMode;
            btnEdit.Visible = !isFormMode;
            btnDelete.Visible = !isFormMode;
            btnSave.Visible = isFormMode;
            btnCancel.Visible = isFormMode;

            // Enable/disable grid and search
            dgvProducts.Enabled = !isFormMode;
            txtSearch.Enabled = !isFormMode;
            btnSearch.Enabled = !isFormMode;

            if (isFormMode)
            {
                txtProductName.Focus();
            }
        }

        private void ClearForm()
        {
            txtProductName.Clear();
            txtDescription.Clear();
            cmbCategory.SelectedIndex = -1;
            cmbUnit.Text = "";
            nudBasePrice.Value = 0;
            nudQuantity.Value = 0;
            nudMinimumStock.Value = 0;
        }
    }
}