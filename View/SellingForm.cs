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
        private DataTable _dtProducts;
        double GrandTotal = 0.0;
        public SellingForm()
        {
            InitializeComponent();
            loadCombobox();
            this.Size = Program.DefaultFormSize;
            this.MinimumSize = this.MaximumSize = this.Size;
            this.StartPosition = FormStartPosition.CenterScreen;
            dataGridView2_Product.DataSource = null;
         
        }
        private void loadCombobox()
        {
            using (var svc = new CategoryService())
            {
                var cats = svc.GetAllCategories().ToList();
                cmbCategory.DataSource = cats;
                cmbCategory.DisplayMember = nameof(Category.CategoryName);
                cmbCategory.ValueMember = nameof(Category.CatID);
                cmbCategory.SelectedIndex = -1;
            }
        }
        private void SellingForm_Load(object sender, EventArgs e)
        {

            int catId = Convert.ToInt32(cmbCategory.SelectedValue);
            var list = new ProductService().GetAllByCategory(catId);

            _dtProducts = new DataTable();
            _dtProducts.Columns.Add("ProdID", typeof(int));
            _dtProducts.Columns.Add("ProdName", typeof(string));
            _dtProducts.Columns.Add("ProdPrice", typeof(decimal));
            _dtProducts.Columns.Add("ProdQty", typeof(int));

            foreach (var p in list)
                _dtProducts.Rows.Add(p.ProdID, p.ProdName, p.ProdPrice, p.ProdQty);

            dataGridView2_Product.DataSource = _dtProducts;
            dataGridView2_Product.Columns["ProdPrice"]
                                 .DefaultCellStyle.Format = "0.##";
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

        private void LoadProductGrid()
        {
            if (cmbCategory.SelectedValue == null)
                return;
            int catId = Convert.ToInt32(cmbCategory.SelectedValue);

            var list = new ProductService()
                           .GetAllByCategory(catId);
            _dtProducts = new DataTable();
            _dtProducts.Columns.Add("ProdID", typeof(int));
            _dtProducts.Columns.Add("ProdName", typeof(string));
            _dtProducts.Columns.Add("ProdPrice", typeof(decimal));
            _dtProducts.Columns.Add("ProdQty", typeof(int));

            foreach (var p in list)
                _dtProducts.Rows.Add(p.ProdID, p.ProdName, p.ProdPrice, p.ProdQty);

            dataGridView2_Product.DataSource = _dtProducts;
            dataGridView2_Product.Columns["ProdPrice"]
                                 .DefaultCellStyle.Format = "0.##";
        }

   
        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProductGrid();
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
            if (!int.TryParse(txtProdID.Text, out int prodId)) return;
            if (!decimal.TryParse(txtPrice.Text, out decimal price)) return;
            if (!int.TryParse(txtQty.Text, out int qty) || qty <= 0) return;

            decimal total = price * qty;
            int idx = dataGridView1_Order.Rows.Add();
            var orderRow = dataGridView1_Order.Rows[idx];
            orderRow.Cells["ProdID"].Value = prodId;
            orderRow.Cells["ProductName"].Value = txtProductName.Text;
            orderRow.Cells["Price"].Value = price.ToString("0.##");
            orderRow.Cells["Quantity"].Value = qty;
            orderRow.Cells["Total"].Value = total.ToString("0.##");

            GrandTotal += (double)total;
            lblGrandTot.Text = GrandTotal.ToString("0.##");
            var found = _dtProducts.Select($"ProdID = {prodId}");
            if (found.Length > 0)
            {
                int cur = (int)found[0]["ProdQty"];
                if (cur < qty)
                {
                    MessageBox.Show($"Chỉ còn {cur} trong kho!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dataGridView1_Order.Rows.RemoveAt(idx);
                    GrandTotal -= (double)total;
                    lblGrandTot.Text = GrandTotal.ToString("0.##");
                    return;
                }
                found[0]["ProdQty"] = cur - qty;
                dataGridView2_Product.Refresh();
            }

        }
        private void btnRefCat_Click(object sender, EventArgs e)
        { 
            dataGridView2_Product.DataSource = null;
            dataGridView2_Product.Rows.Clear();

            txtProdID.Clear();
            txtProductName.Clear();
            txtPrice.Clear();
            txtQty.Clear();
          
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

            if (items.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm ít nhất một sản phẩm vào đơn.", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string billId;
            using (var svc = new BillService())
                billId = svc.CreateSale(Form1.loginID, items);

            txtBillNo.Text = billId;
            txtBillNo.ReadOnly = true;

            RefreshSellingList();

            dataGridView1_Order.Rows.Clear();
            GrandTotal = 0;
            lblGrandTot.Text = "0.00";
            if (cmbCategory.SelectedValue != null)
            {
                LoadProductGrid();
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
        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            if (cmbCategory.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn một danh mục trước khi Search.");
                return;
            }
            LoadProductGrid();
        }
    }
}
