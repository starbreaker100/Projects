using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BusinessManagementSystem.Forms.Templates;
using BusinessManagementSystem.Models;

namespace BusinessManagementSystem.Forms
{
    public partial class UserManagementForm : CrudFormTemplate<User>
    {
        private TextBox _txtUsername;
        private TextBox _txtPassword;
        private TextBox _txtFullName;
        private TextBox _txtEmail;
        private ComboBox _cmbRole;
        private CheckBox _chkIsActive;

        public UserManagementForm()
        {
            InitializeUserForm();
            LoadData();
        }

        private void InitializeUserForm()
        {
            this.Text = "User Management";
            this.Size = new Size(1000, 700);
            
            CreateFormFields();
            ArrangeFormFields();
            SetupFormBindings();
        }

        private void CreateFormFields()
        {
            // Username
            var lblUsername = CreateStyledLabel("Username:", new Font("Segoe UI", 9F, FontStyle.Bold));
            lblUsername.Location = new Point(20, 50);
            _formPanel.Controls.Add(lblUsername);

            _txtUsername = CreateStyledTextBox(250, 25);
            _txtUsername.Location = new Point(120, 47);

            // Password
            var lblPassword = CreateStyledLabel("Password:", new Font("Segoe UI", 9F, FontStyle.Bold));
            lblPassword.Location = new Point(20, 85);
            _formPanel.Controls.Add(lblPassword);

            _txtPassword = CreateStyledTextBox(250, 25);
            _txtPassword.Location = new Point(120, 82);
            _txtPassword.UseSystemPasswordChar = true;

            // Full Name
            var lblFullName = CreateStyledLabel("Full Name:", new Font("Segoe UI", 9F, FontStyle.Bold));
            lblFullName.Location = new Point(20, 120);
            _formPanel.Controls.Add(lblFullName);

            _txtFullName = CreateStyledTextBox(250, 25);
            _txtFullName.Location = new Point(120, 117);

            // Email
            var lblEmail = CreateStyledLabel("Email:", new Font("Segoe UI", 9F, FontStyle.Bold));
            lblEmail.Location = new Point(400, 50);
            _formPanel.Controls.Add(lblEmail);

            _txtEmail = CreateStyledTextBox(250, 25);
            _txtEmail.Location = new Point(500, 47);

            // Role
            var lblRole = CreateStyledLabel("Role:", new Font("Segoe UI", 9F, FontStyle.Bold));
            lblRole.Location = new Point(400, 85);
            _formPanel.Controls.Add(lblRole);

            _cmbRole = CreateStyledComboBox(250, 25);
            _cmbRole.Location = new Point(500, 82);
            _cmbRole.Items.AddRange(new string[] { "Admin", "User", "Manager" });
            _cmbRole.SelectedIndex = 1; // Default to "User"

            // Is Active
            _chkIsActive = new CheckBox
            {
                Text = "Active",
                Location = new Point(500, 120),
                AutoSize = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = TextColor,
                Checked = true
            };
            _formPanel.Controls.Add(_chkIsActive);
        }

        private void ArrangeFormFields()
        {
            // Form fields are already positioned in CreateFormFields()
            // This method can be used for additional layout adjustments
        }

        private void SetupFormBindings()
        {
            // Register form controls with the template
            AddFormControl("Username", _txtUsername, u => u.Username, (u, v) => u.Username = v?.ToString() ?? string.Empty);
            AddFormControl("Password", _txtPassword, u => u.Password, (u, v) => u.Password = v?.ToString() ?? string.Empty);
            AddFormControl("FullName", _txtFullName, u => u.FullName, (u, v) => u.FullName = v?.ToString() ?? string.Empty);
            AddFormControl("Email", _txtEmail, u => u.Email, (u, v) => u.Email = v?.ToString() ?? string.Empty);
            AddFormControl("Role", _cmbRole, u => u.Role, (u, v) => u.Role = v?.ToString() ?? "User");
            AddFormControl("IsActive", _chkIsActive, u => u.IsActive, (u, v) => u.IsActive = (bool)(v ?? true));
        }

        protected override void LoadData()
        {
            try
            {
                var query = @"
                    SELECT UserID, Username, Password, Role, FullName, Email, IsActive, CreatedDate 
                    FROM Users 
                    ORDER BY Username";
                
                var dataTable = _dbHelper.ExecuteQuery(query);
                _dataSource = new List<User>();
                
                foreach (System.Data.DataRow row in dataTable.Rows)
                {
                    _dataSource.Add(new User
                    {
                        UserID = Convert.ToInt32(row["UserID"]),
                        Username = row["Username"].ToString(),
                        Password = row["Password"].ToString(),
                        Role = row["Role"].ToString(),
                        FullName = row["FullName"].ToString(),
                        Email = row["Email"].ToString(),
                        IsActive = Convert.ToBoolean(row["IsActive"]),
                        CreatedDate = Convert.ToDateTime(row["CreatedDate"])
                    });
                }
                
                // Create a display-friendly version for the grid
                var displayData = _dataSource.Select(u => new
                {
                    u.UserID,
                    u.Username,
                    u.FullName,
                    u.Email,
                    u.Role,
                    Status = u.IsActive ? "Active" : "Inactive",
                    CreatedDate = u.CreatedDate.ToString("yyyy-MM-dd")
                }).ToList();
                
                _dataGridView.DataSource = displayData;
                
                // Configure grid columns
                if (_dataGridView.Columns.Count > 0)
                {
                    _dataGridView.Columns["UserID"].Width = 60;
                    _dataGridView.Columns["Username"].Width = 120;
                    _dataGridView.Columns["FullName"].Width = 150;
                    _dataGridView.Columns["Email"].Width = 200;
                    _dataGridView.Columns["Role"].Width = 80;
                    _dataGridView.Columns["Status"].Width = 80;
                    _dataGridView.Columns["CreatedDate"].Width = 100;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading users: {ex.Message}");
            }
        }

        protected override void PerformSearch()
        {
            try
            {
                var searchTerm = _searchBox.Text.Trim();
                
                if (string.IsNullOrEmpty(searchTerm))
                {
                    LoadData();
                    return;
                }
                
                var filteredData = _dataSource.Where(u => 
                    u.Username.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    u.FullName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    u.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    u.Role.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                ).ToList();
                
                var displayData = filteredData.Select(u => new
                {
                    u.UserID,
                    u.Username,
                    u.FullName,
                    u.Email,
                    u.Role,
                    Status = u.IsActive ? "Active" : "Inactive",
                    CreatedDate = u.CreatedDate.ToString("yyyy-MM-dd")
                }).ToList();
                
                _dataGridView.DataSource = displayData;
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error searching users: {ex.Message}");
            }
        }

        protected override void OnAddRecord()
        {
            try
            {
                var query = @"
                    INSERT INTO Users (Username, Password, Role, FullName, Email, IsActive, CreatedDate)
                    VALUES (@Username, @Password, @Role, @FullName, @Email, @IsActive, @CreatedDate)";
                
                var parameters = new Dictionary<string, object>
                {
                    ["@Username"] = _currentRecord.Username,
                    ["@Password"] = _currentRecord.Password,
                    ["@Role"] = _currentRecord.Role,
                    ["@FullName"] = _currentRecord.FullName,
                    ["@Email"] = _currentRecord.Email,
                    ["@IsActive"] = _currentRecord.IsActive,
                    ["@CreatedDate"] = DateTime.Now
                };
                
                _dbHelper.ExecuteNonQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding user: {ex.Message}");
            }
        }

        protected override void OnUpdateRecord()
        {
            try
            {
                var query = @"
                    UPDATE Users 
                    SET Username = @Username, Password = @Password, Role = @Role, 
                        FullName = @FullName, Email = @Email, IsActive = @IsActive
                    WHERE UserID = @UserID";
                
                var parameters = new Dictionary<string, object>
                {
                    ["@UserID"] = _currentRecord.UserID,
                    ["@Username"] = _currentRecord.Username,
                    ["@Password"] = _currentRecord.Password,
                    ["@Role"] = _currentRecord.Role,
                    ["@FullName"] = _currentRecord.FullName,
                    ["@Email"] = _currentRecord.Email,
                    ["@IsActive"] = _currentRecord.IsActive
                };
                
                _dbHelper.ExecuteNonQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating user: {ex.Message}");
            }
        }

        protected override void OnDeleteRecord()
        {
            try
            {
                var selectedRecord = GetSelectedRecord();
                if (selectedRecord != null)
                {
                    var query = "DELETE FROM Users WHERE UserID = @UserID";
                    var parameters = new Dictionary<string, object>
                    {
                        ["@UserID"] = selectedRecord.UserID
                    };
                    
                    _dbHelper.ExecuteNonQuery(query, parameters);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting user: {ex.Message}");
            }
        }

        protected override bool ValidateForm()
        {
            // Clear any previous validation highlights
            ResetValidationHighlights();

            var isValid = true;
            var errorMessages = new List<string>();

            // Username validation
            if (string.IsNullOrWhiteSpace(_txtUsername.Text))
            {
                errorMessages.Add("Username is required.");
                HighlightValidationError(_txtUsername);
                isValid = false;
            }
            else if (_txtUsername.Text.Length < 3)
            {
                errorMessages.Add("Username must be at least 3 characters long.");
                HighlightValidationError(_txtUsername);
                isValid = false;
            }
            else if (!_isEditing && IsUsernameExists(_txtUsername.Text))
            {
                errorMessages.Add("Username already exists.");
                HighlightValidationError(_txtUsername);
                isValid = false;
            }

            // Password validation
            if (string.IsNullOrWhiteSpace(_txtPassword.Text))
            {
                errorMessages.Add("Password is required.");
                HighlightValidationError(_txtPassword);
                isValid = false;
            }
            else if (_txtPassword.Text.Length < 6)
            {
                errorMessages.Add("Password must be at least 6 characters long.");
                HighlightValidationError(_txtPassword);
                isValid = false;
            }

            // Full Name validation
            if (string.IsNullOrWhiteSpace(_txtFullName.Text))
            {
                errorMessages.Add("Full Name is required.");
                HighlightValidationError(_txtFullName);
                isValid = false;
            }

            // Email validation
            if (!string.IsNullOrWhiteSpace(_txtEmail.Text))
            {
                if (!IsValidEmail(_txtEmail.Text))
                {
                    errorMessages.Add("Please enter a valid email address.");
                    HighlightValidationError(_txtEmail);
                    isValid = false;
                }
                else if (!_isEditing && IsEmailExists(_txtEmail.Text))
                {
                    errorMessages.Add("Email already exists.");
                    HighlightValidationError(_txtEmail);
                    isValid = false;
                }
            }

            // Role validation
            if (_cmbRole.SelectedIndex == -1)
            {
                errorMessages.Add("Please select a role.");
                HighlightValidationError(_cmbRole);
                isValid = false;
            }

            if (!isValid)
            {
                ShowErrorMessage(string.Join("\n", errorMessages));
            }

            return isValid;
        }

        private bool IsUsernameExists(string username)
        {
            try
            {
                var query = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                if (_isEditing)
                {
                    query += " AND UserID != @UserID";
                }
                
                var parameters = new Dictionary<string, object>
                {
                    ["@Username"] = username
                };
                
                if (_isEditing)
                {
                    parameters["@UserID"] = _currentRecord.UserID;
                }
                
                var count = _dbHelper.ExecuteScalar(query, parameters);
                return Convert.ToInt32(count) > 0;
            }
            catch
            {
                return false;
            }
        }

        private bool IsEmailExists(string email)
        {
            try
            {
                var query = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
                if (_isEditing)
                {
                    query += " AND UserID != @UserID";
                }
                
                var parameters = new Dictionary<string, object>
                {
                    ["@Email"] = email
                };
                
                if (_isEditing)
                {
                    parameters["@UserID"] = _currentRecord.UserID;
                }
                
                var count = _dbHelper.ExecuteScalar(query, parameters);
                return Convert.ToInt32(count) > 0;
            }
            catch
            {
                return false;
            }
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

        private void HighlightValidationError(Control control)
        {
            control.BackColor = Color.FromArgb(255, 235, 235);
        }

        private void ResetValidationHighlights()
        {
            _txtUsername.BackColor = Color.White;
            _txtPassword.BackColor = Color.White;
            _txtFullName.BackColor = Color.White;
            _txtEmail.BackColor = Color.White;
            _cmbRole.BackColor = Color.White;
        }

        protected override void ClearForm()
        {
            base.ClearForm();
            ResetValidationHighlights();
            _cmbRole.SelectedIndex = 1; // Default to "User"
            _chkIsActive.Checked = true;
        }
    }
}