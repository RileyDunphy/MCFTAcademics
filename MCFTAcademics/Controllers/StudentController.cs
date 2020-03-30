using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCFTAcademics.BL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MCFTAcademics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        
        // POST: api/Student
        [HttpPost]
        public ActionResult<Student> Post([FromBody] Student student)
        {
            int id = student.AddStudent();
            Student newStudent = new Student(id, student.FirstName, student.LastName, student.StudentCode, student.Program, DateTime.Now, student.GraduationDate, student.AcademicAccommodation);
            return newStudent; 
        }

    }
}
