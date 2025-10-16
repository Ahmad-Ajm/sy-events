using System;
using System.IO;
using System.Threading.Tasks;
using EventManagement.Blobs;
using EventManagement.Events;
using Microsoft.AspNetCore.Http;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.Content;

namespace EventManagement;

public class EventImageAppService : ApplicationService, IEventImageAppService
{
    private readonly IBlobContainer<EventImageContainer> _blobContainer;

    public EventImageAppService(IBlobContainer<EventImageContainer> blobContainer)
    {
        _blobContainer = blobContainer;
    }

    public async Task UploadAsync(Guid eventId, IFormFile file)
    {
        using var stream = file.OpenReadStream();
        await _blobContainer.SaveAsync(GetBlobName(eventId), stream, overrideExisting: true);
    }

    public async Task<byte[]> GetAsync(Guid eventId)
    {
        return await _blobContainer.GetAllBytesAsync(GetBlobName(eventId));
    }

    public async Task DeleteAsync(Guid eventId)
    {
        await _blobContainer.DeleteAsync(GetBlobName(eventId));
    }

    private static string GetBlobName(Guid eventId) => $"{eventId}.img";
}


