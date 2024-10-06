﻿using System.IO;
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

        static void Main(string[] args)
        {
            studentManager.LoadStudentsFromJson(); // Load existing students at startup
            UserInterface ui = new(studentManager);
            ui.Menu();
        }
        //Things to add
        // add person class for future use, to add teachers
        // subjects for both teachers and students. 
    }
}