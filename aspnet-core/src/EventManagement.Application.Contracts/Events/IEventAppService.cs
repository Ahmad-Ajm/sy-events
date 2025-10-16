using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using EventManagement.Events.Dtos;

namespace EventManagement.Events
{
    public interface IEventAppService : ICrudAppService<EventDto, Guid, GetEventsInput, CreateUpdateEventDto>
    {
        Task<EventDto> ApproveAsync(Guid id);
        Task<EventDto> RejectAsync(Guid id);
        Task<EventDto> PublishAsync(Guid id);
        Task<EventDto> HideAsync(Guid id);
        Task<List<EventDto>> GetPopularEventsAsync(int count = 10);
        Task<List<EventDto>> GetUpcomingEventsAsync(int count = 10);
        Task<EventStatisticsDto> GetStatisticsAsync(Guid id);

        // تعليق: إدارة الموافقات
        Task<List<EventDto>> GetPendingAsync();
        Task BulkApproveAsync(List<Guid> ids);
    }
}


