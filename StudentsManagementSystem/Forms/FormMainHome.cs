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
    public partial class FormMainHome : Form
    {
        public FormMainHome()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;

            ThemeManager.ApplyTheme(this);  // تطبيق الثيم على الواجهة
        }

        private void FormMainHome_Load(object sender, EventArgs e)
        {
            lblWelcomeUser.Text = $"Welcome : {UserSession.CurrentUser.Username} ({UserSession.CurrentUser.Role})";

            LoadStatistics();

            if (UserSession.CurrentUser.Role != "Admin")
                btnUsers.Enabled = false;

            cardStudents.Paint += (s, e) =>
            {
                ControlPaint.DrawBorder(
                    e.Graphics,
                    cardStudents.ClientRectangle,
                    Color.LightGray, ButtonBorderStyle.Solid
                );
            };


        }

        //تحميل الاحصائيات 
        private void LoadStatistics()
        {
            labelTotalStudent.Text = DbStudent.CountStudents().ToString();
            labelMaleStudent.Text = DbStudent.CountMale().ToString();
            labelFemaleStudent.Text = DbStudent.CountFemale().ToString();
            labelAvgAge.Text = DbStudent.AverageAge().ToString("0.0");
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            this.Hide();
            new FormLogin().Show();
        }

        private void btnStudent_Click(object sender, EventArgs e)
        {
            new FormStudentInfo().Show();
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            new FormUserManagement().Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new FormCourseManagement().Show();
        }

        private void btnGrade_Click(object sender, EventArgs e)
        {
            new FormGradeManagement().Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new FormDepartmentManagement().Show();
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            new FormReportManagement().Show(); 
        }

        private void FormMainHome_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void btnLogOut_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
