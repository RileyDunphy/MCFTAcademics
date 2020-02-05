using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MCFTAcademics.Controllers
{
    [ApiController]
    // XXX: We have to explicitly state authorizations here until we add
    // automatic authorization to startup, the current ones only apply to
    // Razor. For now, just mark this authorize.
    [Authorize]
    public class TranscriptController : ControllerBase
    {
        [Route("DownloadTranscript")]
        [HttpGet]
        public async Task<IActionResult> DownloadTranscriptAsync()
        {
            var bytes = await System.IO.File.ReadAllBytesAsync("./Reports/someReport.pdf");
            return File(bytes, "application/pdf", "Example PDF");
        }
    }
}