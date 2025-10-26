using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace EventManagement.Cities
{
    /// <summary>
    /// واجهة خدمة إدارة المدن - تتيح عمليات CRUD للمدن
    /// </summary>
    public interface ICityAppService : 
        ICrudAppService<CityDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateCityDto, CreateUpdateCityDto>
    {
    }
}


