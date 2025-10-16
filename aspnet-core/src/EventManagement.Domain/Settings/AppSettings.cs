using System;
using Volo.Abp.Domain.Entities;

namespace EventManagement.Settings
{
    /// <summary>
    /// إعدادات التطبيق العامة
    /// </summary>
    public class AppSettings : Entity<Guid>
    {
        /// <summary>
        /// عدد عناصر السلايدر في الصفحة الرئيسية (2-6)
        /// </summary>
        public int SliderItemsCount { get; set; }
        
        /// <summary>
        /// الموافقة التلقائية على الفعاليات الجديدة
        /// </summary>
        public bool AutoApproveEvents { get; set; }
        
        protected AppSettings()
        {
        }
        
        public AppSettings(Guid id) : base(id)
        {
            SliderItemsCount = 3; // القيمة الافتراضية
            AutoApproveEvents = false;
        }
    }
}

