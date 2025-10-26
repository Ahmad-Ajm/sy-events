// تعليق: Interface للخدمة
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using EventManagement.FeaturedBoxes.Dtos;

namespace EventManagement.FeaturedBoxes
{
    /// <summary>
    /// خدمة إدارة المربعات المميزة
    /// </summary>
    public interface IFeaturedBoxAppService : 
        ICrudAppService<FeaturedBoxDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateFeaturedBoxDto>
    {
        /// <summary>
        /// الحصول على المربعات النشطة للعرض في الصفحة الرئيسية
        /// </summary>
        Task<List<FeaturedBoxDto>> GetActiveFeaturedBoxesAsync();
        
        /// <summary>
        /// إعادة ترتيب المربعات
        /// </summary>
        Task ReorderAsync(List<Guid> orderedIds);
    }
}

