using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BusinessManagementSystem.Data;

namespace BusinessManagementSystem.Forms.Templates
{
    public partial class CrudFormTemplate<T> : BaseForm where T : class, new()
    {
        protected DatabaseHelper _dbHelper;
        protected DataGridView _dataGridView;
        protected Panel _searchPanel;
        protected Panel _formPanel;
        protected Panel _buttonPanel;
        protected TextBox _searchBox;
        protected Button _searchButton;
        protected Button _addButton;
        protected Button _editButton;
        protected Button _deleteButton;
        protected Button _refreshButton;
        protected Button _saveButton;
        protected Button _cancelButton;
        protected Label _titleLabel;
        
        protected List<T> _dataSource;
        protected T _currentRecord;
        protected bool _isEditing;
        
        protected Dictionary<string, Control> _formControls;
        protected Dictionary<string, Func<T, object>> _propertyGetters;
        protected Dictionary<string, Action<T, object>> _propertySetters;
        
        public CrudFormTemplate()
        {
            _dbHelper = new DatabaseHelper();
            _dataSource = new List<T>();
            _formControls = new Dictionary<string, Control>();
            _propertyGetters = new Dictionary<string, Func<T, object>>();
            _propertySetters = new Dictionary<string, Action<T, object>>();
            
            InitializeTemplate();
            SetupEventHandlers();
        }
        
        private void InitializeTemplate()
        {
            this.Size = new Size(1000, 700);
            this.Text = $"{typeof(T).Name} Management";
            
            CreateTitleLabel();
            CreateSearchPanel();
            CreateDataGridView();
            CreateFormPanel();
            CreateButtonPanel();
            
            LoadData();
        }
        
        private void CreateTitleLabel()
        {
            _titleLabel = CreateStyledLabel($"{typeof(T).Name} Management", 
                new Font("Segoe UI", 16F, FontStyle.Bold), PrimaryColor);
            _titleLabel.Location = new Point(20, 20);
            this.Controls.Add(_titleLabel);
        }
        
        private void CreateSearchPanel()
        {
            _searchPanel = CreateCardPanel(960, 60);
            _searchPanel.Location = new Point(20, 60);
            
            var searchLabel = CreateStyledLabel("Search:", new Font("Segoe UI", 9F, FontStyle.Bold));
            searchLabel.Location = new Point(10, 20);
            _searchPanel.Controls.Add(searchLabel);
            
            _searchBox = CreateStyledTextBox(300, 25);
            _searchBox.Location = new Point(60, 17);
            _searchPanel.Controls.Add(_searchBox);
            
            _searchButton = CreateStyledButton("Search", PrimaryColor, Color.White, 80, 25);
            _searchButton.Location = new Point(370, 17);
            _searchPanel.Controls.Add(_searchButton);
            
            _refreshButton = CreateStyledButton("Refresh", SecondaryColor, Color.White, 80, 25);
            _refreshButton.Location = new Point(460, 17);
            _searchPanel.Controls.Add(_refreshButton);
            
            this.Controls.Add(_searchPanel);
        }
        
        private void CreateDataGridView()
        {
            _dataGridView = CreateStyledDataGridView();
            _dataGridView.Size = new Size(960, 300);
            _dataGridView.Location = new Point(20, 140);
            this.Controls.Add(_dataGridView);
        }
        
        private void CreateFormPanel()
        {
            _formPanel = CreateCardPanel(960, 180);
            _formPanel.Location = new Point(20, 460);
            
            var formTitle = CreateStyledLabel("Record Details", 
                new Font("Segoe UI", 12F, FontStyle.Bold), PrimaryColor);
            formTitle.Location = new Point(10, 10);
            _formPanel.Controls.Add(formTitle);
            
            this.Controls.Add(_formPanel);
        }
        
        private void CreateButtonPanel()
        {
            _buttonPanel = new Panel
            {
                Size = new Size(960, 50),
                Location = new Point(20, 650),
                BackColor = Color.Transparent
            };
            
            _addButton = CreateStyledButton("Add New", AccentColor, Color.White, 100, 35);
            _addButton.Location = new Point(0, 5);
            _buttonPanel.Controls.Add(_addButton);
            
            _editButton = CreateStyledButton("Edit", WarningColor, Color.White, 100, 35);
            _editButton.Location = new Point(110, 5);
            _buttonPanel.Controls.Add(_editButton);
            
            _deleteButton = CreateStyledButton("Delete", DangerColor, Color.White, 100, 35);
            _deleteButton.Location = new Point(220, 5);
            _buttonPanel.Controls.Add(_deleteButton);
            
            _saveButton = CreateStyledButton("Save", AccentColor, Color.White, 100, 35);
            _saveButton.Location = new Point(660, 5);
            _saveButton.Visible = false;
            _buttonPanel.Controls.Add(_saveButton);
            
            _cancelButton = CreateStyledButton("Cancel", DangerColor, Color.White, 100, 35);
            _cancelButton.Location = new Point(770, 5);
            _cancelButton.Visible = false;
            _buttonPanel.Controls.Add(_cancelButton);
            
            this.Controls.Add(_buttonPanel);
        }
        
        private void SetupEventHandlers()
        {
            _searchButton.Click += SearchButton_Click;
            _refreshButton.Click += RefreshButton_Click;
            _addButton.Click += AddButton_Click;
            _editButton.Click += EditButton_Click;
            _deleteButton.Click += DeleteButton_Click;
            _saveButton.Click += SaveButton_Click;
            _cancelButton.Click += CancelButton_Click;
            _dataGridView.SelectionChanged += DataGridView_SelectionChanged;
            _searchBox.KeyPress += SearchBox_KeyPress;
        }
        
        #region Event Handlers
        
        private void SearchButton_Click(object sender, EventArgs e)
        {
            PerformSearch();
        }
        
        private void RefreshButton_Click(object sender, EventArgs e)
        {
            LoadData();
        }
        
        private void AddButton_Click(object sender, EventArgs e)
        {
            StartAddMode();
        }
        
        private void EditButton_Click(object sender, EventArgs e)
        {
            if (_dataGridView.SelectedRows.Count > 0)
            {
                StartEditMode();
            }
            else
            {
                ShowWarningMessage("Please select a record to edit.");
            }
        }
        
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (_dataGridView.SelectedRows.Count > 0)
            {
                if (ShowConfirmation("Are you sure you want to delete this record?"))
                {
                    DeleteRecord();
                }
            }
            else
            {
                ShowWarningMessage("Please select a record to delete.");
            }
        }
        
        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                SaveRecord();
            }
        }
        
        private void CancelButton_Click(object sender, EventArgs e)
        {
            CancelOperation();
        }
        
        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (_dataGridView.SelectedRows.Count > 0)
            {
                DisplaySelectedRecord();
            }
        }
        
        private void SearchBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                PerformSearch();
            }
        }
        
        #endregion
        
        #region Abstract/Virtual Methods for Override
        
        protected virtual void LoadData()
        {
            // Override this method to load data from database
            // _dataSource = _dbHelper.GetAll<T>();
            // _dataGridView.DataSource = _dataSource;
        }
        
        protected virtual void PerformSearch()
        {
            // Override this method to implement search functionality
            var searchTerm = _searchBox.Text.Trim();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                // Filter data based on search term
            }
        }
        
        protected virtual void StartAddMode()
        {
            _currentRecord = new T();
            _isEditing = false;
            ShowFormMode();
            ClearForm();
        }
        
        protected virtual void StartEditMode()
        {
            _isEditing = true;
            ShowFormMode();
            PopulateForm();
        }
        
        protected virtual void SaveRecord()
        {
            try
            {
                UpdateRecordFromForm();
                
                if (_isEditing)
                {
                    // Update existing record
                    OnUpdateRecord();
                }
                else
                {
                    // Add new record
                    OnAddRecord();
                }
                
                ShowSuccessMessage("Record saved successfully!");
                CancelOperation();
                LoadData();
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error saving record: {ex.Message}");
            }
        }
        
        protected virtual void DeleteRecord()
        {
            try
            {
                OnDeleteRecord();
                ShowSuccessMessage("Record deleted successfully!");
                LoadData();
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error deleting record: {ex.Message}");
            }
        }
        
        protected virtual void OnAddRecord()
        {
            // Override this method to implement add functionality
            // _dbHelper.Insert(_currentRecord);
        }
        
        protected virtual void OnUpdateRecord()
        {
            // Override this method to implement update functionality
            // _dbHelper.Update(_currentRecord);
        }
        
        protected virtual void OnDeleteRecord()
        {
            // Override this method to implement delete functionality
            // var selectedRecord = GetSelectedRecord();
            // _dbHelper.Delete(selectedRecord);
        }
        
        protected virtual bool ValidateForm()
        {
            // Override this method to implement form validation
            return true;
        }
        
        protected virtual void ClearForm()
        {
            foreach (var control in _formControls.Values)
            {
                if (control is TextBox textBox)
                    textBox.Clear();
                else if (control is ComboBox comboBox)
                    comboBox.SelectedIndex = -1;
                else if (control is CheckBox checkBox)
                    checkBox.Checked = false;
            }
        }
        
        protected virtual void PopulateForm()
        {
            _currentRecord = GetSelectedRecord();
            
            foreach (var kvp in _formControls)
            {
                var propertyName = kvp.Key;
                var control = kvp.Value;
                
                if (_propertyGetters.ContainsKey(propertyName))
                {
                    var value = _propertyGetters[propertyName](_currentRecord);
                    
                    if (control is TextBox textBox)
                        textBox.Text = value?.ToString() ?? string.Empty;
                    else if (control is ComboBox comboBox)
                        comboBox.SelectedValue = value;
                    else if (control is CheckBox checkBox)
                        checkBox.Checked = (bool)(value ?? false);
                }
            }
        }
        
        protected virtual void UpdateRecordFromForm()
        {
            foreach (var kvp in _formControls)
            {
                var propertyName = kvp.Key;
                var control = kvp.Value;
                
                if (_propertySetters.ContainsKey(propertyName))
                {
                    object value = null;
                    
                    if (control is TextBox textBox)
                        value = textBox.Text;
                    else if (control is ComboBox comboBox)
                        value = comboBox.SelectedValue;
                    else if (control is CheckBox checkBox)
                        value = checkBox.Checked;
                    
                    _propertySetters[propertyName](_currentRecord, value);
                }
            }
        }
        
        #endregion
        
        #region Helper Methods
        
        protected T GetSelectedRecord()
        {
            if (_dataGridView.SelectedRows.Count > 0)
            {
                var index = _dataGridView.SelectedRows[0].Index;
                return _dataSource[index];
            }
            return default(T);
        }
        
        protected void DisplaySelectedRecord()
        {
            if (!_isEditing && !(_saveButton.Visible))
            {
                PopulateForm();
            }
        }
        
        protected void ShowFormMode()
        {
            _addButton.Visible = false;
            _editButton.Visible = false;
            _deleteButton.Visible = false;
            _saveButton.Visible = true;
            _cancelButton.Visible = true;
            
            // Enable form controls
            foreach (var control in _formControls.Values)
            {
                control.Enabled = true;
            }
        }
        
        protected void CancelOperation()
        {
            _addButton.Visible = true;
            _editButton.Visible = true;
            _deleteButton.Visible = true;
            _saveButton.Visible = false;
            _cancelButton.Visible = false;
            
            _currentRecord = default(T);
            _isEditing = false;
            
            // Disable form controls
            foreach (var control in _formControls.Values)
            {
                control.Enabled = false;
            }
            
            ClearForm();
        }
        
        protected void AddFormControl(string propertyName, Control control, 
            Func<T, object> getter, Action<T, object> setter)
        {
            _formControls[propertyName] = control;
            _propertyGetters[propertyName] = getter;
            _propertySetters[propertyName] = setter;
            
            control.Enabled = false; // Initially disabled
            _formPanel.Controls.Add(control);
        }
        
        #endregion
    }
}