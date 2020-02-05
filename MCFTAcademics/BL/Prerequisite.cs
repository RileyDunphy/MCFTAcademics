using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCFTAcademics.BL
{
    public class Prerequisite
    {

        public Prerequisite()
        {
        }

        public Prerequisite(int courseId, int prereqId, bool isPrereq, bool isCoreq)
        {
            this.CourseId = courseId;
            this.PrereqId = prereqId;
            this.IsPrereq = isPrereq;
            this.IsCoreq = isCoreq;
        }

        public int CourseId { get; set; }
        public int PrereqId { get; }
        public bool IsPrereq { get; }
        public bool IsCoreq { get; }
    }
}
