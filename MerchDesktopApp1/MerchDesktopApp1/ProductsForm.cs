using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace MerchDesktopApp
{
    public class ProductsForm : Form
    {
        private DataGridView dgvProducts;
        private TextBox txtName;
        private TextBox txtCategory;
        private TextBox txtPrice;
        private TextBox txtQuantity;
        private TextBox txtDescription;
        private TextBox txtSearch;
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnRefresh;
        private Button btnSearch;

        private int selectedId = 0;

        public ProductsForm()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void InitializeComponent()
        {
            this.Text = "Товары";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(950, 560);

            dgvProducts = new DataGridView();
            dgvProducts.Location = new Point(20, 20);
            dgvProducts.Size = new Size(890, 220);
            dgvProducts.ReadOnly = true;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.MultiSelect = false;
            dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProducts.CellClick += DgvProducts_CellClick;

            Label lblSearch = new Label();
            lblSearch.Text = "Поиск:";
            lblSearch.Location = new Point(20, 255);
            lblSearch.AutoSize = true;

            txtSearch = new TextBox();
            txtSearch.Location = new Point(80, 252);
            txtSearch.Width = 200;

            btnSearch = new Button();
            btnSearch.Text = "Найти";
            btnSearch.Location = new Point(300, 250);
            btnSearch.Click += BtnSearch_Click;

            Label lblName = new Label();
            lblName.Text = "Название:";
            lblName.Location = new Point(20, 300);
            lblName.AutoSize = true;

            txtName = new TextBox();
            txtName.Location = new Point(120, 297);
            txtName.Width = 250;

            Label lblCategory = new Label();
            lblCategory.Text = "Категория:";
            lblCategory.Location = new Point(20, 335);
            lblCategory.AutoSize = true;

            txtCategory = new TextBox();
            txtCategory.Location = new Point(120, 332);
            txtCategory.Width = 250;

            Label lblPrice = new Label();
            lblPrice.Text = "Цена:";
            lblPrice.Location = new Point(20, 370);
            lblPrice.AutoSize = true;

            txtPrice = new TextBox();
            txtPrice.Location = new Point(120, 367);
            txtPrice.Width = 250;

            Label lblQuantity = new Label();
            lblQuantity.Text = "Количество:";
            lblQuantity.Location = new Point(20, 405);
            lblQuantity.AutoSize = true;

            txtQuantity = new TextBox();
            txtQuantity.Location = new Point(120, 402);
            txtQuantity.Width = 250;

            Label lblDescription = new Label();
            lblDescription.Text = "Описание:";
            lblDescription.Location = new Point(20, 440);
            lblDescription.AutoSize = true;

            txtDescription = new TextBox();
            txtDescription.Location = new Point(120, 437);
            txtDescription.Width = 250;

            btnAdd = new Button();
            btnAdd.Text = "Добавить";
            btnAdd.Location = new Point(450, 300);
            btnAdd.Size = new Size(150, 40);
            btnAdd.Click += BtnAdd_Click;

            btnUpdate = new Button();
            btnUpdate.Text = "Изменить";
            btnUpdate.Location = new Point(450, 355);
            btnUpdate.Size = new Size(150, 40);
            btnUpdate.Click += BtnUpdate_Click;

            btnDelete = new Button();
            btnDelete.Text = "Удалить";
            btnDelete.Location = new Point(450, 410);
            btnDelete.Size = new Size(150, 40);
            btnDelete.Click += BtnDelete_Click;

            btnRefresh = new Button();
            btnRefresh.Text = "Обновить";
            btnRefresh.Location = new Point(450, 465);
            btnRefresh.Size = new Size(150, 40);
            btnRefresh.Click += (s, e) => LoadProducts();

            this.Controls.Add(dgvProducts);
            this.Controls.Add(lblSearch);
            this.Controls.Add(txtSearch);
            this.Controls.Add(btnSearch);

            this.Controls.Add(lblName);
            this.Controls.Add(txtName);
            this.Controls.Add(lblCategory);
            this.Controls.Add(txtCategory);
            this.Controls.Add(lblPrice);
            this.Controls.Add(txtPrice);
            this.Controls.Add(lblQuantity);
            this.Controls.Add(txtQuantity);
            this.Controls.Add(lblDescription);
            this.Controls.Add(txtDescription);

            this.Controls.Add(btnAdd);
            this.Controls.Add(btnUpdate);
            this.Controls.Add(btnDelete);
            this.Controls.Add(btnRefresh);
        }

        private void LoadProducts()
        {
            try
            {
                DataTable dt = Db.GetTable("SELECT ProductID, ProductName, CategoryName, Price, Quantity, DescriptionText FROM Products ORDER BY ProductID");
                dgvProducts.DataSource = dt;

                dgvProducts.Columns["ProductID"].HeaderText = "ID";
                dgvProducts.Columns["ProductName"].HeaderText = "Название";
                dgvProducts.Columns["CategoryName"].HeaderText = "Категория";
                dgvProducts.Columns["Price"].HeaderText = "Цена";
                dgvProducts.Columns["Quantity"].HeaderText = "Количество";
                dgvProducts.Columns["DescriptionText"].HeaderText = "Описание";

                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки товаров: " + ex.Message);
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string searchText = txtSearch.Text.Trim();

                if (string.IsNullOrWhiteSpace(searchText))
                {
                    LoadProducts();
                    return;
                }

                DataTable allProducts = Db.GetTable(
                    "SELECT ProductID, ProductName, CategoryName, Price, Quantity, DescriptionText FROM Products ORDER BY ProductID"
                );

                DataTable result = allProducts.Clone();

                foreach (DataRow row in allProducts.Rows)
                {
                    string name = row["ProductName"].ToString();
                    string category = row["CategoryName"].ToString();

                    if (name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        category.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        result.ImportRow(row);
                    }
                }

                dgvProducts.DataSource = result;

                if (result.Rows.Count == 0)
                {
                    MessageBox.Show("Товары не найдены.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка поиска: " + ex.Message);
            }
        }

        private void DgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvProducts.CurrentRow == null) return;

            selectedId = Convert.ToInt32(dgvProducts.CurrentRow.Cells["ProductID"].Value);
            txtName.Text = dgvProducts.CurrentRow.Cells["ProductName"].Value.ToString();
            txtCategory.Text = dgvProducts.CurrentRow.Cells["CategoryName"].Value.ToString();
            txtPrice.Text = dgvProducts.CurrentRow.Cells["Price"].Value.ToString();
            txtQuantity.Text = dgvProducts.CurrentRow.Cells["Quantity"].Value.ToString();
            txtDescription.Text = dgvProducts.CurrentRow.Cells["DescriptionText"].Value.ToString();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            decimal price;
            int quantity;

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название товара.");
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out price))
            {
                MessageBox.Show("Введите корректную цену.");
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out quantity))
            {
                MessageBox.Show("Введите корректное количество.");
                return;
            }

            try
            {
                Db.Execute(
                    "INSERT INTO Products (ProductName, CategoryName, Price, Quantity, DescriptionText) VALUES (?, ?, ?, ?, ?)",
                    Db.Param(txtName.Text.Trim()),
                    Db.Param(txtCategory.Text.Trim()),
                    Db.Param(price),
                    Db.Param(quantity),
                    Db.Param(txtDescription.Text.Trim())
                );

                MessageBox.Show("Товар добавлен.");
                LoadProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка добавления: " + ex.Message);
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedId == 0)
            {
                MessageBox.Show("Выберите товар.");
                return;
            }

            decimal price;
            int quantity;

            if (!decimal.TryParse(txtPrice.Text, out price))
            {
                MessageBox.Show("Введите корректную цену.");
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out quantity))
            {
                MessageBox.Show("Введите корректное количество.");
                return;
            }

            try
            {
                Db.Execute(
                    "UPDATE Products SET ProductName=?, CategoryName=?, Price=?, Quantity=?, DescriptionText=? WHERE ProductID=?",
                    Db.Param(txtName.Text.Trim()),
                    Db.Param(txtCategory.Text.Trim()),
                    Db.Param(price),
                    Db.Param(quantity),
                    Db.Param(txtDescription.Text.Trim()),
                    Db.Param(selectedId)
                );

                MessageBox.Show("Товар изменен.");
                LoadProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка изменения: " + ex.Message);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (selectedId == 0)
            {
                MessageBox.Show("Выберите товар.");
                return;
            }

            if (MessageBox.Show("Удалить выбранный товар?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    Db.Execute("DELETE FROM Products WHERE ProductID=?", Db.Param(selectedId));
                    MessageBox.Show("Товар удален.");
                    LoadProducts();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка удаления: " + ex.Message);
                }
            }
        }

        private void ClearFields()
        {
            selectedId = 0;
            txtName.Clear();
            txtCategory.Clear();
            txtPrice.Clear();
            txtQuantity.Clear();
            txtDescription.Clear();
        }
    }
}