using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using EventManagement.Enums;
using EventManagement.Categories;
using EventManagement.Cities;
using EventManagement.Users;
using EventManagement.Bookings;

namespace EventManagement.Events
{
    /// <summary>
    /// Event entity - represents an event/activity
    /// </summary>
    public class Event : FullAuditedAggregateRoot<Guid>
    {
        public string Title { get; set; }
        public string TitleEn { get; set; }
        public string Description { get; set; }
        public string DescriptionEn { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Location { get; set; }
        public string LocationEn { get; set; }
        public int? MaxCapacity { get; set; }
        public bool IsApproved { get; set; }
        public EventStatus Status { get; set; }
        public string ImageUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public Guid CategoryId { get; set; }
        public Guid CityId { get; set; }
        public Guid OrganizerId { get; set; }

        public virtual Category Category { get; set; }
        public virtual City City { get; set; }
        public virtual User Organizer { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }

        protected Event() { }

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
            Status = EventStatus.Draft;
            IsApproved = false;

            Bookings = new HashSet<Booking>();
        }

        public void Approve()
        {
            if (Status != EventStatus.Pending)
            {
                throw new InvalidOperationException("Only pending events can be approved");
            }
            IsApproved = true;
            Status = EventStatus.Approved;
        }

        public void Reject()
        {
            if (Status != EventStatus.Pending)
            {
                throw new InvalidOperationException("Only pending events can be rejected");
            }
            IsApproved = false;
            Status = EventStatus.Rejected;
        }

        public void SubmitForApproval()
        {
            if (Status != EventStatus.Draft)
            {
                throw new InvalidOperationException("Only draft events can be submitted");
            }
            Status = EventStatus.Pending;
        }

        public bool HasAvailableCapacity()
        {
            if (!MaxCapacity.HasValue)
            {
                return true;
            }

            var confirmedCount = 0;
            foreach (var booking in Bookings)
            {
                if (booking.Status == BookingStatus.Confirmed)
                {
                    confirmedCount++;
                }
            }
            return confirmedCount < MaxCapacity.Value;
        }

        public int? GetAvailableCapacity()
        {
            if (!MaxCapacity.HasValue)
            {
                return null;
            }

            var confirmedCount = 0;
            foreach (var booking in Bookings)
            {
                if (booking.Status == BookingStatus.Confirmed)
                {
                    confirmedCount++;
                }
            }

            return MaxCapacity.Value - confirmedCount;
        }

        public void Hide()
        {
            Status = EventStatus.Hidden;
        }

        public bool HasPassed()
        {
            return EndDate < DateTime.UtcNow;
        }

        public bool IsUpcoming()
        {
            return StartDate > DateTime.UtcNow;
        }

        public bool IsOngoing()
        {
            var now = DateTime.UtcNow;
            return StartDate <= now && EndDate >= now;
        }
    }
}


