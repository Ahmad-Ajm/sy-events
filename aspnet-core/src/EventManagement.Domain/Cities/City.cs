using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using EventManagement.Events;
using EventManagement.Users;

namespace EventManagement.Cities
{
    /// <summary>
    /// City entity - represents Syrian cities
    /// </summary>
    public class City : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public string NameEn { get; set; }

        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<User> Users { get; set; }

        protected City() { }

        public City(Guid id, string name, string nameEn) : base(id)
        {
            Name = name;
            NameEn = nameEn;
            Events = new HashSet<Event>();
            Users = new HashSet<User>();
        }

        public void UpdateNames(string name, string nameEn)
        {
            Name = name;
            NameEn = nameEn;
        }
    }
}


