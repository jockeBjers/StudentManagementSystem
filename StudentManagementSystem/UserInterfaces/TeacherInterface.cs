using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentManagementSystem.LogicManagers;
using StudentManagementSystem.Models;
using StudentManagementSystem.UserInterfaces;

namespace StudentManagementSystem.UserInterfaces
{
    public class TeacherInterface
    {
        private TeacherManager teacherManager;

        public TeacherInterface(TeacherManager teacherManager)
        {
            this.teacherManager = teacherManager;
        }

        public void Menu()
        {
            bool exit = false;
            while (!exit)
            {
                string choice = InputHelper.GetUserInput<string>("\n- Teacher Management System -\n" +
                    "1. Print All Teachers\n" +
                    "2. Add Teacher\n" +
                    "3. Search for Teacher\n" +
                    "4. Save and Exit\n" +
                    "Select an option: ");

                switch (choice)
                {
                    case "1":
                        // PrintTeachers();
                        break;
                    case "2":
                        //AddTeacher();
                        break;
                    case "3":
                      //  SearchTeacher();
                        break;
                    case "4":
                        Console.WriteLine("Goodbye!");
                       // teacherManager.SaveTeachersToJson();
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        /*
        private void PrintTeachers()
        {
         
        }

        private void AddTeacher()
        {
           
        }

        private void SearchTeacher()
        {
         
        }*/
    }
}
