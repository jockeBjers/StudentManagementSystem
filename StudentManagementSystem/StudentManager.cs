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
    public class StudentManager
    {
        private Dictionary<string, Student> students = new Dictionary<string, Student>();

        // Saving the data in the Data folder, the .. .. takes it up from the debug folder, to ensure it is both loaded and saved in the data folder.
        private static string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Data", "studentlistFile.json");

        public void AddStudent(Student student)
        {
            // try to add, else a message that id already exists.
            if (students.TryAdd(student.StudentID, student))
            {
                Student.existingIDs.Add(student.StudentID);
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
                .OrderBy(s => s.Value.LastName) // Order all students by their lastname and print out
                .ThenBy(s => s.Value.FirstName)
                .ToList();
            Console.WriteLine("Sorted by lastnames");
            foreach (var student in sortedAlphabetically)
            {
                Console.WriteLine($"  Student ID: {student.Value.StudentID}  Name: {student.Value.FirstName} {student.Value.LastName}, Age: {student.Value.Age}, Grade: {student.Value.Grade}, Class: {student.Value.Classroom}");
            }
        }

        public void PrintSingleClassroom(string classroom)
        {
            var studentsInClass = students.Where(s => s.Value.Classroom == classroom)
                .OrderBy(s => s.Value.LastName)
                .ToList();
            if (studentsInClass.Count > 0)
            {
                int studentCount = studentsInClass.Count();
                Console.WriteLine($"Class: {classroom}, students: {studentCount}");
                foreach (var student in studentsInClass)
                {
                    Console.WriteLine($"  Student ID: {student.Value.StudentID}  Name: {student.Value.FirstName} {student.Value.LastName}, Age: {student.Value.Age}, Grade: {student.Value.Grade}");
                }
            }
            else
            {
                Console.WriteLine($"No students found in class {classroom}");
            }
        }

        public void PrintAllClassrooms()
        {
            // Group students by their classroom
            var studentsByClass = students.Values.GroupBy(s => s.Classroom);

            foreach (var classGroup in studentsByClass)
            {
                int studentCount = classGroup.Count();
                var averageGrade = classGroup.Average(s => s.Grade); // count the students in each group and their average

                // Print out the various classes
                Console.WriteLine($"\nClass: {classGroup.Key} Total students: {studentCount}, Average Grade: {averageGrade:F2}");

                // next loop to sort by grade
                var eachClass = classGroup.ToDictionary(s => s.StudentID)
                    .OrderByDescending(s => s.Value.Grade);
                foreach (var student in eachClass)
                {
                    Console.WriteLine($"    Student ID: {student.Value.StudentID}  Name: {student.Value.FirstName} {student.Value.LastName}, Age: {student.Value.Age}, Grade: {student.Value.Grade}");
                }
            }
            // Print the average grade of all students
            var overallAverageGrade = students.Values.Average(s => s.Grade);
            Console.WriteLine($"\nAverage Grade of All Students: {overallAverageGrade:F2}");
        }

        public Student? GetStudentById(string studentID)
        {
            if (students.TryGetValue(studentID, out var student))  // for searching for student to update
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
                  .Distinct() // Makes sure each class room is only printed once
                  .ToList();
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

