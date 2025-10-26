// تعليق: واجهة خدمة الحجوزات - متابعة الفعاليات
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace EventManagement.Bookings
{
    /// <summary>
    /// تعليق: خدمة إدارة الحجوزات ومتابعة الفعاليات
    /// </summary>
    public interface IBookingAppService : IApplicationService
    {
        /// <summary>
        /// تعليق: متابعة فعالية (إنشاء حجز)
        /// </summary>
        Task<BookingDto> FollowEventAsync(Guid eventId);
        
        /// <summary>
        /// تعليق: إلغاء متابعة فعالية
        /// </summary>
        Task UnfollowEventAsync(Guid eventId);
        
        /// <summary>
        /// تعليق: التحقق إذا كان المستخدم يتابع الفعالية
        /// </summary>
        Task<bool> IsFollowingEventAsync(Guid eventId);
        
        /// <summary>
        /// تعليق: تأكيد الحضور للفعالية
        /// </summary>
        Task ConfirmAttendanceAsync(Guid bookingId);
    }
}

