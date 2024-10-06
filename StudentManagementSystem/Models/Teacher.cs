using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Models
{
    public class Teacher : Person
    {
        private static int lastTeacherID = 0;
        private int teacherID;
        private List<EnumSubjects> subjects;

        public List<EnumSubjects> Subjects 
        {
            get { return subjects; }
            set { subjects = value; }  
        }

        public int TeacherID
        {
            get { return teacherID; }
            private set { teacherID = value; }
        }

        public Teacher(string firstName, string lastName, int age, params EnumSubjects[] subjects) : base(firstName, lastName, age)
        {
            TeacherID = ++lastTeacherID;
            this.subjects = new List<EnumSubjects>(subjects);
        }

        public Teacher() : base()
        {
            TeacherID = ++lastTeacherID;
            this.subjects = new List<EnumSubjects>();
        }
        public void PrintSubjects()
        {
            Console.WriteLine("Subjects taught:");
            foreach (var subject in Subjects)
            {
                Console.WriteLine($"   {subject}");
            }
        }

        public override string ToString()
        {
            return $"Teacher: {FirstName} {LastName}, Age: {Age}, ID: {TeacherID}";
        }
    }

}
