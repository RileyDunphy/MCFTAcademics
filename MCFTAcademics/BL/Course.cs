using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCFTAcademics.BL
{
    public class Course
    {
        private string name;
        private decimal credit;
        private DateTime from;
        private DateTime to;

        public Course()
        {
        }

        public Course(string name, decimal credit, DateTime from, DateTime to)
        {
            this.Name = name;
            this.Credit = credit;
            this.From = from;
            this.To = to;
        }

        public string Name { get => name; set => name = value; }
        public decimal Credit { get => credit; set => credit = value; }
        public DateTime From { get => from; set => from = value; }
        public DateTime To { get => to; set => to = value; }

        public bool isEligible(User u)
        {
            return false;
        }
    }
}
