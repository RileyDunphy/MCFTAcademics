USE [mcftacademics]
GO
/****** Object:  User [admin]    Script Date: 3/30/2020 9:28:50 AM ******/
CREATE USER [admin] FOR LOGIN [admin] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [admin]
GO
/****** Object:  Table [dbo].[CourseCodes]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CourseCodes](
	[courseId] [int] NOT NULL,
	[courseCode] [varchar](25) NOT NULL,
	[startDate] [date] NULL,
	[endDate] [date] NULL,
	[dateRevised] [date] NULL,
	[semester] [int] NULL,
 CONSTRAINT [PK_CourseCodes] PRIMARY KEY CLUSTERED 
(
	[courseId] ASC,
	[courseCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Courses]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Courses](
	[courseId] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](max) NOT NULL,
	[program] [varchar](max) NULL,
	[credit] [decimal](18, 0) NOT NULL,
	[dateDeveloped] [date] NULL,
	[dateLastRevised] [date] NULL,
	[lectureHours] [int] NULL,
	[labHours] [int] NULL,
	[examHours] [int] NULL,
	[accreditation] [bit] NULL,
	[Description] [varchar](max) NULL,
	[revisionNumber] [decimal](18, 0) NULL,
	[totalHours]  AS (([lectureHours]+[labHours])+[examHours]),
 CONSTRAINT [PK_Courses] PRIMARY KEY CLUSTERED 
(
	[courseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CourseStaff]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CourseStaff](
	[userId] [int] NOT NULL,
	[courseId] [int] NOT NULL,
	[startDate] [date] NULL,
	[endDate] [date] NULL,
	[tempName] [varchar](max) NULL,
	[instructorType] [varchar](10) NULL,
 CONSTRAINT [PK_CourseStaff] PRIMARY KEY CLUSTERED 
(
	[userId] ASC,
	[courseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Formulas]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Formulas](
	[Formula] [varchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Grades]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Grades](
	[studentId] [int] NOT NULL,
	[courseId] [int] NOT NULL,
	[grade] [decimal](18, 0) NULL,
	[given] [datetime] NULL,
	[lock] [bit] NULL,
	[hoursAttended] [decimal](18, 0) NULL,
	[isSupplemental] [bit] NULL,
	[comment] [varchar](max) NULL,
	[unlockedUntil] [datetime2](7) NULL,
 CONSTRAINT [PK_Grades] PRIMARY KEY CLUSTERED 
(
	[studentId] ASC,
	[courseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Prerequisites]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Prerequisites](
	[courseId] [int] NOT NULL,
	[prereqId] [int] NOT NULL,
	[isPrereq] [bit] NULL,
	[isCoreq] [bit] NULL,
 CONSTRAINT [PK_Prereqs] PRIMARY KEY CLUSTERED 
(
	[courseId] ASC,
	[prereqId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[userId] [int] NOT NULL,
	[roleId] [int] IDENTITY(1,1) NOT NULL,
	[roleName] [varchar](max) NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[userId] ASC,
	[roleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Students]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Students](
	[studentId] [int] IDENTITY(1,1) NOT NULL,
	[firstName] [varchar](max) NOT NULL,
	[lastName] [varchar](max) NOT NULL,
	[studentCode] [varchar](max) NOT NULL,
	[program] [varchar](max) NOT NULL,
	[admissionDate] [datetime2](7) NULL,
	[graduationDate] [datetime2](7) NULL,
	[academicAccommodation] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[studentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[userId] [int] IDENTITY(1,1) NOT NULL,
	[username] [varchar](max) NOT NULL,
	[password] [varchar](max) NOT NULL,
	[realName] [varchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[userId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Students] ADD  CONSTRAINT [default_academicAccommodation]  DEFAULT ((0)) FOR [academicAccommodation]
GO
ALTER TABLE [dbo].[CourseCodes]  WITH CHECK ADD  CONSTRAINT [FK__CourseCod__cours__440B1D61] FOREIGN KEY([courseId])
REFERENCES [dbo].[Courses] ([courseId])
GO
ALTER TABLE [dbo].[CourseCodes] CHECK CONSTRAINT [FK__CourseCod__cours__440B1D61]
GO
ALTER TABLE [dbo].[CourseStaff]  WITH CHECK ADD  CONSTRAINT [FK__CourseSta__cours__44FF419A] FOREIGN KEY([courseId])
REFERENCES [dbo].[Courses] ([courseId])
GO
ALTER TABLE [dbo].[CourseStaff] CHECK CONSTRAINT [FK__CourseSta__cours__44FF419A]
GO
ALTER TABLE [dbo].[CourseStaff]  WITH CHECK ADD FOREIGN KEY([userId])
REFERENCES [dbo].[Users] ([userId])
GO
ALTER TABLE [dbo].[Grades]  WITH CHECK ADD  CONSTRAINT [FK__Grades__courseId__46E78A0C] FOREIGN KEY([courseId])
REFERENCES [dbo].[Courses] ([courseId])
GO
ALTER TABLE [dbo].[Grades] CHECK CONSTRAINT [FK__Grades__courseId__46E78A0C]
GO
ALTER TABLE [dbo].[Grades]  WITH CHECK ADD FOREIGN KEY([studentId])
REFERENCES [dbo].[Students] ([studentId])
GO
ALTER TABLE [dbo].[Prerequisites]  WITH CHECK ADD  CONSTRAINT [FK__Prerequis__cours__48CFD27E] FOREIGN KEY([courseId])
REFERENCES [dbo].[Courses] ([courseId])
GO
ALTER TABLE [dbo].[Prerequisites] CHECK CONSTRAINT [FK__Prerequis__cours__48CFD27E]
GO
ALTER TABLE [dbo].[Prerequisites]  WITH CHECK ADD  CONSTRAINT [FK__Prerequis__prere__49C3F6B7] FOREIGN KEY([prereqId])
REFERENCES [dbo].[Courses] ([courseId])
GO
ALTER TABLE [dbo].[Prerequisites] CHECK CONSTRAINT [FK__Prerequis__prere__49C3F6B7]
GO
ALTER TABLE [dbo].[Roles]  WITH CHECK ADD FOREIGN KEY([userId])
REFERENCES [dbo].[Users] ([userId])
GO
ALTER TABLE [dbo].[CourseCodes]  WITH CHECK ADD  CONSTRAINT [Check_CourseCode_DateOrder] CHECK  (([endDate] IS NULL AND [startDate] IS NULL OR [endDate]>=[startDate]))
GO
ALTER TABLE [dbo].[CourseCodes] CHECK CONSTRAINT [Check_CourseCode_DateOrder]
GO
ALTER TABLE [dbo].[CourseStaff]  WITH CHECK ADD  CONSTRAINT [Check_CourseStaff_DateOrder] CHECK  (([endDate] IS NULL AND [startDate] IS NULL OR [endDate]>=[startDate]))
GO
ALTER TABLE [dbo].[CourseStaff] CHECK CONSTRAINT [Check_CourseStaff_DateOrder]
GO
ALTER TABLE [dbo].[Grades]  WITH CHECK ADD  CONSTRAINT [Check_Grade_Range] CHECK  (([grade]<=(100) AND [grade]>=(0)))
GO
ALTER TABLE [dbo].[Grades] CHECK CONSTRAINT [Check_Grade_Range]
GO
ALTER TABLE [dbo].[Students]  WITH CHECK ADD  CONSTRAINT [Check_Students_DateOrder] CHECK  (([admissionDate] IS NULL OR [graduationDate]>=[admissionDate]))
GO
ALTER TABLE [dbo].[Students] CHECK CONSTRAINT [Check_Students_DateOrder]
GO
/****** Object:  StoredProcedure [dbo].[AddCourseCode]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddCourseCode](
@id int,
@code varchar(max),
@startDate Date,
@endDate Date,
@semester int
)
AS
insert into CourseCodes (courseId, courseCode,startDate,endDate,dateRevised, semester) values (@id, @code,@startDate, @endDate,getDate(), @semester)
GO
/****** Object:  StoredProcedure [dbo].[Create_User]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Create_User]
(
@userPassword varchar(max),
@userName varchar(max),
@userRealName varchar(max)
)
AS
BEGIN
SET NOCOUNT OFF; -- so row count works still
insert into users (username, realName, password) values (@userName, @userRealName, @userPassword);
select * from users where Users.userId = scope_identity();
end
GO
/****** Object:  StoredProcedure [dbo].[DropAllCourseCodesById]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[DropAllCourseCodesById]
(
@id int
)
as
delete from coursecodes where courseId = @id
GO
/****** Object:  StoredProcedure [dbo].[DropAllPrereqsById]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DropAllPrereqsById]
(@id int)
AS
DELETE from Prerequisites where courseId = @id
GO
/****** Object:  StoredProcedure [dbo].[DropAllStaffById]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DropAllStaffById](
@courseId int
)
AS
DELETE FROM coursestaff WHERE courseId = @courseId;
GO
/****** Object:  StoredProcedure [dbo].[Get_All_Grades]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Get_All_Grades]
AS
BEGIN
SET NOCOUNT ON;
-- SELECT studentId, courseId, grade, given, lock, hoursAttended, isSupplemental
-- FROM dbo.Grades
SELECT
	-- This hopes they have no naming conflicts. Grade:
	studentId, courses.courseId, grade, given, lock, hoursAttended, isSupplemental,comment, unlockedUntil,
	-- Course:
	name, program, credit, dateDeveloped, dateLastRevised, lectureHours, labHours, examHours, accreditation, Description, revisionNumber, totalHours
FROM dbo.Grades
INNER JOIN dbo.Courses on dbo.Courses.courseId = dbo.Grades.courseId
end
GO
/****** Object:  StoredProcedure [dbo].[Get_All_Users]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Get_All_Users]
AS
BEGIN
SET NOCOUNT ON;
SELECT userId, username, password, realName
FROM dbo.Users
end
GO
/****** Object:  StoredProcedure [dbo].[Get_AllCourses]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Get_AllCourses]
AS
BEGIN
select * from courses where courses.courseId in (select courseId from coursecodes)
END
GO
/****** Object:  StoredProcedure [dbo].[Get_AllCoursesANDCourseCodes]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Get_AllCoursesANDCourseCodes]
AS
BEGIN
select * from courses, coursecodes where courses.courseid = coursecodes.courseid
END
GO
/****** Object:  StoredProcedure [dbo].[Get_Grades_ByStaff]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Get_Grades_ByStaff]
(
	@staffId int
)
AS
BEGIN
SET NOCOUNT ON;
---- xxx: return the staff here too?
--SELECT g.studentId, g.courseId, g.grade, g.given, g.lock, g.hoursAttended, g.isSupplemental
--FROM dbo.Grades g
--inner join dbo.CourseStaff cs on cs.courseId = g.courseId
---- XXX: seems wrong?
--where cs.userId = @staffId
SELECT
	-- This hopes they have no naming conflicts. Grade:
	studentId, courses.courseId, grade, given, lock, hoursAttended, isSupplemental, comment, unlockedUntil,
	-- Course:
	name, program, credit, dateDeveloped, dateLastRevised, lectureHours, labHours, examHours, accreditation, Description, revisionNumber, totalHours
FROM dbo.Grades
INNER JOIN dbo.Courses on dbo.Courses.courseId = dbo.Grades.courseId
INNER JOIN dbo.CourseStaff cs ON cs.courseId = dbo.Grades.courseId
-- XXX: seems wrong?
WHERE cs.userId = @staffId
end
GO
/****** Object:  StoredProcedure [dbo].[Get_RealName]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,Josh>
-- Create date: <Create Date, Jan 23, 2020>
-- Description:	<Description, Get the real name from username/userId>
-- =============================================

CREATE PROCEDURE [dbo].[Get_RealName]
(
@userIdentity VARCHAR(50)
)
AS
BEGIN
SET NOCOUNT ON;
SELECT  realName
FROM dbo.Users
WHERE username=@userIdentity or userId=@userIdentity
end



--i'll need three for the login mode: one to return real name from id/username, one to return user id from username, one to return all roles assosicated with a user id/name (id preferably)
GO
/****** Object:  StoredProcedure [dbo].[Get_User_ById]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,Josh>
-- Create date: <Create Date, Jan 23, 2020>
-- Description:	<Description, Get the userId from username>
-- =============================================

CREATE PROCEDURE [dbo].[Get_User_ById]
(
@userIdentity int
)
AS
BEGIN
SET NOCOUNT ON;
SELECT userId, username, password, realName
FROM dbo.Users
WHERE userId = @userIdentity
end
GO
/****** Object:  StoredProcedure [dbo].[Get_User_ByName]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,Josh>
-- Create date: <Create Date, Jan 23, 2020>
-- Description:	<Description, Get the userId from username>
-- =============================================

CREATE PROCEDURE [dbo].[Get_User_ByName]
(
@username VARCHAR(50)
)
AS
BEGIN
SET NOCOUNT ON;
SELECT userId, username, password, realName
FROM dbo.Users
WHERE username = @username
end
GO
/****** Object:  StoredProcedure [dbo].[Get_UserId]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,Josh>
-- Create date: <Create Date, Jan 23, 2020>
-- Description:	<Description, Get the userId from username>
-- =============================================

CREATE PROCEDURE [dbo].[Get_UserId]
(
@username VARCHAR(50)
)
AS
BEGIN
SET NOCOUNT ON;
SELECT userId, username, password, realName
FROM dbo.Users
WHERE username = @username
end
GO
/****** Object:  StoredProcedure [dbo].[Get_UserRole_ById]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Get_UserRole_ById]
(
@userIdentity int,
@roleIdentity int
)
AS
BEGIN
SET NOCOUNT ON;
SELECT  Roles.userId, Roles.roleId, Roles.roleName
FROM dbo.Roles
WHERE Roles.userId = @userIdentity AND Roles.roleId = @roleIdentity
end
GO
/****** Object:  StoredProcedure [dbo].[Get_UserRoles]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,Josh>
-- Create date: <Create Date, Jan 23, 2020>
-- Description:	<Description, Get the userId from username>
-- =============================================

CREATE PROCEDURE [dbo].[Get_UserRoles]
(
@userIdentity VARCHAR(50)
)
AS
BEGIN
SET NOCOUNT ON;
SELECT  Users.username, Users.userId, Roles.roleId,Roles.roleName
FROM dbo.Users inner join Roles on Users.userId=Roles.userId
WHERE username=@userIdentity or Users.userId=@userIdentity
end

--i'll need three for the login mode: one to return real name from id/username, one to return user id from username, one to return all roles assosicated with a user id/name (id preferably)
GO
/****** Object:  StoredProcedure [dbo].[Get_UserRoles_ById]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Get_UserRoles_ById]
(
@userIdentity int
)
AS
BEGIN
SET NOCOUNT ON;
SELECT  Roles.userId, Roles.roleId, Roles.roleName
FROM dbo.Roles
WHERE Roles.userId = @userIdentity
end

--i'll need three for the login mode: one to return real name from id/username, one to return user id from username, one to return all roles assosicated with a user id/name (id preferably)
GO
/****** Object:  StoredProcedure [dbo].[GetAllInstructors]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllInstructors]
AS
select *, '' AS 'instructorType'
from users where userId IN (select userId from Roles where roleName = 'Instructor');
GO
/****** Object:  StoredProcedure [dbo].[GetClassRanks]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetClassRanks]
(
@program varchar(max),
@graduationDate Date
)
AS
select students.studentid as id, AVG(grades.grade) as average FROM students, grades where students.studentid = grades.studentid 
and students.program = @program and students.graduationDate = @graduationDate GROUP BY students.studentid ORDER BY AVG(grades.grade) DESC
GO
/****** Object:  StoredProcedure [dbo].[Grant_UserRole]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Grant_UserRole]
(
@userIdentity int,
@roleName varchar(max)
)
AS
BEGIN
SET NOCOUNT OFF;
INSERT INTO Roles (userId, roleName) VALUES (@userIdentity, @roleName);
-- Awkward, but return the row if we can
-- Best ways to return a generated identity:
-- https://stackoverflow.com/questions/42648/best-way-to-get-identity-of-inserted-row#42655
SELECT @userIdentity AS userId, @roleName AS roleName, SCOPE_IDENTITY() AS roleId FROM Roles;
end
GO
/****** Object:  StoredProcedure [dbo].[InsertAndEnrollStudent]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[InsertAndEnrollStudent](
@firstName varchar(max),
@lastName varchar(max),
@studentCode varchar(max),
@program varchar(max),
@gradDate datetime2(7),
@academicAccommodation bit
)
as
--First insert the student into the database
insert into students(firstName, lastName, studentCode, program, admissionDate, graduationDate, academicAccommodation)
values (@firstName, @lastName, @studentCode, @program, getDate(), @gradDate, @academicAccommodation);

--Then declare our variables and get the student id
DECLARE @studentid integer;
DECLARE @courseid integer;
set @studentid = (select top 1 IDENT_CURRENT('Students') from Students);

--Now make a cursor for all the courses in the student's program
DECLARE course_cursor CURSOR FOR     
SELECT courseid from courses where program = @program;

--Open the cursor
open course_cursor
FETCH NEXT FROM course_cursor INTO @courseid

--Insert an entry into grades with the student's id for every course that is in the cursor (courses in their program)
WHILE @@FETCH_STATUS = 0  
BEGIN  
      insert into grades (studentId, courseId, grade, given, hoursAttended, lock, isSupplemental) values (@studentid,@courseid,0, getDate(),0, 0,0)

      FETCH NEXT FROM course_cursor INTO @courseid
END 

CLOSE course_cursor
DEALLOCATE course_cursor

select @studentid as id
GO
/****** Object:  StoredProcedure [dbo].[InsertCourse]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertCourse]
(
@name VARCHAR(MAX),
@credit decimal,
@lectureHours int,
@labHours int,
@examHours int,
@description varchar(MAX),
@revisionNumber decimal,
@program varchar(max),
@accreditation bit
)
AS
INSERT INTO COURSES (name, credit, Description, lectureHours, labHours,examHours,revisionNumber, dateDeveloped, program, accreditation) 
values (@name, @credit, @description, @lectureHours, @labHours, @examHours, @revisionNumber, getDate(), @program, @accreditation)
GO
/****** Object:  StoredProcedure [dbo].[InsertPrereq]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertPrereq](
@courseId int,
@prereqId int,
@isPrereq bit,
@isCoreq bit
)
AS
insert into Prerequisites (courseid, prereqid, isPrereq, isCoreq)
values (@courseId, @prereqId, @isPrereq, @isCoReq)
GO
/****** Object:  StoredProcedure [dbo].[InsertStaff]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertStaff]
(
@courseId int,
@staffId int,
@type VARCHAR(10)
)
AS
INSERT INTO CourseStaff (courseId, userId, instructorType) VALUES (@courseId, @staffId, @type)
GO
/****** Object:  StoredProcedure [dbo].[InsertStudent]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[InsertStudent](
@firstName varchar(max),
@lastName varchar(max),
@studentCode varchar(max),
@program varchar(max),
@gradDate datetime2(7),
@academicAccommodation bit
)
as
insert into students(firstName, lastName, studentCode, program, admissionDate, graduationDate, academicAccommodation)
values (@firstName, @lastName, @studentCode, @program, getDate(), @gradDate, @academicAccommodation)
GO
/****** Object:  StoredProcedure [dbo].[Login_Validation]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Login_Validation]
(
@uname VARCHAR(50)
)
AS
BEGIN
SET NOCOUNT ON;
SELECT  username,[Password]
FROM dbo.Users
WHERE username=@uname
end
GO
/****** Object:  StoredProcedure [dbo].[Login_Validation_ById]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Login_Validation_ById]
(
@userIdentity int
)
AS
BEGIN
SET NOCOUNT ON;
SELECT  username,[Password]
FROM dbo.Users
WHERE userId=@userIdentity
end
GO
/****** Object:  StoredProcedure [dbo].[Revoke_UserRole_ById]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Revoke_UserRole_ById]
(
@userIdentity int,
@roleIdentity int
)
AS
BEGIN
SET NOCOUNT OFF;
DELETE FROM dbo.Roles
WHERE Roles.userId = @userIdentity AND Roles.roleId = @roleIdentity
end
GO
/****** Object:  StoredProcedure [dbo].[SearchCourseCodes]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SearchCourseCodes]
(
@code varchar(max)
)
AS
select * from CourseCodes where courseCode LIKE '%'+@code+'%'
GO
/****** Object:  StoredProcedure [dbo].[SelectAllCourses]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SelectAllCourses]
AS
SELECT * FROM Courses
GO;
GO
/****** Object:  StoredProcedure [dbo].[SelectAllGrades]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SelectAllGrades]
AS
select * from grades
GO
/****** Object:  StoredProcedure [dbo].[SelectAllStudentGrades]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,Josh>
-- Create date: <Create Date, Feb 03, 2020>
-- Description:	get list of all student grades and the respective course
-- =============================================

Create PROCEDURE [dbo].[SelectAllStudentGrades]

AS
BEGIN
SET NOCOUNT ON;
SELECT Grades.studentId, grade, isSupplemental, Grades.courseId, Courses.name, Courses.accreditation
FROM dbo.Grades inner join dbo.Courses on Grades.courseId=Courses.courseId
end
GO
/****** Object:  StoredProcedure [dbo].[SelectAllStudents]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SelectAllStudents]
AS
select * from students
GO
/****** Object:  StoredProcedure [dbo].[SelectAllStudentsByClass]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SelectAllStudentsByClass](
@program varchar(max),
@graduationDate Date
)
AS
select * from Students where program = @program and graduationDate = @graduationDate
GO
/****** Object:  StoredProcedure [dbo].[SelectAverageById]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[SelectAverageById] (
@id int
)
AS
select sum(grades.grade*courses.credit)/sum(courses.credit) as 'average'
from grades inner join courses on grades.courseid = courses.courseid
where studentId = @id
GO
/****** Object:  StoredProcedure [dbo].[SelectAverageByIdAndSemester]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[SelectAverageByIdAndSemester] 
(
@id int,
@semester int
)
AS
select sum(grades.grade*courses.credit)/sum(courses.credit) as 'average'
from grades inner join courses on grades.courseid = courses.courseid
where studentId = @id AND grades.courseId IN (select courseId from coursecodes where semester = @semester)
GO
/****** Object:  StoredProcedure [dbo].[SelectCourseById]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SelectCourseById] @id int
AS
SELECT *
from courses, coursecodes
where courses.courseid = coursecodes.courseid AND courses.courseId = @id
GO
/****** Object:  StoredProcedure [dbo].[SelectCourseCodesById]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SelectCourseCodesById](@courseId int)
AS
SELECT * FROM CourseCodes WHERE courseId = @courseId
GO
/****** Object:  StoredProcedure [dbo].[SelectCoursesByInstructor]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SelectCoursesByInstructor]
(
@userid int
)
AS
BEGIN
SET NOCOUNT ON;
SELECT *
from courses, coursecodes
where courses.courseid = coursecodes.courseid AND courses.courseid IN (select courseid from coursestaff where userid = @userid)
end
GO
/****** Object:  StoredProcedure [dbo].[SelectCoursesByInstructorNoCode]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SelectCoursesByInstructorNoCode]
(
@userid int
)
AS
BEGIN
SET NOCOUNT ON;
-- in because otherwise we get courses without a code and that's no good
SELECT *
from courses
where courses.courseid IN (select courseid from coursestaff where userid = @userid)
AND courses.courseId IN (select courseId from coursecodes)
end
GO
/****** Object:  StoredProcedure [dbo].[SelectFormula]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SelectFormula] as
select * from formulas
GO
/****** Object:  StoredProcedure [dbo].[SelectGradeByCourseAndStudent]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SelectGradeByCourseAndStudent]
(
@courseId int,
@studentId int
)
AS
select * from grades where studentId = @studentId AND courseId = @courseId
GO
/****** Object:  StoredProcedure [dbo].[SelectGradeRanges]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[SelectGradeRanges]
AS
select DISTINCT YEAR(given) from grades;
GO
/****** Object:  StoredProcedure [dbo].[SelectIdByCourseCode]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SelectIdByCourseCode]
(@code varchar(max))
AS
SELECT courseId FROM coursecodes where courseCode = @code
GO
/****** Object:  StoredProcedure [dbo].[SelectLastCourseInsert]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SelectLastCourseInsert]
AS
select IDENT_CURRENT('Courses') from Courses
GO;
GO
/****** Object:  StoredProcedure [dbo].[SelectLastStudentInsert]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[SelectLastStudentInsert] as
select top 1 IDENT_CURRENT('Students') from Students
GO
/****** Object:  StoredProcedure [dbo].[SelectNewestCourseCodeById]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SelectNewestCourseCodeById](@id int)
AS
SELECT TOP(1) * FROM CourseCodes WHERE courseId = @id ORDER  BY dateRevised DESC
GO
/****** Object:  StoredProcedure [dbo].[SelectPrereqsById]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SelectPrereqsById]
(
@id int
)
AS
SELECT * FROM Prerequisites WHERE courseId = @id
GO
/****** Object:  StoredProcedure [dbo].[SelectReportData]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[SelectReportData] 
(
@program varchar(20),
@semester int
)
AS
select Students.studentId, Students.firstName, Students.lastName, 
Grades.grade, Grades.isSupplemental, Grades.given,
Courses.courseId, Courses.name, Courses.program,
CourseCodes.courseCode, CourseCodes.semester, CourseCodes.startDate, CourseCodes.endDate
from Students inner join Grades on Students.studentId=Grades.studentId
              inner join Courses on Grades.courseId= Courses.courseId
              inner join CourseCodes on Courses.courseId=CourseCodes.courseId 
                    where Courses.program=@program and CourseCodes.semester=@semester;
GO
/****** Object:  StoredProcedure [dbo].[SelectReportData2]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[SelectReportData2] 
(
@program varchar(20),
@semester int,
@year int
)
AS
select Students.studentId, Students.firstName, Students.lastName, 
Grades.grade, Grades.isSupplemental, Grades.given,
Courses.courseId, Courses.name, Courses.program,
CourseCodes.courseCode, CourseCodes.semester, CourseCodes.startDate, CourseCodes.endDate
from Students inner join Grades on Students.studentId=Grades.studentId
              inner join Courses on Grades.courseId= Courses.courseId
              inner join CourseCodes on Courses.courseId=CourseCodes.courseId 
                    where Courses.program=@program and CourseCodes.semester=@semester and YEAR(Grades.given)=@year;
GO
/****** Object:  StoredProcedure [dbo].[SelectStaffByCourseIdAndType]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SelectStaffByCourseIdAndType](
@id int,
@type varchar(10))
AS
select userId, realName, @type AS 'instructorType' from Users where userid IN (select userId from coursestaff where courseId = @id and instructorType = @type)
GO
/****** Object:  StoredProcedure [dbo].[SelectStudentByStudentId]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SelectStudentByStudentId]
(
@studentId int
)
AS
select * from students where studentId = @studentId
GO
/****** Object:  StoredProcedure [dbo].[SelectStudentGradeById]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,Josh>
-- Create date: <Create Date, Feb 03, 2020>
-- Description:	Selects a students grade by thier ID
-- =============================================

CREATE PROCEDURE [dbo].[SelectStudentGradeById]
(
@studentId int
)
AS
BEGIN
SET NOCOUNT ON;
SELECT grade, isSupplemental, Grades.courseId, Courses.name,Courses.accreditation
FROM dbo.Grades inner join dbo.Courses on Grades.courseId=Courses.courseId
WHERE studentId = @studentId
end
GO
/****** Object:  StoredProcedure [dbo].[SelectStudentGradeById2]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,Josh>
-- Create date: <Create Date, Feb 03, 2020>
-- Description:	Selects a students grade by thier ID
-- =============================================

CREATE PROCEDURE [dbo].[SelectStudentGradeById2]
(
@studentId int
)
AS
BEGIN
SET NOCOUNT ON;
-- select * from grades where studentId = @studentId
SELECT
	-- This hopes they have no naming conflicts. Grade:
	studentId, courses.courseId, grade, given, lock, hoursAttended, isSupplemental, comment, unlockedUntil,
	-- Course:
	name, program, credit, dateDeveloped, dateLastRevised, lectureHours, labHours, examHours, accreditation, Description, revisionNumber, totalHours
FROM dbo.Grades
INNER JOIN dbo.Courses on dbo.Courses.courseId = dbo.Grades.courseId
WHERE studentId = @studentId
end
GO
/****** Object:  StoredProcedure [dbo].[SelectStudentGradeByIdAndSemester]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SelectStudentGradeByIdAndSemester]
(
@id int,
@semester int
)
AS
select * from grades where studentId = @id AND courseId IN (select courseId from coursecodes where semester = @semester)
GO
/****** Object:  StoredProcedure [dbo].[SelectStudentGradeByIdAndYear]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SelectStudentGradeByIdAndYear](
@id int,
@year int
)
AS
if @year = 1
begin
	select * from grades where studentId = @id AND courseId IN (select courseId from coursecodes where semester = 1 OR semester = 2);
END
else if @year = 2
BEGIN
	select * from grades where studentId = @id AND courseId IN (select courseId from coursecodes where semester = 3 OR semester = 4);
END
GO
/****** Object:  StoredProcedure [dbo].[SelectStudentsByCourseId]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[SelectStudentsByCourseId]
(
@courseId int
)
AS
select * from students where studentId IN (select studentId from GRADES where courseId = @courseId)
GO
/****** Object:  StoredProcedure [dbo].[SelectStudentsByStudentCode]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[SelectStudentsByStudentCode]
(
@studentCode varchar(max)
)
AS
select * from students where studentCode=@studentCode;
GO
/****** Object:  StoredProcedure [dbo].[SelectStudentsByStudentId]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create procedure [dbo].[SelectStudentsByStudentId]
(
@studentId int
)
AS
select * from students where studentId=@studentId;
GO
/****** Object:  StoredProcedure [dbo].[SelectSummerPracticumByStudentId]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SelectSummerPracticumByStudentId] (
@studentId int
)
AS
select * from grades where studentId = @studentId AND courseId IN(select courseId from coursecodes where semester = 0)
GO
/****** Object:  StoredProcedure [dbo].[ToggleGradeLock]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ToggleGradeLock]
(
@studentId int,
@courseId int,
@unlockUntil datetime2(7)
)
AS
DECLARE
@lock BIT
SET @lock = (SELECT lock FROM GRADES WHERE studentId = @studentId AND courseId = @courseId);
	IF(@lock = 1)
		BEGIN
			UPDATE GRADES SET lock = 0, unlockedUntil = @unlockUntil WHERE studentId = @studentId and courseId = @courseId;
		END
	ELSE
		BEGIN
			UPDATE GRADES SET lock = 1, unlockedUntil = null WHERE studentId = @studentId and courseId = @courseId;
		END
GO
/****** Object:  StoredProcedure [dbo].[Update_Password]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Update_Password]
(
@userIdentity int,
@userPassword varchar(max)
)
AS
BEGIN
SET NOCOUNT OFF; -- so row count works still
update users set password = @userPassword where userId = @userIdentity;
end
GO
/****** Object:  StoredProcedure [dbo].[Update_Profile]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Update_Profile]
(
@userIdentity int,
@userName varchar(max),
@userRealName varchar(max)
)
AS
BEGIN
SET NOCOUNT OFF; -- so row count works still
update users set username = @userName, realName = @userRealName where userId = @userIdentity;
end
GO
/****** Object:  StoredProcedure [dbo].[UpdateCourseByid]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateCourseByid]
(
@id int,
@name VARCHAR(MAX),
@credit decimal,
@lectureHours int,
@labHours int,
@examHours int,
@description varchar(MAX),
@revisionNumber decimal,
@program varchar(max),
@accreditation bit
)
AS
UPDATE courses
set name = @name, credit = @credit, lectureHours = @lectureHours, labHours = @labHours, examHours = @examHours, Description = @description, dateLastRevised = getDate(), revisionNumber = @revisionNumber, program = @program, accreditation = @accreditation
WHERE courseId = @id
GO
/****** Object:  StoredProcedure [dbo].[UpdateFormula]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[UpdateFormula]
(
@formula varchar(max)
)
as
update formulas set formula = @formula
GO
/****** Object:  StoredProcedure [dbo].[UpdateGradeByStudentIdAndCourseId]    Script Date: 3/30/2020 9:28:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateGradeByStudentIdAndCourseId]
(
@studentId int,
@courseId int,
@supplemental bit,
@grade decimal,
@comment varchar(max)
)
AS
UPDATE Grades SET grade = @grade, comment = @comment, isSupplemental = @supplemental, given = getDate() WHERE studentId = @studentId AND courseId = @courseId
GO
