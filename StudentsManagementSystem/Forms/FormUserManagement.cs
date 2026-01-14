using System;
using System.Data;
using System.Windows.Forms;

namespace StudentsManagementSystem
{
    public partial class FormUserManagement : Form
    {
        FormUser form;
        private bool isProcessing = false;

        public FormUserManagement()
        {
            InitializeComponent();
            form = new FormUser(this);

            ThemeManager.ApplyTheme(this);  // تطبيق الثيم على الواجهة

        }

        //  عرض المستخدمين 
        public void Display()
        {
            DbStudent.DisplayAndSearch(
                "SELECT UserID, Username, Role FROM Users",
                dataGridView
            );

            dataGridView.ClearSelection();
        }


        //  إعداد الجدول
        private void SetupDataGridView()
        {
            dataGridView.Columns.Clear();
            dataGridView.AutoGenerateColumns = false;

            // زر تعديل
            DataGridViewButtonColumn btnEdit = new DataGridViewButtonColumn();
            btnEdit.Name = "Edit";
            btnEdit.Text = "Edit";
            btnEdit.UseColumnTextForButtonValue = true;
            dataGridView.Columns.Add(btnEdit);

            // زر حذف
            DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
            btnDelete.Name = "Delete";
            btnDelete.Text = "Delete";
            btnDelete.UseColumnTextForButtonValue = true;
            dataGridView.Columns.Add(btnDelete);

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "UserID",
                HeaderText = "ID",
                DataPropertyName = "UserID"
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Username",
                HeaderText = "Username",
                DataPropertyName = "Username"
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Role",
                HeaderText = "Role",
                DataPropertyName = "Role"
            });

            btnEdit.FlatStyle = FlatStyle.Popup;
            btnDelete.FlatStyle = FlatStyle.Popup;
            dataGridView.ReadOnly = true;
            dataGridView.AllowUserToAddRows = false;


        }

        //  البحث 
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string search = txtSearch.Text.Trim();

            string query =
                "SELECT UserID, Username, Role FROM Users " +
                "WHERE Username LIKE '%" + search + "%' " +
                "OR Role LIKE '%" + search + "%'";

            DbStudent.DisplayAndSearch(query, dataGridView);
        }

        private void FormUserManagement_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterParent;

            SetupDataGridView();
            Display();

        }

        private void btnNewStudent_Click_1(object sender, EventArgs e)
        {
            form.AddInfo();
            form.ShowDialog();
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (isProcessing) return;

            isProcessing = true;

            try
            {
                DataGridViewRow row = dataGridView.Rows[e.RowIndex];

                // تعديل
                if (dataGridView.Columns[e.ColumnIndex].Name == "Edit")
                {
                    form.Clear();
                    form.UserID = row.Cells["UserID"].Value.ToString();
                    form.Username = row.Cells["Username"].Value.ToString();
                    form.Role = row.Cells["Role"].Value.ToString();

                    form.UpdateInfo();
                    form.ShowDialog();
                    Display();
                }

                // حذف
                else if (dataGridView.Columns[e.ColumnIndex].Name == "Delete")
                {
                    string id = row.Cells["UserID"].Value.ToString();

                    if (MessageBox.Show("Delete this user?", "Confirm",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        DbStudent.DeleteUser(id);
                        Display();
                    }
                }
            }
            finally
            {
                isProcessing = false;
            }
        }
    }
}
