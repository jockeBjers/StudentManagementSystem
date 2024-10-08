using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using StudentManagementSystem.LogicManagers;
using StudentManagementSystem.Models;
using StudentManagementSystem.UserInterfaces;

namespace StudentManagementSystem
{
    public class Program
    {
        private static TeacherManager teacherManager = new();
        private static StudentManager studentManager = new(teacherManager); // Instance for managing students

        static void Main(string[] args)
        {
            studentManager.LoadStudentsFromJson(); // Load existing students at startup
            teacherManager.LoadTeachersFromJson();

            UserInterface userInterface = new UserInterface(studentManager, teacherManager);

            userInterface.Menu();

        }
    }
}