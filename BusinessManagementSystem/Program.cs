using System;
using System.Windows.Forms;
using BusinessManagementSystem.Forms;

namespace BusinessManagementSystem
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Enable visual styles and DPI awareness for better UI rendering
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                // Show login form
                using (var loginForm = new LoginForm())
                {
                    var loginResult = loginForm.ShowDialog();
                    
                    if (loginResult == DialogResult.OK && loginForm.LoggedInUser != null)
                    {
                        // Login successful, show main application
                        var mainForm = new MainForm(loginForm.LoggedInUser);
                        Application.Run(mainForm);
                    }
                    // If login was cancelled or failed, application will exit
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Application error: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}