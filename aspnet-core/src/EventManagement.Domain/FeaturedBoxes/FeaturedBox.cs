// تعليق: Entity للمربعات المميزة في الصفحة الرئيسية
using System;
using Volo.Abp.Domain.Entities.Auditing;
using EventManagement.Enums;

namespace EventManagement.FeaturedBoxes
{
    /// <summary>
    /// المربعات المميزة (3 مربعات تحت السلايدر)
    /// </summary>
    public class FeaturedBox : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// ترتيب العرض (1, 2, 3)
        /// </summary>
        public int DisplayOrder { get; set; }
        
        /// <summary>
        /// نوع المربع (أحدث/أكثر شعبية/مخصص/قادم)
        /// </summary>
        public FeaturedBoxType Type { get; set; }
        
        /// <summary>
        /// الفعالية المخصصة (في حالة Type = Custom)
        /// </summary>
        public Guid? CustomEventId { get; set; }
        
        /// <summary>
        /// حالة التفعيل
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// العنوان بالعربية
        /// </summary>
        public string? Title { get; set; }
        
        /// <summary>
        /// العنوان بالإنجليزية
        /// </summary>
        public string? TitleEn { get; set; }
        
        /// <summary>
        /// الوصف المختصر بالعربية
        /// </summary>
        public string? Description { get; set; }
        
        /// <summary>
        /// الوصف المختصر بالإنجليزية
        /// </summary>
        public string? DescriptionEn { get; set; }
        
        /// <summary>
        /// رابط الصورة
        /// </summary>
        public string? ImageUrl { get; set; }
        
        /// <summary>
        /// رابط مخصص (اختياري)
        /// </summary>
        public string? CustomLink { get; set; }
        
        // تعليق: Constructor محمي للـ EF Core
        protected FeaturedBox() { }
        
        /// <summary>
        /// Constructor لإنشاء مربع جديد
        /// </summary>
        public FeaturedBox(
            Guid id,
            int displayOrder,
            FeaturedBoxType type,
            bool isActive = true) : base(id)
        {
            DisplayOrder = displayOrder;
            Type = type;
            IsActive = isActive;
        }
        
        /// <summary>
        /// تعيين فعالية مخصصة
        /// </summary>
        public void SetCustomEvent(Guid eventId, string title, string? titleEn = null)
        {
            if (Type != FeaturedBoxType.Custom)
            {
                throw new InvalidOperationException("Can only set custom event when Type is Custom");
            }
            
            CustomEventId = eventId;
            Title = title;
            TitleEn = titleEn;
        }
        
        /// <summary>
        /// تحديث الترتيب
        /// </summary>
        public void SetDisplayOrder(int order)
        {
            if (order < 1 || order > 3)
            {
                throw new ArgumentException("Display order must be between 1 and 3", nameof(order));
            }
            
            DisplayOrder = order;
        }
        
        /// <summary>
        /// تفعيل/تعطيل المربع
        /// </summary>
        public void SetActive(bool isActive)
        {
            IsActive = isActive;
        }
    }
}

