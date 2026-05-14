using System;
using System.Drawing;
using System.Windows.Forms;

namespace MerchDesktopApp
{
    public class LoginForm : Form
    {
        private Label lblTitle;
        private Label lblLogin;
        private Label lblPassword;
        private TextBox txtLogin;
        private TextBox txtPassword;
        private Button btnLogin;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Авторизация";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(350, 230);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            lblTitle = new Label();
            lblTitle.Text = "Вход в систему";
            lblTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(95, 20);

            lblLogin = new Label();
            lblLogin.Text = "Логин:";
            lblLogin.Location = new Point(30, 70);
            lblLogin.AutoSize = true;

            txtLogin = new TextBox();
            txtLogin.Location = new Point(110, 66);
            txtLogin.Width = 180;

            lblPassword = new Label();
            lblPassword.Text = "Пароль:";
            lblPassword.Location = new Point(30, 110);
            lblPassword.AutoSize = true;

            txtPassword = new TextBox();
            txtPassword.Location = new Point(110, 106);
            txtPassword.Width = 180;
            txtPassword.PasswordChar = '*';

            btnLogin = new Button();
            btnLogin.Text = "Войти";
            btnLogin.Location = new Point(110, 145);
            btnLogin.Width = 120;
            btnLogin.Click += BtnLogin_Click;

            this.AcceptButton = btnLogin;

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblLogin);
            this.Controls.Add(txtLogin);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnLogin);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if (!Db.CheckDbFile())
                return;

            try
            {
                object roleObj = Db.Scalar(
                    "SELECT UserRole FROM AppUsers WHERE [Login]=? AND UserPassword=?",
                    Db.Param(txtLogin.Text.Trim()),
                    Db.Param(txtPassword.Text.Trim())
                );

                if (roleObj != null)
                {
                    string role = roleObj.ToString();
                    MainForm mainForm = new MainForm(txtLogin.Text.Trim(), role);
                    mainForm.FormClosed += (s, args) => this.Close();

                    this.Hide();
                    mainForm.Show();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль.", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка входа: " + ex.Message);
            }
        }
    }
}