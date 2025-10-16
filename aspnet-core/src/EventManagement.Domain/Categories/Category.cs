using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using EventManagement.Events;

namespace EventManagement.Categories
{
    /// <summary>
    /// Category entity - represents event categories
    /// </summary>
    public class Category : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public string NameEn { get; set; }
        public string Description { get; set; }
        public string DescriptionEn { get; set; }
        public string Icon { get; set; }

        public virtual ICollection<Event> Events { get; set; }

        protected Category() { }

        public Category(Guid id, string name, string nameEn) : base(id)
        {
            Name = name;
            NameEn = nameEn;
            Events = new HashSet<Event>();
        }

        public void UpdateNames(string name, string nameEn)
        {
            Name = name;
            NameEn = nameEn;
        }

        public void UpdateDescriptions(string description, string descriptionEn)
        {
            Description = description;
            DescriptionEn = descriptionEn;
        }
    }
}


