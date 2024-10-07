using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Models
{
    public class Subject
    {
        public string Name { get; set; }

        public static List<string> AvailableSubjects { get; } = new List<string>
            {
                "Math",
                "English",
                "Science",
                "Music",
                "Physics",
                "Spanish",
                "Biology",
                "Gymnastics",
                ".Net Programming",
                "Art",
                "History"
            };

        public static void PrintAvailableSubjects()
        {
            Console.WriteLine("\nAvailable Subjects:");
            foreach (var subject in AvailableSubjects)
            {
                Console.WriteLine($"- {subject}");
            }
        }
    }




}
