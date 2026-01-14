using System;
using System.Data;
using System.Windows.Forms;

namespace StudentsManagementSystem
{
    public partial class FormCourseManagement : Form
    {
        FormCourse form; // نافذة إضافة/تعديل كورس
        private bool isProcessing = false;

        public FormCourseManagement()
        {
            InitializeComponent();
            form = new FormCourse();

            ThemeManager.ApplyTheme(this);  // تطبيق الثيم على الواجهة

            if (UserSession.CurrentUser.Role != "Admin")
                btnNewCourse.Enabled = false;
        }

        // تحميل الفورم
        private void CoursesForm_Shown(object sender, EventArgs e)
        {
            SetupDataGridView();
            Display();
        }


        // تحميل الأقسام
        private void LoadDepartments()
        {
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

        // عرض الكورسات
        public void Display()
        {
            string query = "SELECT CourseID, CourseName, CreditHours, Description, DepartmentID FROM Courses";

            if (cmbDepartment.SelectedValue != null &&
               int.TryParse(cmbDepartment.SelectedValue.ToString(), out int deptId) && deptId != 0)
               {
                   query += " WHERE DepartmentID = " + deptId;
               }

            DbStudent.DisplayAndSearch(query, dataGridView);

            if (dataGridView.Columns["DepartmentID"] != null)
                dataGridView.Columns["DepartmentID"].Visible = false;
        }


        // إعداد DataGridView
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
                Name = "CourseID",
                HeaderText = "ID",
                DataPropertyName = "CourseID"
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "CourseName",
                HeaderText = "Course Name",
                DataPropertyName = "CourseName"
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "CreditHours",
                HeaderText = "Credit Hours",
                DataPropertyName = "CreditHours"
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Description",
                HeaderText = "Description",
                DataPropertyName = "Description"
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "DepartmentID",
                DataPropertyName = "DepartmentID",
                Visible = false
            });

        }

        // إضافة كورس جديد
        private void btnNewCourse_Click(object sender, EventArgs e)
        {
            form.AddInfo();
            form.ShowDialog();
        }

        // تعديل / حذف
        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (isProcessing) return; //  منع التكرار

            isProcessing = true;

            try
            {
                DataGridViewRow row = dataGridView.Rows[e.RowIndex];

                if (dataGridView.Columns[e.ColumnIndex].Name == "Edit")
                {
                    form.Clear();
                    form.CourseID = row.Cells["CourseID"].Value.ToString();
                    form.CourseName = row.Cells["CourseName"].Value.ToString();
                    form.CreditHours = Convert.ToInt32(row.Cells["CreditHours"].Value);
                    form.Description = row.Cells["Description"].Value.ToString();
                    form.Department = row.Cells["DepartmentID"].Value.ToString();

                    // تحديد القسم في الكومبو عند فتح الفورم
                    if (row.Cells["DepartmentID"].Value != DBNull.Value)
                        cmbDepartment.SelectedValue = row.Cells["DepartmentID"].Value;

                    form.UpdateInfo();
                    form.ShowDialog();
                    Display();
                }
                else if (dataGridView.Columns[e.ColumnIndex].Name == "Delete")
                {
                    string id = row.Cells["CourseID"].Value.ToString();

                    if (MessageBox.Show("Delete this course?", "Confirm",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        DbStudent.DeleteCourse(id);
                        Display();
                    }
                }
            }
            finally
            {
                isProcessing = false;
            }
        }

        // البحث
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string search = textBox2.Text.Trim();
            string query = "SELECT CourseID, CourseName, CreditHours, Description, DepartmentID FROM Courses " +
                           "WHERE CourseName LIKE '%" + search + "%' " +
                           "OR Description LIKE '%" + search + "%'";
            DbStudent.DisplayAndSearch(query, dataGridView);
        }

        private void btnNewCourse_Click_1(object sender, EventArgs e)
        {
            form.AddInfo();
            form.ShowDialog();
        }

        private void FormCourseManagement_Load(object sender, EventArgs e)
        {
            LoadDepartments();
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void cmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            Display();
        }
    }
}
