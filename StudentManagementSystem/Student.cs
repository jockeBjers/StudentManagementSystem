using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StudentManagementSystem
{
    public class Student : Person
    {
        public static List<string> existingIDs = new(); // To track existing IDs
        public string StudentID { get; private set; }
        public string Classroom { get; set; }
        private int grade;

        public int Grade
        {
            get { return grade; }
            set
            {
                if (value >= 1 && value <= 100)
                {
                    grade = value;
                }
                else
                {
                    Console.WriteLine("Grade must be between 1 and 100.");
                }
            }
        }

        // Parameterless constructor for JSON 
        public Student() : base("DefaultFirstName", "DefaultLastName", 0) // Provide default values for Person
        {
            StudentID = GenerateStudentID(); // Generate a unique ID
        }

        // Constructor with parameters
        public Student(string firstName, string lastName, int age, int grade, string classroom)
            : base(firstName, lastName, age)
        {
            Grade = grade;
            Classroom = classroom;
            StudentID = GenerateStudentID(); // Automatically generate StudentID
            existingIDs.Add(StudentID); // Add the generated ID to the list of existing IDs
        }

        // Method to generate unique Student ID
        public static string GenerateStudentID()
        {
            Random random = new();
            string id;
            do
            {
                id = random.Next(1000, 9999).ToString();
            } while (existingIDs.Contains(id));

            return id;
        }
    }

}