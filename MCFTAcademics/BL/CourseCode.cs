﻿using MCFTAcademics.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCFTAcademics.BL
{
    public class CourseCode
    {

        public CourseCode()
        {
        }

        public CourseCode(string code, DateTime from, DateTime to, int semester)
        {
            this.Code = code;
            this.From = from;
            this.To = to;
            this.Semester = semester;
        }

        public string Code { get; }
        public DateTime From { get;}
        public DateTime To { get; }
        public int Semester { get; }

        public static CourseCode GetNewestCourseCodeById(int id)
        {
            return CourseCodeDAL.GetNewestCourseCodeById(id);
        }

        public static bool AddCourseCode(int id, CourseCode c)
        {
            return CourseCodeDAL.AddCourseCode(id, c);
        }

        public static int GetIdByCourseCode(string code)
        {
            return CourseCodeDAL.GetIdByCourseCode(code);
        }

        public static List<CourseCode>GetAllCourseCodesById(int id)
        {
            return CourseCodeDAL.GetAllCourseCodesById(id);
        }
        public static CourseCode CourseCodesById(int id)
        {
            return CourseCodeDAL.GetNewestCourseCodeById(id);
        }
        public static List<string> SearchCourseCodes(string code)
        {
            return CourseCodeDAL.SearchCourseCodes(code);
        }
        public static bool UpdateCourseCodes(List<CourseCode> coursecodes, int id)
        {
            return CourseCodeDAL.UpdateCourseCodes(coursecodes, id);
        }
    }
}
