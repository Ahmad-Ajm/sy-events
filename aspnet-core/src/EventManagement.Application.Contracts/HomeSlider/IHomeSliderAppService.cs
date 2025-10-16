using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using EventManagement.HomeSlider.Dtos;
using EventManagement.Settings.Dtos;

namespace EventManagement.HomeSlider
{
    /// <summary>
    /// واجهة خدمة إدارة السلايدر الرئيسي
    /// </summary>
    public interface IHomeSliderAppService : 
        ICrudAppService<HomeSliderItemDto, Guid, PagedAndSortedResultRequestDto, 
                        CreateUpdateHomeSliderItemDto>
    {
        /// <summary>
        /// الحصول على عناصر السلايدر النشطة للعرض في الصفحة الرئيسية
        /// </summary>
        /// <returns>قائمة بعناصر السلايدر النشطة مع بيانات الفعاليات</returns>
        Task<List<HomeSliderItemDto>> GetActiveSliderItemsAsync();
        
        /// <summary>
        /// إعادة ترتيب عناصر السلايدر
        /// </summary>
        /// <param name="orderedIds">قائمة معرفات العناصر بالترتيب الجديد</param>
        Task ReorderAsync(List<Guid> orderedIds);
        
        /// <summary>
        /// الحصول على إعدادات التطبيق
        /// </summary>
        /// <returns>إعدادات التطبيق</returns>
        Task<AppSettingsDto> GetSettingsAsync();
        
        /// <summary>
        /// تحديث إعدادات التطبيق
        /// </summary>
        /// <param name="input">الإعدادات الجديدة</param>
        Task UpdateSettingsAsync(UpdateAppSettingsDto input);
    }
}

