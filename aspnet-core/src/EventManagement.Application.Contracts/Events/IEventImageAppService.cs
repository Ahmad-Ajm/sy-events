using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Volo.Abp.Application.Services;

namespace EventManagement.Events;

public interface IEventImageAppService : IApplicationService
{
    Task UploadAsync(Guid eventId, IFormFile file);
    Task<byte[]> GetAsync(Guid eventId);
    Task DeleteAsync(Guid eventId);
}


