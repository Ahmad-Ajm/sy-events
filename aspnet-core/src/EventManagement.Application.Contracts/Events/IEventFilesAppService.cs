using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Volo.Abp.Application.Services;

namespace EventManagement.Events
{
    /// <summary>
    /// واجهة خدمة إدارة ملفات الفعاليات المتعددة
    /// </summary>
    public interface IEventFilesAppService : IApplicationService
    {
        /// <summary>
        /// رفع ملفات متعددة لفعالية معينة
        /// </summary>
        Task UploadMultipleAsync(Guid eventId, IFormFile[] files);
    }
}


