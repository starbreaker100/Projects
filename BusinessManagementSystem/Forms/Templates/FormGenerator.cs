using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace BusinessManagementSystem.Forms.Templates
{
    public class FormGenerator
    {
        private readonly Dictionary<Type, Func<Control>> _controlFactories;
        private readonly Dictionary<Type, int> _controlWidths;
        private readonly Dictionary<Type, int> _controlHeights;

        public FormGenerator()
        {
            InitializeControlFactories();
            InitializeControlDimensions();
        }

        private void InitializeControlFactories()
        {
            _controlFactories = new Dictionary<Type, Func<Control>>
            {
                { typeof(string), () => new TextBox { BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 9F) } },
                { typeof(int), () => new NumericUpDown { BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 9F) } },
                { typeof(decimal), () => new NumericUpDown { BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 9F), DecimalPlaces = 2 } },
                { typeof(DateTime), () => new DateTimePicker { Font = new Font("Segoe UI", 9F) } },
                { typeof(bool), () => new CheckBox { Font = new Font("Segoe UI", 9F) } },
                { typeof(double), () => new NumericUpDown { BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 9F), DecimalPlaces = 2 } },
                { typeof(float), () => new NumericUpDown { BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 9F), DecimalPlaces = 2 } }
            };
        }

        private void InitializeControlDimensions()
        {
            _controlWidths = new Dictionary<Type, int>
            {
                { typeof(string), 200 },
                { typeof(int), 150 },
                { typeof(decimal), 150 },
                { typeof(DateTime), 150 },
                { typeof(bool), 100 },
                { typeof(double), 150 },
                { typeof(float), 150 }
            };

            _controlHeights = new Dictionary<Type, int>
            {
                { typeof(string), 25 },
                { typeof(int), 25 },
                { typeof(decimal), 25 },
                { typeof(DateTime), 25 },
                { typeof(bool), 25 },
                { typeof(double), 25 },
                { typeof(float), 25 }
            };
        }

        public GeneratedForm GenerateForm<T>(string title = null, FormGenerationOptions options = null) where T : class, new()
        {
            options = options ?? new FormGenerationOptions();
            var form = new GeneratedForm();
            var entityType = typeof(T);
            
            // Set form properties
            form.Text = title ?? $"{entityType.Name} Form";
            form.Size = new Size(options.FormWidth, options.FormHeight);
            form.StartPosition = FormStartPosition.CenterScreen;
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.MaximizeBox = false;
            form.BackColor = Color.FromArgb(236, 240, 241);
            form.Font = new Font("Segoe UI", 9F);

            // Generate form controls
            var properties = GetEntityProperties<T>(options);
            var controls = CreateFormControls(properties, options);
            
            // Layout controls
            LayoutControls(form, controls, options);
            
            // Create buttons
            CreateFormButtons(form, options);
            
            // Set up form data binding
            SetupDataBinding<T>(form, controls, properties);
            
            return form;
        }

        private List<PropertyInfo> GetEntityProperties<T>(FormGenerationOptions options)
        {
            var properties = typeof(T).GetProperties()
                .Where(p => p.CanRead && p.CanWrite)
                .Where(p => !options.ExcludedProperties.Contains(p.Name))
                .ToList();

            if (options.IncludedProperties.Any())
            {
                properties = properties.Where(p => options.IncludedProperties.Contains(p.Name)).ToList();
            }

            return properties.OrderBy(p => options.PropertyOrder.ContainsKey(p.Name) ? options.PropertyOrder[p.Name] : int.MaxValue).ToList();
        }

        private Dictionary<string, FormControlInfo> CreateFormControls(List<PropertyInfo> properties, FormGenerationOptions options)
        {
            var controls = new Dictionary<string, FormControlInfo>();

            foreach (var property in properties)
            {
                var controlInfo = new FormControlInfo
                {
                    Property = property,
                    Label = CreateLabel(property, options),
                    Control = CreateControl(property, options)
                };

                controls[property.Name] = controlInfo;
            }

            return controls;
        }

        private Label CreateLabel(PropertyInfo property, FormGenerationOptions options)
        {
            var labelText = options.PropertyDisplayNames.ContainsKey(property.Name) 
                ? options.PropertyDisplayNames[property.Name] 
                : FormatPropertyName(property.Name);

            var label = new Label
            {
                Text = labelText + ":",
                AutoSize = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80)
            };

            return label;
        }

        private Control CreateControl(PropertyInfo property, FormGenerationOptions options)
        {
            var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            
            // Check for custom control factory
            if (options.CustomControlFactories.ContainsKey(property.Name))
            {
                return options.CustomControlFactories[property.Name]();
            }

            // Check for enum types
            if (propertyType.IsEnum)
            {
                var comboBox = new ComboBox
                {
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Font = new Font("Segoe UI", 9F),
                    Size = new Size(200, 25)
                };

                foreach (var enumValue in Enum.GetValues(propertyType))
                {
                    comboBox.Items.Add(enumValue);
                }

                return comboBox;
            }

            // Check for foreign key properties (ending with ID)
            if (property.Name.EndsWith("ID") && propertyType == typeof(int))
            {
                var comboBox = new ComboBox
                {
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Font = new Font("Segoe UI", 9F),
                    Size = new Size(200, 25)
                };

                // This can be populated later with foreign key data
                return comboBox;
            }

            // Create control based on property type
            if (_controlFactories.ContainsKey(propertyType))
            {
                var control = _controlFactories[propertyType]();
                control.Size = new Size(
                    _controlWidths.ContainsKey(propertyType) ? _controlWidths[propertyType] : 200,
                    _controlHeights.ContainsKey(propertyType) ? _controlHeights[propertyType] : 25
                );

                // Set specific properties for numeric controls
                if (control is NumericUpDown numericUpDown)
                {
                    numericUpDown.Maximum = options.NumericMaximum;
                    numericUpDown.Minimum = options.NumericMinimum;
                }

                return control;
            }

            // Default to TextBox
            return new TextBox
            {
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 9F),
                Size = new Size(200, 25)
            };
        }

        private void LayoutControls(Form form, Dictionary<string, FormControlInfo> controls, FormGenerationOptions options)
        {
            var currentY = options.StartY;
            var currentX = options.StartX;
            var columnCount = 0;

            foreach (var controlInfo in controls.Values)
            {
                // Position label
                controlInfo.Label.Location = new Point(currentX, currentY);
                form.Controls.Add(controlInfo.Label);

                // Position control
                controlInfo.Control.Location = new Point(currentX + options.LabelWidth, currentY - 3);
                form.Controls.Add(controlInfo.Control);

                // Move to next position
                columnCount++;
                if (columnCount >= options.ColumnsPerRow)
                {
                    currentY += options.RowHeight;
                    currentX = options.StartX;
                    columnCount = 0;
                }
                else
                {
                    currentX += options.ColumnWidth;
                }
            }
        }

        private void CreateFormButtons(Form form, FormGenerationOptions options)
        {
            var buttonY = form.ClientSize.Height - 60;
            var buttonWidth = 100;
            var buttonHeight = 35;
            var buttonSpacing = 10;

            // Save button
            var saveButton = new Button
            {
                Text = "Save",
                Size = new Size(buttonWidth, buttonHeight),
                Location = new Point(form.ClientSize.Width - (buttonWidth * 2) - buttonSpacing - 20, buttonY),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                UseVisualStyleBackColor = false
            };
            saveButton.FlatAppearance.BorderSize = 0;
            form.Controls.Add(saveButton);

            // Cancel button
            var cancelButton = new Button
            {
                Text = "Cancel",
                Size = new Size(buttonWidth, buttonHeight),
                Location = new Point(form.ClientSize.Width - buttonWidth - 20, buttonY),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                UseVisualStyleBackColor = false
            };
            cancelButton.FlatAppearance.BorderSize = 0;
            cancelButton.Click += (s, e) => form.Close();
            form.Controls.Add(cancelButton);

            // Store button references
            ((GeneratedForm)form).SaveButton = saveButton;
            ((GeneratedForm)form).CancelButton = cancelButton;
        }

        private void SetupDataBinding<T>(GeneratedForm form, Dictionary<string, FormControlInfo> controls, List<PropertyInfo> properties)
        {
            form.SetupDataBinding<T>(controls, properties);
        }

        private string FormatPropertyName(string propertyName)
        {
            // Convert PascalCase to readable format
            var result = "";
            for (int i = 0; i < propertyName.Length; i++)
            {
                if (i > 0 && char.IsUpper(propertyName[i]))
                {
                    result += " ";
                }
                result += propertyName[i];
            }
            return result;
        }
    }

    public class FormControlInfo
    {
        public PropertyInfo Property { get; set; }
        public Label Label { get; set; }
        public Control Control { get; set; }
    }

    public class FormGenerationOptions
    {
        public int FormWidth { get; set; } = 800;
        public int FormHeight { get; set; } = 600;
        public int StartX { get; set; } = 20;
        public int StartY { get; set; } = 20;
        public int LabelWidth { get; set; } = 120;
        public int ColumnWidth { get; set; } = 300;
        public int RowHeight { get; set; } = 40;
        public int ColumnsPerRow { get; set; } = 2;
        public decimal NumericMaximum { get; set; } = 999999999;
        public decimal NumericMinimum { get; set; } = -999999999;
        
        public List<string> ExcludedProperties { get; set; } = new List<string> { "CreatedDate", "UpdatedDate", "ID" };
        public List<string> IncludedProperties { get; set; } = new List<string>();
        public Dictionary<string, string> PropertyDisplayNames { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, int> PropertyOrder { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, Func<Control>> CustomControlFactories { get; set; } = new Dictionary<string, Func<Control>>();
    }

    public class GeneratedForm : Form
    {
        public Button SaveButton { get; set; }
        public Button CancelButton { get; set; }
        private Dictionary<string, FormControlInfo> _controls;
        private List<PropertyInfo> _properties;

        public void SetupDataBinding<T>(Dictionary<string, FormControlInfo> controls, List<PropertyInfo> properties)
        {
            _controls = controls;
            _properties = properties;
        }

        public void PopulateForm<T>(T entity) where T : class
        {
            if (entity == null || _controls == null) return;

            foreach (var property in _properties)
            {
                if (!_controls.ContainsKey(property.Name)) continue;

                var control = _controls[property.Name].Control;
                var value = property.GetValue(entity);

                if (control is TextBox textBox)
                {
                    textBox.Text = value?.ToString() ?? string.Empty;
                }
                else if (control is NumericUpDown numericUpDown)
                {
                    if (value != null)
                    {
                        numericUpDown.Value = Convert.ToDecimal(value);
                    }
                }
                else if (control is DateTimePicker dateTimePicker)
                {
                    if (value is DateTime dateTime)
                    {
                        dateTimePicker.Value = dateTime;
                    }
                }
                else if (control is CheckBox checkBox)
                {
                    checkBox.Checked = (bool)(value ?? false);
                }
                else if (control is ComboBox comboBox)
                {
                    if (value != null)
                    {
                        comboBox.SelectedItem = value;
                    }
                }
            }
        }

        public T GetEntityFromForm<T>() where T : class, new()
        {
            if (_controls == null) return null;

            var entity = new T();

            foreach (var property in _properties)
            {
                if (!_controls.ContainsKey(property.Name)) continue;

                var control = _controls[property.Name].Control;
                object value = null;

                if (control is TextBox textBox)
                {
                    value = textBox.Text;
                }
                else if (control is NumericUpDown numericUpDown)
                {
                    value = numericUpDown.Value;
                }
                else if (control is DateTimePicker dateTimePicker)
                {
                    value = dateTimePicker.Value;
                }
                else if (control is CheckBox checkBox)
                {
                    value = checkBox.Checked;
                }
                else if (control is ComboBox comboBox)
                {
                    value = comboBox.SelectedItem;
                }

                if (value != null)
                {
                    try
                    {
                        // Convert value to property type
                        var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                        var convertedValue = Convert.ChangeType(value, propertyType);
                        property.SetValue(entity, convertedValue);
                    }
                    catch
                    {
                        // Handle conversion errors
                        property.SetValue(entity, value);
                    }
                }
            }

            return entity;
        }

        public bool ValidateForm()
        {
            var isValid = true;
            var errorMessages = new List<string>();

            foreach (var controlInfo in _controls.Values)
            {
                var control = controlInfo.Control;
                var propertyName = controlInfo.Property.Name;

                // Reset validation highlighting
                if (control.BackColor != Color.White)
                {
                    control.BackColor = Color.White;
                }

                // Basic validation for required fields
                if (control is TextBox textBox && string.IsNullOrWhiteSpace(textBox.Text))
                {
                    var isRequired = !controlInfo.Property.PropertyType.IsGenericType ||
                                   controlInfo.Property.PropertyType.GetGenericTypeDefinition() != typeof(Nullable<>);

                    if (isRequired && !controlInfo.Property.Name.EndsWith("ID"))
                    {
                        errorMessages.Add($"{FormatPropertyName(propertyName)} is required.");
                        control.BackColor = Color.FromArgb(255, 235, 235);
                        isValid = false;
                    }
                }
            }

            if (!isValid)
            {
                MessageBox.Show(string.Join("\n", errorMessages), "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return isValid;
        }

        private string FormatPropertyName(string propertyName)
        {
            var result = "";
            for (int i = 0; i < propertyName.Length; i++)
            {
                if (i > 0 && char.IsUpper(propertyName[i]))
                {
                    result += " ";
                }
                result += propertyName[i];
            }
            return result;
        }
    }
}