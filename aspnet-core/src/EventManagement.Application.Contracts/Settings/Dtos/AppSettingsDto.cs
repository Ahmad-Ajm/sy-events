using System;
using Volo.Abp.Application.Dtos;

namespace EventManagement.Settings.Dtos
{
    /// <summary>
    /// DTO لعرض إعدادات التطبيق
    /// </summary>
    public class AppSettingsDto : EntityDto<Guid>
    {
        /// <summary>
        /// عدد عناصر السلايدر
        /// </summary>
        public int SliderItemsCount { get; set; }
        
        /// <summary>
        /// الموافقة التلقائية على الفعاليات
        /// </summary>
        public bool AutoApproveEvents { get; set; }
    }
}

