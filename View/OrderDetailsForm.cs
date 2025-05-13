using GoMartApplication.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoMartApplication.View
{
    public partial class OrderDetailsForm: Form
    {
        private string _billId;
        public OrderDetailsForm(string billId)
        {  
            InitializeComponent();
            this.Size = Program.DefaultFormSize;
            this.MinimumSize = this.MaximumSize = this.Size;
            this.StartPosition = FormStartPosition.CenterScreen;
            _billId = billId;
            this.Load += OrderDetailsForm_Load;
        }
        private void OrderDetailsForm_Load(object sender, EventArgs e)
        {
            textBox1.Text = _billId;
            textBox1.Enabled = false;
            using (var svc = new BillService())
            {
                var bill = svc.GetById(_billId);
;
                var details = svc.GetBillDetails(_billId);
                dataGridView1.Rows.Clear();
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
                }
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
