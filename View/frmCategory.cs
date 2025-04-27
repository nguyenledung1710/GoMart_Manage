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
    public partial class frmCategory : Form
    {
       // DBConnect dbCon = new DBConnect();
        public frmCategory()
        {
            InitializeComponent();
        }

        private void frmCategory_Load(object sender, EventArgs e)
        {
            lblCatID.Visible = false;
            btnAddCat.Enabled = true;
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;
            btnDelete.Visible = true;
            btnUpdate.Visible = true;
            btnAddCat.Visible = true;
            
            LoadCategories();
        }

        private void LoadCategories()
        {
            using (var svc = new CategoryService())
            {
                dataGridView1.DataSource = svc.GetAllCategories().ToList();
            }
            dataGridView1.ClearSelection();
        }
        private void btnAddCat_Click(object sender, EventArgs e)
        {

            var name = txtCatname.Text.Trim();
            var desc = rtbCatDesc.Text.Trim();


            try
            {
                using (var svc = new CategoryService())
                    {
                      if (!svc.CreateCategory(name, desc))
                    {
                        MessageBox.Show("CatID đã tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                MessageBox.Show("Thêm Category thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearInputs();
                LoadCategories();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
       
       

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count == 0) return;

            btnUpdate.Visible = true;
            btnDelete.Visible = true;
            btnAddCat.Visible = true;
            lblCatID.Visible = true;
            btnAddCat.Enabled = false;

            var row = dataGridView1.SelectedRows[0];
            lblCatID.Text = row.Cells[0].Value.ToString();
            txtCatname.Text = row.Cells[1].Value.ToString();
            rtbCatDesc.Text = row.Cells[2].Value.ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(lblCatID.Text, out var id)) return;
            var name = txtCatname.Text.Trim();
            var desc = rtbCatDesc.Text.Trim();

            using (var svc = new CategoryService())
            {
                if (svc.UpdateCategory(id, name, desc))
                {
                    MessageBox.Show("Cập nhật Category thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearInputs();
                    LoadCategories();
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(lblCatID.Text, out var id))
            {
                MessageBox.Show("Vui lòng chọn Category để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show($"Bạn có chắc chắn xóa Category {id}?", "Xác nhận",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            using (var svc = new CategoryService())
            {
                if (svc.DeleteCategory(id))
                {
                    MessageBox.Show("Xóa Category thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearInputs();
                    LoadCategories();
                }
                else
                {
                    MessageBox.Show("Xóa thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void ClearInputs()
        {
            txtCatname.Clear();
            rtbCatDesc.Clear();
            lblCatID.Text = string.Empty;

            btnAddCat.Visible = true;
            btnUpdate.Visible = true;
            btnDelete.Visible = true;
            lblCatID.Visible = false;
        }
    }
}
