// تعليق: كيان ملفات الفعالية - لتخزين معلومات الملفات المرفوعة مع الفعالية
using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace EventManagement.Events
{
    public class EventFile : FullAuditedAggregateRoot<Guid>
    {
        // تعليق: معلومات الملف
        public Guid EventId { get; set; }
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        public string FilePath { get; set; }
        public string FileType { get; set; } // Image, PDF, Text
        public string MimeType { get; set; }
        public long FileSize { get; set; } // بالبايتات
        public int DisplayOrder { get; set; } // ترتيب العرض
        
        // تعليق: معلومات إضافية للصور
        public string ThumbnailPath { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        
        // Navigation property
        public virtual Event Event { get; set; }
        
        protected EventFile() { }
        
        // تعليق: Constructor لإنشاء ملف جديد
        public EventFile(
            Guid id,
            Guid eventId,
            string fileName,
            string originalFileName,
            string filePath,
            string fileType,
            string mimeType,
            long fileSize
        ) : base(id)
        {
            EventId = eventId;
            FileName = fileName;
            OriginalFileName = originalFileName;
            FilePath = filePath;
            FileType = fileType;
            MimeType = mimeType;
            FileSize = fileSize;
            DisplayOrder = 0;
        }
        
        // تعليق: تحديد ترتيب العرض
        public void SetDisplayOrder(int order)
        {
            DisplayOrder = order;
        }
        
        // تعليق: إضافة معلومات الصورة المصغرة
        public void SetThumbnail(string thumbnailPath, int? width = null, int? height = null)
        {
            ThumbnailPath = thumbnailPath;
            Width = width;
            Height = height;
        }
    }
}

