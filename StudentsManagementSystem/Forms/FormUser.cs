using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentsManagementSystem
{
    public partial class FormUser : Form
    {
        private readonly FormUserManagement _parent;

        public string UserID;
        public string Username;
        public string Role;
        public FormUser(FormUserManagement parent)
        {
            InitializeComponent();
            _parent = parent;

            ThemeManager.ApplyTheme(this);  // تطبيق الثيم على الواجهة

        }

        // عند فتح الفورم للاضافة
        public void AddInfo()
        {
            lbltext2.Text = "Add User";
            btnAddUser.Text = "Add";
            Clear();
        }

        // عند فتح الفورم للتعديل
        public void UpdateInfo()
        {
            lbltext2.Text = "Update User Data";
            btnAddUser.Text = "Update";

            txtUserName.Text = Username;
            txtPassword.Text = "";
            cmbRole.SelectedItem = Role;
        }

        public void Clear()
        {
            txtUserName.Text = "";
            txtPassword.Text = "";
            cmbRole.SelectedIndex = -1;
        }

        private void FormUser_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterParent;

            cmbRole.Items.Clear();
            cmbRole.Items.Add("Admin");
            cmbRole.Items.Add("User");
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            if (txtUserName.Text == "" || txtPassword.Text == "" || cmbRole.SelectedIndex == -1)
            {
                MessageBox.Show("Fill all fields");
                return;
            }

            User u = new User(
                txtUserName.Text.Trim(),
                txtPassword.Text.Trim(),
                cmbRole.SelectedItem.ToString()
            );

            if (btnAddUser.Text == "Add")
                DbStudent.AddUser(u);
            else
                DbStudent.UpdateUser(u, UserID);

            Clear();

            _parent.Display();
            Close();
        }
    }
   
}
