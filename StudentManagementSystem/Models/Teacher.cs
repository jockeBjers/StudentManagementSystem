using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Models
{
    public class Teacher : Person
    {
       
        public static int lastTeacherID = 1;

  
        public string TeacherID { get; private set; }
        public int Salary { get; set; }
        public List<string> Subjects { get; set; } 

       
        public Teacher(string firstName, string lastName, int age, int salary, params string[] subjects)
            : base(firstName, lastName, age)
        {
            TeacherID = lastTeacherID++.ToString(); // Assign unique ID and increment
            Salary = salary;
            Subjects = new List<string>(subjects); // Initialize subjects list
        }

        public Teacher() : base()
        {
            TeacherID = lastTeacherID++.ToString();
            Subjects = new List<string>();
        }

        // Method to print subjects
        public void PrintSubjects()
        {
            Console.WriteLine("- Subjects:");
            foreach (var subject in Subjects)
            {
                Console.WriteLine($"   {subject}");
            }
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName}, Age: {Age}, Salary: {Salary}, ID: {TeacherID}";
        }
    }


}
