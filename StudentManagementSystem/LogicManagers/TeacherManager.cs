using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.LogicManagers;

public class TeacherManager
{
    private Dictionary<string, Teacher> teachers = new Dictionary<string, Teacher>();
    private static string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Data", "teacherlistFile.json");

    public void PrintTeachers()
    {
        var sortedTeachers = teachers
            .OrderBy(s => s.Value.TeacherID)
            .ToList();

        Console.WriteLine("Teachers: ");
        foreach (var teacher in sortedTeachers)
        {
            Console.WriteLine(teacher.Value.ToString());
            teacher.Value.PrintSubjects(); // Print out their subjects
            Console.WriteLine();
        }
    }

    public void AddTeacher(Teacher teacher)
    {
        // Try to add the teacher to the dictionary, else print a message that the ID already exists(shouldn't happen)
        if (teachers.TryAdd(teacher.TeacherID, teacher))
        {
            Console.WriteLine($"{teacher} added successfully.");
        }
        else
        {
            Console.WriteLine("A teacher with this ID already exists.");
        }
    }

    public void RemoveTeacherById(string teacherID)
    {
        if (teachers.Remove(teacherID)) // If ID is found, will be deleted
        {
            Console.WriteLine("Teacher removed successfully.");
        }
        else
        {
            Console.WriteLine("No teacher found with the provided ID.");
        }
    }

    public Teacher? GetTeacherById(string teacherID)
    {
        if (teachers.TryGetValue(teacherID, out var teacher))  // search for teacher to update
        {
            return teacher;
        }
        else
        {
            Console.WriteLine("teacher not found.");
            return null;
        }
    }

    public List<Teacher> GetTeachersBySubject(string subject) // To use in the StudentManager to show each subjects teacher
    {
        return teachers.Values
            .Where(t => t.Subjects.Contains(subject))
            .ToList();
    }

    public void SaveTeachersToJson()
    {
        try
        {
            string json = JsonSerializer.Serialize(teachers, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json); // Write JSON to file in the project Data folder
            Console.WriteLine($"Teachers saved to: {filePath}");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"An error occurred while saving to the file: {ex.Message}");
        }
    }

    public void LoadTeachersFromJson()
    {
        try
        {
            string json = File.ReadAllText(filePath);
            var loadedTeachers = JsonSerializer.Deserialize<Dictionary<string, Teacher>>(json);

            if (loadedTeachers != null)
            {
                teachers = loadedTeachers;
                Console.WriteLine("Teachers loaded successfully.");
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
