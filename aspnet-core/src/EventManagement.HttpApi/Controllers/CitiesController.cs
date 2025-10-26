using System;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using System.Threading.Tasks;
using EventManagement.Cities;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace EventManagement.Controllers
{
    /// <summary>
    /// API Controller لإدارة المدن - توفر نقاط نهاية CRUD
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/app/city")] 
    public class CitiesController : AbpController
    {
        private readonly ICityAppService _service;
        
        public CitiesController(ICityAppService service) 
        { 
            _service = service; 
        }

        /// <summary>
        /// الحصول على قائمة المدن مع الترحيل والترتيب
        /// </summary>
        [HttpGet]
        public Task<PagedResultDto<CityDto>> GetListAsync(PagedAndSortedResultRequestDto input) => _service.GetListAsync(input);

        /// <summary>
        /// الحصول على مدينة واحدة حسب المعرّف
        /// </summary>
        [HttpGet("{id}")]
        public Task<CityDto> GetAsync(Guid id) => _service.GetAsync(id);

        /// <summary>
        /// إنشاء مدينة جديدة
        /// </summary>
        [HttpPost]
        public Task<CityDto> CreateAsync(CreateUpdateCityDto input) => _service.CreateAsync(input);

        /// <summary>
        /// تحديث مدينة موجودة
        /// </summary>
        [HttpPut("{id}")]
        public Task<CityDto> UpdateAsync(Guid id, CreateUpdateCityDto input) => _service.UpdateAsync(id, input);

        /// <summary>
        /// حذف مدينة
        /// </summary>
        [HttpDelete("{id}")]
        public Task DeleteAsync(Guid id) => _service.DeleteAsync(id);
    }
}


