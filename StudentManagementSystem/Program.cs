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
        private static StudentManager studentManager = new(); // Instance for managing students
        private static TeacherManager teacherManager = new();

        static void Main(string[] args)
        {

            Teacher teacher = new Teacher("John", "Doe", 40, EnumSubjects.Math, EnumSubjects.English);
            Console.WriteLine(teacher);
            teacher.PrintSubjects();

            studentManager.LoadStudentsFromJson(); // Load existing students at startup

            
           // UserInterface userInterface = new UserInterface(studentManager, teacherManager);

           // userInterface.Menu();

        }
        //Things to add
        // add person class for future use, to add teachers
        // subjects for both teachers and students. 
    }
}