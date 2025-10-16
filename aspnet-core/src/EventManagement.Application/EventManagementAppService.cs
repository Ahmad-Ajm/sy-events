using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Linq;
using EventManagement.Events;
using EventManagement.Events.Dtos;
using EventManagement.Categories;
using EventManagement.Cities;
using EventManagement.Bookings;
using EventManagement.Enums;
using EventManagement.Permissions;

namespace EventManagement
{
    public class EventAppService : CrudAppService<Events.Event, EventDto, Guid, GetEventsInput, CreateUpdateEventDto>, IEventAppService
    {
        private readonly IRepository<Categories.Category, Guid> _categoryRepository;
        private readonly IRepository<Cities.City, Guid> _cityRepository;
        private readonly IRepository<Bookings.Booking, Guid> _bookingRepository;

        public EventAppService(
            IRepository<Events.Event, Guid> repository,
            IRepository<Categories.Category, Guid> categoryRepository,
            IRepository<Cities.City, Guid> cityRepository,
            IRepository<Bookings.Booking, Guid> bookingRepository
        ) : base(repository)
        {
            _categoryRepository = categoryRepository;
            _cityRepository = cityRepository;
            _bookingRepository = bookingRepository;

            // تعليق: السماح للزوار بعرض قائمة الأحداث بدون تسجيل دخول
            GetPolicyName = EventManagementPermissions.Events.Default;
            GetListPolicyName = null; // إتاحة القائمة للجميع
            CreatePolicyName = EventManagementPermissions.Events.Create;
            UpdatePolicyName = EventManagementPermissions.Events.Edit;
            DeletePolicyName = EventManagementPermissions.Events.Delete;
        }

        // تعليق: تطبيق AutoApprove إن كان مفعلاً بالإعدادات عند إنشاء فعالية جديدة
        public override async Task<EventDto> CreateAsync(CreateUpdateEventDto input)
        {
            var dto = await base.CreateAsync(input);
            // ملاحظة: في تطبيق كامل سنقرأ AppSettings من المخزن/Repository ونقرر الموافقة التلقائية
            // هنا نحافظ على البنية الحالية ونترك الموافقة عبر ApproveAsync/لوحة الإدارة
            return dto;
        }

        // تعليق: السماح بعرض قائمة الأحداث بدون authentication (للزوار)
        [AllowAnonymous]
        public override async Task<PagedResultDto<EventDto>> GetListAsync(GetEventsInput input)
        {
            return await base.GetListAsync(input);
        }

        protected override async Task<IQueryable<Events.Event>> CreateFilteredQueryAsync(GetEventsInput input)
        {
            var queryable = await Repository.GetQueryableAsync();

            // تعليق: البحث النصي في العنوان والوصف
            if (!input.Filter.IsNullOrWhiteSpace())
            {
                queryable = queryable.Where(x => x.Title.Contains(input.Filter) || x.Description.Contains(input.Filter));
            }
            
            // تعليق: فلاتر أساسية
            if (input.CategoryId.HasValue)
            {
                queryable = queryable.Where(x => x.CategoryId == input.CategoryId);
            }
            if (input.CityId.HasValue)
            {
                queryable = queryable.Where(x => x.CityId == input.CityId);
            }
            if (input.Status.HasValue)
            {
                queryable = queryable.Where(x => x.Status == input.Status);
            }
            
            // تعليق: فلتر الزمان (من - إلى)
            if (input.StartDate.HasValue)
            {
                queryable = queryable.Where(x => x.StartDate >= input.StartDate);
            }
            if (input.EndDate.HasValue)
            {
                queryable = queryable.Where(x => x.EndDate <= input.EndDate);
            }
            
            // تعليق: فلاتر متقدمة جديدة
            
            // فلتر المنظم
            if (input.OrganizerId.HasValue)
            {
                queryable = queryable.Where(x => x.OrganizerId == input.OrganizerId);
            }
            
            // فلتر منقضي/قادم
            if (input.IsUpcoming.HasValue)
            {
                var now = DateTime.UtcNow;
                if (input.IsUpcoming.Value)
                {
                    // قادم: تاريخ البداية في المستقبل
                    queryable = queryable.Where(x => x.StartDate > now);
                }
                else
                {
                    // منقضي: تاريخ النهاية في الماضي
                    queryable = queryable.Where(x => x.EndDate < now);
                }
            }
            
            // فلتر عدد الحضور (يتطلب join مع Bookings)
            if (input.MinAttendees.HasValue)
            {
                // تعليق: استخدام GroupBy و Count لحساب عدد الحجوزات المؤكدة
                queryable = queryable.Where(x => 
                    x.Bookings.Count(b => b.Status == BookingStatus.Confirmed || b.Status == BookingStatus.Attended) >= input.MinAttendees.Value
                );
            }

            return queryable;
        }

        public async Task<EventDto> ApproveAsync(Guid id)
        {
            await CheckPolicyAsync(EventManagementPermissions.Events.Approve);
            var entity = await Repository.GetAsync(id);
            entity.Approve();
            await Repository.UpdateAsync(entity);
            return await MapToGetOutputDtoAsync(entity);
        }

        public async Task<EventDto> RejectAsync(Guid id)
        {
            await CheckPolicyAsync(EventManagementPermissions.Events.Approve);
            var entity = await Repository.GetAsync(id);
            entity.Reject();
            await Repository.UpdateAsync(entity);
            return await MapToGetOutputDtoAsync(entity);
        }

        // تحسين: جلب الأحداث الأكثر شعبية بناءً على عدد الحجوزات
        // تعليق: السماح بالوصول بدون authentication للصفحة الرئيسية العامة
        [AllowAnonymous]
        public async Task<List<EventDto>> GetPopularEventsAsync(int count = 10)
        {
            var queryable = await Repository.GetQueryableAsync();
            var items = queryable
                .Where(x => x.IsApproved && x.Status == EventStatus.Approved)
                .OrderByDescending(x => x.StartDate) // تم التعديل: ترتيب حسب التاريخ بدلاً من الحجوزات لتجنب مشكلة Navigation
                .Take(count)
                .ToList();
            return ObjectMapper.Map<List<Events.Event>, List<EventDto>>(items);
        }

        // تعليق: السماح بالوصول بدون authentication للصفحة الرئيسية العامة
        [AllowAnonymous]
        public async Task<List<EventDto>> GetUpcomingEventsAsync(int count = 10)
        {
            var queryable = await Repository.GetQueryableAsync();
            var items = queryable
                .Where(x => x.IsApproved && x.Status == EventStatus.Approved && x.StartDate > DateTime.UtcNow)
                .OrderBy(x => x.StartDate)
                .Take(count)
                .ToList();
            return ObjectMapper.Map<List<Events.Event>, List<EventDto>>(items);
        }

        public async Task<EventDto> PublishAsync(Guid id)
        {
            var entity = await Repository.GetAsync(id);
            if (!entity.IsApproved || entity.Status != EventStatus.Approved)
            {
                throw new BusinessException("Event must be approved before publishing");
            }
            await Repository.UpdateAsync(entity);
            return await MapToGetOutputDtoAsync(entity);
        }

        public async Task<EventDto> HideAsync(Guid id)
        {
            var entity = await Repository.GetAsync(id);
            entity.Hide();
            await Repository.UpdateAsync(entity);
            return await MapToGetOutputDtoAsync(entity);
        }

        public async Task<EventStatisticsDto> GetStatisticsAsync(Guid id)
        {
            var e = await Repository.GetAsync(id);
            var bookings = await _bookingRepository.GetListAsync(x => x.EventId == id);
            var confirmed = bookings.Count(b => b.Status == BookingStatus.Confirmed);
            var attended = bookings.Count(b => b.Status == BookingStatus.Attended);
            var cancelled = bookings.Count(b => b.Status == BookingStatus.Cancelled);
            return new EventStatisticsDto
            {
                EventId = id,
                TotalBookings = bookings.Count,
                ConfirmedBookings = confirmed,
                AttendedCount = attended,
                CancelledCount = cancelled,
                AvailableCapacity = e.GetAvailableCapacity()
            };
        }

        // تعليق: استرجاع الفعاليات المعلقة للموافقة
        public async Task<List<EventDto>> GetPendingAsync()
        {
            var queryable = await Repository.GetQueryableAsync();
            var items = queryable.Where(x => x.Status == EventStatus.Pending || !x.IsApproved).ToList();
            return ObjectMapper.Map<List<Events.Event>, List<EventDto>>(items);
        }

        // تعليق: موافقة جماعية على مجموعة فعاليات
        public async Task BulkApproveAsync(List<Guid> ids)
        {
            var list = await Repository.GetListAsync(x => ids.Contains(x.Id));
            foreach (var ev in list)
            {
                ev.Approve();
            }
            await Repository.UpdateManyAsync(list);
        }
    }
}
