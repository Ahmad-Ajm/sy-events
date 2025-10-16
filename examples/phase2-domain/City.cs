// EventManagement.Domain/Cities/City.cs
// Place this file in: aspnet-core/src/EventManagement.Domain/Cities/City.cs

using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace EventManagement.Cities
{
    /// <summary>
    /// City entity - represents Syrian cities
    /// </summary>
    public class City : FullAuditedAggregateRoot<Guid>
    {
        #region Properties

        /// <summary>
        /// City name in Arabic
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// City name in English
        /// </summary>
        public string NameEn { get; set; }

        #endregion

        #region Navigation Properties

        /// <summary>
        /// Events in this city
        /// </summary>
        public virtual ICollection<Event> Events { get; set; }

        /// <summary>
        /// Users from this city
        /// </summary>
        public virtual ICollection<User> Users { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Protected constructor for EF Core
        /// </summary>
        protected City()
        {
        }

        /// <summary>
        /// Constructor for creating new city
        /// </summary>
        public City(Guid id, string name, string nameEn) : base(id)
        {
            Name = name;
            NameEn = nameEn;
            Events = new HashSet<Event>();
            Users = new HashSet<User>();
        }

        #endregion

        #region Domain Methods

        /// <summary>
        /// Update city names
        /// </summary>
        public void UpdateNames(string name, string nameEn)
        {
            Name = name;
            NameEn = nameEn;
        }

        #endregion
    }
}

