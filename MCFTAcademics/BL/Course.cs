using MCFTAcademics.DAL;
using System;
using System.Collections.Generic;

namespace MCFTAcademics.BL
{
    public class Course
    {
        private int id;
        private string name;
        private decimal credit;
        private DateTime from;
        private DateTime to;

        public Course()
        {
        }

        public Course(int id, string name, decimal credit, DateTime from, DateTime to)
        {
            this.id = id;
            this.name = name;
            this.credit = credit;
            this.from = from;
            this.to = to;
        }

        public string Name { get => name; }
        public decimal Credit { get => credit; }
        public DateTime From { get => from; }
        public DateTime To { get => to; }
        public int Id { get => id; }

        public bool isEligible(User u)
        {
            return false;
        }

        public static List<Course> getAllCourses()
        {
            return CourseDAL.getAllCourses();
        }
    }
}