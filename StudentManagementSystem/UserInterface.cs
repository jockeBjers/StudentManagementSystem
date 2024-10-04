using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

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
                        Console.WriteLine("Goodbye!");
                        studentManager.SaveStudentsToJson();
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
            Console.Clear();

            int choice = InputHelper.GetUserInput<int>("Press\n1. To print out all students in alphabetic order\n2. To print out all classes and grades\n3. To print out one class.");
            switch (choice)
            {
                case 1:
                    studentManager.PrintStudentsAlphabeticOrder();
                    break;
                case 2:
                    studentManager.PrintAllClassrooms();
                    break;
                case 3:
                    Console.WriteLine("\nClasses:");
                    var classrooms = studentManager.GetAllClassrooms();
                    foreach (var room in classrooms)
                    {
                        Console.WriteLine($"- {room}");
                    }
                    string classroom = InputHelper.GetUserInput<string>("Enter which class you want to see: ").ToLower();
                    studentManager.PrintSingleClassroom(classroom);
                    break;
            }
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

            // Call to confirm classroom
            ConfirmClassroom(classroom);

            var newStudent = new Student(firstName, lastName, age, grade, classroom);
            studentManager.AddStudent(newStudent);
            Console.WriteLine("Student added successfully.");
        }

        private string ConfirmClassroom(string classroom)
        {
            // Check for classrooms
            while (true) // Loop until a valid classroom is confirmed
            {
                if (studentManager.GetAllClassrooms().Contains(classroom))
                {
                    // If classroom exists, break
                    return classroom;
                }
                else
                {
                    Console.WriteLine($"Classroom '{classroom}' does not exist!");
                    bool confirm = false;

                    // if class doesnt exist, ask if it should be added or not.
                    while (!confirm)
                    {
                        string input = InputHelper.GetUserInput<string>("Are you sure you want to add a new classroom? (y/n): ");
                        if (input == "y")
                        {
                            confirm = true; // confirm the new classroom                 
                            return classroom; // Return new added classroom
                        }
                        else if (input == "n")
                        {
                            // Prompt the user for a valid classroom
                            var classrooms = studentManager.GetAllClassrooms();
                            Console.WriteLine("Available Classrooms:");
                            foreach (var room in classrooms)
                            {
                                Console.WriteLine($"- {room}");
                            }
                            Console.Write("Please enter a valid classroom: ");
                            classroom = Console.ReadLine();
                            break; // breaks loop and go back to previous step
                        }
                        else
                        {
                            // Invalid input, prompt again
                            Console.WriteLine("Invalid input. Please enter 'y' for yes or 'n' for no.");
                        }
                    }
                }
            }
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
            student.Grade = newGrade;
            Console.WriteLine("Grade updated successfully.");
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
