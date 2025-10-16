// تعليق: خدمة منتديات النقاش - إضافة/عرض/حذف التعليقات
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using EventManagement.Events;
using EventManagement.Events.Dtos;
using EventManagement.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace EventManagement.Events
{
    [Authorize]
    public class EventDiscussionAppService : ApplicationService
    {
        private readonly IRepository<EventDiscussion, Guid> _discussionRepository;
        private readonly IRepository<Event, Guid> _eventRepository;
        private readonly ICurrentUser _currentUser;

        public EventDiscussionAppService(
            IRepository<EventDiscussion, Guid> discussionRepository,
            IRepository<Event, Guid> eventRepository,
            ICurrentUser currentUser)
        {
            _discussionRepository = discussionRepository;
            _eventRepository = eventRepository;
            _currentUser = currentUser;
        }

        // تعليق: جلب جميع تعليقات فعالية (مع الردود المتداخلة)
        [AllowAnonymous]
        public async Task<List<EventDiscussionDto>> GetEventDiscussionsAsync(Guid eventId)
        {
            // جلب التعليقات الرئيسية فقط (بدون parent)
            var rootComments = await _discussionRepository.GetListAsync(x => 
                x.EventId == eventId && 
                x.ParentId == null &&
                !x.IsHidden
            );
            
            var dtos = ObjectMapper.Map<List<EventDiscussion>, List<EventDiscussionDto>>(
                rootComments.OrderByDescending(x => x.CreationTime).ToList()
            );
            
            // جلب الردود لكل تعليق
            foreach (var dto in dtos)
            {
                dto.Replies = await GetRepliesAsync(dto.Id);
                dto.RepliesCount = dto.Replies.Count;
            }
            
            return dtos;
        }

        // تعليق: جلب الردود على تعليق
        private async Task<List<EventDiscussionDto>> GetRepliesAsync(Guid parentId)
        {
            var replies = await _discussionRepository.GetListAsync(x => 
                x.ParentId == parentId &&
                !x.IsHidden
            );
            
            return ObjectMapper.Map<List<EventDiscussion>, List<EventDiscussionDto>>(
                replies.OrderBy(x => x.CreationTime).ToList()
            );
        }

        // تعليق: إضافة تعليق جديد
        public async Task<EventDiscussionDto> CreateAsync(CreateEventDiscussionDto input)
        {
            // التحقق من وجود الفعالية
            await _eventRepository.GetAsync(input.EventId);
            
            var discussion = new EventDiscussion(
                GuidGenerator.Create(),
                input.EventId,
                _currentUser.GetId(),
                input.Message,
                input.ParentId
            );
            
            await _discussionRepository.InsertAsync(discussion);
            
            return ObjectMapper.Map<EventDiscussion, EventDiscussionDto>(discussion);
        }

        // تعليق: حذف تعليق (للمالك أو المسؤول)
        public async Task DeleteAsync(Guid id)
        {
            var discussion = await _discussionRepository.GetAsync(id);
            
            // التحقق من الصلاحية
            if (discussion.CreatorId != _currentUser.Id)
            {
                await CheckPolicyAsync(EventManagementPermissions.Events.Delete);
            }
            
            await _discussionRepository.DeleteAsync(id);
        }

        // تعليق: إخفاء تعليق (للمسؤولين فقط)
        [Authorize(EventManagementPermissions.Events.Delete)]
        public async Task HideAsync(Guid id, string reason)
        {
            var discussion = await _discussionRepository.GetAsync(id);
            discussion.Hide(reason);
            await _discussionRepository.UpdateAsync(discussion);
        }

        // تعليق: إظهار تعليق مخفي
        [Authorize(EventManagementPermissions.Events.Delete)]
        public async Task ShowAsync(Guid id)
        {
            var discussion = await _discussionRepository.GetAsync(id);
            discussion.Show();
            await _discussionRepository.UpdateAsync(discussion);
        }
    }
}

