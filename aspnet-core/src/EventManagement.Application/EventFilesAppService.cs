using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using EventManagement.Blobs;
using EventManagement.Events;

namespace EventManagement.Events
{
    /// <summary>
    /// خدمة تطبيقية لإدارة ملفات الفعاليات المتعددة
    /// </summary>
    public class EventFilesAppService : ApplicationService, IEventFilesAppService
    {
        private readonly IBlobContainer<EventImageContainer> _container;

        public EventFilesAppService(IBlobContainer<EventImageContainer> container)
        {
            _container = container;
        }

        /// <summary>
        /// رفع ملفات متعددة لفعالية معينة - يتم حفظ كل ملف تحت مسار eventId/index.ext
        /// </summary>
        public async Task UploadMultipleAsync(Guid eventId, IFormFile[] files)
        {
            if (files == null || files.Length == 0) return;
            var index = 0;
            foreach (var file in files)
            {
                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                var safeExt = string.IsNullOrWhiteSpace(ext) ? ".bin" : ext;
                var name = $"{eventId}/{++index}{safeExt}";
                using var stream = file.OpenReadStream();
                await _container.SaveAsync(name, stream, overrideExisting: true);
            }
        }
    }
}



