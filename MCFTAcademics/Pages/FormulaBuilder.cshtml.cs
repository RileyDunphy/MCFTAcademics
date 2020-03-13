using Flee.PublicTypes;
using MCFTAcademics.BL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace MCFTAcademics
{
    public class FormulaBuilderModel : PageModel
    {
        public string alertMessage { get; set; }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            string formula = Request.Form["formula"];
            //Test out the formula when you get it so that you know the formula is valid
            //It will throw an exception if not and you can tell the user the formula is invalid
            try
            {
                ExpressionContext context = new ExpressionContext();
                context.Imports.AddType(typeof(System.Math));
                context.Variables["c"] = 1m;
                context.Variables["b"] = 2m;
                context.Variables["a"] = 3m;
                IDynamicExpression eDynamic = context.CompileDynamic(formula);
                decimal result = (decimal)eDynamic.Evaluate();
            }
            catch(Exception ex)
            {
                this.alertMessage = "Please enter a valid formula";
                return Page();
            }
            Grade.UpdateFormula(formula);
            this.alertMessage = "Formula updated";
            return Page();
        }
    }
}