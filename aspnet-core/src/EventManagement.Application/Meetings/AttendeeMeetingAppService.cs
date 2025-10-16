// تعليق: خدمة جدولة الاجتماعات - إدارة اللقاءات بين الحضور
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using EventManagement.Meetings;
using EventManagement.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace EventManagement.Meetings
{
    [Authorize]
    public class AttendeeMeetingAppService : ApplicationService
    {
        private readonly IRepository<AttendeeMeeting, Guid> _meetingRepository;
        private readonly ICurrentUser _currentUser;

        public AttendeeMeetingAppService(
            IRepository<AttendeeMeeting, Guid> meetingRepository,
            ICurrentUser currentUser)
        {
            _meetingRepository = meetingRepository;
            _currentUser = currentUser;
        }

        // تعليق: إرسال طلب اجتماع
        public async Task<AttendeeMeetingDto> RequestMeetingAsync(CreateAttendeeMeetingDto input)
        {
            var meeting = new AttendeeMeeting(
                GuidGenerator.Create(),
                input.EventId,
                _currentUser.GetId(),
                input.RequestedId,
                input.MeetingTime,
                input.Location,
                input.Notes
            );

            await _meetingRepository.InsertAsync(meeting);
            
            // TODO: إرسال إشعار للمستخدم المطلوب
            
            return ObjectMapper.Map<AttendeeMeeting, AttendeeMeetingDto>(meeting);
        }

        // تعليق: قبول طلب اجتماع
        public async Task<AttendeeMeetingDto> AcceptMeetingAsync(Guid id)
        {
            var meeting = await _meetingRepository.GetAsync(id);
            
            // التحقق من أن المستخدم الحالي هو المطلوب منه الاجتماع
            if (meeting.RequestedId != _currentUser.Id)
            {
                throw new UnauthorizedAccessException("غير مسموح لك بقبول هذا الطلب");
            }
            
            meeting.Accept();
            await _meetingRepository.UpdateAsync(meeting);
            
            // TODO: إرسال إشعار للمستخدم الطالب
            
            return ObjectMapper.Map<AttendeeMeeting, AttendeeMeetingDto>(meeting);
        }

        // تعليق: رفض طلب اجتماع
        public async Task<AttendeeMeetingDto> RejectMeetingAsync(Guid id, string reason)
        {
            var meeting = await _meetingRepository.GetAsync(id);
            
            if (meeting.RequestedId != _currentUser.Id)
            {
                throw new UnauthorizedAccessException("غير مسموح لك برفض هذا الطلب");
            }
            
            meeting.Reject(reason);
            await _meetingRepository.UpdateAsync(meeting);
            
            return ObjectMapper.Map<AttendeeMeeting, AttendeeMeetingDto>(meeting);
        }

        // تعليق: إلغاء اجتماع
        public async Task CancelMeetingAsync(Guid id)
        {
            var meeting = await _meetingRepository.GetAsync(id);
            
            // يمكن للطرفين إلغاء الاجتماع
            if (meeting.RequesterId != _currentUser.Id && meeting.RequestedId != _currentUser.Id)
            {
                throw new UnauthorizedAccessException("غير مسموح لك بإلغاء هذا الاجتماع");
            }
            
            meeting.Cancel();
            await _meetingRepository.UpdateAsync(meeting);
        }

        // تعليق: جلب طلبات الاجتماعات الواردة
        public async Task<List<AttendeeMeetingDto>> GetIncomingRequestsAsync()
        {
            var meetings = await _meetingRepository.GetListAsync(x => 
                x.RequestedId == _currentUser.Id &&
                x.Status == MeetingStatus.Pending
            );
            
            return ObjectMapper.Map<List<AttendeeMeeting>, List<AttendeeMeetingDto>>(meetings);
        }

        // تعليق: جلب طلبات الاجتماعات الصادرة
        public async Task<List<AttendeeMeetingDto>> GetOutgoingRequestsAsync()
        {
            var meetings = await _meetingRepository.GetListAsync(x => 
                x.RequesterId == _currentUser.Id
            );
            
            return ObjectMapper.Map<List<AttendeeMeeting>, List<AttendeeMeetingDto>>(meetings);
        }

        // تعليق: جلب جميع الاجتماعات المقبولة
        public async Task<List<AttendeeMeetingDto>> GetMyMeetingsAsync()
        {
            var meetings = await _meetingRepository.GetListAsync(x => 
                (x.RequesterId == _currentUser.Id || x.RequestedId == _currentUser.Id) &&
                x.Status == MeetingStatus.Accepted
            );
            
            var sorted = meetings.OrderBy(x => x.MeetingTime).ToList();
            return ObjectMapper.Map<List<AttendeeMeeting>, List<AttendeeMeetingDto>>(sorted);
        }
    }
    
    // تعليق: DTOs للاجتماعات
    public class AttendeeMeetingDto
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public Guid RequesterId { get; set; }
        public string RequesterName { get; set; }
        public Guid RequestedId { get; set; }
        public string RequestedName { get; set; }
        public DateTime MeetingTime { get; set; }
        public string Location { get; set; }
        public MeetingStatus Status { get; set; }
        public string Notes { get; set; }
        public string RejectionReason { get; set; }
    }
    
    public class CreateAttendeeMeetingDto
    {
        public Guid EventId { get; set; }
        public Guid RequestedId { get; set; }
        public DateTime MeetingTime { get; set; }
        public string Location { get; set; }
        public string Notes { get; set; }
    }
}

