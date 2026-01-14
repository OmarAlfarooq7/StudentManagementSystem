using System;
using System.Data;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace StudentsManagementSystem
{
    public partial class FormReportManagement : Form
    {
        public FormReportManagement()
        {
            InitializeComponent();
            LoadDepartments();
            LoadStudents();
            LoadCourses();

            ThemeManager.ApplyTheme(this);  // تطبيق الثيم على الواجهة
        }

        private void FormReportManagement_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterParent;

            cmbReportType.Items.Clear();
            cmbReportType.Items.Add("Students Report");
            cmbReportType.Items.Add("Courses Report");
            cmbReportType.Items.Add("Grades Report");

            cmbReportType.SelectedIndex = 0;

            UpdateComboVisibility();
            DisplayReport();
        }

        //Load Combos
        private void LoadDepartments()
        {
            DataTable dt = DbStudent.GetDepartmentsForCombo();

            DataRow allRow = dt.NewRow();
            allRow["DepartmentID"] = DBNull.Value;
            allRow["DepartmentName"] = "All Departments";
            dt.Rows.InsertAt(allRow, 0);

            cmbDepartment.DataSource = dt;
            cmbDepartment.DisplayMember = "DepartmentName";
            cmbDepartment.ValueMember = "DepartmentID";
            cmbDepartment.SelectedIndex = 0;

            cmbDepartment.SelectedIndexChanged += (s, e) => DisplayReport();
        }

        private void LoadStudents()
        {
            DataTable dt = DbStudent.GetStudentsForCombo();

            DataRow allRow = dt.NewRow();
            allRow["StudentID"] = DBNull.Value;
            allRow["FullName"] = "All Students";
            dt.Rows.InsertAt(allRow, 0);

            cmbStudent.DataSource = dt;
            cmbStudent.DisplayMember = "FullName";
            cmbStudent.ValueMember = "StudentID";
            cmbStudent.SelectedIndex = 0;

            cmbStudent.SelectedIndexChanged += (s, e) => DisplayReport();
        }

        private void LoadCourses()
        {
            DataTable dt = DbStudent.GetCoursesForCombo();

            DataRow allRow = dt.NewRow();
            allRow["CourseID"] = DBNull.Value;
            allRow["CourseName"] = "All Courses";
            dt.Rows.InsertAt(allRow, 0);

            cmbCourse.DataSource = dt;
            cmbCourse.DisplayMember = "CourseName";
            cmbCourse.ValueMember = "CourseID";
            cmbCourse.SelectedIndex = 0;

            cmbCourse.SelectedIndexChanged += (s, e) => DisplayReport();
        }


        private void cmbReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateComboVisibility();
            DisplayReport();
        }

        private void UpdateComboVisibility()
        {
            bool isGrades = cmbReportType.SelectedItem?.ToString() == "Grades Report";

            cmbDepartment.Enabled = !isGrades;

            cmbStudent.Enabled = isGrades;
            cmbCourse.Enabled = isGrades;

            cmbStudent.Visible = true;
            cmbCourse.Visible = true;
            cmbDepartment.Visible = true;
        }

        // Search

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            DisplayReport();
        }

        // Generate
        private void btnExportPdf_Click(object sender, EventArgs e)
        {
            if (dgvReport.Rows.Count == 0)
            {
                MessageBox.Show("No data to export", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string defaultFileName = cmbReportType.SelectedItem.ToString();

            if (cmbReportType.SelectedItem.ToString() == "Grades Report" &&
                cmbStudent.SelectedValue != null && cmbStudent.SelectedValue != DBNull.Value)
            {
                string studentName = cmbStudent.Text.Replace(" ", "_");
                defaultFileName = $"Grades_{studentName}";
            }

            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "PDF files (*.pdf)|*.pdf";
            saveFile.FileName = defaultFileName + ".pdf";

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ExportToPdf(dgvReport, saveFile.FileName);
                    MessageBox.Show("PDF saved successfully", "Report Generated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving PDF: {ex.Message}",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void ExportToPdf(DataGridView dgv, string filePath)
        {
            Document document = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
            PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
            document.Open();

            // عنوان التقرير
            Paragraph title = new Paragraph(
                cmbReportType.SelectedItem.ToString(),
                FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16)
            );
            title.Alignment = Element.ALIGN_CENTER;
            title.SpacingAfter = 15;
            document.Add(title);

            // التاريخ
            document.Add(new Paragraph("Date: " + DateTime.Now.ToString("yyyy-MM-dd")));
            document.Add(new Paragraph(" "));

            // إنشاء الجدول
            PdfPTable table = new PdfPTable(dgv.Columns.Count);
            table.WidthPercentage = 100;

            // رؤوس الأعمدة
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (col.Visible) // حفظ الأعمدة المرئية فقط
                {
                    PdfPCell cell = new PdfPCell(new Phrase(col.HeaderText));
                    cell.BackgroundColor = BaseColor.LightGray;
                    table.AddCell(cell);
                }
            }

            // البيانات
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;

                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Visible) table.AddCell(cell.Value?.ToString() ?? "");
                }
            }

            document.Add(table);
            document.Close();
        }

        private void DisplayReport()
        {
            if (cmbReportType.SelectedItem == null)
                return;

            string? reportType = cmbReportType.SelectedItem.ToString();
            string search = txtSearch.Text.Trim();

            DataTable? dt = null;

            if (reportType == "Students Report")
            {
                int? deptId = cmbDepartment.SelectedValue == DBNull.Value
                    ? null
                    : (int?)cmbDepartment.SelectedValue;

                dt = DbStudent.GetStudentsReport(deptId, search);
            }
            else if (reportType == "Courses Report")
            {
                int? deptId = cmbDepartment.SelectedValue == DBNull.Value
                    ? null
                    : (int?)cmbDepartment.SelectedValue;

                dt = DbStudent.GetCoursesReport(deptId, search);
            }
            else if (reportType == "Grades Report")
            {
                int? studentId = cmbStudent.SelectedValue == DBNull.Value
                    ? null
                    : (int?)cmbStudent.SelectedValue;

                int? courseId = cmbCourse.SelectedValue == DBNull.Value
                    ? null
                    : (int?)cmbCourse.SelectedValue;

                dt = DbStudent.GetGradesReport(studentId, courseId, search);
            }

            dgvReport.DataSource = dt;
            lblSummary.Text = $"Total Records: {dt?.Rows.Count ?? 0}";
        }
    }
}
