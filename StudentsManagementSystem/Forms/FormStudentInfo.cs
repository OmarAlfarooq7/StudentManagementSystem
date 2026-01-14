using System;
using System.Data;
using System.Windows.Forms;

namespace StudentsManagementSystem
{
    public partial class FormStudentInfo : Form
    {
        FormStudent form;
        private bool isProcessing = false;
        public FormStudentInfo()
        {
            InitializeComponent();
            form = new FormStudent(this);

            ThemeManager.ApplyTheme(this);  // تطبيق الثيم على الواجهة
        }

        // عرض البيانات حسب القسم المحدد
        public void Display()
        {
            string query = @"SELECT StudentID, FirstName, LastName, Age, Gender, Address, DepartmentID FROM Students";

            if (cmbDepartment.SelectedValue != null &&
                int.TryParse(cmbDepartment.SelectedValue.ToString(), out int deptId) && deptId != 0)
                {
                   query += " WHERE DepartmentID = " + deptId;
                }

            DbStudent.DisplayAndSearch(query, dataGridView);

            // إخفاء عمود DepartmentID في DataGridView
            if (dataGridView.Columns["DepartmentID"] != null)
                dataGridView.Columns["DepartmentID"].Visible = false;
        }

        // تحميل الأقسام
        private void LoadDepartments()
        {
            /*if (!UserSession.IsLoggedIn)
            {
                MessageBox.Show("انتهت الجلسة");
                Application.Exit();
            }*/


            DataTable dt = DbStudent.GetDepartmentsForCombo();

            // إضافة خيار "جميع الأقسام"
            DataRow row = dt.NewRow();
            row["DepartmentID"] = 0;
            row["DepartmentName"] = "All department";
            dt.Rows.InsertAt(row, 0);

            cmbDepartment.DataSource = dt;
            cmbDepartment.DisplayMember = "DepartmentName";
            cmbDepartment.ValueMember = "DepartmentID";
            cmbDepartment.SelectedIndex = 0;
        }

        private void FormStudentInfo_Shown(object sender, EventArgs e)
        {
            SetupDataGridView();
            Display();

            if (UserSession.CurrentUser.Role != "Admin")
            {
                btnNewStudent.Enabled = false;
                dataGridView.Columns["Delete"].Visible = false;
            }
        }

        // زر إضافة طالب جديد
        private void btnNewStudent_Click(object sender, EventArgs e)
        {
            form.AddInfo();
            form.ShowDialog();
        }

        // البحث
        private void txtSearch_VisibleChanged(object sender, EventArgs e)
        {
            showResultSearch();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            showResultSearch();
        }

        private void showResultSearch()
        {
            string search = txtSearch.Text.Trim();

            string query =
                "SELECT StudentID, FirstName, LastName, Age, Gender, Address, DepartmentID FROM Students " +
                "WHERE FirstName LIKE '%" + search + "%' " +
                "OR LastName LIKE '%" + search + "%' " +
                "OR Gender LIKE '%" + search + "%' " +
                "OR Address LIKE '%" + search + "%'";

            DbStudent.DisplayAndSearch(query, dataGridView);
        }

        // تعديل / حذف
        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
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

                    form.StudentID = row.Cells["StudentID"].Value.ToString();
                    form.FirstName = row.Cells["FirstName"].Value.ToString();
                    form.LastName = row.Cells["LastName"].Value.ToString();
                    form.Age = Convert.ToInt32(row.Cells["Age"].Value);
                    form.Gender = row.Cells["Gender"].Value.ToString();
                    form.Address = row.Cells["Address"].Value.ToString();
                    form.Department = row.Cells["DepartmentID"].Value.ToString();

                    // تحديد القسم في الكومبو عند فتح الفورم
                    if (row.Cells["DepartmentID"].Value != DBNull.Value)
                        cmbDepartment.SelectedValue = row.Cells["DepartmentID"].Value;

                    form.UpdateInfo();
                    form.ShowDialog();

                    Display(); // تحديث بعد الإغلاق
                }

                // حذف
                else if (dataGridView.Columns[e.ColumnIndex].Name == "Delete")
                {
                    string studentId = row.Cells["StudentID"].Value.ToString();

                    if (MessageBox.Show("Delete this student?", "Confirm",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        DbStudent.DeleteStudent(studentId);
                        Display();
                    }
                }
            }
            finally
            {
                isProcessing = false;
            }
        }

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
                Name = "StudentID",
                HeaderText = "ID",
                DataPropertyName = "StudentID"
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "FirstName",
                HeaderText = "First Name",
                DataPropertyName = "FirstName"
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "LastName",
                HeaderText = "Last Name",
                DataPropertyName = "LastName"
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Age",
                HeaderText = "Age",
                DataPropertyName = "Age"
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Gender",
                HeaderText = "Gender",
                DataPropertyName = "Gender"
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Address",
                HeaderText = "Address",
                DataPropertyName = "Address"
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "DepartmentID",
                HeaderText = "DepartmentID",
                DataPropertyName = "DepartmentID"
            });
        }

        private void FormStudentInfo_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterParent;

            LoadDepartments();
        }

        private void cmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            Display();
        }
    }
}
