using MCFTAcademics.BL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MCFTAcademics.DAL
{
    public class GradeDAL
    {
        public static Grade GradeFromRow(IDataReader reader)
        {
            return new Grade(Convert.ToDecimal(reader["grade"]), Convert.ToDateTime(reader["given"]), Convert.ToBoolean(reader["lock"]), Convert.ToDecimal(reader["hoursAttended"]),Convert.ToBoolean(reader["isSupplemental"]),Course.GetCourseById(Convert.ToInt32(reader["courseId"])));
        }

    }
}
