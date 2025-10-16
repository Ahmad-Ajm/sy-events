// EventManagement.Domain/Events/Event.cs
// Place this file in: aspnet-core/src/EventManagement.Domain/Events/Event.cs

using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using EventManagement.Enums;

namespace EventManagement.Events
{
    /// <summary>
    /// Event entity - represents an event/activity
    /// </summary>
    public class Event : FullAuditedAggregateRoot<Guid>
    {
        #region Properties

        /// <summary>
        /// Event title in Arabic
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Event title in English
        /// </summary>
        public string TitleEn { get; set; }

        /// <summary>
        /// Event description in Arabic
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Event description in English
        /// </summary>
        public string DescriptionEn { get; set; }

        /// <summary>
        /// Event start date and time
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Event end date and time
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Event location in Arabic
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Event location in English
        /// </summary>
        public string LocationEn { get; set; }

        /// <summary>
        /// Maximum capacity (optional)
        /// </summary>
        public int? MaxCapacity { get; set; }

        /// <summary>
        /// Is event approved by admin
        /// </summary>
        public bool IsApproved { get; set; }

        /// <summary>
        /// Event status (Draft, Pending, Approved, Rejected, Hidden)
        /// </summary>
        public EventStatus Status { get; set; }

        /// <summary>
        /// Event image URL
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Event thumbnail URL
        /// </summary>
        public string ThumbnailUrl { get; set; }

        /// <summary>
        /// Category ID (foreign key)
        /// </summary>
        public Guid CategoryId { get; set; }

        /// <summary>
        /// City ID (foreign key)
        /// </summary>
        public Guid CityId { get; set; }

        /// <summary>
        /// Organizer user ID (foreign key)
        /// </summary>
        public Guid OrganizerId { get; set; }

        #endregion

        #region Navigation Properties

        /// <summary>
        /// Event category
        /// </summary>
        public virtual Category Category { get; set; }

        /// <summary>
        /// Event city
        /// </summary>
        public virtual City City { get; set; }

        /// <summary>
        /// Event organizer
        /// </summary>
        public virtual User Organizer { get; set; }

        /// <summary>
        /// Event bookings
        /// </summary>
        public virtual ICollection<Booking> Bookings { get; set; }

        /// <summary>
        /// Event files
        /// </summary>
        public virtual ICollection<EventFile> Files { get; set; }

        /// <summary>
        /// Social shares
        /// </summary>
        public virtual ICollection<SocialShare> SocialShares { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Protected constructor for EF Core
        /// </summary>
        protected Event()
        {
        }

        /// <summary>
        /// Constructor for creating new event
        /// </summary>
        public Event(
            Guid id,
            string title,
            string description,
            DateTime startDate,
            DateTime endDate,
            string location,
            Guid categoryId,
            Guid cityId,
            Guid organizerId
        ) : base(id)
        {
            Title = title;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            Location = location;
            CategoryId = categoryId;
            CityId = cityId;
            OrganizerId = organizerId;

            // Default values
            Status = EventStatus.Draft;
            IsApproved = false;

            // Initialize collections
            Bookings = new HashSet<Booking>();
            Files = new HashSet<EventFile>();
            SocialShares = new HashSet<SocialShare>();
        }

        #endregion

        #region Domain Methods

        /// <summary>
        /// Approve the event
        /// </summary>
        public void Approve()
        {
            if (Status == EventStatus.Pending)
            {
                IsApproved = true;
                Status = EventStatus.Approved;
            }
            else
            {
                throw new InvalidOperationException("Only pending events can be approved");
            }
        }

        /// <summary>
        /// Reject the event
        /// </summary>
        public void Reject()
        {
            if (Status == EventStatus.Pending)
            {
                IsApproved = false;
                Status = EventStatus.Rejected;
            }
            else
            {
                throw new InvalidOperationException("Only pending events can be rejected");
            }
        }

        /// <summary>
        /// Submit event for approval
        /// </summary>
        public void SubmitForApproval()
        {
            if (Status == EventStatus.Draft)
            {
                Status = EventStatus.Pending;
            }
            else
            {
                throw new InvalidOperationException("Only draft events can be submitted");
            }
        }

        /// <summary>
        /// Publish the event (make it visible)
        /// </summary>
        public void Publish()
        {
            if (IsApproved && Status == EventStatus.Approved)
            {
                // Event is already published
                return;
            }

            throw new InvalidOperationException("Event must be approved before publishing");
        }

        /// <summary>
        /// Hide the event
        /// </summary>
        public void Hide()
        {
            Status = EventStatus.Hidden;
        }

        /// <summary>
        /// Check if event has available capacity
        /// </summary>
        public bool HasAvailableCapacity()
        {
            if (!MaxCapacity.HasValue)
            {
                return true; // Unlimited capacity
            }

            var confirmedBookingsCount = 0;
            foreach (var booking in Bookings)
            {
                if (booking.Status == BookingStatus.Confirmed)
                {
                    confirmedBookingsCount++;
                }
            }

            return confirmedBookingsCount < MaxCapacity.Value;
        }

        /// <summary>
        /// Get available capacity
        /// </summary>
        public int? GetAvailableCapacity()
        {
            if (!MaxCapacity.HasValue)
            {
                return null; // Unlimited
            }

            var confirmedBookingsCount = 0;
            foreach (var booking in Bookings)
            {
                if (booking.Status == BookingStatus.Confirmed)
                {
                    confirmedBookingsCount++;
                }
            }

            return MaxCapacity.Value - confirmedBookingsCount;
        }

        /// <summary>
        /// Check if event has passed
        /// </summary>
        public bool HasPassed()
        {
            return EndDate < DateTime.UtcNow;
        }

        /// <summary>
        /// Check if event is upcoming
        /// </summary>
        public bool IsUpcoming()
        {
            return StartDate > DateTime.UtcNow;
        }

        /// <summary>
        /// Check if event is ongoing
        /// </summary>
        public bool IsOngoing()
        {
            var now = DateTime.UtcNow;
            return StartDate <= now && EndDate >= now;
        }

        #endregion
    }
}

