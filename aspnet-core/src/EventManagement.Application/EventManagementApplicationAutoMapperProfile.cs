// تعليق: AutoMapper Profile - تكوين التحويلات بين Entities و DTOs
using AutoMapper;
using EventManagement.Events;
using EventManagement.Events.Dtos;
using EventManagement.Bookings;
using EventManagement.HomeSlider;
using EventManagement.HomeSlider.Dtos;
using EventManagement.Settings;
using EventManagement.Settings.Dtos;
using EventManagement.FeaturedBoxes;
using EventManagement.FeaturedBoxes.Dtos;
using EventManagement.Users.Dtos;
using EventManagement.Users;
using EventManagement.Meetings;

namespace EventManagement
{
    public class EventManagementApplicationAutoMapperProfile : Profile
    {
        public EventManagementApplicationAutoMapperProfile()
        {
            // تعليق: Event mappings
            CreateMap<Event, EventDto>();
            CreateMap<CreateUpdateEventDto, Event>();
            
            // تعليق: City mappings
            CreateMap<Cities.City, Cities.CityDto>();
            CreateMap<Cities.CreateUpdateCityDto, Cities.City>();
            
            // تعليق: HomeSliderItem mappings
            CreateMap<HomeSliderItem, HomeSliderItemDto>();
            CreateMap<CreateUpdateHomeSliderItemDto, HomeSliderItem>();
            
            // تعليق: AppSettings mappings
            CreateMap<AppSettings, AppSettingsDto>();
            CreateMap<UpdateAppSettingsDto, AppSettings>();
            
            // تعليق: FeaturedBox mappings (جديد)
            CreateMap<FeaturedBox, FeaturedBoxDto>();
            CreateMap<CreateUpdateFeaturedBoxDto, FeaturedBox>();
            
            // تعليق: UserProfile mappings
            CreateMap<UserProfile, UserProfileDto>();
            CreateMap<UpdateUserProfileDto, UserProfile>();
            
            // تعليق: EventFile mappings
            CreateMap<EventFile, EventFileDto>();
            
            // تعليق: EventDiscussion mappings
            CreateMap<EventDiscussion, EventDiscussionDto>();
            
            // تعليق: AttendeeMeeting mappings
            CreateMap<AttendeeMeeting, Meetings.AttendeeMeetingDto>();
            
            // تعليق: Booking mappings
            CreateMap<Booking, BookingDto>();
        }
    }
}
