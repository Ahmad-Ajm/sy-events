// EventManagement.Domain/Categories/Category.cs
// Place this file in: aspnet-core/src/EventManagement.Domain/Categories/Category.cs

using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace EventManagement.Categories
{
    /// <summary>
    /// Category entity - represents event categories
    /// </summary>
    public class Category : FullAuditedAggregateRoot<Guid>
    {
        #region Properties

        /// <summary>
        /// Category name in Arabic
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Category name in English
        /// </summary>
        public string NameEn { get; set; }

        /// <summary>
        /// Category description in Arabic (optional)
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Category description in English (optional)
        /// </summary>
        public string DescriptionEn { get; set; }

        #endregion

        #region Navigation Properties

        /// <summary>
        /// Events in this category
        /// </summary>
        public virtual ICollection<Event> Events { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Protected constructor for EF Core
        /// </summary>
        protected Category()
        {
        }

        /// <summary>
        /// Constructor for creating new category
        /// </summary>
        public Category(Guid id, string name, string nameEn) : base(id)
        {
            Name = name;
            NameEn = nameEn;
            Events = new HashSet<Event>();
        }

        #endregion

        #region Domain Methods

        /// <summary>
        /// Update category names
        /// </summary>
        public void UpdateNames(string name, string nameEn)
        {
            Name = name;
            NameEn = nameEn;
        }

        /// <summary>
        /// Update descriptions
        /// </summary>
        public void UpdateDescriptions(string description, string descriptionEn)
        {
            Description = description;
            DescriptionEn = descriptionEn;
        }

        #endregion
    }
}

