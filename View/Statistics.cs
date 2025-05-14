using GoMartApplication.BLL;
using GoMartApplication.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            if (cmbMode.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn chế độ thống kê.", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int totalBills = 0;
            int totalItems = 0;
            decimal totalRevenue = 0;

            using (var svc = new BillService())
            {
                var allBills = svc.GetAllBills();
                IEnumerable<Bill> filteredBills;

                string mode = cmbMode.SelectedItem.ToString();
                switch (mode)
                {
                    case "All":
                        filteredBills = allBills;
                        break;

                    case "By Date":
                        var date = dtpDate.Value.Date;
                        filteredBills = allBills.Where(b => b.SellDate.Date == date);
                        break;

                    case "By Seller":
                  
                        var sel = cmbSeller.SelectedItem as Seller;
                        if (sel == null)
                        {
                            MessageBox.Show("Vui lòng chọn người bán.", "Thông báo",
                                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
           
                        filteredBills = allBills.Where(b => b.Seller.SellerId == sel.SellerId);
                        break;

                    default:
                        filteredBills = allBills;
                        break;
                }

                totalBills = filteredBills.Count();

                foreach (var bill in filteredBills)
                {
                    var details = svc.GetBillDetails(bill.Bill_ID);
                    foreach (var d in details)
                    {
                        dataGridView1.Rows.Add(
                            d.ProdID,
                            d.Product.ProdName,
                            d.Qty,
                            d.Price.ToString("0.##"),
                            d.Total.ToString("0.##"),
                            bill.Seller.SellerName,
                            bill.SellDate.ToString("dd/MM/yyyy HH:mm:ss")
                        );
                        totalItems += d.Qty;
                        totalRevenue += d.Total;
                    }
                }
            }
            lblTotalBills.Text = totalBills.ToString();
            lblTotalItems.Text = totalItems.ToString();
            lblTotalRevenue.Text = totalRevenue.ToString("0.##");

        }

        private void Statistics_Load(object sender, EventArgs e)
        {

        }
    }
}
