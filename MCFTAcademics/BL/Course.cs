using MCFTAcademics.DAL;
using System;
using System.Collections.Generic;

namespace MCFTAcademics.BL
{
    public class Course
    {
        public Course()
        {
            this.Prerequisites = new List<Prerequisite>();
        }

        public Course(int id, string name, decimal credit, string description, int lectureHours, int labHours, int examHours, int totalHours, decimal revisionNumber,string program, bool accreditation, List<Prerequisite> prerequisites)
        {
            this.Id = id;
            this.Name = name;
            this.Credit = credit;
            this.Description = description;
            this.LectureHours = lectureHours;
            this.LabHours = labHours;
            this.ExamHours = examHours;
            this.TotalHours = totalHours;
            this.Prerequisites = prerequisites;
            this.RevisionNumber = revisionNumber;
            this.Program = program;
            this.Accreditation = accreditation;
        }

        public string Name { get; }
        public decimal Credit { get; }
        public int Id { get; }
        public string Description { get; }
        public int LectureHours { get; }
        public int LabHours { get; }
        public int ExamHours { get; }
        public List<Prerequisite> Prerequisites { get; }
        public int TotalHours { get;  }
        public decimal RevisionNumber { get; }
        public string Program { get; }
        public bool Accreditation { get; }

        public bool IsEligible(User u)
        {
            return false;
        }
        public static List<Course> getCoursesByInstructor(int userid)
        {
            return CourseDAL.getCoursesByInstructor(userid);
        }

        public static List<Course> GetAllCourses()
        {
            return CourseDAL.GetAllCourses();
        }

        public static Course GetCourseById(int id)
        {
            return CourseDAL.GetCourseById(id);
        }
        public bool UpdateCourse()
        {
            return CourseDAL.UpdateCourse(this);
        }
        public static int AddCourse(Course c)
        {
            return CourseDAL.AddCourse(c);
        }
    }
}