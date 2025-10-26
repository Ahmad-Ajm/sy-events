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
        // حفظ اسم الملف الأصلي ضمن الامتداد، وتخزينه تحت upload/{eventId}/
        var extension = Path.GetExtension(file.FileName);
        var blobName = GetBlobName(eventId, extension);
        using var stream = file.OpenReadStream();
        await _blobContainer.SaveAsync(blobName, stream, overrideExisting: true);
    }

    public async Task<byte[]> GetAsync(Guid eventId)
    {
        // محاولات لاسترجاع الامتدادات الشائعة
        foreach (var ext in new[]{ ".jpg", ".jpeg", ".png", ".webp", ".img" })
        {
            var name = GetBlobName(eventId, ext);
            try
            {
                return await _blobContainer.GetAllBytesAsync(name);
            }
            catch { /* تجاهل وجرب التالي */ }
        }
        throw new FileNotFoundException("Event image not found");
    }

    public async Task DeleteAsync(Guid eventId)
    {
        foreach (var ext in new[]{ ".jpg", ".jpeg", ".png", ".webp", ".img" })
        {
            await _blobContainer.DeleteAsync(GetBlobName(eventId, ext));
        }
    }

    private static string GetBlobName(Guid eventId, string extension)
        => $"{eventId}/{eventId}{extension}";
}


