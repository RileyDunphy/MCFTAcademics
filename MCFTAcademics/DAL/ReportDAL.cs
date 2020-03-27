using MCFTAcademics.BL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Common;

namespace MCFTAcademics.DAL
{
    public static class ReportDAL
    {
        public static List<ReportColumn> SelectReportData(string program, int semester, bool semesterReport)
        {
            SqlConnection conn = DbConn.GetConnection();
            
            List<ReportColumn> reportData = new List<ReportColumn>();

            try
            {
                conn.Open();
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectReportData", conn);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                selectCommand.Parameters.AddWithValue("@program", program);
                selectCommand.Parameters.AddWithValue("@semester", semester);
                SqlDataReader reader = selectCommand.ExecuteReader();

                //loop through the resultset
                while (reader.Read())
                {
                    var studentId = (int)reader["studentId"];
                    var firstName = (string)reader["firstName"];
                    var lastName = (string)reader["lastName"];
                    var grade = (decimal)reader["grade"];
                    var supplemental = (bool)reader["isSupplemental"];
                    var courseId = (int)reader["courseId"];
                    var name = (string)reader["name"];
                    var prog = (string)reader["program"];
                    var courseCode = (string)reader["courseCode"];
                    var startDate = (DateTime)reader["startDate"];
                    var endDate = (DateTime)reader["endDate"];

                    ReportColumn reportRecord=new ReportColumn(studentId,firstName,lastName,grade,supplemental,courseId,name,prog,courseCode,startDate,endDate,semester,semesterReport);

                    reportData.Add(reportRecord);

                }
                return reportData;
                
            }
            catch (Exception ex)
            {
               
            }
            finally
            {
                conn.Close();
            }
            return reportData;
        }
        public static List<ReportColumn> SelectReportDataYearly(string program, int semester, int year, bool semesterReport)
        {
            SqlConnection conn = DbConn.GetConnection();

            List<ReportColumn> reportData = new List<ReportColumn>();

            try
            {
                conn.Open();
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectReportData2", conn);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                selectCommand.Parameters.AddWithValue("@program", program);
                selectCommand.Parameters.AddWithValue("@semester", semester);
                selectCommand.Parameters.AddWithValue("@year", year);
                SqlDataReader reader = selectCommand.ExecuteReader();

                //loop through the resultset
                while (reader.Read())
                {
                    var studentId = (int)reader["studentId"];
                    var firstName = (string)reader["firstName"];
                    var lastName = (string)reader["lastName"];
                    var grade = (decimal)reader["grade"];
                    var supplemental = (bool)reader["isSupplemental"];
                    var courseId = (int)reader["courseId"];
                    var name = (string)reader["name"];
                    var prog = (string)reader["program"];
                    var courseCode = (string)reader["courseCode"];
                    var startDate = (DateTime)reader["startDate"];
                    var endDate = (DateTime)reader["endDate"];

                    ReportColumn reportRecord = new ReportColumn(studentId, firstName, lastName, grade, supplemental, courseId, name, prog, courseCode, startDate, endDate, semester, semesterReport);

                    reportData.Add(reportRecord);

                }
                return reportData;

            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }
            return reportData;
        }
    }
}
