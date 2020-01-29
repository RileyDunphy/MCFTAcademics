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
        private decimal revisionNumber;

        public CourseCode()
        {
        }

        public CourseCode(string code, DateTime from, DateTime to, decimal revisionNumber)
        {
            this.code = code;
            this.from = from;
            this.to = to;
            this.revisionNumber = revisionNumber;
        }

        public string Code { get => code; }
        public DateTime From { get => from;}
        public DateTime To { get => to; }
        public decimal RevisionNumber { get => revisionNumber; }

        public static CourseCode getNewestCourseCodeById(int id)
        {
            return CourseCodeDAL.getNewestCourseCodeById(id);
        }
    }
}
