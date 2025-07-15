using System;
using System.Drawing;
using System.Windows.Forms;

namespace BusinessManagementSystem.Forms
{
    public partial class BaseForm : Form
    {
        protected Color PrimaryColor = Color.FromArgb(41, 128, 185);
        protected Color SecondaryColor = Color.FromArgb(52, 152, 219);
        protected Color AccentColor = Color.FromArgb(46, 204, 113);
        protected Color DangerColor = Color.FromArgb(231, 76, 60);
        protected Color WarningColor = Color.FromArgb(241, 196, 15);
        protected Color BackgroundColor = Color.FromArgb(236, 240, 241);
        protected Color CardColor = Color.White;
        protected Color TextColor = Color.FromArgb(44, 62, 80);

        public BaseForm()
        {
            InitializeBaseForm();
            ApplyModernStyling();
        }

        private void InitializeBaseForm()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = BackgroundColor;
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.ForeColor = TextColor;
        }

        private void ApplyModernStyling()
        {
            // Add subtle shadow effect
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        protected Button CreateStyledButton(string text, Color backgroundColor, Color textColor, int width = 120, int height = 35)
        {
            var button = new Button
            {
                Text = text,
                Size = new Size(width, height),
                FlatStyle = FlatStyle.Flat,
                BackColor = backgroundColor,
                ForeColor = textColor,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };

            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = ControlPaint.Dark(backgroundColor, 0.1f);
            button.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(backgroundColor, 0.2f);

            // Add hover effects
            button.MouseEnter += (s, e) => button.BackColor = ControlPaint.Dark(backgroundColor, 0.1f);
            button.MouseLeave += (s, e) => button.BackColor = backgroundColor;

            return button;
        }

        protected TextBox CreateStyledTextBox(int width = 200, int height = 30)
        {
            var textBox = new TextBox
            {
                Size = new Size(width, height),
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                BackColor = Color.White,
                ForeColor = TextColor
            };

            // Add focus effects
            textBox.GotFocus += (s, e) => textBox.BackColor = Color.FromArgb(245, 245, 245);
            textBox.LostFocus += (s, e) => textBox.BackColor = Color.White;

            return textBox;
        }

        protected Label CreateStyledLabel(string text, Font font = null, Color? color = null)
        {
            var label = new Label
            {
                Text = text,
                AutoSize = true,
                Font = font ?? new Font("Segoe UI", 9F, FontStyle.Regular),
                ForeColor = color ?? TextColor,
                BackColor = Color.Transparent
            };

            return label;
        }

        protected ComboBox CreateStyledComboBox(int width = 200, int height = 30)
        {
            var comboBox = new ComboBox
            {
                Size = new Size(width, height),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                BackColor = Color.White,
                ForeColor = TextColor
            };

            return comboBox;
        }

        protected Panel CreateCardPanel(int width = 400, int height = 300)
        {
            var panel = new Panel
            {
                Size = new Size(width, height),
                BackColor = CardColor,
                BorderStyle = BorderStyle.FixedSingle
            };

            return panel;
        }

        protected DataGridView CreateStyledDataGridView()
        {
            var dataGridView = new DataGridView
            {
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = PrimaryColor,
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    SelectionBackColor = PrimaryColor,
                    SelectionForeColor = Color.White
                },
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.White,
                    ForeColor = TextColor,
                    Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                    SelectionBackColor = Color.FromArgb(200, 200, 200),
                    SelectionForeColor = TextColor
                },
                EnableHeadersVisualStyles = false,
                GridColor = Color.FromArgb(230, 230, 230),
                ReadOnly = true,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            return dataGridView;
        }

        protected void ShowSuccessMessage(string message)
        {
            MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        protected void ShowWarningMessage(string message)
        {
            MessageBox.Show(message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        protected bool ShowConfirmation(string message)
        {
            var result = MessageBox.Show(message, "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }
    }
}