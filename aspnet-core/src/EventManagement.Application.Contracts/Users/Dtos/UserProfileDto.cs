// تعليق: DTO لملف تعريف المستخدم
using System;
using Volo.Abp.Application.Dtos;

namespace EventManagement.Users.Dtos
{
    public class UserProfileDto : FullAuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        
        // معلومات الملف الشخصي
        public string Bio { get; set; }
        public string ProfileImageUrl { get; set; }
        public string CoverImageUrl { get; set; }
        
        // معلومات مهنية
        public string JobTitle { get; set; }
        public string Company { get; set; }
        public string Website { get; set; }
        
        // وسائل التواصل
        public string LinkedInUrl { get; set; }
        public string TwitterHandle { get; set; }
        public string FacebookUrl { get; set; }
        
        // إعدادات الخصوصية
        public bool IsPublic { get; set; }
        public bool ShowEmail { get; set; }
        public bool ShowPhone { get; set; }
        
        // إحصائيات
        public int EventsAttendedCount { get; set; }
        public int EventsOrganizedCount { get; set; }
        
        // الاهتمامات والمهارات (arrays)
        public string[] Interests { get; set; }
        public string[] Skills { get; set; }
    }
    
    // تعليق: DTO لتحديث الملف الشخصي
    public class UpdateUserProfileDto
    {
        public string Bio { get; set; }
        public string ProfileImageUrl { get; set; }
        public string CoverImageUrl { get; set; }
        public string JobTitle { get; set; }
        public string Company { get; set; }
        public string Website { get; set; }
        public string LinkedInUrl { get; set; }
        public string TwitterHandle { get; set; }
        public string FacebookUrl { get; set; }
        public bool IsPublic { get; set; }
        public bool ShowEmail { get; set; }
        public bool ShowPhone { get; set; }
        public string[] Interests { get; set; }
        public string[] Skills { get; set; }
    }
}

