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
            this.from = from;
            this.to = to;
            this.tempName = tempName;
        }

        public DateTime From { get => from;  }
        public DateTime To { get => to;  }
        public string TempName { get => tempName;  }
    }
}
