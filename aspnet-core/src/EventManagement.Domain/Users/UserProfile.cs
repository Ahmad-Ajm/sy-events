// تعليق: كيان ملف تعريف المستخدم - معلومات إضافية للمستخدمين
using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace EventManagement.Users
{
    public class UserProfile : FullAuditedAggregateRoot<Guid>
    {
        // تعليق: معرف المستخدم من AbpUsers
        public Guid UserId { get; set; }
        
        // تعليق: معلومات الملف الشخصي
        public string Bio { get; set; } // نبذة تعريفية (حتى 500 حرف)
        public string ProfileImageUrl { get; set; }
        public string CoverImageUrl { get; set; }
        
        // تعليق: معلومات مهنية
        public string JobTitle { get; set; }
        public string Company { get; set; }
        public string Website { get; set; }
        
        // تعليق: معلومات التواصل الاجتماعي
        public string LinkedInUrl { get; set; }
        public string TwitterHandle { get; set; }
        public string FacebookUrl { get; set; }
        
        // تعليق: الاهتمامات والمهارات (مصممة كحقول مصفوفية مبسطة لتفادي التعقيد في ExtraProperties)
        public string[] Interests { get; set; }
        public string[] Skills { get; set; }
        
        // تعليق: إعدادات الخصوصية
        public bool IsPublic { get; set; } // الملف عام أم خاص
        public bool ShowEmail { get; set; }
        public bool ShowPhone { get; set; }
        
        // تعليق: إحصائيات (محسوبة)
        public int EventsAttendedCount { get; set; }
        public int EventsOrganizedCount { get; set; }
        
        protected UserProfile() { }
        
        // تعليق: Constructor
        public UserProfile(Guid id, Guid userId) : base(id)
        {
            UserId = userId;
            IsPublic = true;
            ShowEmail = false;
            ShowPhone = false;
            EventsAttendedCount = 0;
            EventsOrganizedCount = 0;
            Interests = Array.Empty<string>();
            Skills = Array.Empty<string>();
        }
        
        // تعليق: تحديث النبذة التعريفية
        public void UpdateBio(string bio)
        {
            if (bio != null && bio.Length > 500)
            {
                throw new ArgumentException("النبذة التعريفية يجب ألا تتجاوز 500 حرف");
            }
            Bio = bio;
        }
        
        // تعليق: تحديث صورة الملف الشخصي
        public void UpdateProfileImage(string imageUrl)
        {
            ProfileImageUrl = imageUrl;
        }
        
        // تعليق: تحديث إعدادات الخصوصية
        public void UpdatePrivacySettings(bool isPublic, bool showEmail, bool showPhone)
        {
            IsPublic = isPublic;
            ShowEmail = showEmail;
            ShowPhone = showPhone;
        }
        
        // تعليق: زيادة عداد الفعاليات المحضورة
        public void IncrementAttendedEvents()
        {
            EventsAttendedCount++;
        }
        
        // تعليق: زيادة عداد الفعاليات المنظمة
        public void IncrementOrganizedEvents()
        {
            EventsOrganizedCount++;
        }
    }
}

