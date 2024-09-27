using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace StudentManagementSystem
{
    public class Program
    {
        private static StudentManager studentManager = new StudentManager(); // Instance for managing students

        private static string filePath = "studentlistFile.json"; // File path for saving/loading students

        static void Main(string[] args)
        {
            studentManager.LoadStudentsFromJson(filePath); // Load existing students at startup
            UserInterface ui = new UserInterface(studentManager);
            ui.Menu();
        }
    }


}