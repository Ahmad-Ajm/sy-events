// تعليق: Controller لرفع ملفات الفعالية - يدعم رفع متعدد (3 صور + PDF + TXT)
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Domain.Repositories;
using EventManagement.Events;
using EventManagement.Events.Dtos; // DTOs for upload results and file data
using EventManagement.Permissions; // Permission constants
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace EventManagement.Controllers
{
    [ApiController]
    [Route("api/app/event/{eventId}/files")]
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class EventFileController : AbpControllerBase
    {
        private readonly IRepository<EventFile, Guid> _fileRepository;
        private readonly IRepository<Event, Guid> _eventRepository;
        private const long MaxImageSize = 5 * 1024 * 1024; // 5MB
        private const long MaxPdfSize = 10 * 1024 * 1024; // 10MB
        private const long MaxTextSize = 2 * 1024 * 1024; // 2MB

        public EventFileController(
            IRepository<EventFile, Guid> fileRepository,
            IRepository<Event, Guid> eventRepository)
        {
            _fileRepository = fileRepository;
            _eventRepository = eventRepository;
        }

        // تعليق: رفع ملفات متعددة (3 صور + 1 PDF + 1 TXT)
        [HttpPost("upload-multiple")]
        [Authorize(EventManagementPermissions.Events.Edit)]
        public async Task<UploadFilesResultDto> UploadMultiple(Guid eventId, [FromForm] List<IFormFile> files)
        {
            // تعليق: التحقق من وجود الفعالية
            var eventEntity = await _eventRepository.GetAsync(eventId);
            
            var result = new UploadFilesResultDto
            {
                UploadedFiles = new List<EventFileDto>(),
                Errors = new List<string>()
            };

            if (files == null || files.Count == 0)
            {
                result.Errors.Add("لم يتم تحديد أي ملفات");
                return result;
            }

            // تعليق: تصنيف الملفات حسب النوع
            var images = files.Where(f => IsImage(f)).ToList();
            var pdfs = files.Where(f => IsPdf(f)).ToList();
            var texts = files.Where(f => IsText(f)).ToList();

            // تعليق: التحقق من الحدود
            if (images.Count > 3)
            {
                result.Errors.Add($"يمكن رفع 3 صور فقط (تم تحديد {images.Count})");
            }
            if (pdfs.Count > 1)
            {
                result.Errors.Add($"يمكن رفع ملف PDF واحد فقط (تم تحديد {pdfs.Count})");
            }
            if (texts.Count > 1)
            {
                result.Errors.Add($"يمكن رفع ملف نصي واحد فقط (تم تحديد {texts.Count})");
            }

            if (result.Errors.Any())
            {
                result.FailedCount = files.Count;
                return result;
            }

            // تعليق: رفع الصور (حتى 3)
            foreach (var image in images.Take(3))
            {
                try
                {
                    var uploaded = await UploadFile(eventId, image, "Image", MaxImageSize);
                    result.UploadedFiles.Add(uploaded);
                    result.SuccessCount++;
                }
                catch (Exception ex)
                {
                    result.Errors.Add($"فشل رفع {image.FileName}: {ex.Message}");
                    result.FailedCount++;
                }
            }

            // تعليق: رفع PDF (واحد فقط)
            if (pdfs.Any())
            {
                try
                {
                    var uploaded = await UploadFile(eventId, pdfs.First(), "PDF", MaxPdfSize);
                    result.UploadedFiles.Add(uploaded);
                    result.SuccessCount++;
                }
                catch (Exception ex)
                {
                    result.Errors.Add($"فشل رفع PDF: {ex.Message}");
                    result.FailedCount++;
                }
            }

            // تعليق: رفع ملف نصي (واحد فقط)
            if (texts.Any())
            {
                try
                {
                    var uploaded = await UploadFile(eventId, texts.First(), "Text", MaxTextSize);
                    result.UploadedFiles.Add(uploaded);
                    result.SuccessCount++;
                }
                catch (Exception ex)
                {
                    result.Errors.Add($"فشل رفع ملف نصي: {ex.Message}");
                    result.FailedCount++;
                }
            }

            return result;
        }

        // تعليق: رفع ملف واحد
        private async Task<EventFileDto> UploadFile(Guid eventId, IFormFile file, string fileType, long maxSize)
        {
            // التحقق من الحجم
            if (file.Length > maxSize)
            {
                throw new UserFriendlyException($"حجم الملف يتجاوز الحد الأقصى ({maxSize / (1024 * 1024)} MB)");
            }

            // تعليق: إنشاء مجلد الفعالية
            var uploadDir = Path.Combine("upload", eventId.ToString());
            if (!Directory.Exists(uploadDir))
            {
                Directory.CreateDirectory(uploadDir);
            }

            // تعليق: توليد اسم فريد للملف
            var fileExtension = Path.GetExtension(file.FileName);
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadDir, uniqueFileName);

            // تعليق: حفظ الملف
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // تعليق: حفظ معلومات الملف في قاعدة البيانات
            var eventFile = new EventFile(
                GuidGenerator.Create(),
                eventId,
                uniqueFileName,
                file.FileName,
                Path.Combine(eventId.ToString(), uniqueFileName),
                fileType,
                file.ContentType,
                file.Length
            );

            await _fileRepository.InsertAsync(eventFile);

            return ObjectMapper.Map<EventFile, EventFileDto>(eventFile);
        }

        // تعليق: التحقق من نوع الملف
        private bool IsImage(IFormFile file)
        {
            var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/webp" };
            return allowedTypes.Contains(file.ContentType?.ToLower());
        }

        private bool IsPdf(IFormFile file)
        {
            return file.ContentType?.ToLower() == "application/pdf";
        }

        private bool IsText(IFormFile file)
        {
            var allowedTypes = new[] { "text/plain", "text/markdown", "text/csv" };
            return allowedTypes.Contains(file.ContentType?.ToLower());
        }
    }
}

