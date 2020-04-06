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
        [HttpGet("DownloadTranscript/{reportName}", Name = "DownloadTranscript")]
        public async Task<IActionResult> DownloadTranscriptAsync(string reportName)
        {
            var bytes = await System.IO.File.ReadAllBytesAsync("./Reports/" + reportName + ".pdf");
            // If you give this a filename, it demands a download instead of displaying inline.
            return File(bytes, "application/pdf");
        }
    }
}