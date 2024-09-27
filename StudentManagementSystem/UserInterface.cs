using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem
{
    public class UserInterface
    {

        private StudentManager studentManager;
        public UserInterface(StudentManager studentManager)
        {
            this.studentManager = studentManager;
        }

        public void Menu()
        {
            bool exit = false;
            while (!exit)
            {
                string choice = InputHelper.GetUserInput<string>("\n- Student Management System -\n" +
                    "1. Print All Students\n" +
                    "2. Add Student\n" +
                    "3. Search for Student\n" +
                    "4. Save and Exit\n" +
                    "Select an option: ");

                switch (choice)
                {
                    case "1":
                        PrintStudents();
                        break;
                    case "2":
                        AddStudent();
                        break;
                    case "3":
                        SearchStudent();
                        break;
                    case "4":
                        studentManager.SaveStudentsToJson("studentsFile.txt");
                        Console.WriteLine("Goodbye!");
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        private void PrintStudents()
        {
            var gradeGroups = new[]
            {
            (LowerBound: 91, UpperBound: 100, GroupName: "Group 5: Excellent"),
            (LowerBound: 71, UpperBound: 90, GroupName: "Group 4: Great"),
            (LowerBound: 51, UpperBound: 70, GroupName: "Group 3: Average"),
            (LowerBound: 21, UpperBound: 50, GroupName: "Group 2: Poor"),
            (LowerBound: 0, UpperBound: 20, GroupName: "Group 1: Low")
        };

            studentManager.PrintAllStudents(gradeGroups);
        }

        private void AddStudent()
        {
            string firstName = InputHelper.GetUserInput<string>("Enter student's first name:");
            string lastName = InputHelper.GetUserInput<string>("Enter student's last name:");
            int age = InputHelper.GetUserInput<int>("Enter student's age:");
            int grade = InputHelper.GetUserInput<int>("Enter student's grade (1-100):");

            Console.WriteLine("\nAvailable Classes:");
            var classrooms = studentManager.GetAllClassrooms();
            foreach (var room in classrooms)
            {
                Console.WriteLine($"- {room}");
            }

            string classroom = InputHelper.GetUserInput<string>("Enter class name:");

            var newStudent = new Student(firstName, lastName, age, grade, classroom);


            studentManager.AddStudent(newStudent);
            Console.WriteLine("Student added successfully.");
        }

        private void SearchStudent()
        {
            string studentID = InputHelper.GetUserInput<string>("Enter the student ID to search:");
            var student = studentManager.GetStudentById(studentID);

            if (student != null)
            {
                Console.WriteLine($"\nFound Student: {student.FirstName} {student.LastName}");
                Console.WriteLine($"  Age: {student.Age}");
                Console.WriteLine($"  Grade: {student.Grade}");
                Console.WriteLine($"  Class: {student.Classroom}");

                string option = InputHelper.GetUserInput<string>("\nOptions\n1. Change Grade\n2. Change Class\n3. Remove Student\n4. Press anywhere to go back");
                switch (option)
                {
                    case "1":
                        ChangeGrade(student);
                        break;
                    case "2":
                        ChangeClassroom(student);
                        break;
                    case "3":
                        studentManager.RemoveStudentById(studentID);
                        Console.WriteLine("Student removed successfully.");
                        break;
                    default:
                        return;
                }
            }
            else
            {
                Console.WriteLine("Student not found.");
            }
        }

        private static void ChangeGrade(Student student)
        {
            int newGrade = InputHelper.GetUserInput<int>("Enter new grade between 1 - 100:");
            if (newGrade >= 1 && newGrade <= 100)
            {
                student.Grade = newGrade;
                Console.WriteLine("Grade updated successfully.");
            }
            else
            {
                return;
            }
        }

        private void ChangeClassroom(Student student)
        {
            Console.WriteLine("\nAvailable Class:");
            var classrooms = studentManager.GetAllClassrooms();
            foreach (var room in classrooms)
            {
                Console.WriteLine($"- {room}");
            }

            string newClassroom = InputHelper.GetUserInput<string>("Enter new class name:");
            student.Classroom = newClassroom;
            Console.WriteLine("Class updated successfully.");
        }
    }

}
