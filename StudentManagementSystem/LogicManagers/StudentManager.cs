using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.LogicManagers
{
    public class StudentManager
    {
        private TeacherManager teacherManager;

        public StudentManager(TeacherManager teacherManager)
        {
            this.teacherManager = teacherManager;
        }

        private Dictionary<string, Student> students = new Dictionary<string, Student>();

        // Saving the data in the Data folder, the .. .. takes it up from the debug folder, to ensure it is both loaded and saved in the data folder.
        private static string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Data", "studentlistFile.json");

        public void AddStudent(Student student)
        {
            // Try to add the student, else display a message if the ID already exists.
            if (students.TryAdd(student.StudentID, student))
            {
                Student.existingIDs.Add(student.StudentID); // Add to existing IDs list
            }
            else
            {
                Console.WriteLine("A student with this ID already exists.");
            }
        }

        public void RemoveStudentById(string studentId)
        {
            if (students.Remove(studentId))
            {
                // Also remove the student's ID from the existing IDs list
                Student.RemoveStudentID(studentId);
                Console.WriteLine($"Student with ID {studentId} has been removed.");
            }
            else
            {
                Console.WriteLine($"No student found with ID {studentId}.");
            }
        }

        public void PrintStudentsAlphabeticOrder()
        {
            var sortedAlphabetically = students
                .OrderBy(s => s.Value.LastName)
                .ThenBy(s => s.Value.FirstName)
                .ToList();

            Console.WriteLine("Sorted by last names:");
            foreach (var student in sortedAlphabetically)
            {
                Console.WriteLine(student.Value.ToString());
            }
        }

        public void PrintAllSubjects()
        {
            var allSubjects = students.Values
                .SelectMany(s => s.SubjectGrades.Keys) // Get all subjects from SubjectGrades
                .Distinct() // Get unique subjects
                .OrderBy(s => s) // Sort subjects
                .ToList();

            Console.WriteLine("All Subjects:");
            foreach (var subject in allSubjects)
            {
                // Get the teacher of this subject from TeacherManager
                var teachersForSubject = teacherManager.GetTeachersBySubject(subject);

                foreach (var teacher in teachersForSubject)
                {
                    Console.WriteLine($"\n- Subject: {subject}, Teacher: {teacher.FirstName} {teacher.LastName}");
                }

                // Get students in the current subject
                var studentsInSubject = students.Values
                    .Where(s => s.SubjectGrades.ContainsKey(subject))
                    .OrderByDescending(s => s.GetGrade(subject))
                    .ToList();

                if (studentsInSubject.Count != 0)
                {
                    foreach (var student in studentsInSubject)
                    {
                        Console.WriteLine("     " + student.ToString() + ", Grade: " + student.GetGrade(subject));
                    }
                }
                else
                {
                    Console.WriteLine("  No students found studying this subject.");
                }
            }
        }

        public void PrintStudentsBySubject(string subject)
        {
            // Get the teacher for this subject from TeacherManager
            var teachersForSubject = teacherManager.GetTeachersBySubject(subject);

            //  students in the specified subject
            var studentsInSubject = students.Values
                .Where(s => s.SubjectGrades.ContainsKey(subject))
                .OrderByDescending(s => s.GetGrade(subject))
                .ToList();

            foreach (var teacher in teachersForSubject)
            {
                Console.WriteLine($"\n- Subject: {subject}, Students in class: {studentsInSubject.Count}, Teacher: {teacher.FirstName} {teacher.LastName}");
            }

            // Print students in subject
            foreach (var student in studentsInSubject)
            {
                Console.WriteLine("     " + student.ToString() + ", Grade: " + student.GetGrade(subject));

            }
        }

        public Student? GetStudentById(string studentID)
        {
            if (students.TryGetValue(studentID, out var student))
            {
                return student;
            }
            else
            {
                Console.WriteLine("Student not found.");
                return null;
            }
        }

        public void SaveStudentsToJson()
        {
            try
            {
                string json = System.Text.Json.JsonSerializer.Serialize(students, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, json); // Write JSON to file in the project Data folder
                Console.WriteLine($"Students saved to: {filePath}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"An error occurred while saving to the file: {ex.Message}");
            }
        }

        public void LoadStudentsFromJson()
        {
            try
            {
                string json = File.ReadAllText(filePath);
                var loadedStudents = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, Student>>(json);

                if (loadedStudents != null)
                {
                    students = loadedStudents;
                    Console.WriteLine("Students loaded successfully.");
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found.");
            }
            catch (System.Text.Json.JsonException)
            {
                Console.WriteLine("Error deserializing the JSON file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}

