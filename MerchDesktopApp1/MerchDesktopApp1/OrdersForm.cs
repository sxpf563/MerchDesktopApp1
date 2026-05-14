using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace MerchDesktopApp
{
    public class OrdersForm : Form
    {
        private DataGridView dgvOrders;
        private ComboBox cmbStatus;
        private Button btnUpdateStatus;
        private Button btnRefresh;

        private int selectedOrderId = 0;

        public OrdersForm()
        {
            InitializeComponent();
            LoadOrders();
        }

        private void InitializeComponent()
        {
            this.Text = "Заказы";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(900, 500);

            dgvOrders = new DataGridView();
            dgvOrders.Location = new Point(20, 20);
            dgvOrders.Size = new Size(840, 260);
            dgvOrders.ReadOnly = true;
            dgvOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvOrders.MultiSelect = false;
            dgvOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvOrders.CellClick += DgvOrders_CellClick;

            Label lblStatus = new Label();
            lblStatus.Text = "Статус заказа:";
            lblStatus.Location = new Point(20, 320);
            lblStatus.AutoSize = true;

            cmbStatus = new ComboBox();
            cmbStatus.Location = new Point(130, 317);
            cmbStatus.Width = 200;
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.Items.AddRange(new string[]
            {
                "Новый",
                "Подтвержден",
                "В сборке",
                "Отправлен",
                "Доставлен",
                "Отменен"
            });

            btnUpdateStatus = new Button();
            btnUpdateStatus.Text = "Изменить статус";
            btnUpdateStatus.Location = new Point(360, 312);
            btnUpdateStatus.Size = new Size(160, 35);
            btnUpdateStatus.Click += BtnUpdateStatus_Click;

            btnRefresh = new Button();
            btnRefresh.Text = "Обновить";
            btnRefresh.Location = new Point(540, 312);
            btnRefresh.Size = new Size(120, 35);
            btnRefresh.Click += (s, e) => LoadOrders();

            this.Controls.Add(dgvOrders);
            this.Controls.Add(lblStatus);
            this.Controls.Add(cmbStatus);
            this.Controls.Add(btnUpdateStatus);
            this.Controls.Add(btnRefresh);
        }

        private void LoadOrders()
        {
            try
            {
                DataTable dt = Db.GetTable(
                    "SELECT CustomerOrders.OrderID, Customers.FullName, CustomerOrders.OrderDate, CustomerOrders.OrderStatus, CustomerOrders.TotalAmount " +
                    "FROM Customers INNER JOIN CustomerOrders ON Customers.CustomerID = CustomerOrders.CustomerID " +
                    "ORDER BY CustomerOrders.OrderID DESC"
                );

                dgvOrders.DataSource = dt;

                dgvOrders.Columns["OrderID"].HeaderText = "ID заказа";
                dgvOrders.Columns["FullName"].HeaderText = "Клиент";
                dgvOrders.Columns["OrderDate"].HeaderText = "Дата";
                dgvOrders.Columns["OrderStatus"].HeaderText = "Статус";
                dgvOrders.Columns["TotalAmount"].HeaderText = "Сумма";

                selectedOrderId = 0;
                cmbStatus.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки заказов: " + ex.Message);
            }
        }

        private void DgvOrders_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvOrders.CurrentRow == null) return;

            selectedOrderId = Convert.ToInt32(dgvOrders.CurrentRow.Cells["OrderID"].Value);
            cmbStatus.Text = dgvOrders.CurrentRow.Cells["OrderStatus"].Value.ToString();
        }

        private void BtnUpdateStatus_Click(object sender, EventArgs e)
        {
            if (selectedOrderId == 0)
            {
                MessageBox.Show("Выберите заказ.");
                return;
            }

            if (cmbStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите статус.");
                return;
            }

            try
            {
                Db.Execute(
                    "UPDATE CustomerOrders SET OrderStatus=? WHERE OrderID=?",
                    Db.Param(cmbStatus.Text),
                    Db.Param(selectedOrderId)
                );

                MessageBox.Show("Статус заказа изменен.");
                LoadOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка изменения статуса: " + ex.Message);
            }
        }
    }
}