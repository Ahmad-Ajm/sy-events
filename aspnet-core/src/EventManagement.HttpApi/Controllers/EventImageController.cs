using System;
using System.Threading.Tasks;
using EventManagement.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace EventManagement.Controllers;

[Route("api/app/event-image")] 
[ApiExplorerSettings(IgnoreApi = true)]
public class EventImageController : AbpController
{
    private readonly IEventImageAppService _eventImageAppService;

    public EventImageController(IEventImageAppService eventImageAppService)
    {
        _eventImageAppService = eventImageAppService;
    }

    [HttpPost]
    [Route("{eventId}")]
    public async Task UploadAsync(Guid eventId, IFormFile file)
    {
        await _eventImageAppService.UploadAsync(eventId, file);
    }

    [HttpGet]
    [Route("{eventId}")]
    public async Task<FileContentResult> GetAsync(Guid eventId)
    {
        var bytes = await _eventImageAppService.GetAsync(eventId);
        return File(bytes, "image/jpeg");
    }

    [HttpDelete]
    [Route("{eventId}")]
    public Task DeleteAsync(Guid eventId) => _eventImageAppService.DeleteAsync(eventId);
}


