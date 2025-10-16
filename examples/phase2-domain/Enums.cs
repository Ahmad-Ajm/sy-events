// EventManagement.Domain.Shared/Enums.cs
// Place this file in: aspnet-core/src/EventManagement.Domain.Shared/Enums.cs

namespace EventManagement.Enums
{
    /// <summary>
    /// User roles in the system
    /// </summary>
    public enum UserRole
    {
        /// <summary>
        /// System administrator - full access
        /// </summary>
        Admin = 1,

        /// <summary>
        /// Event organizer - can create and manage events
        /// </summary>
        Organizer = 2,

        /// <summary>
        /// Content editor - can edit event content
        /// </summary>
        Editor = 3,

        /// <summary>
        /// Support staff - limited access
        /// </summary>
        Support = 4,

        /// <summary>
        /// Regular viewer - can only view and book events
        /// </summary>
        Viewer = 5
    }

    /// <summary>
    /// Event status in the approval workflow
    /// </summary>
    public enum EventStatus
    {
        /// <summary>
        /// Draft - not yet submitted
        /// </summary>
        Draft = 1,

        /// <summary>
        /// Pending admin approval
        /// </summary>
        Pending = 2,

        /// <summary>
        /// Approved and published
        /// </summary>
        Approved = 3,

        /// <summary>
        /// Rejected by admin
        /// </summary>
        Rejected = 4,

        /// <summary>
        /// Hidden from public view
        /// </summary>
        Hidden = 5
    }

    /// <summary>
    /// Booking status
    /// </summary>
    public enum BookingStatus
    {
        /// <summary>
        /// Booking is confirmed
        /// </summary>
        Confirmed = 1,

        /// <summary>
        /// Booking was cancelled
        /// </summary>
        Cancelled = 2,

        /// <summary>
        /// User attended the event
        /// </summary>
        Attended = 3,

        /// <summary>
        /// User did not show up
        /// </summary>
        NoShow = 4
    }

    /// <summary>
    /// Email reminder timing
    /// </summary>
    public enum ReminderTime
    {
        /// <summary>
        /// One hour before event
        /// </summary>
        OneHour = 1,

        /// <summary>
        /// 24 hours before event
        /// </summary>
        TwentyFourHours = 24,

        /// <summary>
        /// 72 hours (3 days) before event
        /// </summary>
        SeventyTwoHours = 72,

        /// <summary>
        /// One week before event
        /// </summary>
        OneWeek = 168
    }
}

