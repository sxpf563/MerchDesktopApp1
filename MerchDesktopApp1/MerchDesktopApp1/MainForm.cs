using System.Drawing;
using System.Windows.Forms;

namespace MerchDesktopApp
{
    public class MainForm : Form
    {
        private Label lblInfo;
        private Button btnProducts;
        private Button btnOrders;
        private Button btnCustomers;
        private Button btnExit;

        public MainForm(string login, string role)
        {
            InitializeComponent(login, role);
        }

        private void InitializeComponent(string login, string role)
        {
            this.Text = "Главное меню";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(420, 300);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            lblInfo = new Label();
            lblInfo.Text = "Пользователь: " + login + " | Роль: " + role;
            lblInfo.Location = new Point(20, 20);
            lblInfo.AutoSize = true;
            lblInfo.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            btnProducts = new Button();
            btnProducts.Text = "Товары";
            btnProducts.Size = new Size(150, 40);
            btnProducts.Location = new Point(30, 70);
            btnProducts.Click += (s, e) => { new ProductsForm().ShowDialog(); };

            btnOrders = new Button();
            btnOrders.Text = "Заказы";
            btnOrders.Size = new Size(150, 40);
            btnOrders.Location = new Point(210, 70);
            btnOrders.Click += (s, e) => { new OrdersForm().ShowDialog(); };

            btnCustomers = new Button();
            btnCustomers.Text = "Клиенты";
            btnCustomers.Size = new Size(150, 40);
            btnCustomers.Location = new Point(30, 130);
            btnCustomers.Click += (s, e) => { new CustomersForm().ShowDialog(); };

            btnExit = new Button();
            btnExit.Text = "Выход";
            btnExit.Size = new Size(150, 40);
            btnExit.Location = new Point(210, 130);
            btnExit.Click += (s, e) => { this.Close(); };

            this.Controls.Add(lblInfo);
            this.Controls.Add(btnProducts);
            this.Controls.Add(btnOrders);
            this.Controls.Add(btnCustomers);
            this.Controls.Add(btnExit);
        }
    }
}