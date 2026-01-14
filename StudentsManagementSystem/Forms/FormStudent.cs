using System;
using System.Windows.Forms;

namespace StudentsManagementSystem
{
    public partial class FormStudent : Form
    {
        private readonly FormStudentInfo _parent;

        // متغيرات لاستقبال البيانات عند التعديل
        public string StudentID;
        public string FirstName;
        public string LastName;
        public int Age;
        public string Gender;
        public string Address;
        public string Department;

        public FormStudent(FormStudentInfo parent)
        {
            InitializeComponent();
            _parent = parent;

            ThemeManager.ApplyTheme(this);  // تطبيق الثيم على الواجهة

        }

        private void LoadDepartments()
        {
            cmbDepartment.DataSource = DbStudent.GetDepartmentsForCombo();
            cmbDepartment.DisplayMember = "DepartmentName";
            cmbDepartment.ValueMember = "DepartmentID";
            cmbDepartment.SelectedIndex = -1;
        }
        // عند فتح الفورم للتعديل
        public void UpdateInfo()
        {
            lbltext.Text = "Update Student Data";
            btnAddUser.Text = "Update";

            txtFirstName.Text = FirstName;
            textLastName.Text = LastName;
            numAge.Value = Age;
            cmbGender.SelectedItem = Gender;
            textAddress.Text = Address;
            cmbDepartment.SelectedItem = Department;
        }

        // عند فتح الفورم للإضافة
        public void AddInfo()
        {
            lbltext.Text = "Add Student";
            btnAddUser.Text = "Add";
            Clear();
        }

        // تنظيف الحقول
        public void Clear()
        {
            txtFirstName.Text = "";
            textLastName.Text = "";
            textAddress.Text = "";
            numAge.Value = 18;
            cmbGender.SelectedIndex = -1;
            cmbDepartment.SelectedIndex = -1;
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            // التحقق من الإدخال
            if (txtFirstName.Text.Trim().Length < 2 ||
                textLastName.Text.Trim().Length < 2 ||
                cmbGender.SelectedIndex == -1 ||
                textAddress.Text.Trim().Length < 3 ||
                cmbDepartment.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill all required fields", "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // إنشاء كائن الطالب
            Student std = new Student(
                txtFirstName.Text.Trim(),
                textLastName.Text.Trim(),
                (int)numAge.Value,
                cmbGender.SelectedItem.ToString(),
                textAddress.Text.Trim(),
                cmbDepartment.SelectedValue.ToString()
            );

            // إضافة
            if (btnAddUser.Text == "Add")
            {
                DbStudent.AddStudent(std);
            }
            // تعديل
            else if (btnAddUser.Text == "Update")
            {
                DbStudent.UpdateStudent(std, StudentID);
            }

            _parent.Display();
            this.Close();
        }

        private void FormStudent_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterParent;

            LoadDepartments();
        }
    }
}
