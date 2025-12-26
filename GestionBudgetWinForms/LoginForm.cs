using System;
using System.Drawing;
using System.Windows.Forms;

namespace GestionBudgetWinForms
{
    public class LoginForm : Form
    {
        Label lblTitle;
        Label lblUsername;
        TextBox txtUsername;
        Label lblPassword;
        TextBox txtPassword;
        Button btnLogin;
        Button btnCancel;
        Button btnRegister;
        private BudgetRepository repository;

        public LoginForm()
        {
            CreateLoginInterface();
            // Initialize repository with the connection string from app.config
            repository = new BudgetRepository(
                System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString
            );

            // Optional: initialize database and tables if not done
            repository.InitialiserBaseDeDonnees();

            repository.CreateDefaultUser();
        }


        private void CreateLoginInterface()
        {
            // Form settings
            this.Text = "Login";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(240, 240, 240); // light gray background

            // Title label
            lblTitle = new Label
            {
                Text = "Welcome!",
                Font = new Font("Arial", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 50),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 60
            };

            // Username label
            lblUsername = new Label
            {
                Text = "Username:",
                Location = new Point(50, 80),
                AutoSize = true,
                Font = new Font("Arial", 10, FontStyle.Regular)
            };

            // Username textbox
            txtUsername = new TextBox
            {
                Location = new Point(150, 75),
                Width = 180,
                Font = new Font("Arial", 10)
            };

            // Password label
            lblPassword = new Label
            {
                Text = "Password:",
                Location = new Point(50, 130),
                AutoSize = true,
                Font = new Font("Arial", 10, FontStyle.Regular)
            };

            // Password textbox
            txtPassword = new TextBox
            {
                Location = new Point(150, 125),
                Width = 180,
                Font = new Font("Arial", 10),
                PasswordChar = '*'
            };

            // Login button
            btnLogin = new Button
            {
                Text = "Login",
                Location = new Point(150, 180),
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Width = 80,
                Height = 30
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;
            // Register button
            btnRegister = new Button
            {
                Text = "Register",
                Location = new Point(150, 220), // below Login
                BackColor = Color.FromArgb(60, 180, 75), // green
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Width = 180,
                Height = 30
            };
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Click += BtnRegister_Click;


            // Cancel button
            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(250, 180),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Width = 80,
                Height = 30
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.Close();

            // Add controls
            this.Controls.AddRange(new Control[]
            {
                lblTitle,
                lblUsername, txtUsername,
                lblPassword, txtPassword,
                btnLogin, btnCancel,
                btnRegister
            });

        }
        private void BtnRegister_Click(object sender, EventArgs e)
        {
            RegisterForm registerForm = new RegisterForm(repository);
            registerForm.ShowDialog();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            // Use the repository login function
            if (repository.Login(username, password, out User user))
            {
                Session.CurrentUser = user;
                MessageBox.Show($"Welcome {user.Username}!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Open MainForm
                MainForm mainForm = new MainForm();
                mainForm.Show();

                // Close LoginForm
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid username or password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtUsername.Focus();
            }
        }


    }
}
