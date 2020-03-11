using Flee.PublicTypes;
using MCFTAcademics.BL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MCFTAcademics
{
    public class FormulaBuilderModel : PageModel
    {
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            string formula = Request.Form["formula"];
            Grade.UpdateFormula(formula);
            return Page();
        }
    }
}