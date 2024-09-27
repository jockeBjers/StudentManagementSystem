using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace StudentManagementSystem
{
    //Things to add:
    // Method for changing a students grade
    // Method to search for grade group
    // Method for removing students
    // separate input from logic
    public class StudentManager
    {
        private Dictionary<string, Student> students = new Dictionary<string, Student>();

        public void AddStudent(Student student)
        {
            if (!GetAllClassrooms().Contains(student.Classroom))
            {
                Console.WriteLine($"Classroom '{student.Classroom}' does not exist. Adding it to the list.");
            }

            // Add student to the dictionary
            students.TryAdd(student.StudentID, student);
            SaveStudentsToJson("studentlistFile.json"); // Save after adding a student
        }


        public bool RemoveStudentById(string studentID)
        {
            bool removed = students.Remove(studentID);
            if (removed)
            {
                SaveStudentsToJson("studentlistFile.json"); // Save after removing a student
            }
            return removed;
        }


        // Method to print all students, grouped by classroom
        public void PrintAllStudents((int LowerBound, int UpperBound, string GroupName)[] gradeGroups)
        {
            // Group students by their classroom
            var studentsByClass = students.Values.GroupBy(s => s.Classroom);

            // Iterate over each class group
            foreach (var classGroup in studentsByClass)
            {
                // Count the number of students in this classroom
                int studentCount = classGroup.Count();

                // Calculate the average grade for students in this classroom
                var averageGrade = classGroup.Average(s => s.Grade);

                // Print the classroom name along with the number of students and the average grade
                Console.WriteLine($"\nClass: {classGroup.Key} Total students: {studentCount}, Average Grade: {averageGrade:F2}");

                // Print the students in this classroom along with their grade groups
                PrintStudentsGroupedByGrade(classGroup, gradeGroups);
            }

            // Print the average grade of all students

            var overallAverageGrade = students.Values.Average(s => s.Grade);
            Console.WriteLine($"\nAverage Grade of All Students: {overallAverageGrade:F2}");
        }

        // Helper method to print students grouped by grade ranges
        private static void PrintStudentsGroupedByGrade(IEnumerable<Student> students, (int LowerBound, int UpperBound, string GroupName)[] gradeGroups)
        {
            foreach (var gradeGroup in gradeGroups)
            {
                // Get students in the current grade group
                var studentsInGroup = students
                    .Where(s => s.Grade >= gradeGroup.LowerBound && s.Grade <= gradeGroup.UpperBound)
                    .OrderByDescending(s => s.Grade)
                    .ToList();

                if (studentsInGroup.Any())
                {
                    // Set text color based on grade group performance
                    switch (gradeGroup.GroupName)
                    {
                        case "Group 1: Low": // Failing group
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                        case "Group 5: Excellent": // Top-performing group
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            break;
                        case "Group 4: Great":
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Yellow; // Other groups
                            break;
                    }

                    // Print group header
                    Console.WriteLine($"{gradeGroup.GroupName} ({gradeGroup.LowerBound}-{gradeGroup.UpperBound})");

                    // Print students in this grade group
                    foreach (var student in studentsInGroup)
                    {
                        Console.WriteLine($"  Student ID: {student.StudentID}  Name: {student.FirstName} {student.LastName}, Age: {student.Age}, Grade: {student.Grade}, Class: {student.Classroom}");
                    }

                    // Reset console color after printing each group
                    Console.ResetColor();
                }
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

        public List<string> GetAllClassrooms()
        {
            // Get a distinct list of all classrooms from the students
            return students.Values
                  .Where(s => !string.IsNullOrEmpty(s.Classroom)) // Ensure the classroom is not null or empty
                  .Select(s => s.Classroom)
                  .Distinct()
                  .ToList();
        }

        public void SaveStudentsToJson(string filePath)
        {
            try
            {
                // Serialize the student dictionary to JSON
                string json = System.Text.Json.JsonSerializer.Serialize(students, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, json); // Write JSON to file
                Console.WriteLine("Students saved to JSON file successfully.");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"An error occurred while saving to the file: {ex.Message}");
            }
        }


        // Method to load students from JSON
        public void LoadStudentsFromJson(string filePath)
        {
            try
            {
                if (File.Exists(filePath)) // Check if the file exists before trying to read it
                {
                    var json = File.ReadAllText(filePath);
                    var loadedStudents = JsonConvert.DeserializeObject<Dictionary<string, Student>>(json);

                    foreach (var person in loadedStudents)
                    {
                        var student = person.Value; // Access the Student object

                        // Add the student to your dictionary
                        students[student.StudentID] = student;

                        // Add to existing IDs
                        if (!Student.existingIDs.Contains(student.StudentID))
                        {
                            Student.existingIDs.Add(student.StudentID); 
                        }
                    }

                    Console.WriteLine("Students loaded successfully.");
                }
                else
                {
                    Console.WriteLine("No existing student file found. Starting with an empty list.");
                }
            }
            catch (System.Text.Json.JsonException jsonEx)
            {
                Console.WriteLine($"JSON error while loading students: {jsonEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading students: {ex.Message}");
            }
        }


    }
}


