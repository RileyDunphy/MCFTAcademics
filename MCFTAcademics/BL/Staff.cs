using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCFTAcademics.BL
{
    public class Staff
    {
        private DateTime from;
        private DateTime to;
        private string tempName;

        public Staff()
        {
        }

        public Staff(DateTime from, DateTime to, string tempName)
        {
            this.From = from;
            this.To = to;
            this.TempName = tempName;
        }

        public DateTime From { get => from; set => from = value; }
        public DateTime To { get => to; set => to = value; }
        public string TempName { get => tempName; set => tempName = value; }
    }
}
