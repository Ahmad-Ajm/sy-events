// تعليق: خدمة إدارة ملفات الفعاليات - رفع/حذف/عرض الملفات
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using EventManagement.Events;
using EventManagement.Events.Dtos;
using EventManagement.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace EventManagement.Events
{
    [Authorize]
    public class EventFileAppService : ApplicationService, IEventFileAppService
    {
        private readonly IRepository<EventFile, Guid> _fileRepository;
        private readonly IRepository<Event, Guid> _eventRepository;

        public EventFileAppService(
            IRepository<EventFile, Guid> fileRepository,
            IRepository<Event, Guid> eventRepository)
        {
            _fileRepository = fileRepository;
            _eventRepository = eventRepository;
        }

        // تعليق: جلب جميع ملفات فعالية
        public async Task<List<EventFileDto>> GetEventFilesAsync(Guid eventId)
        {
            var files = await _fileRepository.GetListAsync(x => x.EventId == eventId);
            var orderedFiles = files.OrderBy(x => x.DisplayOrder).ToList();
            return ObjectMapper.Map<List<EventFile>, List<EventFileDto>>(orderedFiles);
        }

        // تعليق: جلب ملف واحد
        public async Task<EventFileDto> GetAsync(Guid id)
        {
            var file = await _fileRepository.GetAsync(id);
            return ObjectMapper.Map<EventFile, EventFileDto>(file);
        }

        // تعليق: حذف ملف
        [Authorize(EventManagementPermissions.Events.Edit)]
        public async Task DeleteAsync(Guid id)
        {
            var file = await _fileRepository.GetAsync(id);
            
            // TODO: حذف الملف الفعلي من FileSystem
            // var filePath = Path.Combine("upload", file.FilePath);
            // if (File.Exists(filePath)) File.Delete(filePath);
            
            await _fileRepository.DeleteAsync(id);
        }

        // تعليق: تحديث ترتيب العرض
        [Authorize(EventManagementPermissions.Events.Edit)]
        public async Task UpdateDisplayOrderAsync(Guid id, int order)
        {
            var file = await _fileRepository.GetAsync(id);
            file.SetDisplayOrder(order);
            await _fileRepository.UpdateAsync(file);
        }
    }
}

