using MCFTAcademics.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCFTAcademics.BL
{
    public class CourseCode
    {
        private string code;
        private DateTime from;
        private DateTime to;

        public CourseCode()
        {
        }

        public CourseCode(string code, DateTime from, DateTime to)
        {
            this.code = code;
            this.from = from;
            this.to = to;
        }

        public string Code { get => code; }
        public DateTime From { get => from;}
        public DateTime To { get => to; }

        public static CourseCode getNewestCourseCodeById(int id)
        {
            return CourseCodeDAL.getNewestCourseCodeById(id);
        }

        public static bool addCourseCode(int id, string code)
        {
            return CourseCodeDAL.addCourseCode(id, code);
        }
    }
}
