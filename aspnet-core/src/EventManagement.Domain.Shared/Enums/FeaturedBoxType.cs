// تعليق: أنواع المربعات المميزة في الصفحة الرئيسية
namespace EventManagement.Enums
{
    /// <summary>
    /// نوع المربع المميز
    /// </summary>
    public enum FeaturedBoxType
    {
        /// <summary>
        /// أحدث الفعاليات
        /// </summary>
        Latest = 1,
        
        /// <summary>
        /// الأكثر شعبية
        /// </summary>
        Popular = 2,
        
        /// <summary>
        /// فعالية محددة يدوياً
        /// </summary>
        Custom = 3,
        
        /// <summary>
        /// القادمة قريباً
        /// </summary>
        Upcoming = 4
    }
}

