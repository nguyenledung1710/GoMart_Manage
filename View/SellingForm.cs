using GoMartApplication.BLL;
using GoMartApplication.DAL;
using GoMartApplication.DTO;
using GoMartApplication.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
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
            dataGridView1.CellDoubleClick += dataGridView1_CellContentClick;
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
            RefreshSellingList();
        }

        private void RefreshSellingList()
        {
            using (var svc = new BillService())
            {
                var list = svc.GetAllBills()
                    .Select(b => new
                    {
                        BillNumber = b.Bill_ID,
                        DateTime = b.SellDate.ToString("dd/MM/yyyy HH:mm:ss"),
                        Seller = b.Seller.SellerName,
                        Amount = b.TotalAmt.ToString("0.##")
                    })
                    .ToList();
                dataGridView1.DataSource = list;
                dataGridView1.Columns["BillNumber"].HeaderText = "Bill Number";
                dataGridView1.Columns["DateTime"].HeaderText = "Date & Time";
                dataGridView1.Columns["Seller"].HeaderText = "Seller Name";
                dataGridView1.Columns["Amount"].HeaderText = "Total";
            }
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
                dataGridView2_Product.Columns["ProdPrice"]
                        .DefaultCellStyle.Format = "0.##";
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
            orderRow.Cells["Price"].Value = price.ToString("0.##");
            orderRow.Cells["Quantity"].Value = qty;
            orderRow.Cells["Total"].Value = total.ToString("0.##");

            GrandTotal += (double)total;
            lblGrandTot.Text = GrandTotal.ToString("0.##");
        }

        private void btnRefCat_Click(object sender, EventArgs e)
        { // Xóa hết nội dung DataGridView
            dataGridView2_Product.DataSource = null;
            dataGridView2_Product.Rows.Clear();

            // Xóa hết các ô TextBox
            txtProdID.Clear();
            txtProductName.Clear();
            txtPrice.Clear();
            txtQty.Clear();
            //using (var svc = new CategoryService())
            //{
            //    cmbCategory.DataSource = svc.GetAllCategories().ToList();
            //    cmbCategory.DisplayMember = nameof(Category.CategoryName);
            //    cmbCategory.ValueMember = nameof(Category.CatID);
            //}
        }

        private void btnAddBill_Details_Click(object sender, EventArgs e)
        {

            // 1) Tập hợp các item đang có trong dataGridView1_Order
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

            if (items.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm ít nhất một sản phẩm vào đơn.", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2) Gọi BLL lưu hoá đơn
            string billId;
            using (var svc = new BillService())
                billId = svc.CreateSale(Form1.loginname, items);

            // 3) Hiển thị Bill_Number lên TextBox
            txtBillNo.Text = billId;
            txtBillNo.ReadOnly = true;

            // 4) Cập nhật lại Selling List
            RefreshSellingList();

            // 5) Reset khung Order để bán hoá đơn mới
            dataGridView1_Order.Rows.Clear();
            GrandTotal = 0;
            lblGrandTot.Text = "0.00";
            if (cmbCategory.SelectedValue is int catId)
            {
                // Gọi lại hàm Search để load products mới
                button3_Click(this, EventArgs.Empty);
            }
            MessageBox.Show($"Hoá đơn {billId} đã được lưu.", "Thành công",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
       
            

        private void dataGridView2_Product_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2_Product.SelectedRows.Count == 0) return;
            var row = dataGridView2_Product.SelectedRows[0];
            txtProdID.Text = row.Cells["ProdID"].Value.ToString();
            txtProductName.Text = row.Cells["ProdName"].Value.ToString();
            var price = Convert.ToDecimal(row.Cells["ProdPrice"].Value);
            txtPrice.Text = price.ToString("0.##");
            txtQty.Text = "1";
            txtQty.Focus();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var billId = dataGridView1.Rows[e.RowIndex]
                .Cells["BillNumber"].Value.ToString();
            using (var detailsForm = new OrderDetailsForm(billId))
            {
                detailsForm.ShowDialog();
            }

        }
    }
}
