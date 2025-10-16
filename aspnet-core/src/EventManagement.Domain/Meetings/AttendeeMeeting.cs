// تعليق: كيان اجتماعات الحضور - جدولة لقاءات بين المشاركين
using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace EventManagement.Meetings
{
    public enum MeetingStatus
    {
        Pending = 1,    // قيد الانتظار
        Accepted = 2,   // مقبول
        Rejected = 3,   // مرفوض
        Cancelled = 4   // ملغى
    }
    
    public class AttendeeMeeting : FullAuditedAggregateRoot<Guid>
    {
        // تعليق: معرفات
        public Guid EventId { get; set; }
        public Guid RequesterId { get; set; } // من طلب الاجتماع
        public Guid RequestedId { get; set; } // المطلوب الاجتماع معه
        
        // تعليق: تفاصيل الاجتماع
        public DateTime MeetingTime { get; set; }
        public string Location { get; set; }
        public MeetingStatus Status { get; set; }
        public string Notes { get; set; }
        public string RejectionReason { get; set; }
        
        // Navigation properties
        public virtual Events.Event Event { get; set; }
        
        protected AttendeeMeeting() { }
        
        // تعليق: Constructor
        public AttendeeMeeting(
            Guid id,
            Guid eventId,
            Guid requesterId,
            Guid requestedId,
            DateTime meetingTime,
            string location,
            string notes = null
        ) : base(id)
        {
            EventId = eventId;
            RequesterId = requesterId;
            RequestedId = requestedId;
            MeetingTime = meetingTime;
            Location = location;
            Notes = notes;
            Status = MeetingStatus.Pending;
        }
        
        // تعليق: قبول الاجتماع
        public void Accept()
        {
            if (Status != MeetingStatus.Pending)
            {
                throw new InvalidOperationException("يمكن قبول الاجتماعات المعلقة فقط");
            }
            Status = MeetingStatus.Accepted;
        }
        
        // تعليق: رفض الاجتماع
        public void Reject(string reason)
        {
            if (Status != MeetingStatus.Pending)
            {
                throw new InvalidOperationException("يمكن رفض الاجتماعات المعلقة فقط");
            }
            Status = MeetingStatus.Rejected;
            RejectionReason = reason;
        }
        
        // تعليق: إلغاء الاجتماع
        public void Cancel()
        {
            if (Status == MeetingStatus.Cancelled)
            {
                throw new InvalidOperationException("الاجتماع ملغى بالفعل");
            }
            Status = MeetingStatus.Cancelled;
        }
    }
}

