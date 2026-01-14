using System;
using System.Windows.Forms;

namespace StudentsManagementSystem
{
    public partial class FormDepartmentManagement : Form
    {
        public FormDepartmentManagement()
        {
            InitializeComponent();

            SetupGrid();
            DisplayDepartments();

            ThemeManager.ApplyTheme(this);  // تطبيق الثيم على الواجهة

            if (UserSession.CurrentUser.Role != "Admin")
            {
                btnAdd.Enabled = false;
                dataGridView.Columns["Delete"].Visible = false;
            }
                

        }

        private void SetupGrid()
        {
            dataGridView.Columns.Clear();
            dataGridView.AutoGenerateColumns = false;

            // زر تعديل
            DataGridViewButtonColumn edit = new DataGridViewButtonColumn();
            edit.Name = "Edit";
            edit.Text = "Edit";
            edit.UseColumnTextForButtonValue = true;
            dataGridView.Columns.Add(edit);

            // زر حذف
            DataGridViewButtonColumn delete = new DataGridViewButtonColumn();
            delete.Name = "Delete";
            delete.Text = "Delete";
            delete.UseColumnTextForButtonValue = true;
            dataGridView.Columns.Add(delete);

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "DepartmentID",
                DataPropertyName = "DepartmentID",
                HeaderText = "ID"
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "DepartmentName",
                DataPropertyName = "DepartmentName",
                HeaderText = "Department Name"
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Description",
                DataPropertyName = "Description",
                HeaderText = "Description"
            });
        }

        private void DisplayDepartments()
        {
            DbStudent.DisplayDepartments(dataGridView);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDepartmentName.Text))
            {
                MessageBox.Show("Please enter department name");
                return;
            }

            if (string.IsNullOrEmpty(txtDepartmentID.Text))
            {
                DbStudent.AddDepartment(
                    txtDepartmentName.Text.Trim(),
                    txtDescription.Text.Trim());
            }
            else
            {
                DbStudent.UpdateDepartment(
                    txtDepartmentID.Text,
                    txtDepartmentName.Text.Trim(),
                    txtDescription.Text.Trim());
            }

            Clear();
            DisplayDepartments();
        }

        private void dataGridViewDepartments_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dataGridView.Rows[e.RowIndex];

            if (dataGridView.Columns[e.ColumnIndex].Name == "Edit")
            {
                txtDepartmentID.Text = row.Cells["DepartmentID"].Value.ToString();
                txtDepartmentName.Text = row.Cells["DepartmentName"].Value.ToString();
                txtDescription.Text = row.Cells["Description"].Value.ToString();
                btnAdd.Text = "Update";
            }
            else if (dataGridView.Columns[e.ColumnIndex].Name == "Delete")
            {
                if (MessageBox.Show("Delete this department?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    DbStudent.DeleteDepartment(row.Cells["DepartmentID"].Value.ToString());
                    DisplayDepartments();
                }
            }
        }

        private void Clear()
        {
            txtDepartmentID.Text = "";
            txtDepartmentName.Text = "";
            txtDescription.Text = "";
            btnAdd.Text = "Add";
        }

        private void FormDepartmentManagement_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            showResultSearch();
        }

        private void showResultSearch()
        {
            string search = textBox4.Text.Trim();

            string query = @"
              SELECT 
              DepartmentID,
              DepartmentName,
              Description
         FROM Departments
         WHERE DepartmentName LIKE '%" + search + "%' " +
         "OR Description LIKE '%" + search + "%' ";

            DbStudent.DisplayAndSearch(query, dataGridView);
        }
    }
}
