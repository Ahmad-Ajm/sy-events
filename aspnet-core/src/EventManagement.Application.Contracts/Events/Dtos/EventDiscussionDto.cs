// تعليق: DTO لمناقشات الفعاليات
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace EventManagement.Events.Dtos
{
    public class EventDiscussionDto : FullAuditedEntityDto<Guid>
    {
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string UserProfileImage { get; set; }
        public string Message { get; set; }
        public Guid? ParentId { get; set; }
        public bool IsHidden { get; set; }
        public string HiddenReason { get; set; }
        
        // للردود المتداخلة
        public List<EventDiscussionDto> Replies { get; set; }
        public int RepliesCount { get; set; }
    }
    
    // تعليق: DTO لإضافة تعليق
    public class CreateEventDiscussionDto
    {
        public Guid EventId { get; set; }
        public string Message { get; set; }
        public Guid? ParentId { get; set; } // للردود
    }
}

