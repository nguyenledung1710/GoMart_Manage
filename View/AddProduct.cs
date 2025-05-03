using GoMartApplication.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoMartApplication
{
    public partial class AddProduct : Form
    {
        public AddProduct()
        {
            InitializeComponent();
            lblProdID.Visible = false;
            btnUpdate.Visible = true;
            btnDelete.Visible = true;
            btnAdd.Enabled = true;
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            LoadCategories();
            BindProductList();
        }
        private void LoadCategories()
        {
            using (var svc = new CategoryService())
            {
                var cats = svc.GetAllCategories().ToList();

                cmbCategory.DataSource = cats;
                cmbCategory.DisplayMember = "CategoryName";
                cmbCategory.ValueMember = "CatID";
                cmbCategory.SelectedIndex = -1;

                cbbsearch.DataSource = cats.Select(c => new {
                    c.CatID,
                    c.CategoryName
                })
                                           .ToList();
                cbbsearch.DisplayMember = "CategoryName";
                cbbsearch.ValueMember = "CatID";
                cbbsearch.SelectedIndex = -1;
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
            ClearInputs();
        }

        private void AddProduct_Load(object sender, EventArgs e)
        {
            BindProductList();
            lblProdID.Visible = true;
            btnUpdate.Visible = true;
            btnDelete.Visible = true;
            btnAdd.Visible = true;
       
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
                BindProductList();
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
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            int cnt = dataGridView1.SelectedRows.Count;
            if (cnt == 1)
            {
                var row = dataGridView1.SelectedRows[0];
                lblProdID.Text = row.Cells["ProdID"].Value.ToString();
                txtProdName.Text = row.Cells["ProdName"].Value.ToString();
                cmbCategory.SelectedValue = (int)row.Cells["ProdCatID"].Value;
                txtPrice.Text = row.Cells["ProdPrice"].Value.ToString();
                txtQty.Text = row.Cells["ProdQty"].Value.ToString();

                btnAdd.Enabled = false;
                btnUpdate.Enabled = true;   
                btnDelete.Enabled = true;
                lblProdID.Visible = true;
            }
            else if (cnt > 1)
            {
                btnAdd.Enabled = false;
                btnUpdate.Enabled = false;    
                btnDelete.Enabled = true;    
                lblProdID.Visible = false;

                txtProdName.Clear();
                txtPrice.Clear();
                txtQty.Clear();
            }
            else
            {
                ClearInputs();
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

        private void cmbsearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
       

        private void button2_Click(object sender, EventArgs e)
        {
            if (cbbsearch.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn Category để tìm.", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int catId = (int)cbbsearch.SelectedValue;
            using (var svc = new ProductService())
            {
                dataGridView1.DataSource = svc.GetProductsByCategory(catId)
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
            ClearInputs();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadCategories();
            BindProductList();
        }
        private void ClearInputs()
        {
            txtProdName.Clear();
            txtPrice.Clear();
            txtQty.Clear();
            lblProdID.Text = string.Empty;

            lblProdID.Visible = false;

            btnAdd.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
