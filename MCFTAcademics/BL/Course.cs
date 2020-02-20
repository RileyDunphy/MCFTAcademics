using MCFTAcademics.DAL;
using System;
using System.Collections.Generic;

namespace MCFTAcademics.BL
{
    public class Course
    {
        public Course()
        {
        }

        public Course(int id, string name, decimal credit, string description, int lectureHours, int labHours, int examHours, int totalHours, decimal revisionNumber,string program, bool accreditation)
        {
            this.Id = id;
            this.Name = name;
            this.Credit = credit;
            this.Description = description;
            this.LectureHours = lectureHours;
            this.LabHours = labHours;
            this.ExamHours = examHours;
            this.TotalHours = totalHours;
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
        public int TotalHours { get;  }
        public decimal RevisionNumber { get; }
        public string Program { get; }
        public bool Accreditation { get; }

        // These were previously cached unconditionally, but led a lot of logic issues and performance issues.
        public IEnumerable<Prerequisite> GetPrerequisites() => PrerequisiteDAL.GetPrereqs(Id);

        public Staff GetLeadStaff() => StaffDAL.GetStaffByCourseIdAndType(Id, "lead");

        public Staff GetSupportStaff() => StaffDAL.GetStaffByCourseIdAndType(Id, "support");

        public bool IsEligible(User u)
        {
            return false;
        }
        public static List<Course> GetCoursesByInstructor(int userid)
        {
            return CourseDAL.GetCoursesByInstructor(userid);
        }

        public static List<Course> GetAllCourses()
        {
            return CourseDAL.GetAllCourses();
        }

        public static Course GetCourseById(int id)
        {
            return CourseDAL.GetCourseById(id);
        }
        public bool UpdateCourse(Staff leadStaff, Staff supportStaff, IEnumerable<Prerequisite> prerequisites)
        {
            return CourseDAL.UpdateCourse(this, leadStaff, supportStaff, prerequisites);
        }
        public static int AddCourse(Course c, Staff leadStaff, Staff supportStaff, IEnumerable<Prerequisite> prerequisites)
        {
            return CourseDAL.AddCourse(c, leadStaff, supportStaff, prerequisites);
        }
    }
}