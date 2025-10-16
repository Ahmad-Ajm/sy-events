// EventManagement.Domain/Bookings/Booking.cs
// Place this file in: aspnet-core/src/EventManagement.Domain/Bookings/Booking.cs

using System;
using Volo.Abp.Domain.Entities.Auditing;
using EventManagement.Enums;

namespace EventManagement.Bookings
{
    /// <summary>
    /// Booking entity - represents event bookings/registrations
    /// </summary>
    public class Booking : FullAuditedAggregateRoot<Guid>
    {
        #region Properties

        /// <summary>
        /// User ID who made the booking
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Event ID being booked
        /// </summary>
        public Guid EventId { get; set; }

        /// <summary>
        /// Booking status
        /// </summary>
        public BookingStatus Status { get; set; }

        /// <summary>
        /// Reminder timing (optional)
        /// </summary>
        public ReminderTime? ReminderTime { get; set; }

        /// <summary>
        /// When user actually attended (optional)
        /// </summary>
        public DateTime? AttendedAt { get; set; }

        #endregion

        #region Navigation Properties

        /// <summary>
        /// The user who made the booking
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// The booked event
        /// </summary>
        public virtual Event Event { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Protected constructor for EF Core
        /// </summary>
        protected Booking()
        {
        }

        /// <summary>
        /// Constructor for creating new booking
        /// </summary>
        public Booking(Guid id, Guid userId, Guid eventId) : base(id)
        {
            UserId = userId;
            EventId = eventId;
            Status = BookingStatus.Confirmed;
        }

        #endregion

        #region Domain Methods

        /// <summary>
        /// Cancel the booking
        /// </summary>
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

        /// <summary>
        /// Mark booking as attended
        /// </summary>
        public void MarkAsAttended()
        {
            if (Status == BookingStatus.Cancelled)
            {
                throw new InvalidOperationException("Cannot mark cancelled booking as attended");
            }

            Status = BookingStatus.Attended;
            AttendedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Mark booking as no-show
        /// </summary>
        public void MarkAsNoShow()
        {
            if (Status == BookingStatus.Cancelled)
            {
                throw new InvalidOperationException("Cannot mark cancelled booking as no-show");
            }

            Status = BookingStatus.NoShow;
        }

        /// <summary>
        /// Set reminder time
        /// </summary>
        public void SetReminderTime(ReminderTime reminderTime)
        {
            ReminderTime = reminderTime;
        }

        /// <summary>
        /// Check if reminder should be sent
        /// </summary>
        public bool ShouldSendReminder(DateTime eventStartTime)
        {
            if (!ReminderTime.HasValue || Status != BookingStatus.Confirmed)
            {
                return false;
            }

            var now = DateTime.UtcNow;
            var hoursBeforeEvent = (int)ReminderTime.Value;
            var reminderTime = eventStartTime.AddHours(-hoursBeforeEvent);

            // Send reminder if current time is close to reminder time (within 10 minutes)
            var timeDifference = Math.Abs((reminderTime - now).TotalMinutes);
            return timeDifference <= 10;
        }

        #endregion
    }
}

