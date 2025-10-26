// تعليق: خدمة الحجوزات - متابعة وإلغاء متابعة الفعاليات
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using EventManagement.Events;
using EventManagement.Enums;

namespace EventManagement.Bookings
{
    /// <summary>
    /// تعليق: تطبيق خدمة الحجوزات
    /// </summary>
    [Authorize]
    public class BookingAppService : ApplicationService, IBookingAppService
    {
        private readonly IRepository<Booking, Guid> _bookingRepository;
        private readonly IRepository<Event, Guid> _eventRepository;
        private readonly ICurrentUser _currentUser;

        public BookingAppService(
            IRepository<Booking, Guid> bookingRepository,
            IRepository<Event, Guid> eventRepository,
            ICurrentUser currentUser)
        {
            _bookingRepository = bookingRepository;
            _eventRepository = eventRepository;
            _currentUser = currentUser;
        }

        /// <summary>
        /// تعليق: متابعة فعالية (إنشاء حجز مؤكد)
        /// </summary>
        public async Task<BookingDto> FollowEventAsync(Guid eventId)
        {
            var userId = _currentUser.GetId();
            
            // تعليق: التحقق من عدم وجود حجز سابق
            var existingBooking = await _bookingRepository.FirstOrDefaultAsync(
                b => b.EventId == eventId && b.UserId == userId);
            
            if (existingBooking != null)
            {
                throw new UserFriendlyException("أنت تتابع هذه الفعالية بالفعل");
            }
            
            // تعليق: التحقق من وجود الفعالية مع تحميل Bookings
            var queryable = await _eventRepository.WithDetailsAsync(x => x.Bookings);
            var eventEntity = await AsyncExecuter.FirstOrDefaultAsync(queryable.Where(e => e.Id == eventId));
            
            if (eventEntity == null)
            {
                throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Event), eventId);
            }
            
            // تعليق: التحقق من الموافقة على الفعالية
            if (!eventEntity.IsApproved)
            {
                throw new UserFriendlyException("هذه الفعالية غير معتمدة بعد");
            }
            
            // تعليق: التحقق من السعة المتاحة - الآن Bookings محملة
            if (!eventEntity.HasAvailableCapacity())
            {
                throw new UserFriendlyException("عذراً، الفعالية مكتملة");
            }
            
            // تعليق: إنشاء الحجز
            var booking = new Booking(
                GuidGenerator.Create(),
                userId,
                eventId
            );
            
            await _bookingRepository.InsertAsync(booking);
            
            return ObjectMapper.Map<Booking, BookingDto>(booking);
        }

        /// <summary>
        /// تعليق: إلغاء متابعة فعالية
        /// </summary>
        public async Task UnfollowEventAsync(Guid eventId)
        {
            var userId = _currentUser.GetId();
            
            var booking = await _bookingRepository.FirstOrDefaultAsync(
                b => b.EventId == eventId && b.UserId == userId);
            
            if (booking == null)
            {
                throw new UserFriendlyException("أنت لا تتابع هذه الفعالية");
            }
            
            await _bookingRepository.DeleteAsync(booking);
        }

        /// <summary>
        /// تعليق: التحقق إذا كان المستخدم يتابع الفعالية
        /// </summary>
        [AllowAnonymous]
        public async Task<bool> IsFollowingEventAsync(Guid eventId)
        {
            if (!_currentUser.IsAuthenticated)
            {
                return false;
            }
            
            var userId = _currentUser.GetId();
            
            var count = await _bookingRepository.CountAsync(
                b => b.EventId == eventId && b.UserId == userId);
            
            return count > 0;
        }

        /// <summary>
        /// تعليق: تأكيد الحضور للفعالية
        /// </summary>
        public async Task ConfirmAttendanceAsync(Guid bookingId)
        {
            var booking = await _bookingRepository.GetAsync(bookingId);
            
            // تعليق: التحقق من أن الحجز للمستخدم الحالي
            if (booking.UserId != _currentUser.GetId())
            {
                throw new UserFriendlyException("لا يمكنك تأكيد حضور حجز لمستخدم آخر");
            }
            
            booking.Status = BookingStatus.Attended;
            await _bookingRepository.UpdateAsync(booking);
        }
    }
}

