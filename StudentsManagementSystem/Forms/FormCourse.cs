using System;
using System.Windows.Forms;

namespace StudentsManagementSystem
{
    public partial class FormCourse : Form
    {
        private readonly FormCourseManagement _parent;

        public string CourseID;
        public string CourseName;
        public int CreditHours;
        public string Description;
        public string Department;


        public FormCourse()
        {
            InitializeComponent();

            ThemeManager.ApplyTheme(this);  // تطبيق الثيم على الواجهة
            
            if (UserSession.CurrentUser.Role != "Admin")
                btnAddCourse.Enabled = false;
        }

        private void LoadDepartments()
        {
            cmbDepartment.DataSource = DbStudent.GetDepartmentsForCombo();
            cmbDepartment.DisplayMember = "DepartmentName";
            cmbDepartment.ValueMember = "DepartmentID";
            cmbDepartment.SelectedIndex = -1;
        }

        // عند فتح الفورم للإضافة
        public void AddInfo()
        {
            lblTitle.Text = "Add Course";
            btnAddCourse.Text = "Add";
            Clear();
        }

        // عند فتح الفورم للتعديل
        public void UpdateInfo()
        {
            lblTitle.Text = "Update Course";
            btnAddCourse.Text = "Update";

            txtCourseName.Text = CourseName;
            txtCreditHours.Text = CreditHours.ToString();
            txtDescription.Text = Description;
            cmbDepartment.SelectedItem = Department;

        }

        // تفريغ الحقول
        public void Clear()
        {
            txtCourseName.Text = "";
            txtCreditHours.Text = "";
            txtDescription.Text = "";
            cmbDepartment.SelectedIndex = -1;
        }


        private void FormCourse_Load(object sender, EventArgs e)
        {
            LoadDepartments();
            this.StartPosition = FormStartPosition.CenterParent;
        }

        // اضافة كورس
        private void btnAddCourse_Click(object sender, EventArgs e)
        {
            if (txtCourseName.Text == "" || txtCreditHours.Text == "" || cmbDepartment.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill all required fields", "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtCreditHours.Text, out int hours))
            {
                MessageBox.Show("Credit Hours must be a number");
                return;
            }

            Course c = new Course
            {
                CourseName = txtCourseName.Text.Trim(),
                CreditHours = hours,
                Description = txtDescription.Text.Trim(),
                Department = cmbDepartment.SelectedValue.ToString()
            };

            if (btnAddCourse.Text == "Add")
                DbStudent.AddCourse(c);
            else
                DbStudent.UpdateCourse(c, CourseID);


            _parent.Display();
            Close();
        }
    }
}
