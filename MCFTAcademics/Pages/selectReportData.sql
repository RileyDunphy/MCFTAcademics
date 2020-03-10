USE [mcftacademics]
GO
/****** Object:  StoredProcedure [dbo].[selectReportData]    Script Date: 2/27/2020 9:09:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create procedure [dbo].[selectReportData] 
(
@program varchar,
@semester int
)
AS
select Students.studentId, Students.firstName, Students.lastName, 
Grades.grade, Grades.isSupplemental,
Courses.courseId, Courses.name, Courses.program,
CourseCodes.courseCode, CourseCodes.semester, CourseCodes.startDate, CourseCodes.endDate
from Students inner join Grades on Students.studentId=Grades.studentId
              inner join Courses on Grades.courseId= Courses.courseId
              inner join CourseCodes on Courses.courseId=CourseCodes.courseId 
                    where Course.program=@program and CourseCodes.semester=@semester;