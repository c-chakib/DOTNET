using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace GestionBudgetWinForms
{
    public class RegisterForm : Form
    {
        private TextBox txtUsername;
        private TextBox txtPassword;
        private TextBox txtConfirmPassword;
        private Button btnRegister;
        private Button btnCancel;

        private BudgetRepository repository;

        public RegisterForm(BudgetRepository repo)
        {
            repository = repo;
            CreateRegisterUI();
        }

        private void CreateRegisterUI()
        {
            // Form settings
            this.Text = "Register";
            this.Width = 350;
            this.Height = 320;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(240, 240, 240);

            // Username
            Label lblUsername = new Label
            {
                Text = "Username",
                Left = 30,
                Top = 30,
                AutoSize = true
            };

            txtUsername = new TextBox
            {
                Left = 30,
                Top = 50,
                Width = 250
            };

            // Password
            Label lblPassword = new Label
            {
                Text = "Password",
                Left = 30,
                Top = 85,
                AutoSize = true
            };

            txtPassword = new TextBox
            {
                Left = 30,
                Top = 105,
                Width = 250,
                PasswordChar = '*'
            };

            // Confirm Password
            Label lblConfirm = new Label
            {
                Text = "Confirm Password",
                Left = 30,
                Top = 140,
                AutoSize = true
            };

            txtConfirmPassword = new TextBox
            {
                Left = 30,
                Top = 160,
                Width = 250,
                PasswordChar = '*'
            };

            // Register button
            btnRegister = new Button
            {
                Text = "Register",
                Left = 30,
                Top = 200,
                Width = 250,
                BackColor = Color.FromArgb(60, 180, 75),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Click += BtnRegister_Click;

            // Cancel button
            btnCancel = new Button
            {
                Text = "Cancel",
                Left = 30,
                Top = 240,
                Width = 250,
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.Close();

            // Set Enter and Escape keys
            this.AcceptButton = btnRegister;
            this.CancelButton = btnCancel;

            // Add controls
            this.Controls.Add(lblUsername);
            this.Controls.Add(txtUsername);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(lblConfirm);
            this.Controls.Add(txtConfirmPassword);
            this.Controls.Add(btnRegister);
            this.Controls.Add(btnCancel);
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            // Validate inputs
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Please fill all fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (username.Length < 3)
            {
                MessageBox.Show("Username must be at least 3 characters.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (repository.UserExists(username))
            {
                MessageBox.Show("Username already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Hash password
            string hashedPassword = HashPassword(password);

            // Add user
            repository.AjouterUser(new User
            {
                Username = username,
                PasswordHash = password
            });

            MessageBox.Show("Registration successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        // SHA256 hashing function
        private string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }
    }
}
