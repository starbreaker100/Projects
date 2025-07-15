using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BusinessManagementSystem.Forms.Templates;
using BusinessManagementSystem.Models;
using BusinessManagementSystem.Data;

namespace BusinessManagementSystem.Forms.Examples
{
    public partial class CustomerFormExample : Form
    {
        private DatabaseHelper _dbHelper;
        private Customer _currentCustomer;
        private bool _isEditMode;

        public CustomerFormExample()
        {
            _dbHelper = new DatabaseHelper();
            InitializeComponent();
            CreateCustomerForm();
        }

        public CustomerFormExample(Customer customer)
        {
            _dbHelper = new DatabaseHelper();
            _currentCustomer = customer;
            _isEditMode = true;
            InitializeComponent();
            CreateCustomerForm();
        }

        private void CreateCustomerForm()
        {
            // Create form generator
            var generator = new FormGenerator();

            // Configure form generation options
            var options = new FormGenerationOptions
            {
                FormWidth = 800,
                FormHeight = 650,
                ColumnsPerRow = 2,
                StartX = 30,
                StartY = 50,
                LabelWidth = 100,
                ColumnWidth = 350,
                RowHeight = 50,
                
                // Exclude auto-generated fields
                ExcludedProperties = new List<string> 
                { 
                    "CustomerID", 
                    "CreatedDate", 
                    "LastPurchaseDate", 
                    "TotalPurchases" 
                },
                
                // Custom display names
                PropertyDisplayNames = new Dictionary<string, string>
                {
                    { "FirstName", "First Name" },
                    { "LastName", "Last Name" },
                    { "Email", "Email Address" },
                    { "Phone", "Phone Number" },
                    { "Address", "Street Address" },
                    { "City", "City" },
                    { "State", "State/Province" },
                    { "ZipCode", "Zip/Postal Code" },
                    { "DateOfBirth", "Date of Birth" },
                    { "IsActive", "Active Customer" },
                    { "CustomerType", "Customer Type" },
                    { "CreditLimit", "Credit Limit" }
                },
                
                // Control the order of fields
                PropertyOrder = new Dictionary<string, int>
                {
                    { "FirstName", 1 },
                    { "LastName", 2 },
                    { "Email", 3 },
                    { "Phone", 4 },
                    { "Address", 5 },
                    { "City", 6 },
                    { "State", 7 },
                    { "ZipCode", 8 },
                    { "DateOfBirth", 9 },
                    { "CustomerType", 10 },
                    { "CreditLimit", 11 },
                    { "IsActive", 12 }
                },
                
                // Custom control factories
                CustomControlFactories = new Dictionary<string, Func<Control>>
                {
                    { "CustomerType", () => {
                        var combo = new ComboBox 
                        { 
                            DropDownStyle = ComboBoxStyle.DropDownList,
                            Font = new Font("Segoe UI", 9F),
                            Size = new Size(200, 25)
                        };
                        combo.Items.AddRange(new[] { "Regular", "Premium", "VIP", "Corporate" });
                        combo.SelectedIndex = 0;
                        return combo;
                    }},
                    { "State", () => {
                        var combo = new ComboBox 
                        { 
                            DropDownStyle = ComboBoxStyle.DropDownList,
                            Font = new Font("Segoe UI", 9F),
                            Size = new Size(200, 25)
                        };
                        combo.Items.AddRange(new[] { "AL", "AK", "AZ", "AR", "CA", "CO", "CT", "DE", "FL", "GA", "HI", "ID", "IL", "IN", "IA", "KS", "KY", "LA", "ME", "MD", "MA", "MI", "MN", "MS", "MO", "MT", "NE", "NV", "NH", "NJ", "NM", "NY", "NC", "ND", "OH", "OK", "OR", "PA", "RI", "SC", "SD", "TN", "TX", "UT", "VT", "VA", "WA", "WV", "WI", "WY" });
                        return combo;
                    }},
                    { "Address", () => {
                        var textBox = new TextBox 
                        { 
                            Multiline = true,
                            Size = new Size(200, 50),
                            BorderStyle = BorderStyle.FixedSingle,
                            Font = new Font("Segoe UI", 9F),
                            ScrollBars = ScrollBars.Vertical
                        };
                        return textBox;
                    }}
                }
            };

            // Generate the form
            var generatedForm = generator.GenerateForm<Customer>(
                _isEditMode ? "Edit Customer" : "Add New Customer", 
                options
            );

            // Copy form properties to this form
            this.Text = generatedForm.Text;
            this.Size = generatedForm.Size;
            this.StartPosition = generatedForm.StartPosition;
            this.FormBorderStyle = generatedForm.FormBorderStyle;
            this.MaximizeBox = generatedForm.MaximizeBox;
            this.BackColor = generatedForm.BackColor;
            this.Font = generatedForm.Font;

            // Move all controls from generated form to this form
            var controlsToMove = new List<Control>();
            foreach (Control control in generatedForm.Controls)
            {
                controlsToMove.Add(control);
            }

            foreach (Control control in controlsToMove)
            {
                generatedForm.Controls.Remove(control);
                this.Controls.Add(control);
            }

            // Set up event handlers
            generatedForm.SaveButton.Click += SaveButton_Click;
            generatedForm.CancelButton.Click += CancelButton_Click;

            // Store reference to generated form for data binding
            _generatedForm = generatedForm;

            // If editing, populate the form
            if (_isEditMode && _currentCustomer != null)
            {
                _generatedForm.PopulateForm(_currentCustomer);
            }

            // Add title label
            var titleLabel = new Label
            {
                Text = _isEditMode ? "Edit Customer Information" : "Add New Customer",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(41, 128, 185),
                Location = new Point(30, 15),
                AutoSize = true
            };
            this.Controls.Add(titleLabel);
        }

        private GeneratedForm _generatedForm;

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (_generatedForm.ValidateForm())
            {
                try
                {
                    var customer = _generatedForm.GetEntityFromForm<Customer>();
                    
                    // Additional custom validation
                    if (!IsValidCustomer(customer))
                    {
                        return;
                    }

                    if (_isEditMode)
                    {
                        // Update existing customer
                        customer.CustomerID = _currentCustomer.CustomerID;
                        customer.CreatedDate = _currentCustomer.CreatedDate;
                        UpdateCustomer(customer);
                        MessageBox.Show("Customer updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // Add new customer
                        customer.CreatedDate = DateTime.Now;
                        AddCustomer(customer);
                        MessageBox.Show("Customer added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving customer: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private bool IsValidCustomer(Customer customer)
        {
            var errorMessages = new List<string>();

            // Email validation
            if (!string.IsNullOrWhiteSpace(customer.Email) && !IsValidEmail(customer.Email))
            {
                errorMessages.Add("Please enter a valid email address.");
            }

            // Phone validation
            if (!string.IsNullOrWhiteSpace(customer.Phone) && !IsValidPhone(customer.Phone))
            {
                errorMessages.Add("Please enter a valid phone number.");
            }

            // Credit limit validation
            if (customer.CreditLimit < 0)
            {
                errorMessages.Add("Credit limit cannot be negative.");
            }

            // Age validation
            var age = DateTime.Now.Year - customer.DateOfBirth.Year;
            if (customer.DateOfBirth > DateTime.Now.AddYears(-age))
            {
                age--;
            }
            if (age < 18)
            {
                errorMessages.Add("Customer must be at least 18 years old.");
            }

            if (errorMessages.Count > 0)
            {
                MessageBox.Show(string.Join("\n", errorMessages), "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhone(string phone)
        {
            // Simple phone validation - remove non-digits and check length
            var digits = System.Text.RegularExpressions.Regex.Replace(phone, @"[^\d]", "");
            return digits.Length >= 10 && digits.Length <= 15;
        }

        private void AddCustomer(Customer customer)
        {
            var query = @"
                INSERT INTO Customers 
                (FirstName, LastName, Email, Phone, Address, City, State, ZipCode, 
                 DateOfBirth, IsActive, CustomerType, CreditLimit, CreatedDate, TotalPurchases)
                VALUES 
                (@FirstName, @LastName, @Email, @Phone, @Address, @City, @State, @ZipCode, 
                 @DateOfBirth, @IsActive, @CustomerType, @CreditLimit, @CreatedDate, @TotalPurchases)";

            var parameters = new Dictionary<string, object>
            {
                ["@FirstName"] = customer.FirstName,
                ["@LastName"] = customer.LastName,
                ["@Email"] = customer.Email,
                ["@Phone"] = customer.Phone,
                ["@Address"] = customer.Address,
                ["@City"] = customer.City,
                ["@State"] = customer.State,
                ["@ZipCode"] = customer.ZipCode,
                ["@DateOfBirth"] = customer.DateOfBirth,
                ["@IsActive"] = customer.IsActive,
                ["@CustomerType"] = customer.CustomerType,
                ["@CreditLimit"] = customer.CreditLimit,
                ["@CreatedDate"] = customer.CreatedDate,
                ["@TotalPurchases"] = customer.TotalPurchases
            };

            _dbHelper.ExecuteNonQuery(query, parameters);
        }

        private void UpdateCustomer(Customer customer)
        {
            var query = @"
                UPDATE Customers 
                SET FirstName = @FirstName, LastName = @LastName, Email = @Email, Phone = @Phone,
                    Address = @Address, City = @City, State = @State, ZipCode = @ZipCode,
                    DateOfBirth = @DateOfBirth, IsActive = @IsActive, CustomerType = @CustomerType,
                    CreditLimit = @CreditLimit
                WHERE CustomerID = @CustomerID";

            var parameters = new Dictionary<string, object>
            {
                ["@CustomerID"] = customer.CustomerID,
                ["@FirstName"] = customer.FirstName,
                ["@LastName"] = customer.LastName,
                ["@Email"] = customer.Email,
                ["@Phone"] = customer.Phone,
                ["@Address"] = customer.Address,
                ["@City"] = customer.City,
                ["@State"] = customer.State,
                ["@ZipCode"] = customer.ZipCode,
                ["@DateOfBirth"] = customer.DateOfBirth,
                ["@IsActive"] = customer.IsActive,
                ["@CustomerType"] = customer.CustomerType,
                ["@CreditLimit"] = customer.CreditLimit
            };

            _dbHelper.ExecuteNonQuery(query, parameters);
        }
    }

    // Usage example class
    public static class CustomerFormUsageExample
    {
        public static void ShowAddCustomerForm()
        {
            var form = new CustomerFormExample();
            var result = form.ShowDialog();
            
            if (result == DialogResult.OK)
            {
                // Customer was added successfully
                Console.WriteLine("Customer added successfully!");
            }
        }

        public static void ShowEditCustomerForm(Customer customer)
        {
            var form = new CustomerFormExample(customer);
            var result = form.ShowDialog();
            
            if (result == DialogResult.OK)
            {
                // Customer was updated successfully
                Console.WriteLine("Customer updated successfully!");
            }
        }

        public static void ShowSimpleGeneratedForm()
        {
            // Quick way to generate a form without customization
            var generator = new FormGenerator();
            var form = generator.GenerateForm<Customer>("Simple Customer Form");
            
            form.SaveButton.Click += (s, e) => {
                if (form.ValidateForm())
                {
                    var customer = form.GetEntityFromForm<Customer>();
                    // Save customer logic here
                    MessageBox.Show($"Customer: {customer.FirstName} {customer.LastName}", "Customer Created");
                    form.Close();
                }
            };
            
            form.ShowDialog();
        }
    }
}