using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using EventManagement.Events;
using EventManagement.Bookings;
using EventManagement.Enums;

namespace EventManagement.Calendar
{
    public class CalendarAppService : ApplicationService, ICalendarAppService
    {
        private readonly IRepository<Event, Guid> _eventRepo;
        private readonly IRepository<Booking, Guid> _bookingRepo;

        public CalendarAppService(IRepository<Event, Guid> eventRepo, IRepository<Booking, Guid> bookingRepo)
        {
            _eventRepo = eventRepo;
            _bookingRepo = bookingRepo;
        }

        public async Task<List<CalendarEventItemDto>> GetMyEventsAsync()
        {
            var userId = CurrentUser.Id;
            if (!userId.HasValue)
            {
                return new List<CalendarEventItemDto>();
            }
            return await GetUserEventsAsync(userId.Value);
        }

        public async Task<List<CalendarEventItemDto>> GetUserEventsAsync(Guid userId)
        {
            var now = DateTime.UtcNow;
            var bookings = await _bookingRepo.GetListAsync(x => x.UserId == userId);
            var eventIds = bookings.Select(b => b.EventId).Distinct().ToList();
            var events = await _eventRepo.GetListAsync(x => eventIds.Contains(x.Id));

            var result = new List<CalendarEventItemDto>();
            foreach (var ev in events)
            {
                var b = bookings.First(x => x.EventId == ev.Id);
                var status = b.Status switch
                {
                    BookingStatus.Attended => "attended",
                    BookingStatus.NoShow => "noShow",
                    _ => ev.EndDate < now ? "pastNotFollowed" : "upcomingFollowed"
                };

                result.Add(new CalendarEventItemDto
                {
                    Id = ev.Id,
                    Title = ev.Title,
                    Start = ev.StartDate,
                    End = ev.EndDate,
                    Status = status,
                    Location = ev.Location,
                    Description = ev.Description
                });
            }
            return result;
        }

        public async Task<List<CalendarEventItemDto>> GetEventsByRangeAsync(DateTime start, DateTime end)
        {
            var list = await _eventRepo.GetListAsync(x => x.StartDate <= end && x.EndDate >= start && x.IsApproved && x.Status == EventStatus.Approved);
            return list.Select(ev => new CalendarEventItemDto
            {
                Id = ev.Id,
                Title = ev.Title,
                Start = ev.StartDate,
                End = ev.EndDate,
                Status = ev.EndDate < DateTime.UtcNow ? "pastNotFollowed" : "upcomingNotFollowed",
                Location = ev.Location,
                Description = ev.Description
            }).ToList();
        }
    }
}


