// تعليق: DTO للحجز
using System;
using Volo.Abp.Application.Dtos;
using EventManagement.Enums;

namespace EventManagement.Bookings
{
    /// <summary>
    /// تعليق: بيانات الحجز (متابعة الفعالية)
    /// </summary>
    public class BookingDto : EntityDto<Guid>
    {
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }
        public BookingStatus Status { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}












