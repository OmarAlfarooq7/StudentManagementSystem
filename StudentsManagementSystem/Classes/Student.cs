using Microsoft.VisualBasic;
using MySql.Data.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsManagementSystem
{
    public class Student
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Department { get; set; }


        public Student(string firstName, string lastName, int age, string gender, string address, string department)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            Gender = gender;
            Address = address;
            Department = department;
        }
    }

}
