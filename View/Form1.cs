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
using GoMartApplication.BLL;
using GoMartApplication.DTO;
namespace GoMartApplication
{
    public partial class Form1 : Form
    {
        //DBConnect dbCon = new DBConnect();
        public static string loginname, logintype;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string user = txtUsername.Text.Trim();
            string pass = txtPass.Text.Trim();
            string role = cmbRole.Text;

            using (var auth = new AuthService())
            {
                if (role == "Admin")
                {
                    if (auth.AuthenticateAdmin(user, pass, out Admin admin))
                    {
                        // thành công -> mở frmMain với thông tin Admin
                        Form1.loginname = admin.FullName;
                        Form1.logintype = "Admin";
                        this.Hide();
                        new frmMain().Show();
                        return;
                    }
                }
                else if (role == "Seller")
                {
                    if (auth.AuthenticateSeller(user, pass, out Seller seller))
                    {
                        Form1.loginname = seller.SellerName;
                        Form1.logintype = "Seller";
                        this.Hide();
                        new frmMain().Show();
                        return;
                    }
                }
            }

            MessageBox.Show("Đăng nhập không hợp lệ. Vui lòng kiểm tra lại!\", \"Lỗi\", MessageBoxButtons.OK, MessageBoxIcon.Error");
        }
            
        

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("Do you really want to close this Application ?", "CLOSE", MessageBoxButtons.YesNo, MessageBoxIcon.Stop))
            {
                Application.Exit();
            }
        }

        private void clrValue()
        {
            cmbRole.SelectedIndex = 0;
            txtUsername.Clear();
            txtPass.Clear();
        }
    }
}
