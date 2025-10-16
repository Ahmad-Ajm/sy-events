using System;
using System.ComponentModel.DataAnnotations;

namespace EventManagement.HomeSlider.Dtos
{
    /// <summary>
    /// DTO لإنشاء أو تحديث عنصر السلايدر
    /// </summary>
    public class CreateUpdateHomeSliderItemDto
    {
        /// <summary>
        /// ترتيب العرض (1-6)
        /// </summary>
        [Range(1, 6)]
        public int DisplayOrder { get; set; }
        
        /// <summary>
        /// نوع العنصر
        /// </summary>
        [Required]
        public SliderItemType Type { get; set; }
        
        /// <summary>
        /// معرف الفعالية المخصصة (مطلوب عند Type = Custom)
        /// </summary>
        public Guid? CustomEventId { get; set; }
        
        /// <summary>
        /// هل العنصر نشط؟
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// عنوان مخصص (اختياري)
        /// </summary>
        [StringLength(200)]
        public string Title { get; set; }
        
        /// <summary>
        /// عنوان مخصص باللغة الإنجليزية (اختياري)
        /// </summary>
        [StringLength(200)]
        public string TitleEn { get; set; }
        
        /// <summary>
        /// رابط صورة مخصصة (اختياري)
        /// </summary>
        [StringLength(500)]
        public string ImageUrl { get; set; }
    }
}

