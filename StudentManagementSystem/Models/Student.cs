using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace StudentManagementSystem.Models
{
    public class Student : Person
    {
        public static List<string> existingIDs = new(); // To track existing IDs
        public string StudentID { get; set; }
        public Dictionary<string, int> SubjectGrades { get; set; } = new Dictionary<string, int>();
        public List<string> Subjects { get; set; } = new List<string>();

        public Student() : base()
        {
            Subjects = new List<string>();
            SubjectGrades = new Dictionary<string, int>();
        }

        public Student(string firstName, string lastName, int age, string studentID, params string[] subjects)
        : base(firstName, lastName, age)
        {
            StudentID = studentID; 
            existingIDs.Add(StudentID); // Add the ID to the list of existing IDs
            Subjects = new List<string>(subjects);
            foreach (var subject in subjects)
            {
                SubjectGrades[subject] = 0; // Default grade is 0
            }
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

        // Method to remove an ID when a student is deleted
        public static void RemoveStudentID(string id)
        {
            existingIDs.Remove(id);
        }

        // Method to add a subject to the student
        public void AddSubject(string subject)
        {
            if (!Subjects.Contains(subject))
            {
                Subjects.Add(subject);
            }
            else
            {
                Console.WriteLine($"{subject} already assigned");
            }
        }

        public void RemoveSubject(string subject)
        {
            if (Subjects.Contains(subject))
            {
                Subjects.Remove(subject); // Remove the subject from the list
                SubjectGrades.Remove(subject); // Remove the associated grade
                Console.WriteLine($"{subject} has been removed.");
            }
            else
            {
                Console.WriteLine($"{subject} is not assigned to this student.");
            }
        }

        public int? GetGrade(string subject)
        {
            if (SubjectGrades.TryGetValue(subject, out int grade))
            {
                return grade; // Return the grade if found
            }
            else
            {
                Console.WriteLine($"{subject} is not assigned to this student.");
                return null; // Return null if the subject is not found
            }
        }

        public void SetGrade(string subject, int grade)
        {
            if (Subjects.Contains(subject))
            {
                if (grade >= 1 && grade <= 100)
                {
                    SubjectGrades[subject] = grade;
                }
                else
                {
                    Console.WriteLine("Grade must be between 1 and 100.");
                }
            }
            else
            {
                Console.WriteLine($"{subject} is not assigned to this student.");
            }
        }

        public void PrintSubjectsAndGrades()
        {
            Console.WriteLine("- Subjects and Grades:");
            foreach (var subject in Subjects)
            {
                if (SubjectGrades.TryGetValue(subject, out int grade))
                {
                    Console.WriteLine($"   {subject}: {grade}");
                }
                else
                {
                    Console.WriteLine($"   {subject}: No grade assigned");
                }
            }
        }

        public override string ToString()
        {
            return $"Student ID: {StudentID}  Name: {FirstName} {LastName}, Age: {Age}";
        }
    }
}
