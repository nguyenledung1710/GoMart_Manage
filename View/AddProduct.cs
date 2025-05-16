using GoMartApplication.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace GoMartApplication
{
    public partial class AddProduct : Form
    {
        public AddProduct()
        {
            InitializeComponent();
            this.Size = Program.DefaultFormSize;
            this.MinimumSize = this.MaximumSize = this.Size;
            this.StartPosition = FormStartPosition.CenterScreen;

            dataGridView1.CellClick += dataGridView1_CellClick;

            LoadCategories();
            BindProductList();
        }
        private void LoadCategories()
        {
            btnUpdate.Visible = true;
            btnDelete.Visible = true;
            btnAdd.Enabled = true;

            using (var svc = new CategoryService())
            {
                var cats = svc.GetAllCategories().ToList();
                var searchItems = new List<dynamic>();
                searchItems.Add(new { CatID = 0, CategoryName = "All" });
                searchItems.AddRange(cats.Select(c => new {
                    c.CatID,
                    c.CategoryName
                }));
                cbbsearch.DataSource = searchItems;
                cbbsearch.DisplayMember = "CategoryName";
                cbbsearch.ValueMember = "CatID";
                cbbsearch.SelectedValue = 0;  

                cmbCategory.DataSource = cats;
                cmbCategory.DisplayMember = "CategoryName";
                cmbCategory.ValueMember = "CatID";
                cmbCategory.SelectedIndex = -1;
            }
        }
        private void BindProductList()
        {
            using (var svc = new ProductService())
            {
                dataGridView1.DataSource = svc.GetAllProducts()
            .Select(p => new {
                p.ProdID,
                p.ProdName,
                p.ProdCatID,
                Category = p.Category.CategoryName,
                p.ProdPrice,
                p.ProdQty
            })
            .ToList();
            }
            var col = dataGridView1.Columns["ProdPrice"];
            if (col != null)
            {
                col.DefaultCellStyle.Format = "0";
            }
        }

        private void AddProduct_Load(object sender, EventArgs e)
        {
            BindProductList();
            lblProdID.Visible = true;
            btnUpdate.Visible = true;
            btnDelete.Visible = true;
            btnAdd.Visible = true;
       
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dataGridView1.Rows[e.RowIndex];
            PopulateFormFromRow(row);
        }
        private void PopulateFormFromRow(DataGridViewRow row)
        {
            lblProdID.Text = row.Cells["ProdID"].Value?.ToString();
            txtProdName.Text = row.Cells["ProdName"].Value?.ToString();
            cmbCategory.SelectedValue = (int)row.Cells["ProdCatID"].Value;

            if (decimal.TryParse(row.Cells["ProdPrice"].Value?.ToString(), out var priceValue))
                txtPrice.Text = priceValue.ToString("0");
            else
                txtPrice.Text = row.Cells["ProdPrice"].Value?.ToString();

            txtQty.Text = row.Cells["ProdQty"].Value?.ToString();

            btnAdd.Enabled = false;
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;
            lblProdID.Visible = true;
        }

        private void LoadProductsByCategory(int catId)
        {
            using (var svc = new ProductService())
            {
                var list = svc.GetAllByCategory(catId)
                    .Select(p => new {
                        p.ProdID,
                        p.ProdName,
                        p.ProdCatID,
                        Category = p.Category?.CategoryName ?? "[Chưa có danh mục]",
                        p.ProdPrice,
                        p.ProdQty
                    })
                    .ToList();

                dataGridView1.DataSource = list;
            }
            var col = dataGridView1.Columns["ProdPrice"];
            if (col != null)
                col.DefaultCellStyle.Format = "0";
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cmbCategory.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn Category để thêm sản phẩm.", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var name = txtProdName.Text.Trim();
                var catId = (int)cmbCategory.SelectedValue;   
                var price = decimal.Parse(txtPrice.Text.Trim());
                var qty = int.Parse(txtQty.Text.Trim());

                using (var svc = new ProductService())
                    svc.CreateProduct(name, catId, price, qty);

                MessageBox.Show("Thêm sản phẩm thành công.", "Thành công",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadProductsByCategory(catId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(lblProdID.Text, out var id)) return;
            try
            {
                var name = txtProdName.Text.Trim();
                var catId = (int)cmbCategory.SelectedValue;
                var price = decimal.Parse(txtPrice.Text.Trim());
                var qty = int.Parse(txtQty.Text.Trim());

                using (ProductService svc = new ProductService())
                {
                    if (svc.UpdateProduct(id, name, catId, price, qty))
                    {
                        MessageBox.Show("Cập nhật sản phẩm thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        BindProductList();
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(lblProdID.Text, out var id))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show($"Bạn có chắc chắn xóa sản phẩm {id}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            using (var svc = new ProductService())
            {
                if (svc.DeleteProduct(id))
                {
                    MessageBox.Show("Xóa sản phẩm thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    BindProductList();
                }
                else
                {
                    MessageBox.Show("Xóa thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void ClearInputs()
        {
            txtProdName.Clear();
            txtPrice.Clear();
            txtQty.Clear();
            lblProdID.Text = string.Empty;

            lblProdID.Visible = false;

            btnAdd.Enabled = true;
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;
        }


        private void btnRefresh_Click(object sender, EventArgs e)
        {

            ClearInputs();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (cbbsearch.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn Category để tìm.", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int catId = (int)cbbsearch.SelectedValue;
            if (catId == 0)
            {
                BindProductList();
            }
            else
            {
                LoadProductsByCategory(catId);
            }

            ClearInputs();
        }
    }
}
