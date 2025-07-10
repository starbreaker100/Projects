using System;
using System.Drawing;
using System.Windows.Forms;
using BusinessManagementSystem.Models;

namespace BusinessManagementSystem.Forms
{
    public partial class MainForm : Form
    {
        private User _currentUser;

        public MainForm(User currentUser)
        {
            _currentUser = currentUser;
            InitializeComponent();
            UpdateUserInfo();
        }

        private void InitializeComponent()
        {
            this.lblWelcome = new Label();
            this.lblUserInfo = new Label();
            this.btnCategories = new Button();
            this.btnProducts = new Button();
            this.btnNewOrder = new Button();
            this.btnOrderHistory = new Button();
            this.btnLogout = new Button();
            this.groupBoxModules = new GroupBox();
            this.statusStrip = new StatusStrip();
            this.toolStripStatusLabel = new ToolStripStatusLabel();
            this.SuspendLayout();

            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Bold);
            this.lblWelcome.ForeColor = Color.FromArgb(0, 123, 255);
            this.lblWelcome.Location = new Point(30, 30);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new Size(300, 29);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "Business Management System";

            // 
            // lblUserInfo
            // 
            this.lblUserInfo.AutoSize = true;
            this.lblUserInfo.Font = new Font("Microsoft Sans Serif", 10F);
            this.lblUserInfo.Location = new Point(30, 70);
            this.lblUserInfo.Name = "lblUserInfo";
            this.lblUserInfo.Size = new Size(100, 17);
            this.lblUserInfo.TabIndex = 1;
            this.lblUserInfo.Text = "Welcome, User!";

            // 
            // groupBoxModules
            // 
            this.groupBoxModules.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            this.groupBoxModules.Location = new Point(30, 110);
            this.groupBoxModules.Name = "groupBoxModules";
            this.groupBoxModules.Size = new Size(740, 350);
            this.groupBoxModules.TabIndex = 2;
            this.groupBoxModules.TabStop = false;
            this.groupBoxModules.Text = "Application Modules";

            // 
            // btnCategories
            // 
            this.btnCategories.BackColor = Color.FromArgb(40, 167, 69);
            this.btnCategories.ForeColor = Color.White;
            this.btnCategories.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Bold);
            this.btnCategories.Location = new Point(50, 140);
            this.btnCategories.Name = "btnCategories";
            this.btnCategories.Size = new Size(150, 80);
            this.btnCategories.TabIndex = 3;
            this.btnCategories.Text = "📁\r\nCategory\r\nManagement";
            this.btnCategories.UseVisualStyleBackColor = false;
            this.btnCategories.Click += new EventHandler(this.btnCategories_Click);

            // 
            // btnProducts
            // 
            this.btnProducts.BackColor = Color.FromArgb(255, 193, 7);
            this.btnProducts.ForeColor = Color.Black;
            this.btnProducts.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Bold);
            this.btnProducts.Location = new Point(220, 140);
            this.btnProducts.Name = "btnProducts";
            this.btnProducts.Size = new Size(150, 80);
            this.btnProducts.TabIndex = 4;
            this.btnProducts.Text = "📦\r\nProduct\r\nManagement";
            this.btnProducts.UseVisualStyleBackColor = false;
            this.btnProducts.Click += new EventHandler(this.btnProducts_Click);

            // 
            // btnNewOrder
            // 
            this.btnNewOrder.BackColor = Color.FromArgb(0, 123, 255);
            this.btnNewOrder.ForeColor = Color.White;
            this.btnNewOrder.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Bold);
            this.btnNewOrder.Location = new Point(390, 140);
            this.btnNewOrder.Name = "btnNewOrder";
            this.btnNewOrder.Size = new Size(150, 80);
            this.btnNewOrder.TabIndex = 5;
            this.btnNewOrder.Text = "🛒\r\nNew Order";
            this.btnNewOrder.UseVisualStyleBackColor = false;
            this.btnNewOrder.Click += new EventHandler(this.btnNewOrder_Click);

            // 
            // btnOrderHistory
            // 
            this.btnOrderHistory.BackColor = Color.FromArgb(108, 117, 125);
            this.btnOrderHistory.ForeColor = Color.White;
            this.btnOrderHistory.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Bold);
            this.btnOrderHistory.Location = new Point(560, 140);
            this.btnOrderHistory.Name = "btnOrderHistory";
            this.btnOrderHistory.Size = new Size(150, 80);
            this.btnOrderHistory.TabIndex = 6;
            this.btnOrderHistory.Text = "📋\r\nOrder History";
            this.btnOrderHistory.UseVisualStyleBackColor = false;
            this.btnOrderHistory.Click += new EventHandler(this.btnOrderHistory_Click);

            // 
            // btnLogout
            // 
            this.btnLogout.BackColor = Color.FromArgb(220, 53, 69);
            this.btnLogout.ForeColor = Color.White;
            this.btnLogout.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            this.btnLogout.Location = new Point(650, 480);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new Size(120, 40);
            this.btnLogout.TabIndex = 7;
            this.btnLogout.Text = "🚪 Logout";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new EventHandler(this.btnLogout_Click);

            // 
            // statusStrip
            // 
            this.statusStrip.Items.Add(this.toolStripStatusLabel);
            this.statusStrip.Location = new Point(0, 539);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new Size(800, 22);
            this.statusStrip.TabIndex = 8;
            this.statusStrip.Text = "statusStrip1";

            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new Size(39, 17);
            this.toolStripStatusLabel.Text = "Ready";

            // Add additional information panel
            var infoPanel = new Panel
            {
                Location = new Point(50, 250),
                Size = new Size(660, 180),
                BackColor = Color.FromArgb(248, 249, 250),
                BorderStyle = BorderStyle.FixedSingle
            };

            var infoLabel = new Label
            {
                Text = "📊 Dashboard Information\n\n" +
                       "• Categories: Organize your products into categories with pricing rules\n" +
                       "• Products: Manage inventory, pricing, and stock levels\n" +
                       "• Orders: Create new orders with flexible pricing adjustments\n" +
                       "• History: View and reprint past orders\n\n" +
                       "💡 Quick Tips:\n" +
                       "• Category price adjustments apply automatically to products\n" +
                       "• Orders support both global and per-item price adjustments\n" +
                       "• PDF receipts are generated automatically for each order",
                Location = new Point(15, 15),
                Size = new Size(630, 150),
                Font = new Font("Microsoft Sans Serif", 9F),
                ForeColor = Color.FromArgb(73, 80, 87)
            };

            infoPanel.Controls.Add(infoLabel);
            this.Controls.Add(infoPanel);

            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(800, 561);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnOrderHistory);
            this.Controls.Add(this.btnNewOrder);
            this.Controls.Add(this.btnProducts);
            this.Controls.Add(this.btnCategories);
            this.Controls.Add(this.groupBoxModules);
            this.Controls.Add(this.lblUserInfo);
            this.Controls.Add(this.lblWelcome);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Business Management System - Dashboard";
            this.FormClosing += new FormClosingEventHandler(this.MainForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Label lblWelcome;
        private Label lblUserInfo;
        private Button btnCategories;
        private Button btnProducts;
        private Button btnNewOrder;
        private Button btnOrderHistory;
        private Button btnLogout;
        private GroupBox groupBoxModules;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel toolStripStatusLabel;

        private void UpdateUserInfo()
        {
            lblUserInfo.Text = $"Welcome, {_currentUser.FullName} ({_currentUser.Role}) - {DateTime.Now:MMM dd, yyyy}";
            toolStripStatusLabel.Text = $"Logged in as: {_currentUser.Username} | Role: {_currentUser.Role}";
        }

        private void btnCategories_Click(object sender, EventArgs e)
        {
            try
            {
                var categoryForm = new CategoryForm();
                categoryForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening Category Management: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            try
            {
                var productForm = new ProductForm();
                productForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening Product Management: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNewOrder_Click(object sender, EventArgs e)
        {
            try
            {
                var orderForm = new OrderForm(_currentUser);
                orderForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening New Order: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOrderHistory_Click(object sender, EventArgs e)
        {
            try
            {
                var historyForm = new OrderHistoryForm();
                historyForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening Order History: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to logout?", "Confirm Logout", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}