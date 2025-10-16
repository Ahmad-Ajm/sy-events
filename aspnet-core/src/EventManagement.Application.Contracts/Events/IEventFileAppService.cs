// تعليق: واجهة خدمة ملفات الفعالية
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using EventManagement.Events.Dtos;

namespace EventManagement.Events
{
    public interface IEventFileAppService : IApplicationService
    {
        // تعليق: جلب ملفات فعالية
        Task<List<EventFileDto>> GetEventFilesAsync(Guid eventId);
        
        // تعليق: جلب ملف واحد
        Task<EventFileDto> GetAsync(Guid id);
        
        // تعليق: حذف ملف
        Task DeleteAsync(Guid id);
        
        // تعليق: تحديث ترتيب العرض
        Task UpdateDisplayOrderAsync(Guid id, int order);
    }
}

