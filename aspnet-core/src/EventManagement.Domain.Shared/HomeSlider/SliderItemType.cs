namespace EventManagement.HomeSlider
{
    /// <summary>
    /// نوع عنصر السلايدر - يحدد كيفية اختيار الفعالية المعروضة
    /// </summary>
    public enum SliderItemType
    {
        /// <summary>
        /// أحدث الفعاليات
        /// </summary>
        Latest = 1,
        
        /// <summary>
        /// الأكثر شعبية (حسب عدد الحجوزات)
        /// </summary>
        Popular = 2,
        
        /// <summary>
        /// فعالية محددة يدوياً
        /// </summary>
        Custom = 3
    }
}

