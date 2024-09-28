using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace StudentManagementSystem
{
    public class Program
    {
        private static StudentManager studentManager = new(); // Instance for managing students

        static void Main(string[] args)
        {
            studentManager.LoadStudentsFromJson(); // Load existing students at startup
            UserInterface ui = new(studentManager);
            ui.Menu();
        }
    }
}