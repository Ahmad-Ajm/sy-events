using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using EventManagement.Events;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace EventManagement.Controllers
{
    /// <summary>
    /// API Controller لرفع ملفات متعددة للفعاليات
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/app/event/{eventId}/files")] 
    public class EventFilesController : AbpController
    {
        private readonly IEventFilesAppService _service;

        public EventFilesController(IEventFilesAppService service)
        {
            _service = service;
        }

        /// <summary>
        /// رفع ملفات متعددة لفعالية معينة
        /// </summary>
        [HttpPost("upload-multiple")]
        public async Task<IActionResult> UploadMultiple(Guid eventId, [FromForm] IFormFile[] files)
        {
            await _service.UploadMultipleAsync(eventId, files);
            return NoContent();
        }
    }
}



