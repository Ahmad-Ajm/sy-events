// تعليق: Controller للحجوزات - متابعة الفعاليات
using System;
using System.Threading.Tasks;
using EventManagement.Bookings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace EventManagement.Controllers
{
    /// <summary>
    /// تعليق: Controller لإدارة متابعة الفعاليات
    /// </summary>
    [Route("api/app/booking")]
    public class BookingController : AbpController
    {
        private readonly IBookingAppService _bookingAppService;

        public BookingController(IBookingAppService bookingAppService)
        {
            _bookingAppService = bookingAppService;
        }

        /// <summary>
        /// تعليق: متابعة فعالية
        /// </summary>
        [HttpPost("follow-event")]
        public async Task<BookingDto> FollowEventAsync([FromQuery] Guid eventId)
        {
            return await _bookingAppService.FollowEventAsync(eventId);
        }

        /// <summary>
        /// تعليق: إلغاء متابعة فعالية
        /// </summary>
        [HttpPost("unfollow-event")]
        public async Task UnfollowEventAsync([FromQuery] Guid eventId)
        {
            await _bookingAppService.UnfollowEventAsync(eventId);
        }

        /// <summary>
        /// تعليق: التحقق من متابعة فعالية
        /// </summary>
        [HttpGet("is-following-event")]
        [AllowAnonymous]
        public async Task<bool> IsFollowingEventAsync([FromQuery] Guid eventId)
        {
            return await _bookingAppService.IsFollowingEventAsync(eventId);
        }

        /// <summary>
        /// تعليق: تأكيد الحضور
        /// </summary>
        [HttpPost("confirm-attendance")]
        public async Task ConfirmAttendanceAsync([FromQuery] Guid bookingId)
        {
            await _bookingAppService.ConfirmAttendanceAsync(bookingId);
        }
    }
}

