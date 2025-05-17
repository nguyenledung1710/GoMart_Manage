using GoMartApplication.BLL;
using GoMartApplication.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace GoMartApplication
{
    partial class Statistics : Form
    {
        public Statistics()
        {
            InitializeComponent();
            this.Size = Program.DefaultFormSize;
            this.MinimumSize = this.MaximumSize = this.Size;
            this.StartPosition = FormStartPosition.CenterScreen;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            setcmMode();
            ReloadSellerList();
            UpdateModeControls();
        }
        public void setcmMode()
        {
            cmbMode.Items.Clear();
            cmbMode.Items.AddRange(new object[] { "All", "By Date", "By Seller" });
            cmbMode.SelectedIndex = 0;
            cmbMode.SelectedIndexChanged += (s, e) => UpdateModeControls();

            cmbMode.SelectedIndex = 0;

        }
        public void ReloadSellerList()
        {
            var svc = new SellerService();
            var list = svc.GetAllSellers().ToList();
            cmbSeller.DataSource = list;
            cmbSeller.DisplayMember = "SellerName";
            cmbSeller.ValueMember = "SellerID";
            cmbSeller.SelectedIndex = -1;
        }

        private void UpdateModeControls()
        {
            string mode = cmbMode.SelectedItem.ToString();
            bool isByDate = mode == "By Date";
            bool isBySeller = mode == "By Seller";

            lbDate.Visible = isByDate;
            dtpDate.Visible = isByDate;
            lbSeller.Visible = isBySeller;
            cmbSeller.Visible = isBySeller;
        }



        private void btnGenerate_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();

            var mode = cmbMode.SelectedItem?.ToString();
            DateTime? date = mode == "By Date" ? dtpDate.Value.Date : (DateTime?)null;
            string sellerId = mode == "By Seller" ? (cmbSeller.SelectedItem as Seller)?.SellerId : null;

            if (mode == null ||
                (mode == "By Seller" && sellerId == null))
            {
                MessageBox.Show("Chế độ chưa hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataTable table;
            int totalBills, totalItems;
            decimal totalRevenue;
            using (var svc = new BillService())
            {
                (table, totalBills, totalItems, totalRevenue)
                    = svc.GenerateStatistics(mode, date, sellerId);
            }
            dataGridView1.DataSource = table;

            dataGridView1.Columns["SellDate"].HeaderText = "Date & Time";
            dataGridView1.Columns["SellDate"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
            dataGridView1.Columns["Price"].DefaultCellStyle.Format = "0.##";
            dataGridView1.Columns["Total"].DefaultCellStyle.Format = "0.##";

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;

            lblTotalBills.Text = totalBills.ToString();
            lblTotalItems.Text = totalItems.ToString();
            lblTotalRevenue.Text = totalRevenue.ToString("0.##");
        }



    }
}
