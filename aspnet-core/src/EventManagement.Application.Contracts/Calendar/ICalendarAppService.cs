using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace EventManagement.Calendar
{
    public class CalendarEventItemDto
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public required string Status { get; set; } // attended, noShow, pastNotFollowed, upcomingNotFollowed, upcomingFollowed
        public required string Location { get; set; }
        public required string Description { get; set; }
    }

    public interface ICalendarAppService : IApplicationService
    {
        Task<List<CalendarEventItemDto>> GetMyEventsAsync();
        Task<List<CalendarEventItemDto>> GetUserEventsAsync(Guid userId);
        Task<List<CalendarEventItemDto>> GetEventsByRangeAsync(DateTime start, DateTime end);
    }
}


