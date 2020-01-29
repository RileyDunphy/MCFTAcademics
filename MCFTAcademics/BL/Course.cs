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
        private string description;
        private int lectureHours;
        private int labHours;
        private int examHours;
        private int totalHours;
        private List<Prerequisite> prerequisites;
        
        public Course()
        {
        }

        public Course(int id, string name, decimal credit, DateTime from, DateTime to, string description, int lectureHours, int labHours, int examHours, int totalHours, List<Prerequisite> prerequisites)
        {
            this.id = id;
            this.name = name;
            this.credit = credit;
            this.from = from;
            this.to = to;
            this.description = description;
            this.lectureHours = lectureHours;
            this.labHours = labHours;
            this.examHours = examHours;
            this.totalHours = totalHours;
            this.prerequisites = prerequisites;
        }

        public string Name { get => name; }
        public decimal Credit { get => credit; }
        public DateTime From { get => from; }
        public DateTime To { get => to; }
        public int Id { get => id; }
        public string Description { get => description; }
        public int LectureHours { get => lectureHours; }
        public int LabHours { get => labHours; }
        public int ExamHours { get => examHours; }
        public List<Prerequisite> Prerequisites { get => prerequisites; }
        public int TotalHours { get => totalHours;  }

        public bool isEligible(User u)
        {
            return false;
        }

        public static List<Course> getAllCourses()
        {
            return CourseDAL.getAllCourses();
        }

        public static Course getCourseById(int id)
        {
            return CourseDAL.getCourseById(id);
        }
    }
}