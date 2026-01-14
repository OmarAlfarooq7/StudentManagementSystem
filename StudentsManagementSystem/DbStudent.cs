using Microsoft.Data.SqlClient;
using System.Data;

namespace StudentsManagementSystem
{
    internal class DbStudent
    {
        private static string connectionString =
            @"Data Source=.\SQLEXPRESS;
              Initial Catalog=StudentDB;
              Integrated Security=True;
              TrustServerCertificate=True";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }


        // ===================== الطلاب =====================

        public static void AddStudent(Student s)
        {
            string sql =
                "INSERT INTO Students (FirstName, LastName, Age, Gender, Address, DepartmentID) " +
                "VALUES (@FirstName, @LastName, @Age, @Gender, @Address, @DepartmentID)";

            using (SqlConnection con = GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                con.Open();
                cmd.Parameters.AddWithValue("@FirstName", s.FirstName);
                cmd.Parameters.AddWithValue("@LastName", s.LastName);
                cmd.Parameters.AddWithValue("@Age", s.Age);
                cmd.Parameters.AddWithValue("@Gender", s.Gender);
                cmd.Parameters.AddWithValue("@Address", s.Address);
                cmd.Parameters.AddWithValue("@DepartmentID", s.Department);

                cmd.ExecuteNonQuery();
            }
        }

        public static void UpdateStudent(Student s, string id)
        {
            string sql =
                "UPDATE Students SET FirstName=@FirstName, LastName=@LastName, " +
                "Age=@Age, Gender=@Gender, Address=@Address WHERE StudentID=@StudentID";

            using (SqlConnection con = GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                con.Open();

                cmd.Parameters.AddWithValue("@StudentID", id);
                cmd.Parameters.AddWithValue("@FirstName", s.FirstName);
                cmd.Parameters.AddWithValue("@LastName", s.LastName);
                cmd.Parameters.AddWithValue("@Age", s.Age);
                cmd.Parameters.AddWithValue("@Gender", s.Gender);
                cmd.Parameters.AddWithValue("@Address", s.Address);

                cmd.ExecuteNonQuery();
            }
        }

        public static void DeleteStudent(string id)
        {
            string sql = "DELETE FROM Students WHERE StudentID=@StudentID";

            using (SqlConnection con = GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                con.Open();

                cmd.Parameters.AddWithValue("@StudentID", id);
                cmd.ExecuteNonQuery();
            }
        }

        public static void DisplayAndSearch(string query, DataGridView dgv)
        {
            using (SqlConnection con = GetConnection())
            using (SqlDataAdapter adapter = new SqlDataAdapter(query, con))
            {
                DataTable table = new DataTable();
                adapter.Fill(table);
                dgv.DataSource = table;
            }
        }

        public static DataTable GetStudents()
        {
            using (SqlConnection con = GetConnection())
            {
                string query = @"
            SELECT 
                StudentID,
                (FirstName + ' ' + LastName) AS FullName
            FROM Students
        ";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }


        // ===================== احصائيات الطلاب الصفحة الرئيسية =====================
        public static int CountStudents()
        {
            using (SqlConnection con = GetConnection())
            using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Students", con))
            {
                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public static int CountMale()
        {
            using (SqlConnection con = GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                "SELECT COUNT(*) FROM Students WHERE Gender='Male'", con))
            {
                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public static int CountFemale()
        {
            using (SqlConnection con = GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                "SELECT COUNT(*) FROM Students WHERE Gender='Female'", con))
            {
                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public static double AverageAge()
        {
            using (SqlConnection con = GetConnection())
            using (SqlCommand cmd = new SqlCommand("SELECT AVG(Age) FROM Students", con))
            {
                con.Open();
                object result = cmd.ExecuteScalar();
                return result == DBNull.Value ? 0 : Convert.ToDouble(result);
            }
        }



        // ===================== المستخدمون =====================

        // إضافة مستخدم
        public static void AddUser(User u)
        {
            string sql =
                "INSERT INTO Users (Username, Password, Role) " +
                "VALUES (@Username, @Password, @Role)";

            using (SqlConnection con = GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                con.Open();

                cmd.Parameters.AddWithValue("@Username", u.Username);
                cmd.Parameters.AddWithValue("@Password", Security.HashPassword(u.Password));
                cmd.Parameters.AddWithValue("@Role", u.Role);
                cmd.ExecuteNonQuery();
            }
        }

        // تعديل مستخدم
        public static void UpdateUser(User u, string id)
        {
            string sql;

            if (string.IsNullOrWhiteSpace(u.Password))
            {
                // بدون تغيير الباسورد
                sql = "UPDATE Users SET Username=@Username, Role=@Role WHERE UserID=@UserID";
            }
            else
            {
                // مع تغيير الباسورد
                sql = "UPDATE Users SET Username=@Username, Password=@Password, Role=@Role WHERE UserID=@UserID";
            }

            using (SqlConnection con = GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                con.Open();

                cmd.Parameters.AddWithValue("@UserID", id);
                cmd.Parameters.AddWithValue("@Username", u.Username);
                cmd.Parameters.AddWithValue("@Role", u.Role);

                if (!string.IsNullOrWhiteSpace(u.Password))
                    cmd.Parameters.AddWithValue("@Password", Security.HashPassword(u.Password));

                cmd.ExecuteNonQuery();
            }
        }


        // حذف مستخدم
        public static void DeleteUser(string id)
        {
            string sql = "DELETE FROM Users WHERE UserID=@UserID";

            using (SqlConnection con = GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                con.Open();

                cmd.Parameters.AddWithValue("@UserID", id);
                cmd.ExecuteNonQuery();
            }
        }


        // ===================== تسجيل الدخول =====================
        public static User Login(string username, string password)
        {
            string sql =
                "SELECT UserID, Username, Role FROM Users " +
                "WHERE Username=@user AND Password=@pass";

            using (SqlConnection con = GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@user", username);
                cmd.Parameters.AddWithValue("@pass", Security.HashPassword(password));

                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return new User
                        {
                            UserID = dr["UserID"]?.ToString() ?? string.Empty,
                            Username = dr["Username"]?.ToString() ?? string.Empty,
                            Role = dr["Role"]?.ToString() ?? string.Empty
                        };
                    }
                }
            }
            return null;
        }



        // ===================== الكورسات =====================
        public static DataTable GetCourses()
        {
            using (SqlConnection con = GetConnection())
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT CourseID, CourseName FROM Courses", con))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public static void DisplayCourses(string query, DataGridView dgv)
        {

            DisplayAndSearch(query, dgv);
        }

        // إضافة كورس جديد
        public static void AddCourse(Course course)
        {
            using (var con = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Courses (CourseName, CreditHours, Description, DepartmentID) VALUES (@name, @hours, @desc, @Depart)";
                using (var cmd = new SqlCommand(query, con))
                {
                    con.Open();

                    cmd.Parameters.AddWithValue("@name", course.CourseName);
                    cmd.Parameters.AddWithValue("@hours", course.CreditHours);
                    cmd.Parameters.AddWithValue("@desc", course.Description);
                    cmd.Parameters.AddWithValue("@Depart", course.Department);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // تعديل كورس موجود
        public static void UpdateCourse(Course course, string courseID)
        {
            using (var con = new SqlConnection(connectionString))
            {
                string query = "UPDATE Courses SET CourseName=@name, CreditHours=@hours, Description=@desc, DepartmentID=@Depart WHERE CourseID=@id";
                using (var cmd = new SqlCommand(query, con))
                {
                    con.Open();

                    cmd.Parameters.AddWithValue("@name", course.CourseName);
                    cmd.Parameters.AddWithValue("@hours", course.CreditHours);
                    cmd.Parameters.AddWithValue("@desc", course.Description);
                    cmd.Parameters.AddWithValue("@id", courseID);
                    cmd.Parameters.AddWithValue("@Depart", course.Department);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public static void DeleteCourse(string courseID)
        {
            using (SqlConnection con = GetConnection())
            using (SqlCommand cmd = new SqlCommand("DELETE FROM Courses WHERE CourseID=@CourseID", con))
            {
                con.Open();

                cmd.Parameters.AddWithValue("@CourseID", courseID);
                cmd.ExecuteNonQuery();
            }
        }




        // ===================== الدرجات =====================
        public static void DisplayGrades(DataGridView dgv)
        {
            string query = @"
        SELECT 
            g.GradeID,
            g.StudentID,
            g.CourseID,
            (s.FirstName + ' ' + s.LastName) AS StudentName,
            c.CourseName,
            g.Grade
        FROM Grades g
        JOIN Students s ON g.StudentID = s.StudentID
        JOIN Courses c ON g.CourseID = c.CourseID";

            DisplayAndSearch(query, dgv);
        }



        public static void AddGrade(GradeClas grade)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "INSERT INTO Grades (StudentID, CourseID, Grade, GradeDate) VALUES (@StudentID, @CourseID, @Grade, @GradeDate)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@StudentID", grade.StudentID);
                    cmd.Parameters.AddWithValue("@CourseID", grade.CourseID);
                    cmd.Parameters.AddWithValue("@Grade", grade.Grade);
                    cmd.Parameters.AddWithValue("@GradeDate", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateGrade(GradeClas grade)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "UPDATE Grades SET StudentID=@StudentID, CourseID=@CourseID, Grade=@Grade WHERE GradeID=@GradeID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@GradeID", grade.GradeID);
                    cmd.Parameters.AddWithValue("@StudentID", grade.StudentID);
                    cmd.Parameters.AddWithValue("@CourseID", grade.CourseID);
                    cmd.Parameters.AddWithValue("@Grade", grade.Grade);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public static void DeleteGrade(string id)
        {
            using (SqlConnection con = GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                "DELETE FROM Grades WHERE GradeID=@id", con))
            {
                con.Open();
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }



        // ===================== الأقسام =====================

        // جلب الأقسام
        public static DataTable GetDepartments()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlDataAdapter da = new SqlDataAdapter(
                "SELECT DepartmentID, DepartmentName, Description FROM Departments", con))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        // عرض الأقسام
        public static void DisplayDepartments(DataGridView dgv)
        {
            string query = "SELECT DepartmentID, DepartmentName, Description FROM Departments";
            DisplayAndSearch(query, dgv);
        }

        // إضافة قسم
        public static void AddDepartment(string name, string description)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Departments (DepartmentName, Description) VALUES (@name, @desc)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@desc", description);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // تعديل قسم
        public static void UpdateDepartment(string id, string name, string description)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE Departments SET DepartmentName=@name, Description=@desc WHERE DepartmentID=@id";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@desc", description);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // حذف قسم
        public static void DeleteDepartment(string id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "DELETE FROM Departments WHERE DepartmentID=@id", con))
            {
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // جلب الاقسام
        public static DataTable GetDepartmentsForCombo()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlDataAdapter da = new SqlDataAdapter(
                "SELECT DepartmentID, DepartmentName FROM Departments", con))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }


        // ===================== التقارير =====================
        /*public static DataTable GetStudentStatsByGender()
        {
            string sql = "SELECT Gender, COUNT(*) AS Count FROM Students GROUP BY Gender";
            using (SqlConnection con = GetConnection())
            using (SqlDataAdapter adapter = new SqlDataAdapter(sql, con))
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public static DataTable GetUserStatsByRole()
        {
            string sql = "SELECT Role, COUNT(*) AS Count FROM Users GROUP BY Role";
            using (SqlConnection con = GetConnection())
            using (SqlDataAdapter adapter = new SqlDataAdapter(sql, con))
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }*/



        public static DataTable GetStudentsForCombo()
        {
            using var conn = new SqlConnection(connectionString);
            using var cmd = new SqlCommand("SELECT StudentID, FirstName + ' ' + LastName AS FullName FROM Students", conn);
            using var da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public static DataTable GetCoursesForCombo()
        {
            using var conn = new SqlConnection(connectionString);
            using var cmd = new SqlCommand("SELECT CourseID, CourseName FROM Courses", conn);
            using var da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }


        public static DataTable GetStudentsReport(int? departmentId, string search)
        {
            using var conn = new SqlConnection(connectionString);
            string sql = "SELECT StudentID, FirstName, LastName, Age, Gender, Address, DepartmentID FROM Students WHERE 1=1";

            if (departmentId.HasValue)
                sql += " AND DepartmentID = @deptId";

            if (!string.IsNullOrEmpty(search))
                sql += " AND (FirstName LIKE @search OR LastName LIKE @search OR Gender LIKE @search OR Address LIKE @search)";

            using var cmd = new SqlCommand(sql, conn);

            if (departmentId.HasValue)
                cmd.Parameters.AddWithValue("@deptId", departmentId.Value);

            if (!string.IsNullOrEmpty(search))
                cmd.Parameters.AddWithValue("@search", $"%{search}%");

            using var da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public static DataTable GetCoursesReport(int? departmentId, string search)
        {
            using var conn = new SqlConnection(connectionString);
            string sql = "SELECT CourseID, CourseName, CreditHours, Description, DepartmentID FROM Courses WHERE 1=1";

            if (departmentId.HasValue)
                sql += " AND DepartmentID = @deptId";

            if (!string.IsNullOrEmpty(search))
                sql += " AND (CourseName LIKE @search OR Description LIKE @search)";

            using var cmd = new SqlCommand(sql, conn);

            if (departmentId.HasValue)
                cmd.Parameters.AddWithValue("@deptId", departmentId.Value);

            if (!string.IsNullOrEmpty(search))
                cmd.Parameters.AddWithValue("@search", $"%{search}%");

            using var da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public static DataTable GetGradesReport(int? studentId, int? courseId, string search)
        {
            using var conn = new SqlConnection(connectionString);

            string sql = @"SELECT g.GradeID, (s.FirstName + ' ' + s.LastName) AS StudentName, 
                           c.CourseName, g.Grade
                           FROM Grades g
                           JOIN Students s ON g.StudentID = s.StudentID
                           JOIN Courses c ON g.CourseID = c.CourseID
                           WHERE 1=1";

            if (studentId.HasValue)
                sql += " AND s.StudentID = @studentId";

            if (courseId.HasValue)
                sql += " AND c.CourseID = @courseId";

            if (!string.IsNullOrEmpty(search))
                sql += " AND ((s.FirstName + ' ' + s.LastName) LIKE @search OR c.CourseName LIKE @search)";

            using var cmd = new SqlCommand(sql, conn);

            if (studentId.HasValue)
                cmd.Parameters.AddWithValue("@studentId", studentId.Value);

            if (courseId.HasValue)
                cmd.Parameters.AddWithValue("@courseId", courseId.Value);

            if (!string.IsNullOrEmpty(search))
                cmd.Parameters.AddWithValue("@search", $"%{search}%");

            using var da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }



    }
}
