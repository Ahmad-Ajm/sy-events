using System;
using Volo.Abp.Application.Dtos;
using EventManagement.Enums;

namespace EventManagement.Events.Dtos
{
    public class EventDto : FullAuditedEntityDto<Guid>
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

        public string CategoryName { get; set; }
        public string CategoryNameEn { get; set; }
        public string CityName { get; set; }
        public string CityNameEn { get; set; }
        public string OrganizerName { get; set; }

        public int BookingsCount { get; set; }
        public int? AvailableCapacity { get; set; }
    }
}


