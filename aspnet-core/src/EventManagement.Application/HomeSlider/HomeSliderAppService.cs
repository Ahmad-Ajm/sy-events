using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using EventManagement.Permissions;
using EventManagement.HomeSlider.Dtos;
using EventManagement.Settings;
using EventManagement.Settings.Dtos;
using EventManagement.Events;
using EventManagement.Enums;

namespace EventManagement.HomeSlider
{
    /// <summary>
    /// خدمة إدارة السلايدر الرئيسي
    /// تعليق: هذه الخدمة تدير عناصر السلايدر في الصفحة الرئيسية
    /// </summary>
    [Authorize(EventManagementPermissions.Admin.Settings)]
    public class HomeSliderAppService : 
        CrudAppService<HomeSliderItem, HomeSliderItemDto, Guid, 
                       PagedAndSortedResultRequestDto, CreateUpdateHomeSliderItemDto>,
        IHomeSliderAppService
    {
        private readonly IRepository<Event, Guid> _eventRepository;
        private readonly IRepository<AppSettings, Guid> _settingsRepository;
        
        public HomeSliderAppService(
            IRepository<HomeSliderItem, Guid> repository,
            IRepository<Event, Guid> eventRepository,
            IRepository<AppSettings, Guid> settingsRepository)
            : base(repository)
        {
            _eventRepository = eventRepository;
            _settingsRepository = settingsRepository;
        }
        
        /// <summary>
        /// تعليق: الحصول على عناصر السلايدر النشطة - متاح بدون تسجيل دخول
        /// </summary>
        [AllowAnonymous]
        public async Task<List<HomeSliderItemDto>> GetActiveSliderItemsAsync()
        {
            // تعليق: الحصول على الإعدادات لمعرفة عدد العناصر المطلوب عرضها
            var settings = await GetOrCreateSettingsAsync();
            var count = settings.SliderItemsCount;
            
            // تعليق: جلب العناصر النشطة مرتبة حسب DisplayOrder
            var items = await Repository.GetListAsync();
            var activeItems = items
                .Where(x => x.IsActive)
                .OrderBy(x => x.DisplayOrder)
                .Take(count)
                .ToList();
            
            var dtos = new List<HomeSliderItemDto>();
            
            // تعليق: معالجة كل عنصر وملء بيانات الفعالية حسب النوع
            foreach (var item in activeItems)
            {
                var dto = ObjectMapper.Map<HomeSliderItem, HomeSliderItemDto>(item);
                
                // تعليق: إذا كان النوع Latest - نحضر أحدث فعالية معتمدة
                if (item.Type == SliderItemType.Latest)
                {
                    var allEvents = await _eventRepository.GetListAsync();
                    var latestEvent = allEvents
                        .Where(x => x.IsApproved && x.Status == EventStatus.Approved)
                        .OrderByDescending(x => x.CreationTime)
                        .FirstOrDefault();
                    
                    if (latestEvent != null)
                    {
                        dto.EventTitle = latestEvent.Title;
                        dto.EventTitleEn = latestEvent.TitleEn;
                        dto.EventStartDate = latestEvent.StartDate;
                        dto.EventImageUrl = latestEvent.ImageUrl;
                        dto.CustomEventId = latestEvent.Id;
                    }
                }
                // تعليق: إذا كان النوع Popular - نحضر الفعالية الأكثر شعبية (حسب التاريخ حاليا لتجنب lazy loading issue)
                else if (item.Type == SliderItemType.Popular)
                {
                    var allEvents = await _eventRepository.GetListAsync();
                    var popularEvent = allEvents
                        .Where(x => x.IsApproved && x.Status == EventStatus.Approved)
                        .OrderByDescending(x => x.StartDate)
                        .FirstOrDefault();
                    
                    if (popularEvent != null)
                    {
                        dto.EventTitle = popularEvent.Title;
                        dto.EventTitleEn = popularEvent.TitleEn;
                        dto.EventStartDate = popularEvent.StartDate;
                        dto.EventImageUrl = popularEvent.ImageUrl;
                        dto.CustomEventId = popularEvent.Id;
                    }
                }
                // تعليق: إذا كان النوع Custom - نستخدم الفعالية المحددة يدوياً
                else if (item.Type == SliderItemType.Custom && item.CustomEventId.HasValue)
                {
                    var customEvent = await _eventRepository.GetAsync(item.CustomEventId.Value);
                    if (customEvent != null)
                    {
                        dto.EventTitle = customEvent.Title;
                        dto.EventTitleEn = customEvent.TitleEn;
                        dto.EventStartDate = customEvent.StartDate;
                        dto.EventImageUrl = customEvent.ImageUrl;
                    }
                }
                
                dtos.Add(dto);
            }
            
            return dtos;
        }
        
        /// <summary>
        /// تعليق: إعادة ترتيب عناصر السلايدر
        /// </summary>
        public async Task ReorderAsync(List<Guid> orderedIds)
        {
            var items = await Repository.GetListAsync();
            
            // تعليق: تحديث ترتيب كل عنصر حسب موقعه في القائمة
            for (int i = 0; i < orderedIds.Count; i++)
            {
                var item = items.FirstOrDefault(x => x.Id == orderedIds[i]);
                if (item != null)
                {
                    item.DisplayOrder = i + 1;
                }
            }
            
            await Repository.UpdateManyAsync(items);
        }
        
        /// <summary>
        /// تعليق: الحصول على إعدادات التطبيق - متاح بدون تسجيل دخول
        /// </summary>
        [AllowAnonymous]
        public async Task<AppSettingsDto> GetSettingsAsync()
        {
            var settings = await GetOrCreateSettingsAsync();
            return ObjectMapper.Map<AppSettings, AppSettingsDto>(settings);
        }
        
        /// <summary>
        /// تعليق: تحديث إعدادات التطبيق
        /// </summary>
        public async Task UpdateSettingsAsync(UpdateAppSettingsDto input)
        {
            var settings = await GetOrCreateSettingsAsync();
            
            settings.SliderItemsCount = input.SliderItemsCount;
            settings.AutoApproveEvents = input.AutoApproveEvents;
            
            await _settingsRepository.UpdateAsync(settings);
        }
        
        /// <summary>
        /// تعليق: الحصول على الإعدادات أو إنشاؤها إذا لم تكن موجودة
        /// </summary>
        private async Task<AppSettings> GetOrCreateSettingsAsync()
        {
            var settings = await _settingsRepository.FirstOrDefaultAsync();
            if (settings == null)
            {
                // تعليق: إنشاء إعدادات افتراضية
                settings = new AppSettings(GuidGenerator.Create())
                {
                    SliderItemsCount = 3, // العدد الافتراضي
                    AutoApproveEvents = false
                };
                await _settingsRepository.InsertAsync(settings);
            }
            return settings;
        }
    }
}

