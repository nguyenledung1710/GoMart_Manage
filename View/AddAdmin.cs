using GoMartApplication.BLL;
using System;
using System.Data;
using System.Linq;
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
                        btnSearch_Click(this, EventArgs.Empty);
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
            btnSearch_Click(this, EventArgs.Empty);
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

                        btnSearch_Click(this, EventArgs.Empty);
                        
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

                btnSearch_Click(this, EventArgs.Empty);
            
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtAdminID.Clear();
            txtPass.Clear();
            txtAdminName.Clear();
            txtAdAddress.Clear();
            txtAdminID.ReadOnly = false;
            btnAdd.Enabled = true;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            using (var svc = new AdminService())
            {
                var list = svc.SearchAdmins(keyword).ToList();
                dataGridView1.DataSource = list;
            }
            dataGridView1.ClearSelection();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            btnAdd.Visible = true;
            btnUpdate.Visible = true;
            btnDelete.Visible = true;

            var count = dataGridView1.SelectedRows.Count;
            btnAdd.Enabled = (count == 0);
            btnUpdate.Enabled = (count == 1);
            btnDelete.Enabled = (count >= 1);

            var row = dataGridView1.Rows[e.RowIndex];

            txtAdminID.Text = row.Cells["AdminId"].Value?.ToString();
            txtPass.Text = row.Cells["Password"].Value?.ToString();
            txtAdminName.Text = row.Cells["FullName"].Value?.ToString();
            txtAdAddress.Text = row.Cells["address"].Value?.ToString();

            txtAdminID.ReadOnly = true;
        }
    }
}
