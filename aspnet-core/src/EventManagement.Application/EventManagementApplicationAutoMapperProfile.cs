// تعليق: AutoMapper Profile - إضافة mappings للكيانات الجديدة
using AutoMapper;
using EventManagement.Events;
using EventManagement.Events.Dtos;
using EventManagement.HomeSlider;
using EventManagement.HomeSlider.Dtos;
using EventManagement.Users;
using EventManagement.Users.Dtos;
using Volo.Abp.ObjectExtending; // for GetProperty on ExtraProperties

namespace EventManagement
{
    public class EventManagementApplicationAutoMapperProfile : Profile
    {
        public EventManagementApplicationAutoMapperProfile()
        {
            // تعليق: Mappings للفعاليات
            CreateMap<Event, EventDto>();
            CreateMap<CreateUpdateEventDto, Event>();
            
            // تعليق: Mappings للسلايدر
            CreateMap<HomeSliderItem, HomeSliderItemDto>();
            CreateMap<CreateUpdateHomeSliderItemDto, HomeSliderItem>();
            
            // تعليق: Mappings للملفات
            CreateMap<EventFile, EventFileDto>()
                .ForMember(dest => dest.FileSizeFormatted, opt => opt.Ignore())
                .ForMember(dest => dest.DownloadUrl, opt => opt.Ignore())
                .ForMember(dest => dest.ThumbnailUrl, opt => opt.Ignore());
            
            // تعليق: Mappings لملفات التعريف
            CreateMap<UserProfile, UserProfileDto>()
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.Ignore());
            
            CreateMap<UpdateUserProfileDto, UserProfile>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore());
            
            // تعليق: Mappings للمناقشات
            CreateMap<EventDiscussion, EventDiscussionDto>()
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.UserProfileImage, opt => opt.Ignore())
                .ForMember(dest => dest.Replies, opt => opt.Ignore())
                .ForMember(dest => dest.RepliesCount, opt => opt.Ignore());
        }
    }
}
