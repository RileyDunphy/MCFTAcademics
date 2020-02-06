using MCFTAcademics.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCFTAcademics.BL
{
    public class CourseCode
    {

        public CourseCode()
        {
        }

        public CourseCode(string code, DateTime from, DateTime to, int semester)
        {
            this.Code = code;
            this.From = from;
            this.To = to;
            this.Semester = semester;
        }

        public string Code { get; }
        public DateTime From { get;}
        public DateTime To { get; }
        public int Semester { get; }

        public static CourseCode GetNewestCourseCodeById(int id)
        {
            return CourseCodeDAL.GetNewestCourseCodeById(id);
        }

        public static bool AddCourseCode(int id, CourseCode c)
        {
            return CourseCodeDAL.AddCourseCode(id, c);
        }

        public static int GetIdByCourseCode(string code)
        {
            return CourseCodeDAL.GetIdByCourseCode(code);
        }
    }
}
