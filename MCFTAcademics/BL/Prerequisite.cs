using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCFTAcademics.BL
{
    public class Prerequisite
    {
        private int courseId;
        private int prereqId;
        private bool isPrereq;
        private bool isCoreq;

        public Prerequisite()
        {
        }

        public Prerequisite(int courseId, int prereqId, bool isPrereq, bool isCoreq)
        {
            this.courseId = courseId;
            this.prereqId = prereqId;
            this.isPrereq = isPrereq;
            this.isCoreq = isCoreq;
        }

        public int CourseId { get; set; }
        public int PrereqId { get => prereqId; }
        public bool IsPrereq { get => isPrereq; }
        public bool IsCoreq { get => isCoreq; }
    }
}
