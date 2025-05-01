using GoMartApplication.BLL;
using GoMartApplication.DAL;
using GoMartApplication.DTO;
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
    public partial class SellingForm : Form
    {
     //   DBConnect dbCon = new DBConnect();
        public SellingForm()
        {
            InitializeComponent();
        }
        double GrandTotal = 0.0;
        int n = 0;
        private void SellingForm_Load(object sender, EventArgs e)
        {
            lblDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            // Load all categories
            using (var svc = new CategoryService())
            {
                cmbCategory.DataSource = svc.GetAllCategories().ToList();
                cmbCategory.DisplayMember = nameof(Category.CategoryName);
                cmbCategory.ValueMember = nameof(Category.CatID);
                cmbCategory.SelectedIndex = -1;
            }
            dataGridView2_Product.DataSource = null;
        }
      
    
     
        private void button3_Click(object sender, EventArgs e)
        {
            if (cmbCategory.SelectedValue == null) return;
            int catId = (int)cmbCategory.SelectedValue;
            using (var svc = new ProductService())
            {
                var products = svc.GetProductsByCategory(catId)
                    .Select(p => new
                    {
                        p.ProdID,
                        p.ProdName,
                        ProdPrice = p.ProdPrice,
                        ProdQty = p.ProdQty
                    })
                    .ToList();
                dataGridView2_Product.DataSource = products;
            }
        }

        private void dataGridView2_Product_DoubleClick(object sender, EventArgs e)
        {
           
        }

        private void dataGridView2_Product_Click(object sender, EventArgs e)
        {
            if (dataGridView2_Product.SelectedRows.Count == 0) return;
            var row = dataGridView2_Product.SelectedRows[0];
            txtProdID.Text = row.Cells["ProdID"].Value.ToString();
            txtProductName.Text = row.Cells["ProdName"].Value.ToString();
            txtPrice.Text = row.Cells["ProdPrice"].Value.ToString();
            txtQty.Text = row.Cells["ProdQty"].Value.ToString();
            txtQty.Focus();
        }

        private void btnAddOrder_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProdID.Text)) return;
            if (!decimal.TryParse(txtPrice.Text, out var price)) return;
            if (!int.TryParse(txtQty.Text, out var qty)) return;

            decimal total = price * qty;
            int idx = dataGridView1_Order.Rows.Add();
            var orderRow = dataGridView1_Order.Rows[idx];
            orderRow.Cells["ProdID"].Value = txtProdID.Text;
            orderRow.Cells["ProductName"].Value = txtProductName.Text;
            orderRow.Cells["Price"].Value = price;
            orderRow.Cells["Quantity"].Value = qty;
            orderRow.Cells["Total"].Value = total;

            GrandTotal += (double)total;
            lblGrandTot.Text = GrandTotal.ToString("N2");
        }

        private void btnRefCat_Click(object sender, EventArgs e)
        {
            if (cmbCategory.SelectedValue == null) return;
            int catId = (int)cmbCategory.SelectedValue;
            using (var svc = new ProductService())
            {
                dataGridView2_Product.DataSource = svc.GetProductsByCategory(catId)
                    .Select(p => new { p.ProdID, p.ProdName, p.ProdPrice, p.ProdQty })
                    .ToList();
            }
        }

        private void btnAddBill_Details_Click(object sender, EventArgs e)
        {
            var items = new List<(int ProdID, int Qty, decimal Price)>();
            foreach (DataGridViewRow row in dataGridView1_Order.Rows)
            {
                if (row.IsNewRow) continue;
                items.Add((
                    ProdID: Convert.ToInt32(row.Cells["ProdID"].Value),
                    Qty: Convert.ToInt32(row.Cells["Quantity"].Value),
                    Price: Convert.ToDecimal(row.Cells["Price"].Value)
                ));
            }

            string billId;
            using (var svc = new BillService())
                billId = svc.CreateSale(Form1.loginname, items);

            MessageBox.Show($"Hóa đơn {billId} đã lưu thành công", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            dataGridView1_Order.Rows.Clear();
            GrandTotal = 0.0;
            lblGrandTot.Text = "0.00";
        }
       
            

        private void dataGridView2_Product_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dataGridView2_Product.Rows[e.RowIndex];
            txtProdID.Text = row.Cells["ProdID"].Value.ToString();
            txtProductName.Text = row.Cells["ProdPrice"].Value.ToString();
            txtPrice.Text = row.Cells["ProdPrice"].Value.ToString();
            txtQty.Text = "1";
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
