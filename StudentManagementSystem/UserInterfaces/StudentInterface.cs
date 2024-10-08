﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentManagementSystem.LogicManagers;
using StudentManagementSystem.Models;
using StudentManagementSystem.UserInterfaces;

namespace StudentManagementSystem.UserInterfaces
{
    public class StudentInterface
    {
        private StudentManager studentManager;
        public StudentInterface(StudentManager studentManager)
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
                        Console.Clear();
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

            int choice = InputHelper.GetUserInput<int>("Press\n1. To print out all students in alphabetic order\n2. To print out all subjects\n3. To print out students by subject.");

            switch (choice)
            {
                case 1:
                    studentManager.PrintStudentsAlphabeticOrder();
                    break;
                case 2:
                    studentManager.PrintAllSubjects(); // print all subjects and all students
                    Console.ReadLine();
                    break;
                case 3:
                    Subject.PrintAvailableSubjects(); // prints out available subjects to search from.
                    string selectedSubject = InputHelper.GetUserInput<string>("Enter the subject you want to see: ");
                    studentManager.PrintStudentsBySubject(selectedSubject); // prints out the chosen subject
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }

        private void AddStudent()
        {
            Console.Clear();

            string firstName = InputHelper.GetUserInput<string>("Enter student's first name:");
            string lastName = InputHelper.GetUserInput<string>("Enter student's last name:");
            int age = InputHelper.GetUserInput<int>("Enter student's age:");
            string studentID = Student.GenerateStudentID();

            var newStudent = new Student(firstName, lastName, age, studentID);
            studentManager.AddStudent(newStudent);
            Console.WriteLine("Student added successfully.");

            // Ask if the user wants to add subjects
            while (true)
            {
                string addSubject = InputHelper.GetUserInput<string>("Do you want to add a subject for this student? (y/n): ");
                if (addSubject.ToLower() == "y")
                {
                    string subject = InputHelper.GetUserInput<string>("Enter subject name: ");
                    newStudent.AddSubject(subject);

                    // Ask for the grade for the added subject
                    int grade = InputHelper.GetUserInput<int>("Enter grade for this subject (1-100):");
                    newStudent.SetGrade(subject, grade); // Set the grade for the subject
                }
                else if (addSubject.ToLower() == "n")
                {
                    break; // Exit the loop
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter 'y' or 'n'.");
                }
            }
            Console.WriteLine("New Student Added!" +
                "");
            Console.WriteLine(newStudent.ToString() + "\n");
            newStudent.PrintSubjectsAndGrades();
            Console.WriteLine("Press to continue");
            Console.ReadLine();
        }

        private void SearchStudent()
        {
            string studentID = InputHelper.GetUserInput<string>("Enter the student ID to search:");
            var student = studentManager.GetStudentById(studentID);

            if (student != null)
            {
                Console.Clear();
                Console.WriteLine(student.ToString() + "\n");
                student.PrintSubjectsAndGrades(); // Print subjects and grades

                string option = InputHelper.GetUserInput<string>("\nOptions\n1. Change Grade\n2. Remove Student\n3. Press anywhere to go back");
                switch (option)
                {
                    case "1":
                        ChangeGrade(student);  // change grade in subjects
                        break;
                    case "2":
                        studentManager.RemoveStudentById(studentID);  // remove student
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

        // To add, ability to add subjects and remove, similar as for teachers
        private static void ChangeGrade(Student student)
        {
            Subject.PrintAvailableSubjects();

            while (true)
            {
                string subjectInput = InputHelper.GetUserInput<string>("Enter a subject name (or type 'exit' to stop):");
                if (subjectInput == "exit")
                {
                    break;
                }
                // Check if the subject is valid
                if (Subject.AvailableSubjects.Contains(subjectInput))
                {

                    // Check if the subject is already in the teacher's subjects list and remove it if so. 
                    if (student.Subjects.Contains(subjectInput))
                    {
                        Console.WriteLine($"{subjectInput} is already assigned to the student.");

                        string action = InputHelper.GetUserInput<string>("Do you want to 1. update grade or 2. remove the subject?");

                        if (action == "1")
                        {
                            // Update grade
                            int newGrade = InputHelper.GetUserInput<int>("Enter new grade (1-100):");
                            student.SetGrade(subjectInput, newGrade); // Update grade
                            Console.WriteLine("Grade updated successfully.");
                        }
                        else if (action == "2")
                        {
                            // Remove the subject and associated grade
                            student.RemoveSubject(subjectInput); // Use the RemoveSubject method
                            Console.WriteLine($"{subjectInput} removed successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid option. Please enter 1 to update grade or 2 to remove the subject.");
                        }
                    }
                    else
                    {
                        int grade = InputHelper.GetUserInput<int>("Enter grade for this subject (1-100):");
                        student.SetGrade(subjectInput, grade); // Set the grade for the subject

                        // Add the subject if it doesn't exist
                        student.Subjects.Add(subjectInput);
                        Console.WriteLine($"{subjectInput} added.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid subject. Please select a valid subject.");
                }
            }

            Console.WriteLine("Subjects updated successfully.");
            Console.WriteLine(student.ToString() + "\n");
            student.PrintSubjectsAndGrades();
            Console.WriteLine("Press to continue");
            Console.ReadLine();
            Console.Clear();
        }
    }
}
