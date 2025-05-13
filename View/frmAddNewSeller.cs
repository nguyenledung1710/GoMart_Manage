using GoMartApplication.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;


namespace GoMartApplication
{
    public partial class frmAddNewSeller : Form
    {
        public frmAddNewSeller()
        {
            InitializeComponent();
            this.Size = Program.DefaultFormSize;
            this.MinimumSize = this.MaximumSize = this.Size;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Load += frmAddNewSeller_Load;
            btnSearchSeller.Click += btnSearchSeller_Click;
            this.dataGridView1.CellClick += dataGridView1_CellContentClick;
            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
        }

        private void frmAddNewSeller_Load(object sender, EventArgs e)
        {
            btnUpdate.Visible = true;
            btnDelete.Visible = true;
            btnAdd.Visible = true;
            btnAdd.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            txtSellerID.ReadOnly = false;

            BindSeller();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var id = txtSellerID.Text.Trim();
            var name = txtSellerName.Text.Trim();
            if (!int.TryParse(txtAge.Text.Trim(), out var age))
            {
                MessageBox.Show("Age phải là số nguyên.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var phone = txtPhone.Text.Trim();
            if (!Regex.IsMatch(phone, @"^0\d{9}$"))
            {
                MessageBox.Show(
                    "Số điện thoại không hợp lệ. Phải có 10 chữ số và bắt đầu bằng số 0.",
                    "Lỗi đầu vào",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }
            var pass = txtPass.Text.Trim();

            try
            {
                using (var svc = new SellerService())
                {
                    bool ok = svc.CreateSeller(id, name, age, phone, pass);
                    if (!ok)
                    {
                        MessageBox.Show("SellerID đã tồn tại.", "Thông báo",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                MessageBox.Show("Thêm Seller thành công!", "Thành công",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                var statsForm = Application.OpenForms
                  .OfType<Statistics>()
                  .FirstOrDefault();
                statsForm?.ReloadSellerList();
                ClearInputs();
                BindSeller();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi đầu vào",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

       

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            var id = txtSellerID.Text.Trim();
            var name = txtSellerName.Text.Trim();
            if (!int.TryParse(txtAge.Text.Trim(), out var age))
            {
                MessageBox.Show("Age phải là số nguyên.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var phone = txtPhone.Text.Trim();
            if (!Regex.IsMatch(phone, @"^0\d{9}$"))
            {
                MessageBox.Show(
                    "Số điện thoại không hợp lệ. Phải có 10 chữ số và bắt đầu bằng số 0.",
                    "Lỗi đầu vào",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }
            var pass = txtPass.Text.Trim();

            using (var svc = new SellerService())
            {
                bool ok = svc.UpdateSeller(id, name, age, phone, pass);
                if (ok)
                {
                    MessageBox.Show("Cập nhật Seller thành công.", "Thành công",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearInputs();
                    BindSeller();
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại.", "Lỗi",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var rows = dataGridView1.SelectedRows;
            if (rows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một Seller để xóa.",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show($"Bạn có chắc chắn xóa {rows.Count} Seller?",
                                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                != DialogResult.Yes) return;

            using (var svc = new SellerService())
            {
                foreach (DataGridViewRow row in rows)
                {
                    var id = row.Cells[0].Value?.ToString();
                    svc.DeleteSeller(id);
                }
            }
            MessageBox.Show("Xóa Seller thành công.", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearInputs();
            BindSeller();
           
        }
        private void BindSeller()
        {
            using (var svc = new SellerService())
            {
                var list = svc.GetAllSellers().ToList();
                dataGridView1.DataSource = list;
            }
            if (dataGridView1.Columns.Contains("Bills"))
                dataGridView1.Columns["Bills"].Visible = false;
            dataGridView1.ClearSelection();

        }

      

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnSearchSeller_Click(object sender, EventArgs e)
        {
            var key = txtSearch.Text.Trim();
            using (var svc = new SellerService())
            {
                var all = svc.GetAllSellers();
                var filtered = string.IsNullOrEmpty(key)
                    ? all
                    : all.Where(s =>
                        (s.SellerId?.IndexOf(key, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0
                     || (s.SellerName?.IndexOf(key, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0
                    );
                dataGridView1.DataSource = filtered.ToList();
            }
        }
        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            var rows = dataGridView1.SelectedRows;
            btnUpdate.Enabled = (rows.Count == 1);
            btnDelete.Enabled = (rows.Count >= 1);

            if (rows.Count == 1)
            {
                var r = rows[0];
                txtSellerID.Text = r.Cells[0].Value?.ToString();
                txtSellerName.Text = r.Cells[1].Value?.ToString();
                txtAge.Text = r.Cells[2].Value?.ToString();
                txtPhone.Text = r.Cells[3].Value?.ToString();
                txtPass.Text = r.Cells[4].Value?.ToString();
                txtSellerID.ReadOnly = true;
                btnAdd.Enabled = false;
            }
            else
            {
                ClearInputs();
            }
        }
        private void ClearInputs()
        {
            txtSellerID.Clear();
            txtSellerName.Clear();
            txtAge.Clear();
            txtPhone.Clear();
            txtPass.Clear();
            txtSellerID.ReadOnly = false;
            btnAdd.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtSellerID.Clear();
            txtSellerName.Clear();
            txtAge.Clear();
            txtPhone.Clear();
            txtPass.Clear();
            txtSellerID.ReadOnly = false;
            btnAdd.Enabled = true;
          
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dataGridView1.Rows[e.RowIndex];
            row.Selected = true;

            txtSellerID.Text = row.Cells["SellerId"].Value?.ToString();
            txtSellerName.Text = row.Cells["SellerName"].Value?.ToString();
            txtAge.Text = row.Cells["SellerAge"].Value?.ToString();
            txtPhone.Text = row.Cells["SellerPhone"].Value?.ToString();
            txtPass.Text = row.Cells["SellerPass"].Value?.ToString();

            txtSellerID.ReadOnly = true;
            btnAdd.Enabled = false;
        }
    }
}
