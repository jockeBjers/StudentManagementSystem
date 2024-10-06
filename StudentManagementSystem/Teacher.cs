using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem
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

        public override string ToString()
        {
            string subjectList = string.Join(", ", Subjects);
            return $"Teacher: {FirstName} {LastName}, Age: {Age}, ID: {TeacherID}, Subjects: {subjectList}";
        }
    }

}
