using System;
using Volo.Abp.Domain.Entities.Auditing;
using EventManagement.Enums;
using EventManagement.Users;
using EventManagement.Events;

namespace EventManagement.Bookings
{
    /// <summary>
    /// Booking entity - represents event bookings/registrations
    /// </summary>
    public class Booking : FullAuditedAggregateRoot<Guid>
    {
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
        public BookingStatus Status { get; set; }
        public ReminderTime? ReminderTime { get; set; }
        public DateTime? AttendedAt { get; set; }

        public virtual User User { get; set; }
        public virtual Event Event { get; set; }

        protected Booking() { }

        public Booking(Guid id, Guid userId, Guid eventId) : base(id)
        {
            UserId = userId;
            EventId = eventId;
            Status = BookingStatus.Confirmed;
        }

        public void Cancel()
        {
            if (Status == BookingStatus.Cancelled)
            {
                throw new InvalidOperationException("Booking is already cancelled");
            }
            if (Status == BookingStatus.Attended)
            {
                throw new InvalidOperationException("Cannot cancel a booking that was attended");
            }
            Status = BookingStatus.Cancelled;
        }

        public void MarkAsAttended()
        {
            if (Status == BookingStatus.Cancelled)
            {
                throw new InvalidOperationException("Cannot mark cancelled booking as attended");
            }
            Status = BookingStatus.Attended;
            AttendedAt = DateTime.UtcNow;
        }

        public void MarkAsNoShow()
        {
            if (Status == BookingStatus.Cancelled)
            {
                throw new InvalidOperationException("Cannot mark cancelled booking as no-show");
            }
            Status = BookingStatus.NoShow;
        }

        public void SetReminderTime(ReminderTime reminderTime)
        {
            ReminderTime = reminderTime;
        }

        public bool ShouldSendReminder(DateTime eventStartTime)
        {
            if (!ReminderTime.HasValue || Status != BookingStatus.Confirmed)
            {
                return false;
            }
            var now = DateTime.UtcNow;
            var hoursBeforeEvent = (int)ReminderTime.Value;
            var reminderTime = eventStartTime.AddHours(-hoursBeforeEvent);

            // Send if within 10 minutes window
            var timeDifference = Math.Abs((reminderTime - now).TotalMinutes);
            return timeDifference <= 10;
        }
    }
}


