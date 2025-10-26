using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using EventManagement.Cities;

namespace EventManagement.Cities
{
    /// <summary>
    /// خدمة تطبيقية لإدارة المدن - توفر عمليات CRUD للمدن
    /// </summary>
    public class CityAppService : CrudAppService<City, CityDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateCityDto, CreateUpdateCityDto>, ICityAppService
    {
        public CityAppService(IRepository<City, Guid> repository) : base(repository)
        {
            // إلغاء سياسات الصلاحيات للسماح بالوصول العام
            CreatePolicyName = null;
            UpdatePolicyName = null;
            DeletePolicyName = null;
            GetPolicyName = null;
            GetListPolicyName = null;
        }
    }
}


