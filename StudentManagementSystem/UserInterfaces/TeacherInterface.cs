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
                        teacherManager.PrintTeachers();
                        break;
                    case "2":
                        AddTeacher();
                        break;
                    case "3":
                        SearchTeacher();
                        break;
                    case "4":
                        Console.WriteLine("Goodbye!");
                        teacherManager.SaveTeachersToJson();
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        private void AddTeacher()
        {
            string firstName = InputHelper.GetUserInput<string>("Enter teacher's first name:");
            string lastName = InputHelper.GetUserInput<string>("Enter teacher's last name:");
            int age = InputHelper.GetUserInput<int>("Enter teacher's age:");
            int salary = InputHelper.GetUserInput<int>("Enter teacher's salary:");

            List<string> subjectsList = GetTeacherSubjects();

            // Create new teacher with selected subjects
            var newTeacher = new Teacher(firstName, lastName, age, salary, subjectsList.ToArray());
            teacherManager.AddTeacher(newTeacher);
            Console.WriteLine("Teacher added successfully.");
        }

        private List<string> GetTeacherSubjects()
        {
            List<string> subjectsList = new();

            // Call the method from the Subject class to print available subjects
            Subject.PrintAvailableSubjects();

            string subjectInput;
            do
            {
                subjectInput = InputHelper.GetUserInput<string>("Enter a subject name (or type 'exit' to stop):");

                if (subjectInput.ToLower() != "exit" && Subject.AvailableSubjects.Contains(subjectInput))
                {
                    subjectsList.Add(subjectInput);
                    Console.WriteLine($"{subjectInput} added.");
                }
                else if (subjectInput.ToLower() != "exit")
                {
                    Console.WriteLine("Invalid subject. Please select a valid subject.");
                }

            } while (subjectInput.ToLower() != "exit");

            return subjectsList;
        }

        private void SearchTeacher()
        {
            Console.Clear();
            string teacherID = InputHelper.GetUserInput<string>("Enter the Teacher ID to search:");
            Teacher? foundTeacher = teacherManager.GetTeacherById(teacherID);

            if (foundTeacher != null) // Check if the teacher was found
            {
                Console.WriteLine("Teacher found:");
                Console.WriteLine(foundTeacher.ToString()); // Print the teacher's details
                foundTeacher.PrintSubjects(); // Print their subjects

                // Prompt the user for the next action
                string action = InputHelper.GetUserInput<string>("Would you like to (1) Update or (2) Remove this teacher? (Enter 1 or 2):");

                switch (action)
                {
                    case "1":
                        UpdateTeacher(foundTeacher);
                        break;
                    case "2":
                        teacherManager.RemoveTeacherById(teacherID);
                        Console.WriteLine("Student removed successfully.");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Returning to the main menu.");
                        break;
                }
            }
        }

        private void UpdateTeacher(Teacher teacher)
        {
            bool updating = true;

            while (updating)
            {
                Console.Clear();

                Console.WriteLine(teacher.ToString());
                teacher.PrintSubjects();

                Console.WriteLine("Select the information to update:");
                Console.WriteLine("1. First Name");
                Console.WriteLine("2. Last Name");
                Console.WriteLine("3. Age");
                Console.WriteLine("4. Salary");
                Console.WriteLine("5. Subjects");
                Console.WriteLine("6. Done Updating");

                string choice = InputHelper.GetUserInput<string>("Enter your choice:");

                switch (choice)
                {
                    case "1":
                        string newFirstName = InputHelper.GetUserInput<string>("Enter new first name:");
                        teacher.FirstName = newFirstName; // Update without check
                        Console.WriteLine("First name updated successfully.");
                        break;

                    case "2":
                        string newLastName = InputHelper.GetUserInput<string>("Enter new last name:");
                        teacher.LastName = newLastName; // Update without check
                        Console.WriteLine("Last name updated successfully.");
                        break;

                    case "3":
                        int newAge = InputHelper.GetUserInput<int>("Enter new age:");
                        teacher.Age = newAge; // Update without check
                        Console.WriteLine("Age updated successfully.");
                        break;

                    case "4":
                        int newSalary = InputHelper.GetUserInput<int>("Enter new salary:");
                        teacher.Salary = newSalary; // Update without check
                        Console.WriteLine("Salary updated successfully.");
                        break;

                    case "5":
                        List<string> newSubjects = GetTeacherSubjects(); // Get updated subjects from user
                        foreach (var subject in newSubjects)
                        {
                            if (!teacher.Subjects.Contains(subject))
                            {
                                teacher.Subjects.Add(subject); // Add if it doesn't already exist
                                Console.WriteLine($"{subject} added.");
                            }
                            else
                            {
                                Console.WriteLine($"{subject} already exists and was not added.");
                            }
                        }
                        Console.WriteLine("Subjects updated successfully.");
                        break;

                    case "6":
                        updating = false; // Exit the loop when done updating
                        break;

                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
                // Pause after each update for user to read the message
                Console.WriteLine("Press to continue: ");
                Console.ReadLine();
            }

            Console.WriteLine("Teacher updated successfully.");

        }
    }
}
