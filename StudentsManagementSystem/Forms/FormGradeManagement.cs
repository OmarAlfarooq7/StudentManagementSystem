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
    public partial class FormGradeManagement : Form
    {
        public FormGradeManagement()
        {
            InitializeComponent();
            SetupDataGridView();
            DisplayGrades();
            LoadStudents();
            LoadCourses();

            ThemeManager.ApplyTheme(this);  // تطبيق الثيم على الواجهة

            if (UserSession.CurrentUser.Role != "Admin")
            { 
                btnAdd.Enabled = false;
                dataGridView.Columns["Delete"].Visible = false;
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

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "GradeID", HeaderText = "ID", DataPropertyName = "GradeID" });
           
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "StudentID",
                DataPropertyName = "StudentID",
                Visible = false
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "CourseID",
                DataPropertyName = "CourseID",
                Visible = false
            });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "StudentName", HeaderText = "Student", DataPropertyName = "StudentName" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "CourseName", HeaderText = "Course", DataPropertyName = "CourseName" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Grade", HeaderText = "Grade", DataPropertyName = "Grade" });
        }

        private void DisplayGrades()
        {
            DbStudent.DisplayGrades(dataGridView);
        }

        private void LoadStudents()
        {
            // تحميل الطلاب في cmbStudent
            var dt = DbStudent.GetStudents(); // دالة ترجع DataTable
            cmbStudent.DataSource = dt;
            cmbStudent.DisplayMember = "FullName";
            cmbStudent.ValueMember = "StudentID";
        }

        private void LoadCourses()
        {
            // تحميل الكورسات في cmbCourse
            var dt = DbStudent.GetCourses(); 
            cmbCourse.DataSource = dt;
            cmbCourse.DisplayMember = "CourseName";
            cmbCourse.ValueMember = "CourseID";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //  التحقق من اختيار الطالب 
            if (cmbStudent.SelectedValue == null)
            {
                MessageBox.Show("Please select a student", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //  التحقق من اختيار الكورس 
            if (cmbCourse.SelectedValue == null)
            {
                MessageBox.Show("Please select a course", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //  التحقق من إدخال الدرجة 
            if (string.IsNullOrWhiteSpace(txtGrade.Text))
            {
                MessageBox.Show("Please enter grade value", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGrade.Focus();
                return;
            }

            //  التحقق من أن الدرجة رقم 
            if (!decimal.TryParse(txtGrade.Text.Trim(), out decimal gradeValue))
            {
                MessageBox.Show("Grade must be a numeric value", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtGrade.Focus();
                return;
            }

            //  التحقق من مجال الدرجة) 
            if (gradeValue < 0 || gradeValue > 100)
            {
                MessageBox.Show("Grade must be between 0 and 100", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGrade.Focus();
                return;
            }

           
            GradeClas grade = new GradeClas
            {
                GradeID = txtGradeID.Text,
                StudentID = cmbStudent.SelectedValue?.ToString() ?? string.Empty,
                CourseID = cmbCourse.SelectedValue?.ToString() ?? string.Empty,
                Grade = gradeValue
            };

            //  إضافة أو تعديل 
            if (string.IsNullOrEmpty(txtGradeID.Text))
            {
                DbStudent.AddGrade(grade);
                MessageBox.Show("Grade added successfully", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DbStudent.UpdateGrade(grade);
                MessageBox.Show("Grade updated successfully", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            ClearFields();
            DisplayGrades();
        }

        private void FormGradeManagement_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dataGridView.Rows[e.RowIndex];

            if (dataGridView.Columns[e.ColumnIndex].Name == "Edit")
            {
                txtGradeID.Text = row.Cells["GradeID"].Value.ToString();

                // تعيين الـ ComboBox حسب الـ ID
                cmbStudent.SelectedValue = row.Cells["StudentID"].Value;
                cmbCourse.SelectedValue = row.Cells["CourseID"].Value;
                txtGrade.Text = row.Cells["Grade"].Value.ToString();
                btnAdd.Text = "Update";

            }

            // ====== حذف ======
            else if (dataGridView.Columns[e.ColumnIndex].Name == "Delete")
            {
                if (row.Cells["GradeID"].Value == null) 
                    return;

                string? id = row.Cells["GradeID"].Value.ToString();

                if (MessageBox.Show("Delete this grade?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    if (string.IsNullOrEmpty(id))
                    {
                        MessageBox.Show("Please select a record first");
                        return;
                    }
                    DbStudent.DeleteGrade(id);
                    DisplayGrades();
                }
            }
        }

        private void ClearFields()
        {
            txtGradeID.Text = "";
            txtGrade.Text = "";
            cmbStudent.SelectedIndex = 0;
            cmbCourse.SelectedIndex = 0;
            btnAdd.Text = "Add";
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            string search = textBox3.Text.Trim();

            string query = @"SELECT 
                 g.GradeID,
                 g.StudentID,
                 g.CourseID,
                 (s.FirstName + ' ' + s.LastName) AS StudentName,
                 c.CourseName,
                 g.Grade
             FROM Grades g
             JOIN Students s ON g.StudentID = s.StudentID
             JOIN Courses c ON g.CourseID = c.CourseID
             WHERE 
               s.FirstName LIKE '%" + search + "%' " +
               "OR s.LastName LIKE '%" + search + "%' " +
               "OR c.CourseName LIKE '%" + search + "%' ";
            DbStudent.DisplayAndSearch(query, dataGridView);

        }
    }

}
