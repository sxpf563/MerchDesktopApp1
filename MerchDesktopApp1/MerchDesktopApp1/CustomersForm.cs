using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace MerchDesktopApp
{
    public class CustomersForm : Form
    {
        private DataGridView dgvCustomers;
        private TextBox txtFullName;
        private TextBox txtPhone;
        private TextBox txtEmail;
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnRefresh;

        private int selectedId = 0;

        public CustomersForm()
        {
            InitializeComponent();
            LoadCustomers();
        }

        private void InitializeComponent()
        {
            this.Text = "Клиенты";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(850, 500);

            dgvCustomers = new DataGridView();
            dgvCustomers.Location = new Point(20, 20);
            dgvCustomers.Size = new Size(790, 220);
            dgvCustomers.ReadOnly = true;
            dgvCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCustomers.MultiSelect = false;
            dgvCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCustomers.CellClick += DgvCustomers_CellClick;

            Label lblFullName = new Label();
            lblFullName.Text = "ФИО:";
            lblFullName.Location = new Point(20, 280);
            lblFullName.AutoSize = true;

            txtFullName = new TextBox();
            txtFullName.Location = new Point(120, 277);
            txtFullName.Width = 250;

            Label lblPhone = new Label();
            lblPhone.Text = "Телефон:";
            lblPhone.Location = new Point(20, 320);
            lblPhone.AutoSize = true;

            txtPhone = new TextBox();
            txtPhone.Location = new Point(120, 317);
            txtPhone.Width = 250;

            Label lblEmail = new Label();
            lblEmail.Text = "Email:";
            lblEmail.Location = new Point(20, 360);
            lblEmail.AutoSize = true;

            txtEmail = new TextBox();
            txtEmail.Location = new Point(120, 357);
            txtEmail.Width = 250;

            btnAdd = new Button();
            btnAdd.Text = "Добавить";
            btnAdd.Location = new Point(450, 275);
            btnAdd.Size = new Size(150, 40);
            btnAdd.Click += BtnAdd_Click;

            btnUpdate = new Button();
            btnUpdate.Text = "Изменить";
            btnUpdate.Location = new Point(450, 325);
            btnUpdate.Size = new Size(150, 40);
            btnUpdate.Click += BtnUpdate_Click;

            btnDelete = new Button();
            btnDelete.Text = "Удалить";
            btnDelete.Location = new Point(450, 375);
            btnDelete.Size = new Size(150, 40);
            btnDelete.Click += BtnDelete_Click;

            btnRefresh = new Button();
            btnRefresh.Text = "Обновить";
            btnRefresh.Location = new Point(620, 275);
            btnRefresh.Size = new Size(150, 40);
            btnRefresh.Click += (s, e) => LoadCustomers();

            this.Controls.Add(dgvCustomers);

            this.Controls.Add(lblFullName);
            this.Controls.Add(txtFullName);
            this.Controls.Add(lblPhone);
            this.Controls.Add(txtPhone);
            this.Controls.Add(lblEmail);
            this.Controls.Add(txtEmail);

            this.Controls.Add(btnAdd);
            this.Controls.Add(btnUpdate);
            this.Controls.Add(btnDelete);
            this.Controls.Add(btnRefresh);
        }

        private void LoadCustomers()
        {
            try
            {
                DataTable dt = Db.GetTable("SELECT CustomerID, FullName, Phone, Email FROM Customers ORDER BY CustomerID");
                dgvCustomers.DataSource = dt;

                dgvCustomers.Columns["CustomerID"].HeaderText = "ID";
                dgvCustomers.Columns["FullName"].HeaderText = "ФИО";
                dgvCustomers.Columns["Phone"].HeaderText = "Телефон";
                dgvCustomers.Columns["Email"].HeaderText = "Email";

                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки клиентов: " + ex.Message);
            }
        }

        private void DgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCustomers.CurrentRow == null) return;

            selectedId = Convert.ToInt32(dgvCustomers.CurrentRow.Cells["CustomerID"].Value);
            txtFullName.Text = dgvCustomers.CurrentRow.Cells["FullName"].Value.ToString();
            txtPhone.Text = dgvCustomers.CurrentRow.Cells["Phone"].Value.ToString();
            txtEmail.Text = dgvCustomers.CurrentRow.Cells["Email"].Value.ToString();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Введите ФИО клиента.");
                return;
            }

            try
            {
                Db.Execute(
                    "INSERT INTO Customers (FullName, Phone, Email) VALUES (?, ?, ?)",
                    Db.Param(txtFullName.Text.Trim()),
                    Db.Param(txtPhone.Text.Trim()),
                    Db.Param(txtEmail.Text.Trim())
                );

                MessageBox.Show("Клиент добавлен.");
                LoadCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка добавления клиента: " + ex.Message);
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedId == 0)
            {
                MessageBox.Show("Выберите клиента.");
                return;
            }

            try
            {
                Db.Execute(
                    "UPDATE Customers SET FullName=?, Phone=?, Email=? WHERE CustomerID=?",
                    Db.Param(txtFullName.Text.Trim()),
                    Db.Param(txtPhone.Text.Trim()),
                    Db.Param(txtEmail.Text.Trim()),
                    Db.Param(selectedId)
                );

                MessageBox.Show("Клиент изменен.");
                LoadCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка изменения клиента: " + ex.Message);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (selectedId == 0)
            {
                MessageBox.Show("Выберите клиента.");
                return;
            }

            if (MessageBox.Show("Удалить выбранного клиента?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    Db.Execute("DELETE FROM Customers WHERE CustomerID=?", Db.Param(selectedId));
                    MessageBox.Show("Клиент удален.");
                    LoadCustomers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка удаления клиента: " + ex.Message);
                }
            }
        }

        private void ClearFields()
        {
            selectedId = 0;
            txtFullName.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
        }
    }
}