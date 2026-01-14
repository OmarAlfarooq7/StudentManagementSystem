using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace StudentsManagementSystem
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
            ThemeManager.ApplyTheme(this);  // تطبيق الثيم على الواجهة
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            User user = DbStudent.Login(
                txtUserName.Text.Trim(),
                txtPassword.Text.Trim()
            );

            if (user != null)
            {
                UserSession.CurrentUser = user;
                FormMainHome frm = new FormMainHome();
                frm.WindowState = FormWindowState.Maximized;
                frm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid login data");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void FormLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
