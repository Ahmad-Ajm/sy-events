using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using EventManagement.Cities;
using EventManagement.Categories;
using EventManagement.Users;
using EventManagement.Events;
using EventManagement.Enums;

namespace EventManagement.Domain.Data
{
    // تعليق: Seed أساسي لمدن/تصنيفات/مستخدمين/فعاليات وسلايدر أولي
    public class EventManagementDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<City, Guid> _cityRepo;
        private readonly IRepository<Category, Guid> _categoryRepo;
        private readonly IRepository<User, Guid> _userRepo;
        private readonly IRepository<Event, Guid> _eventRepo;
        private readonly IGuidGenerator _guidGenerator;

        public EventManagementDataSeedContributor(
            IRepository<City, Guid> cityRepo,
            IRepository<Category, Guid> categoryRepo,
            IRepository<User, Guid> userRepo,
            IRepository<Event, Guid> eventRepo,
            IGuidGenerator guidGenerator)
        {
            _cityRepo = cityRepo;
            _categoryRepo = categoryRepo;
            _userRepo = userRepo;
            _eventRepo = eventRepo;
            _guidGenerator = guidGenerator;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            // مدن
            if (await _cityRepo.GetCountAsync() == 0)
            {
                await _cityRepo.InsertManyAsync(new[]
                {
                    new City(_guidGenerator.Create(), "دمشق", "Damascus"),
                    new City(_guidGenerator.Create(), "حلب", "Aleppo"),
                    new City(_guidGenerator.Create(), "حمص", "Homs"),
                    new City(_guidGenerator.Create(), "اللاذقية", "Latakia"),
                });
            }

            // تصنيفات
            if (await _categoryRepo.GetCountAsync() == 0)
            {
                await _categoryRepo.InsertManyAsync(new[]
                {
                    new Category(_guidGenerator.Create(), "مؤتمر", "Conference")
                    {
                        Description = "فعاليات مؤتمرات",
                        DescriptionEn = "Conference events",
                        Icon = "conference"
                    },
                    new Category(_guidGenerator.Create(), "ورشة عمل", "Workshop")
                    {
                        Description = "ورشات عمل تدريبية",
                        DescriptionEn = "Training workshops",
                        Icon = "workshop"
                    },
                    new Category(_guidGenerator.Create(), "معرض", "Exhibition")
                    {
                        Description = "معارض ومنتديات",
                        DescriptionEn = "Exhibitions and forums",
                        Icon = "exhibition"
                    }
                });
            }

            // مستخدمون (منظم + مستخدم عادي)
            if (await _userRepo.GetCountAsync() == 0)
            {
                var damascus = (await _cityRepo.GetListAsync()).FirstOrDefault();
                await _userRepo.InsertManyAsync(new[]
                {
                    new User(_guidGenerator.Create(), "organizer@example.com", "Organizer One", "hashed-pass", UserRole.Organizer)
                    {
                        CityId = damascus?.Id,
                        Phone = "+963-11-1234567",
                        Profession = "Event Organizer",
                        Interests = "Technology, Events",
                        Reason = "Professional event management"
                    },
                    new User(_guidGenerator.Create(), "viewer@example.com", "Viewer One", "hashed-pass", UserRole.Viewer)
                    {
                        CityId = damascus?.Id,
                        Phone = "+963-11-7654321",
                        Profession = "Software Developer",
                        Interests = "Technology, Workshops",
                        Reason = "Learning and networking"
                    }
                });
            }

            // فعاليات أولية
            if (await _eventRepo.GetCountAsync() == 0)
            {
                var cities = await _cityRepo.GetListAsync();
                var cats = await _categoryRepo.GetListAsync();
                var organizer = (await _userRepo.GetListAsync(x => x.Role == UserRole.Organizer)).FirstOrDefault();
                var now = DateTime.UtcNow;

                if (organizer != null && cities.Any() && cats.Any())
                {
                    await _eventRepo.InsertManyAsync(new[]
                    {
                        new Event(_guidGenerator.Create(), "مؤتمر التقنية السنوي", "وصف مؤتمر", now.AddDays(7), now.AddDays(7).AddHours(3), "فندق الشام - دمشق", cats[0].Id, cities[0].Id, organizer.Id)
                        {
                            Status = EventStatus.Approved,
                            IsApproved = true,
                            ImageUrl = "",
                            ThumbnailUrl = ""
                        },
                        new Event(_guidGenerator.Create(), "ورشة عمل تطوير الويب", "وصف ورشة", now.AddDays(14), now.AddDays(14).AddHours(4), "مركز التدريب - حلب", cats[1].Id, cities[1].Id, organizer.Id)
                        {
                            Status = EventStatus.Pending,
                            IsApproved = false
                        }
                    });
                }
            }
        }
    }
}


