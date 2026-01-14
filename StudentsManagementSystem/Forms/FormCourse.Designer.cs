namespace StudentsManagementSystem
{
    partial class FormCourse
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cmbDepartment = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCreditHours = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAddCourse = new System.Windows.Forms.Button();
            this.txtCourseName = new System.Windows.Forms.TextBox();
            this.lblPriezvisko = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Highlight;
            this.panel1.Controls.Add(this.lblTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(447, 100);
            this.panel1.TabIndex = 3;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(32, 32);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(133, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Add Course";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cmbDepartment);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.txtDescription);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.txtCreditHours);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.btnAddCourse);
            this.panel2.Controls.Add(this.txtCourseName);
            this.panel2.Controls.Add(this.lblPriezvisko);
            this.panel2.Location = new System.Drawing.Point(32, 116);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(365, 223);
            this.panel2.TabIndex = 2;
            // 
            // cmbDepartment
            // 
            this.cmbDepartment.Items.AddRange(new object[] {
            "Male",
            "Female"});
            this.cmbDepartment.Location = new System.Drawing.Point(26, 102);
            this.cmbDepartment.Name = "cmbDepartment";
            this.cmbDepartment.Size = new System.Drawing.Size(121, 23);
            this.cmbDepartment.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(26, 82);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 15);
            this.label5.TabIndex = 15;
            this.label5.Text = "Department";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(26, 151);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.PlaceholderText = "Description";
            this.txtDescription.Size = new System.Drawing.Size(141, 23);
            this.txtDescription.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 131);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 12;
            this.label2.Text = "Description";
            // 
            // txtCreditHours
            // 
            this.txtCreditHours.Location = new System.Drawing.Point(204, 45);
            this.txtCreditHours.Name = "txtCreditHours";
            this.txtCreditHours.PlaceholderText = "Credit Hours";
            this.txtCreditHours.Size = new System.Drawing.Size(141, 23);
            this.txtCreditHours.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(204, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "Credit Hours";
            // 
            // btnAddCourse
            // 
            this.btnAddCourse.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnAddCourse.ForeColor = System.Drawing.Color.White;
            this.btnAddCourse.Location = new System.Drawing.Point(150, 181);
            this.btnAddCourse.Name = "btnAddCourse";
            this.btnAddCourse.Size = new System.Drawing.Size(75, 23);
            this.btnAddCourse.TabIndex = 8;
            this.btnAddCourse.Text = "Add";
            this.btnAddCourse.UseVisualStyleBackColor = false;
            this.btnAddCourse.Click += new System.EventHandler(this.btnAddCourse_Click);
            // 
            // txtCourseName
            // 
            this.txtCourseName.Location = new System.Drawing.Point(26, 45);
            this.txtCourseName.Name = "txtCourseName";
            this.txtCourseName.PlaceholderText = "Course Name";
            this.txtCourseName.Size = new System.Drawing.Size(141, 23);
            this.txtCourseName.TabIndex = 9;
            // 
            // lblPriezvisko
            // 
            this.lblPriezvisko.AutoSize = true;
            this.lblPriezvisko.Location = new System.Drawing.Point(26, 25);
            this.lblPriezvisko.Name = "lblPriezvisko";
            this.lblPriezvisko.Size = new System.Drawing.Size(79, 15);
            this.lblPriezvisko.TabIndex = 10;
            this.lblPriezvisko.Text = "Course Name";
            // 
            // FormCourse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(447, 371);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "FormCourse";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormCourse";
            this.Load += new System.EventHandler(this.FormCourse_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private Label lblTitle;
        private Panel panel2;
        private TextBox txtDescription;
        private Label label2;
        private TextBox txtCreditHours;
        private Label label1;
        private Button btnAddCourse;
        private TextBox txtCourseName;
        private Label lblPriezvisko;
        private ComboBox cmbDepartment;
        private Label label5;
    }
}