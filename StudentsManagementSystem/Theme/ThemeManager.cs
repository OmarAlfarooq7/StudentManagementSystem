using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsManagementSystem
{
    public static class ThemeManager
    {
        public static Color PrimaryColor = Color.FromArgb(52, 152, 219); // أزرق هادئ
        public static Color SecondaryColor = Color.White;
        public static Color ButtonColor = Color.FromArgb(41, 128, 185);
        public static Color ButtonTextColor = Color.White;
        public static Color TextBoxColor = Color.White;
        public static Color TextColor = Color.Black;
        public static Font DefaultFont = new Font("Segoe UI", 10);

        // تطبيق الثيم على فورم كامل recursively
        public static void ApplyTheme(Form form)
        {
            form.BackColor = SecondaryColor;
            form.Font = DefaultFont;

            ApplyThemeToControls(form.Controls);
        }

        private static void ApplyThemeToControls(Control.ControlCollection controls)
        {
            foreach (Control ctrl in controls)
            {
                switch (ctrl)
                {
                    case Button btn:
                        btn.BackColor = ButtonColor;
                        btn.ForeColor = ButtonTextColor;
                        btn.FlatStyle = FlatStyle.Flat;
                        btn.FlatAppearance.BorderColor = ButtonColor;
                        break;

                    case TextBox tb:
                        tb.BackColor = TextBoxColor;
                        tb.ForeColor = TextColor;
                        break;

                    case ComboBox cb:
                        cb.BackColor = TextBoxColor;
                        cb.ForeColor = TextColor;
                        break;

                    case Label lbl:
                        lbl.ForeColor = TextColor;
                        break;

                    case DataGridView dgv:
                        dgv.BackgroundColor = SecondaryColor;
                        dgv.DefaultCellStyle.BackColor = SecondaryColor;
                        dgv.DefaultCellStyle.ForeColor = TextColor;
                        dgv.ColumnHeadersDefaultCellStyle.BackColor = PrimaryColor;
                        dgv.ColumnHeadersDefaultCellStyle.ForeColor = ButtonTextColor;
                        dgv.EnableHeadersVisualStyles = false;
                        break;
                }

                // تطبيق recursively على الـ Panel أو GroupBox
                if (ctrl.HasChildren)
                    ApplyThemeToControls(ctrl.Controls);
            }
        }
    }
}
