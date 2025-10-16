using System.ComponentModel.DataAnnotations;

namespace EventManagement.Settings.Dtos
{
    /// <summary>
    /// DTO لتحديث إعدادات التطبيق
    /// </summary>
    public class UpdateAppSettingsDto
    {
        /// <summary>
        /// عدد عناصر السلايدر (2-6)
        /// </summary>
        [Range(2, 6)]
        public int SliderItemsCount { get; set; }
        
        /// <summary>
        /// الموافقة التلقائية على الفعاليات الجديدة
        /// </summary>
        public bool AutoApproveEvents { get; set; }
    }
}

