using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BusinessManagementSystem.Data;
using BusinessManagementSystem.Models;

namespace BusinessManagementSystem.Forms
{
    public partial class CategoryForm : Form
    {
        private DatabaseHelper _dbHelper;
        private BindingSource _bindingSource;
        private bool _isEditing = false;
        private int _editingCategoryId = 0;

        public CategoryForm()
        {
            InitializeComponent();
            _dbHelper = new DatabaseHelper();
            _bindingSource = new BindingSource();
            LoadCategories();
        }

        private void InitializeComponent()
        {
            this.dgvCategories = new DataGridView();
            this.txtCategoryName = new TextBox();
            this.txtDescription = new TextBox();
            this.nudPriceIncrease = new NumericUpDown();
            this.lblCategoryName = new Label();
            this.lblDescription = new Label();
            this.lblPriceIncrease = new Label();
            this.btnAdd = new Button();
            this.btnEdit = new Button();
            this.btnDelete = new Button();
            this.btnSave = new Button();
            this.btnCancel = new Button();
            this.btnClose = new Button();
            this.groupBoxForm = new GroupBox();
            this.groupBoxList = new GroupBox();
            this.SuspendLayout();

            // 
            // dgvCategories
            // 
            this.dgvCategories.AllowUserToAddRows = false;
            this.dgvCategories.AllowUserToDeleteRows = false;
            this.dgvCategories.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCategories.BackgroundColor = Color.White;
            this.dgvCategories.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCategories.Location = new Point(20, 30);
            this.dgvCategories.MultiSelect = false;
            this.dgvCategories.Name = "dgvCategories";
            this.dgvCategories.ReadOnly = true;
            this.dgvCategories.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvCategories.Size = new Size(750, 300);
            this.dgvCategories.TabIndex = 0;
            this.dgvCategories.SelectionChanged += new EventHandler(this.dgvCategories_SelectionChanged);

            // 
            // groupBoxList
            // 
            this.groupBoxList.Controls.Add(this.dgvCategories);
            this.groupBoxList.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            this.groupBoxList.Location = new Point(12, 12);
            this.groupBoxList.Name = "groupBoxList";
            this.groupBoxList.Size = new Size(790, 350);
            this.groupBoxList.TabIndex = 1;
            this.groupBoxList.TabStop = false;
            this.groupBoxList.Text = "Categories List";

            // 
            // groupBoxForm
            // 
            this.groupBoxForm.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            this.groupBoxForm.Location = new Point(12, 380);
            this.groupBoxForm.Name = "groupBoxForm";
            this.groupBoxForm.Size = new Size(790, 200);
            this.groupBoxForm.TabIndex = 2;
            this.groupBoxForm.TabStop = false;
            this.groupBoxForm.Text = "Category Details";

            // 
            // lblCategoryName
            // 
            this.lblCategoryName.AutoSize = true;
            this.lblCategoryName.Font = new Font("Microsoft Sans Serif", 9F);
            this.lblCategoryName.Location = new Point(30, 410);
            this.lblCategoryName.Name = "lblCategoryName";
            this.lblCategoryName.Size = new Size(95, 15);
            this.lblCategoryName.TabIndex = 3;
            this.lblCategoryName.Text = "Category Name:";

            // 
            // txtCategoryName
            // 
            this.txtCategoryName.Font = new Font("Microsoft Sans Serif", 9F);
            this.txtCategoryName.Location = new Point(140, 407);
            this.txtCategoryName.Name = "txtCategoryName";
            this.txtCategoryName.Size = new Size(200, 21);
            this.txtCategoryName.TabIndex = 4;

            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new Font("Microsoft Sans Serif", 9F);
            this.lblDescription.Location = new Point(30, 440);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new Size(70, 15);
            this.lblDescription.TabIndex = 5;
            this.lblDescription.Text = "Description:";

            // 
            // txtDescription
            // 
            this.txtDescription.Font = new Font("Microsoft Sans Serif", 9F);
            this.txtDescription.Location = new Point(140, 437);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new Size(200, 60);
            this.txtDescription.TabIndex = 6;

            // 
            // lblPriceIncrease
            // 
            this.lblPriceIncrease.AutoSize = true;
            this.lblPriceIncrease.Font = new Font("Microsoft Sans Serif", 9F);
            this.lblPriceIncrease.Location = new Point(360, 410);
            this.lblPriceIncrease.Name = "lblPriceIncrease";
            this.lblPriceIncrease.Size = new Size(118, 15);
            this.lblPriceIncrease.TabIndex = 7;
            this.lblPriceIncrease.Text = "Price Increase (%):";

            // 
            // nudPriceIncrease
            // 
            this.nudPriceIncrease.DecimalPlaces = 2;
            this.nudPriceIncrease.Font = new Font("Microsoft Sans Serif", 9F);
            this.nudPriceIncrease.Location = new Point(490, 408);
            this.nudPriceIncrease.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            this.nudPriceIncrease.Name = "nudPriceIncrease";
            this.nudPriceIncrease.Size = new Size(80, 21);
            this.nudPriceIncrease.TabIndex = 8;

            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = Color.FromArgb(40, 167, 69);
            this.btnAdd.ForeColor = Color.White;
            this.btnAdd.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            this.btnAdd.Location = new Point(30, 520);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new Size(80, 30);
            this.btnAdd.TabIndex = 9;
            this.btnAdd.Text = "Add New";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);

            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = Color.FromArgb(255, 193, 7);
            this.btnEdit.ForeColor = Color.Black;
            this.btnEdit.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            this.btnEdit.Location = new Point(120, 520);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new Size(80, 30);
            this.btnEdit.TabIndex = 10;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new EventHandler(this.btnEdit_Click);

            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = Color.FromArgb(220, 53, 69);
            this.btnDelete.ForeColor = Color.White;
            this.btnDelete.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            this.btnDelete.Location = new Point(210, 520);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new Size(80, 30);
            this.btnDelete.TabIndex = 11;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);

            // 
            // btnSave
            // 
            this.btnSave.BackColor = Color.FromArgb(0, 123, 255);
            this.btnSave.ForeColor = Color.White;
            this.btnSave.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            this.btnSave.Location = new Point(320, 520);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new Size(80, 30);
            this.btnSave.TabIndex = 12;
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
            this.btnCancel.Location = new Point(410, 520);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(80, 30);
            this.btnCancel.TabIndex = 13;
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
            this.btnClose.Location = new Point(700, 520);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(80, 30);
            this.btnClose.TabIndex = 14;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new EventHandler(this.btnClose_Click);

            // 
            // CategoryForm
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(814, 570);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.nudPriceIncrease);
            this.Controls.Add(this.lblPriceIncrease);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtCategoryName);
            this.Controls.Add(this.lblCategoryName);
            this.Controls.Add(this.groupBoxForm);
            this.Controls.Add(this.groupBoxList);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CategoryForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Category Management";
            this.ResumeLayout(false);
            this.PerformLayout();

            SetFormMode(false);
        }

        private DataGridView dgvCategories;
        private TextBox txtCategoryName;
        private TextBox txtDescription;
        private NumericUpDown nudPriceIncrease;
        private Label lblCategoryName;
        private Label lblDescription;
        private Label lblPriceIncrease;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnSave;
        private Button btnCancel;
        private Button btnClose;
        private GroupBox groupBoxForm;
        private GroupBox groupBoxList;

        private void LoadCategories()
        {
            try
            {
                var categories = _dbHelper.GetAllCategories();
                _bindingSource.DataSource = categories.Select(c => new
                {
                    CategoryID = c.CategoryID,
                    CategoryName = c.CategoryName,
                    Description = c.Description,
                    PriceIncreasePercentage = c.PriceIncreasePercentage,
                    CreatedDate = c.CreatedDate.ToString("yyyy-MM-dd")
                }).ToList();

                dgvCategories.DataSource = _bindingSource;

                // Configure columns
                if (dgvCategories.Columns.Count > 0)
                {
                    dgvCategories.Columns["CategoryID"].HeaderText = "ID";
                    dgvCategories.Columns["CategoryID"].Width = 50;
                    dgvCategories.Columns["CategoryName"].HeaderText = "Category Name";
                    dgvCategories.Columns["Description"].HeaderText = "Description";
                    dgvCategories.Columns["PriceIncreasePercentage"].HeaderText = "Price Increase %";
                    dgvCategories.Columns["PriceIncreasePercentage"].Width = 120;
                    dgvCategories.Columns["CreatedDate"].HeaderText = "Created Date";
                    dgvCategories.Columns["CreatedDate"].Width = 100;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvCategories_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCategories.SelectedRows.Count > 0 && !_isEditing)
            {
                var selectedRow = dgvCategories.SelectedRows[0];
                txtCategoryName.Text = selectedRow.Cells["CategoryName"].Value?.ToString() ?? "";
                txtDescription.Text = selectedRow.Cells["Description"].Value?.ToString() ?? "";
                nudPriceIncrease.Value = Convert.ToDecimal(selectedRow.Cells["PriceIncreasePercentage"].Value ?? 0);
                
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            _isEditing = false;
            _editingCategoryId = 0;
            ClearForm();
            SetFormMode(true);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvCategories.SelectedRows.Count == 0) return;

            _isEditing = true;
            _editingCategoryId = Convert.ToInt32(dgvCategories.SelectedRows[0].Cells["CategoryID"].Value);
            SetFormMode(true);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvCategories.SelectedRows.Count == 0) return;

            var categoryName = dgvCategories.SelectedRows[0].Cells["CategoryName"].Value?.ToString();
            var result = MessageBox.Show($"Are you sure you want to delete category '{categoryName}'?", 
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var categoryId = Convert.ToInt32(dgvCategories.SelectedRows[0].Cells["CategoryID"].Value);
                    if (_dbHelper.DeleteCategory(categoryId))
                    {
                        MessageBox.Show("Category deleted successfully!", "Success", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadCategories();
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete category.", "Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting category: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
            {
                MessageBox.Show("Please enter a category name.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCategoryName.Focus();
                return;
            }

            try
            {
                var category = new Category
                {
                    CategoryID = _editingCategoryId,
                    CategoryName = txtCategoryName.Text.Trim(),
                    Description = txtDescription.Text.Trim(),
                    PriceIncreasePercentage = nudPriceIncrease.Value,
                    IsActive = true
                };

                bool success;
                string message;

                if (_isEditing)
                {
                    success = _dbHelper.UpdateCategory(category);
                    message = success ? "Category updated successfully!" : "Failed to update category.";
                }
                else
                {
                    var categoryId = _dbHelper.InsertCategory(category);
                    success = categoryId > 0;
                    message = success ? "Category added successfully!" : "Failed to add category.";
                }

                if (success)
                {
                    MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCategories();
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
                MessageBox.Show($"Error saving category: {ex.Message}", "Error", 
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
            txtCategoryName.Enabled = isFormMode;
            txtDescription.Enabled = isFormMode;
            nudPriceIncrease.Enabled = isFormMode;

            // Show/hide buttons
            btnAdd.Visible = !isFormMode;
            btnEdit.Visible = !isFormMode;
            btnDelete.Visible = !isFormMode;
            btnSave.Visible = isFormMode;
            btnCancel.Visible = isFormMode;

            // Enable/disable grid
            dgvCategories.Enabled = !isFormMode;

            if (isFormMode)
            {
                txtCategoryName.Focus();
            }
        }

        private void ClearForm()
        {
            txtCategoryName.Clear();
            txtDescription.Clear();
            nudPriceIncrease.Value = 0;
        }
    }
}