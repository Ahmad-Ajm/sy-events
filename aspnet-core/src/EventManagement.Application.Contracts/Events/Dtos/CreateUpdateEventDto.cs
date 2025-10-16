using System;
using System.ComponentModel.DataAnnotations;

namespace EventManagement.Events.Dtos
{
    public class CreateUpdateEventDto
    {
        [Required]
        [StringLength(300)]
        public string Title { get; set; }

        [StringLength(300)]
        public string TitleEn { get; set; }

        [Required]
        public string Description { get; set; }

        public string DescriptionEn { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [StringLength(400)]
        public string Location { get; set; }

        [StringLength(400)]
        public string LocationEn { get; set; }

        public int? MaxCapacity { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        public Guid CityId { get; set; }

        public string ImageUrl { get; set; }
        public string ThumbnailUrl { get; set; }
    }
}


