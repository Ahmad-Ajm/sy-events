// تعليق: خدمة إدارة المربعات المميزة
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using EventManagement.Permissions;
using EventManagement.FeaturedBoxes.Dtos;
using EventManagement.Events;
using EventManagement.Enums;
using EventManagement.Bookings;

namespace EventManagement.FeaturedBoxes
{
    /// <summary>
    /// خدمة إدارة المربعات المميزة في الصفحة الرئيسية
    /// تعليق: هذه الخدمة تدير المربعات الثلاث تحت السلايدر
    /// </summary>
    [Authorize(EventManagementPermissions.Admin.Settings)]
    public class FeaturedBoxAppService : 
        CrudAppService<FeaturedBox, FeaturedBoxDto, Guid, 
                       PagedAndSortedResultRequestDto, CreateUpdateFeaturedBoxDto>,
        IFeaturedBoxAppService
    {
        private readonly IRepository<Event, Guid> _eventRepository;
        private readonly IRepository<Booking, Guid> _bookingRepository;
        
        public FeaturedBoxAppService(
            IRepository<FeaturedBox, Guid> repository,
            IRepository<Event, Guid> eventRepository,
            IRepository<Booking, Guid> bookingRepository)
            : base(repository)
        {
            _eventRepository = eventRepository;
            _bookingRepository = bookingRepository;
        }
        
        /// <summary>
        /// تعليق: الحصول على المربعات النشطة - متاح بدون تسجيل دخول
        /// </summary>
        [AllowAnonymous]
        public async Task<List<FeaturedBoxDto>> GetActiveFeaturedBoxesAsync()
        {
            // تعليق: جلب المربعات النشطة مرتبة حسب DisplayOrder
            var boxes = await Repository.GetListAsync();
            var activeBoxes = boxes
                .Where(x => x.IsActive)
                .OrderBy(x => x.DisplayOrder)
                .Take(3) // فقط 3 مربعات
                .ToList();
            
            var dtos = new List<FeaturedBoxDto>();
            
            // تعليق: معالجة كل مربع وملء بيانات الفعالية حسب النوع
            foreach (var box in activeBoxes)
            {
                var dto = ObjectMapper.Map<FeaturedBox, FeaturedBoxDto>(box);
                
                Event? eventToDisplay = null;
                
                // تعليق: إذا كان النوع Latest - نحضر أحدث فعالية معتمدة
                if (box.Type == FeaturedBoxType.Latest)
                {
                    var allEvents = await _eventRepository.GetListAsync();
                    eventToDisplay = allEvents
                        .Where(x => x.IsApproved && x.Status == EventStatus.Approved)
                        .OrderByDescending(x => x.CreationTime)
                        .FirstOrDefault();
                    
                    dto.Title = dto.Title ?? "أحدث الفعاليات";
                    dto.TitleEn = dto.TitleEn ?? "Latest Events";
                }
                // تعليق: إذا كان النوع Popular - نحضر الفعالية الأكثر شعبية
                else if (box.Type == FeaturedBoxType.Popular)
                {
                    var allEvents = await _eventRepository.GetListAsync();
                    var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
                    var allBookings = await _bookingRepository.GetListAsync();
                    
                    // تعليق: حساب الشعبية بناءً على عدد الحجوزات في آخر 30 يوم
                    eventToDisplay = allEvents
                        .Where(x => x.IsApproved && x.Status == EventStatus.Approved)
                        .Select(e => new
                        {
                            Event = e,
                            BookingsCount = allBookings.Count(b => 
                                b.EventId == e.Id && 
                                b.CreationTime >= thirtyDaysAgo)
                        })
                        .OrderByDescending(x => x.BookingsCount)
                        .ThenByDescending(x => x.Event.StartDate)
                        .Select(x => x.Event)
                        .FirstOrDefault();
                    
                    dto.Title = dto.Title ?? "الأكثر شعبية";
                    dto.TitleEn = dto.TitleEn ?? "Most Popular";
                }
                // تعليق: إذا كان النوع Upcoming - نحضر أقرب فعالية قادمة
                else if (box.Type == FeaturedBoxType.Upcoming)
                {
                    var now = DateTime.UtcNow;
                    var allEvents = await _eventRepository.GetListAsync();
                    eventToDisplay = allEvents
                        .Where(x => x.IsApproved && 
                                   x.Status == EventStatus.Approved &&
                                   x.StartDate > now)
                        .OrderBy(x => x.StartDate)
                        .FirstOrDefault();
                    
                    dto.Title = dto.Title ?? "قادم قريباً";
                    dto.TitleEn = dto.TitleEn ?? "Coming Soon";
                }
                // تعليق: إذا كان النوع Custom - نستخدم الفعالية المحددة يدوياً
                else if (box.Type == FeaturedBoxType.Custom && box.CustomEventId.HasValue)
                {
                    eventToDisplay = await _eventRepository.GetAsync(box.CustomEventId.Value);
                }
                
                // تعليق: ملء بيانات الفعالية في الـ DTO
                if (eventToDisplay != null)
                {
                    dto.EventTitle = eventToDisplay.Title;
                    dto.EventTitleEn = eventToDisplay.TitleEn;
                    dto.EventStartDate = eventToDisplay.StartDate;
                    dto.EventImageUrl = eventToDisplay.ImageUrl;
                    dto.EventLocation = eventToDisplay.Location;
                    
                    // استخدام صورة الفعالية إذا لم يكن هناك صورة مخصصة
                    if (string.IsNullOrEmpty(dto.ImageUrl))
                    {
                        dto.ImageUrl = eventToDisplay.ImageUrl;
                    }
                    
                    // إنشاء رابط للفعالية إذا لم يكن هناك رابط مخصص
                    if (string.IsNullOrEmpty(dto.CustomLink))
                    {
                        dto.CustomLink = $"/events/{eventToDisplay.Id}";
                    }
                }
                
                dtos.Add(dto);
            }
            
            return dtos;
        }
        
        /// <summary>
        /// تعليق: إعادة ترتيب المربعات
        /// </summary>
        public async Task ReorderAsync(List<Guid> orderedIds)
        {
            if (orderedIds.Count > 3)
            {
                throw new ArgumentException("Maximum 3 featured boxes allowed");
            }
            
            var boxes = await Repository.GetListAsync();
            
            // تعليق: تحديث ترتيب كل مربع حسب موقعه في القائمة
            for (int i = 0; i < orderedIds.Count; i++)
            {
                var box = boxes.FirstOrDefault(x => x.Id == orderedIds[i]);
                if (box != null)
                {
                    box.SetDisplayOrder(i + 1);
                }
            }
            
            await Repository.UpdateManyAsync(boxes);
        }
    }
}

