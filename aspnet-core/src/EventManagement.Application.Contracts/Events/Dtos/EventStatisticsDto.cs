using System;

namespace EventManagement.Events.Dtos
{
    public class EventStatisticsDto
    {
        public Guid EventId { get; set; }
        public int TotalBookings { get; set; }
        public int ConfirmedBookings { get; set; }
        public int AttendedCount { get; set; }
        public int CancelledCount { get; set; }
        public int? AvailableCapacity { get; set; }
    }
}


