using System;
using Volo.Abp.Application.Dtos;
using EventManagement.Enums;

namespace EventManagement.Events.Dtos
{
    public class GetEventsInput : PagedAndSortedResultRequestDto
    {
        // تعليق: البحث النصي في العنوان والوصف
        public string Filter { get; set; } = string.Empty;
        
        // تعليق: فلاتر أساسية
        public Guid? CategoryId { get; set; }
        public Guid? CityId { get; set; }
        public EventStatus? Status { get; set; }
        
        // تعليق: فلتر الزمان
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        
        // تعليق: فلاتر متقدمة جديدة
        public Guid? OrganizerId { get; set; } // المنظم
        public bool? IsUpcoming { get; set; } // قادم (true) أو منقضي (false) أو الكل (null)
        public int? MinAttendees { get; set; } // عدد الحضور أكبر من X
    }
}


