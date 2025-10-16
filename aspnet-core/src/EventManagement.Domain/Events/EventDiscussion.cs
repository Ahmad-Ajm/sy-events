// تعليق: كيان مناقشات الفعاليات - التعليقات والردود
using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace EventManagement.Events
{
    public class EventDiscussion : FullAuditedAggregateRoot<Guid>
    {
        // تعليق: معرفات
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }
        
        // تعليق: محتوى التعليق
        public string Message { get; set; }
        
        // تعليق: للردود المتداخلة (Nested Comments)
        public Guid? ParentId { get; set; }
        
        // تعليق: الإشراف
        public bool IsHidden { get; set; }
        public string HiddenReason { get; set; }
        
        // Navigation properties
        public virtual Event Event { get; set; }
        public virtual EventDiscussion Parent { get; set; }
        
        protected EventDiscussion() { }
        
        // تعليق: Constructor
        public EventDiscussion(
            Guid id,
            Guid eventId,
            Guid userId,
            string message,
            Guid? parentId = null
        ) : base(id)
        {
            EventId = eventId;
            UserId = userId;
            Message = message;
            ParentId = parentId;
            IsHidden = false;
        }
        
        // تعليق: إخفاء التعليق
        public void Hide(string reason)
        {
            IsHidden = true;
            HiddenReason = reason;
        }
        
        // تعليق: إظهار التعليق
        public void Show()
        {
            IsHidden = false;
            HiddenReason = null;
        }
    }
}

