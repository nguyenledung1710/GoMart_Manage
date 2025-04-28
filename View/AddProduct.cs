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
        //DBConnect dbCon = new DBConnect();
      
        public AddProduct()
        {
            InitializeComponent();
            lblProdID.Visible = false;
            btnUpdate.Visible = true;
            btnDelete.Visible = true;
            btnAdd.Enabled = true;

            LoadCategories(); // fill cbbsearch
            BindProductList();
        }
        private void LoadCategories()
        {
            using (var svc = new CategoryService())
            {
                var cats = svc.GetAllCategories().ToList();

                // → ComboBox dùng để thêm sản phẩm
                cmbCategory.DataSource = cats;
                cmbCategory.DisplayMember = "CategoryName";
                cmbCategory.ValueMember = "CatID";
                cmbCategory.SelectedIndex = -1;

                // → ComboBox dùng để search
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
            // Chỗ này format cột ProdPrice
            var col = dataGridView1.Columns["ProdPrice"];
            if (col != null)
            {
                // “0” = không có thập phân; nếu muốn có dấu ngăn hàng nghìn thì “N0”
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
            // Validate phải chọn Category ở cmbCategory
            if (cmbCategory.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn Category để thêm sản phẩm.", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var name = txtProdName.Text.Trim();
                var catId = (int)cmbCategory.SelectedValue;   // ← DÙNG cmbCategory
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

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;
            var row = dataGridView1.SelectedRows[0];

            btnAdd.Enabled = false;
            btnUpdate.Visible = true;
            btnDelete.Visible = true;
            lblProdID.Visible = true;

            lblProdID.Text = row.Cells["ProdID"].Value.ToString();
            txtProdName.Text = row.Cells["ProdName"].Value.ToString();
            cmbCategory.SelectedValue = (int)row.Cells["ProdCatID"].Value; // sửa lại dùng cmbCategory
            txtPrice.Text = row.Cells["ProdPrice"].Value.ToString();
            txtQty.Text = row.Cells["ProdQty"].Value.ToString();
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
        {// Search: sử dụng cbbsearch
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

            btnAdd.Enabled = true;
            btnUpdate.Visible = true;
            btnDelete.Visible = true;
            lblProdID.Visible = false;
            cbbsearch.SelectedIndex = -1;
        }
    }
}
