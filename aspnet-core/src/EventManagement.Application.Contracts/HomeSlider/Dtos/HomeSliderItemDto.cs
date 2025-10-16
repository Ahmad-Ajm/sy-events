using System;
using Volo.Abp.Application.Dtos;

namespace EventManagement.HomeSlider.Dtos
{
    /// <summary>
    /// DTO لعرض عنصر السلايدر
    /// </summary>
    public class HomeSliderItemDto : FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// ترتيب العرض
        /// </summary>
        public int DisplayOrder { get; set; }
        
        /// <summary>
        /// نوع العنصر
        /// </summary>
        public SliderItemType Type { get; set; }
        
        /// <summary>
        /// معرف الفعالية المخصصة
        /// </summary>
        public Guid? CustomEventId { get; set; }
        
        /// <summary>
        /// هل العنصر نشط؟
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// عنوان مخصص (عربي)
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// عنوان مخصص (إنجليزي)
        /// </summary>
        public string TitleEn { get; set; }
        
        /// <summary>
        /// رابط صورة مخصصة
        /// </summary>
        public string ImageUrl { get; set; }
        
        // تعليق: بيانات الفعالية المرتبطة (يتم ملؤها ديناميكياً)
        
        /// <summary>
        /// عنوان الفعالية (عربي)
        /// </summary>
        public string EventTitle { get; set; }
        
        /// <summary>
        /// عنوان الفعالية (إنجليزي)
        /// </summary>
        public string EventTitleEn { get; set; }
        
        /// <summary>
        /// تاريخ بداية الفعالية
        /// </summary>
        public DateTime? EventStartDate { get; set; }
        
        /// <summary>
        /// رابط صورة الفعالية
        /// </summary>
        public string EventImageUrl { get; set; }
    }
}

