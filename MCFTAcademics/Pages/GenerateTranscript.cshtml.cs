﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Data.SqlClient;
using MCFTAcademics.DAL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using MCFTAcademics.BL;
using System.IO;
using Rotativa;
using Microsoft.AspNetCore.Authorization;

namespace MCFTAcademics
{
    // Subject matter expert (XXX: what role?)
    [Authorize(Roles = "Admin")]
    public class GenerateTranscript : PageModel
    {

        public ActionResult OnGetAjax(int studentId,bool official, string esig,string type)
        {
            try
            {

                Student s = Student.GetStudent(studentId);

                Transcript t = new Transcript(s, official, esig, type, DateTime.Now);

                string path = t.GenerateTranscript();
                path = path.Replace("./Reports/", "");
                path = path.Replace(".pdf", "");
                return new JsonResult(path);
            }
            catch(Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }


    }




            
        
}
