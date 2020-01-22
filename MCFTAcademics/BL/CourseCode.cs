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
            this.Code = code;
            this.From = from;
            this.To = to;
        }

        public string Code { get => code; set => code = value; }
        public DateTime From { get => from; set => from = value; }
        public DateTime To { get => to; set => to = value; }
    }
}
