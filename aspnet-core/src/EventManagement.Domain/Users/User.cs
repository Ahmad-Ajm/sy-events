using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using EventManagement.Enums;
using EventManagement.Cities;
using EventManagement.Events;
using EventManagement.Bookings;

namespace EventManagement.Users
{
    /// <summary>
    /// User entity - represents system users with different roles
    /// </summary>
    public class User : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// User's email address (unique)
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User's full name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Hashed password
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Phone number (optional)
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// User's profession (optional)
        /// </summary>
        public string Profession { get; set; }

        /// <summary>
        /// City ID (optional foreign key)
        /// </summary>
        public Guid? CityId { get; set; }

        /// <summary>
        /// User interests (optional)
        /// </summary>
        public string Interests { get; set; }

        /// <summary>
        /// Reason for joining (optional)
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// User role (Admin, Organizer, Editor, Support, Viewer)
        /// </summary>
        public UserRole Role { get; set; }

        /// <summary>
        /// User's city
        /// </summary>
        public virtual City City { get; set; }

        /// <summary>
        /// Events organized by this user
        /// </summary>
        public virtual ICollection<Event> OrganizedEvents { get; set; }

        /// <summary>
        /// User's bookings
        /// </summary>
        public virtual ICollection<Booking> Bookings { get; set; }

        /// <summary>
        /// Protected constructor for EF Core
        /// </summary>
        protected User()
        {
        }

        /// <summary>
        /// Constructor for creating new user
        /// </summary>
        public User(
            Guid id,
            string email,
            string name,
            string passwordHash,
            UserRole role = UserRole.Viewer
        ) : base(id)
        {
            Email = email;
            Name = name;
            PasswordHash = passwordHash;
            Role = role;

            OrganizedEvents = new HashSet<Event>();
            Bookings = new HashSet<Booking>();
        }

        /// <summary>
        /// Change user role
        /// </summary>
        public void ChangeRole(UserRole newRole)
        {
            Role = newRole;
        }

        /// <summary>
        /// Update user profile
        /// </summary>
        public void UpdateProfile(string name, string phone, string profession)
        {
            Name = name;
            Phone = phone;
            Profession = profession;
        }

        /// <summary>
        /// Set user city
        /// </summary>
        public void SetCity(Guid? cityId)
        {
            CityId = cityId;
        }

        /// <summary>
        /// Check if user can organize events
        /// </summary>
        public bool CanOrganizeEvents()
        {
            return Role == UserRole.Admin || Role == UserRole.Organizer;
        }

        /// <summary>
        /// Check if user can approve events
        /// </summary>
        public bool CanApproveEvents()
        {
            return Role == UserRole.Admin;
        }
    }
}


