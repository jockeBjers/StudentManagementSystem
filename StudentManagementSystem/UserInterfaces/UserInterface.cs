using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using StudentManagementSystem.LogicManagers;
using StudentManagementSystem.Models;
using StudentManagementSystem.UserInterfaces;


namespace StudentManagementSystem.UserInterfaces
{
    public class UserInterface
    {
        private StudentManager studentManager;
        private TeacherManager teacherManager;

        public UserInterface(StudentManager studentManager, TeacherManager teacherManager)
        {
            this.studentManager = studentManager;
            this.teacherManager = teacherManager;
        }

        public void Menu()
        {
            bool exit = false;
            while (!exit)
            {
                string choice = InputHelper.GetUserInput<string>("\n- Main Menu -\n" +
                    "1. Manage Students\n" +
                    "2. Manage Teachers\n" +
                    "3. Exit\n" +
                    "Select an option: ");

                switch (choice)
                {
                    case "1":
                        var studentInterface = new StudentInterface(studentManager);
                        studentInterface.Menu();
                        break;
                    case "2":
                        var teacherInterface = new TeacherInterface(teacherManager);
                        teacherInterface.Menu();
                        break;
                    case "3":
                        Console.WriteLine("Goodbye!");
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
    }
}
