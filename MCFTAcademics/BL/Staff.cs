using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCFTAcademics.BL
{
    public class Staff
    {
        public Staff()
        {
        }

        public Staff(DateTime from, DateTime to, string tempName)
        {
            this.From = from;
            this.To = to;
            this.TempName = tempName;
        }

        public DateTime From { get; }
        public DateTime To { get;  }
        public string TempName { get;  }
    }
}
