using System;
using Volo.Abp.Application.Dtos;

namespace EventManagement.Cities
{
    /// <summary>
    /// DTO لعرض معلومات المدينة
    /// </summary>
    public class CityDto : EntityDto<Guid>
    {
        /// <summary>
        /// اسم المدينة بالعربية
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// اسم المدينة بالإنجليزية
        /// </summary>
        public string NameEn { get; set; } = string.Empty;
    }
}


