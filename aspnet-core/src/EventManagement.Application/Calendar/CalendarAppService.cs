using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventManagement.Bookings;
using EventManagement.Events;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace EventManagement.Calendar
{
    public class CalendarAppService : ApplicationService, ICalendarAppService, ITransientDependency
    {
        private readonly IRepository<Event, Guid> _eventRepository;
        private readonly IRepository<Booking, Guid> _bookingRepository;
        private readonly ICurrentUser _currentUser;

        public CalendarAppService(
            IRepository<Event, Guid> eventRepository,
            IRepository<Booking, Guid> bookingRepository,
            ICurrentUser currentUser)
        {
            _eventRepository = eventRepository;
            _bookingRepository = bookingRepository;
            _currentUser = currentUser;
        }

        [AllowAnonymous]
        public async Task<List<CalendarEventItemDto>> GetEventsByRangeAsync(DateTime start, DateTime end)
        {
            var events = await _eventRepository.GetListAsync(e => e.StartDate <= end && e.EndDate >= start && e.IsApproved);
            return await MapWithStatusAsync(events, _currentUser.Id);
        }

        public async Task<List<CalendarEventItemDto>> GetMyEventsAsync()
        {
            var userId = _currentUser.Id;
            var events = await _eventRepository.GetListAsync(e => e.IsApproved);
            return await MapWithStatusAsync(events, userId);
        }

        [AllowAnonymous]
        public async Task<List<CalendarEventItemDto>> GetUserEventsAsync(Guid userId)
        {
            var events = await _eventRepository.GetListAsync(e => e.IsApproved);
            return await MapWithStatusAsync(events, userId);
        }

        private async Task<List<CalendarEventItemDto>> MapWithStatusAsync(IEnumerable<Event> events, Guid? userId)
        {
            var now = DateTime.UtcNow;
            var list = events.ToList();
            var eventIds = list.Select(e => e.Id).ToList();

            var bookings = userId.HasValue
                ? await _bookingRepository.GetListAsync(b => eventIds.Contains(b.EventId) && b.UserId == userId)
                : new List<Booking>();

            var bookingByEvent = bookings.GroupBy(b => b.EventId).ToDictionary(g => g.Key, g => g.ToList());

            string ResolveStatus(Event e)
            {
                var hasBookings = bookingByEvent.TryGetValue(e.Id, out var userBookings);
                var isPast = e.EndDate < now;
                if (hasBookings)
                {
                    var anyAttended = userBookings.Any(b => b.Status == Enums.BookingStatus.Attended);
                    var anyConfirmed = userBookings.Any(b => b.Status == Enums.BookingStatus.Confirmed);
                    if (anyAttended) return "attended";
                    if (isPast && anyConfirmed) return "noShow";
                    if (!isPast && anyConfirmed) return "upcomingFollowed";
                }
                return isPast ? "pastNotFollowed" : "upcomingNotFollowed";
            }

            return list.Select(e => new CalendarEventItemDto
            {
                Id = e.Id,
                Title = e.Title,
                Start = e.StartDate,
                End = e.EndDate,
                Status = ResolveStatus(e),
                Location = e.Location,
                Description = e.Description
            }).ToList();
        }
    }
}
