using System;
using Volo.Abp.Domain.Entities.Auditing;
using EventManagement.Events;

namespace EventManagement.HomeSlider
{
    /// <summary>
    /// عنصر في السلايدر الرئيسي للصفحة الرئيسية
    /// </summary>
    public class HomeSliderItem : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// ترتيب العرض (1-6)
        /// </summary>
        public int DisplayOrder { get; set; }
        
        /// <summary>
        /// نوع العنصر (أحدث/أكثر شعبية/مخصص)
        /// </summary>
        public SliderItemType Type { get; set; }
        
        /// <summary>
        /// معرف الفعالية المخصصة (عندما Type = Custom)
        /// </summary>
        public Guid? CustomEventId { get; set; }
        
        /// <summary>
        /// هل العنصر نشط؟
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// عنوان مخصص (اختياري) - بالعربية
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// عنوان مخصص (اختياري) - بالإنجليزية
        /// </summary>
        public string TitleEn { get; set; }
        
        /// <summary>
        /// رابط صورة مخصصة (اختياري)
        /// </summary>
        public string ImageUrl { get; set; }
        
        /// <summary>
        /// Navigation property للفعالية المخصصة
        /// </summary>
        public virtual Event CustomEvent { get; set; }
        
        protected HomeSliderItem()
        {
        }
        
        public HomeSliderItem(
            Guid id,
            int displayOrder,
            SliderItemType type,
            bool isActive = true
        ) : base(id)
        {
            DisplayOrder = displayOrder;
            Type = type;
            IsActive = isActive;
        }
    }
}

