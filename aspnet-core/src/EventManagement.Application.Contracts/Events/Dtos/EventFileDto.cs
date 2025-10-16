// تعليق: DTO لملفات الفعالية
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace EventManagement.Events.Dtos
{
    public class EventFileDto : FullAuditedEntityDto<Guid>
    {
        public Guid EventId { get; set; }
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        public string FilePath { get; set; }
        public string FileType { get; set; } // Image, PDF, Text
        public string MimeType { get; set; }
        public long FileSize { get; set; }
        public int DisplayOrder { get; set; }
        
        // للصور
        public string ThumbnailPath { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        
        // معلومات إضافية
        public string DownloadUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public string FileSizeFormatted { get; set; } // مثل: "2.5 MB"
    }
    
    // تعليق: DTO لرفع ملفات متعددة
    public class UploadMultipleFilesInput
    {
        public Guid EventId { get; set; }
        // الملفات سترسل عبر IFormFile من Controller
    }
    
    // تعليق: نتيجة رفع الملفات
    public class UploadFilesResultDto
    {
        public int SuccessCount { get; set; }
        public int FailedCount { get; set; }
        public List<string> Errors { get; set; } = new();
        public List<EventFileDto> UploadedFiles { get; set; } = new();
    }
}

