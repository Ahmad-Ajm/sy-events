// تعليق: DTOs للمربعات المميزة
using System;
using Volo.Abp.Application.Dtos;
using EventManagement.Enums;

namespace EventManagement.FeaturedBoxes.Dtos
{
    /// <summary>
    /// DTO لعرض المربع المميز
    /// </summary>
    public class FeaturedBoxDto : EntityDto<Guid>
    {
        public int DisplayOrder { get; set; }
        public FeaturedBoxType Type { get; set; }
        public Guid? CustomEventId { get; set; }
        public bool IsActive { get; set; }
        public string? Title { get; set; }
        public string? TitleEn { get; set; }
        public string? Description { get; set; }
        public string? DescriptionEn { get; set; }
        public string? ImageUrl { get; set; }
        public string? CustomLink { get; set; }
        
        // تعليق: بيانات الفعالية (إذا كان Type ليس Custom)
        public string? EventTitle { get; set; }
        public string? EventTitleEn { get; set; }
        public DateTime? EventStartDate { get; set; }
        public string? EventImageUrl { get; set; }
        public string? EventLocation { get; set; }
    }
    
    /// <summary>
    /// DTO لإنشاء/تحديث المربع
    /// </summary>
    public class CreateUpdateFeaturedBoxDto
    {
        public int DisplayOrder { get; set; }
        public FeaturedBoxType Type { get; set; }
        public Guid? CustomEventId { get; set; }
        public bool IsActive { get; set; }
        public string? Title { get; set; }
        public string? TitleEn { get; set; }
        public string? Description { get; set; }
        public string? DescriptionEn { get; set; }
        public string? ImageUrl { get; set; }
        public string? CustomLink { get; set; }
    }
}

