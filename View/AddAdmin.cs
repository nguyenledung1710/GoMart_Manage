using GoMartApplication.BLL;
using GoMartApplication.DTO;
using GoMartApplication.View;
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
    public partial class AddAdmin : Form
    {
        public AddAdmin()
        {
            InitializeComponent();
            this.Size = Program.DefaultFormSize;
            this.MinimumSize = this.MaximumSize = this.Size;
            this.StartPosition = FormStartPosition.CenterScreen;
            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
            btnSearch.Click += button1_Click;
        }

    
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string id = txtAdminID.Text.Trim();
            string pass = txtPass.Text.Trim();
            var fullname = txtAdminName.Text.Trim();
            string address = txtAdAddress.Text.Trim();

            try
            {
                using (var service = new AdminService())
                {
                    bool ok = service.CreateAdmin(id, pass, fullname, address);
                    if (!ok)
                    {
                        MessageBox.Show("AdminID đã tồn tại.", "Thông báo",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (ok)
                    {
                        MessageBox.Show("Thêm Admin thành công!", "Thành công",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);


                        txtAdminID.Clear();
                        txtPass.Clear();
                        txtAdminName.Clear();
                        txtAdAddress.Clear();            
                        txtAdminID.Focus();
                        button1_Click(this, EventArgs.Empty);
                    }
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi đầu vào",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void AddAdmin_Load(object sender, EventArgs e)
        {
            txtAdminName.Focus();
            txtSearch.Clear();
            button1_Click(this, EventArgs.Empty);
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
        }
   
 

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string adminId = txtAdminID.Text.Trim();
            string password = txtPass.Text.Trim();
            string fullName = txtAdminName.Text.Trim();
            string address = txtAdAddress.Text.Trim();
            if (string.IsNullOrEmpty(adminId))
            {
                MessageBox.Show("Vui lòng chọn Admin để cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                using (var svc = new AdminService())
                {
                    bool ok = svc.UpdateAdmin(adminId, password, fullName, address);
                    if (ok)
                    {

                        MessageBox.Show("Cập nhật Admin thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        button1_Click(this, EventArgs.Empty);
                        
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật thất bại. Vui lòng kiểm tra lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var rows = dataGridView1.SelectedRows;
            if (rows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một Admin để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show($"Bạn có chắc muốn xóa {rows.Count} Admin?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            try
            {
                using (var svc = new AdminService())
                {
                    foreach (DataGridViewRow row in rows)
                    {
                        string id = row.Cells[0].Value?.ToString();
                        svc.DeleteAdmin(id);
                    }
                }
                MessageBox.Show("Xóa Admin thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
               
                button1_Click(this, EventArgs.Empty);
            
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void dataGridView1_Click(object sender, EventArgs e)
        {

            try
            {
                btnUpdate.Visible = true;
                btnDelete.Visible = true;
                btnAdd.Visible = true;
                btnAdd.Enabled = false;
                txtAdminID.ReadOnly = true;

                txtAdminID.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                txtPass.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                txtAdminName.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            using (var svc = new AdminService())
            {
                var all = svc.GetAllAdmins();
                var list = string.IsNullOrEmpty(keyword)
                    ? all.ToList()
                    : all.Where(a => (a.AdminId != null && a.AdminId.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0) ||
        (a.FullName != null && a.FullName.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0))
                         .ToList();
                dataGridView1.DataSource = list;
            }

            dataGridView1.ClearSelection();
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            int count = dataGridView1.SelectedRows.Count;
         
            btnUpdate.Enabled = (count == 1);
            btnDelete.Enabled = (count >= 1);

            if (count == 1)
            {
                var row = dataGridView1.SelectedRows[0];
                txtAdminID.Text = row.Cells[0].Value?.ToString();
                txtPass.Text = row.Cells[1].Value?.ToString();
                txtAdminName.Text = row.Cells[2].Value?.ToString();
                txtAdAddress.Text = row.Cells[3].Value?.ToString();
                txtAdminID.ReadOnly = true;
            }
            else
            {
                txtAdminID.Clear();
                txtPass.Clear();
                txtAdminName.Clear();
                txtAdAddress.Clear();
                txtAdminID.ReadOnly = false;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            DataGridView1_SelectionChanged(sender, e);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtAdminID.Clear();
            txtPass.Clear();
            txtAdminName.Clear();
            txtAdAddress.Clear();
            txtAdminID.ReadOnly = false;
        }
    }
}
