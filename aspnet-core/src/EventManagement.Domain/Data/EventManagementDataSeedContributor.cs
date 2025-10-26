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
using EventManagement.HomeSlider;
using EventManagement.Settings;
using EventManagement.FeaturedBoxes;

namespace EventManagement.Domain.Data
{
    // تعليق: Seed أساسي لمدن/تصنيفات/مستخدمين/فعاليات وسلايدر أولي
    public class EventManagementDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<City, Guid> _cityRepo;
        private readonly IRepository<Category, Guid> _categoryRepo;
        private readonly IRepository<User, Guid> _userRepo;
        private readonly IRepository<Event, Guid> _eventRepo;
        private readonly IRepository<HomeSliderItem, Guid> _sliderRepo;
        private readonly IRepository<AppSettings, Guid> _settingsRepo;
        private readonly IRepository<FeaturedBox, Guid> _featuredBoxRepo;
        private readonly IGuidGenerator _guidGenerator;

        public EventManagementDataSeedContributor(
            IRepository<City, Guid> cityRepo,
            IRepository<Category, Guid> categoryRepo,
            IRepository<User, Guid> userRepo,
            IRepository<Event, Guid> eventRepo,
            IRepository<HomeSliderItem, Guid> sliderRepo,
            IRepository<AppSettings, Guid> settingsRepo,
            IRepository<FeaturedBox, Guid> featuredBoxRepo,
            IGuidGenerator guidGenerator)
        {
            _cityRepo = cityRepo;
            _categoryRepo = categoryRepo;
            _userRepo = userRepo;
            _eventRepo = eventRepo;
            _sliderRepo = sliderRepo;
            _settingsRepo = settingsRepo;
            _featuredBoxRepo = featuredBoxRepo;
            _guidGenerator = guidGenerator;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            // تعليق: المدن السورية (14 محافظة)
            if (await _cityRepo.GetCountAsync() == 0)
            {
                await _cityRepo.InsertManyAsync(new[]
                {
                    new City(_guidGenerator.Create(), "دمشق", "Damascus"),
                    new City(_guidGenerator.Create(), "ريف دمشق", "Damascus Countryside"),
                    new City(_guidGenerator.Create(), "حلب", "Aleppo"),
                    new City(_guidGenerator.Create(), "حمص", "Homs"),
                    new City(_guidGenerator.Create(), "حماة", "Hama"),
                    new City(_guidGenerator.Create(), "اللاذقية", "Latakia"),
                    new City(_guidGenerator.Create(), "طرطوس", "Tartus"),
                    new City(_guidGenerator.Create(), "السويداء", "As-Suwayda"),
                    new City(_guidGenerator.Create(), "درعا", "Daraa"),
                    new City(_guidGenerator.Create(), "دير الزور", "Deir ez-Zor"),
                    new City(_guidGenerator.Create(), "الرقة", "Raqqa"),
                    new City(_guidGenerator.Create(), "إدلب", "Idlib"),
                    new City(_guidGenerator.Create(), "الحسكة", "Al-Hasakah"),
                    new City(_guidGenerator.Create(), "القنيطرة", "Quneitra")
                });
            }

            // تعليق: التصنيفات (طبي، تقني، هندسي، تجاري، سياسي، إنساني، وغيرها)
            if (await _categoryRepo.GetCountAsync() == 0)
            {
                await _categoryRepo.InsertManyAsync(new[]
                {
                    new Category(_guidGenerator.Create(), "تقني", "Technology")
                    {
                        Description = "فعاليات تقنية وبرمجية",
                        DescriptionEn = "Technology and programming events",
                        Icon = "fa-laptop-code"
                    },
                    new Category(_guidGenerator.Create(), "طبي", "Medical")
                    {
                        Description = "مؤتمرات ومحاضرات طبية",
                        DescriptionEn = "Medical conferences and lectures",
                        Icon = "fa-stethoscope"
                    },
                    new Category(_guidGenerator.Create(), "هندسي", "Engineering")
                    {
                        Description = "فعاليات هندسية ومعمارية",
                        DescriptionEn = "Engineering and architecture events",
                        Icon = "fa-drafting-compass"
                    },
                    new Category(_guidGenerator.Create(), "تجاري", "Business")
                    {
                        Description = "أعمال ومعارض تجارية",
                        DescriptionEn = "Business and commercial exhibitions",
                        Icon = "fa-briefcase"
                    },
                    new Category(_guidGenerator.Create(), "سياسي", "Political")
                    {
                        Description = "ندوات ومؤتمرات سياسية",
                        DescriptionEn = "Political seminars and conferences",
                        Icon = "fa-landmark"
                    },
                    new Category(_guidGenerator.Create(), "إنساني", "Humanitarian")
                    {
                        Description = "فعاليات إنسانية وخيرية",
                        DescriptionEn = "Humanitarian and charity events",
                        Icon = "fa-hands-helping"
                    },
                    new Category(_guidGenerator.Create(), "تعليمي", "Educational")
                    {
                        Description = "ورشات عمل تعليمية وتدريبية",
                        DescriptionEn = "Educational workshops and training",
                        Icon = "fa-graduation-cap"
                    },
                    new Category(_guidGenerator.Create(), "ثقافي", "Cultural")
                    {
                        Description = "فعاليات ثقافية وفنية",
                        DescriptionEn = "Cultural and artistic events",
                        Icon = "fa-theater-masks"
                    },
                    new Category(_guidGenerator.Create(), "رياضي", "Sports")
                    {
                        Description = "فعاليات ومسابقات رياضية",
                        DescriptionEn = "Sports events and competitions",
                        Icon = "fa-running"
                    },
                    new Category(_guidGenerator.Create(), "ديني", "Religious")
                    {
                        Description = "فعاليات ومحاضرات دينية",
                        DescriptionEn = "Religious events and lectures",
                        Icon = "fa-mosque"
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

            // تعليق: فعاليات واقعية متنوعة (12 فعالية)
            if (await _eventRepo.GetCountAsync() == 0)
            {
                var cities = await _cityRepo.GetListAsync();
                var cats = await _categoryRepo.GetListAsync();
                var organizer = (await _userRepo.GetListAsync(x => x.Role == UserRole.Organizer)).FirstOrDefault();
                var now = DateTime.UtcNow;

                if (organizer != null && cities.Any() && cats.Any())
                {
                    // تعليق: الحصول على التصنيفات أو أول تصنيف متاح كـ fallback
                    var firstCat = cats.First();
                    var firstCity = cities.First();
                    
                    var techCat = cats.FirstOrDefault(c => c.Name == "تقني") ?? firstCat;
                    var medicalCat = cats.FirstOrDefault(c => c.Name == "طبي") ?? firstCat;
                    var businessCat = cats.FirstOrDefault(c => c.Name == "تجاري") ?? firstCat;
                    var culturalCat = cats.FirstOrDefault(c => c.Name == "ثقافي") ?? firstCat;
                    var educationalCat = cats.FirstOrDefault(c => c.Name == "تعليمي") ?? firstCat;
                    
                    var damascus = cities.FirstOrDefault(c => c.Name == "دمشق") ?? firstCity;
                    var aleppo = cities.FirstOrDefault(c => c.Name == "حلب") ?? firstCity;
                    var homs = cities.FirstOrDefault(c => c.Name == "حمص") ?? firstCity;
                    var latakia = cities.FirstOrDefault(c => c.Name == "اللاذقية") ?? firstCity;

                    await _eventRepo.InsertManyAsync(new[]
                    {
                        // تعليق: فعالية 1 - مؤتمر تقني (معتمدة)
                        new Event(_guidGenerator.Create(), 
                            "مؤتمر سوريا للتقنية والابتكار 2025", 
                            "مؤتمر تقني سنوي يجمع أبرز الخبراء والمطورين في سوريا لمناقشة أحدث التقنيات والاتجاهات في عالم البرمجة والذكاء الاصطناعي. يتضمن ورش عمل تفاعلية ومحاضرات ملهمة.", 
                            now.AddDays(7), 
                            now.AddDays(7).AddHours(8), 
                            "فندق الشام - قاعة المؤتمرات الكبرى", 
                            techCat.Id, 
                            damascus.Id, 
                            organizer.Id)
                        {
                            TitleEn = "Syria Tech & Innovation Conference 2025",
                            DescriptionEn = "Annual tech conference bringing together top experts and developers to discuss latest trends in AI and software development.",
                            LocationEn = "Al-Sham Hotel - Grand Conference Hall",
                            Status = EventStatus.Approved,
                            IsApproved = true,
                            MaxCapacity = 500,
                            ImageUrl = "/images/events/tech-conference.jpg",
                            ThumbnailUrl = "/images/events/thumbs/tech-conference.jpg"
                        },
                        
                        // تعليق: فعالية 2 - ورشة تطوير الويب (معتمدة)
                        new Event(_guidGenerator.Create(), 
                            "ورشة عمل: بناء تطبيقات الويب الحديثة", 
                            "ورشة عملية مكثفة لمدة يومين تغطي أحدث تقنيات تطوير الويب مثل React, Angular, Node.js. مناسبة للمبتدئين والمحترفين.", 
                            now.AddDays(14), 
                            now.AddDays(14).AddHours(6), 
                            "مركز حلب للتدريب التقني - الطابق الثالث", 
                            techCat.Id, 
                            aleppo.Id, 
                            organizer.Id)
                        {
                            TitleEn = "Workshop: Building Modern Web Applications",
                            DescriptionEn = "Intensive 2-day workshop covering modern web technologies including React, Angular, Node.js.",
                            LocationEn = "Aleppo Technical Training Center - 3rd Floor",
                            Status = EventStatus.Approved,
                            IsApproved = true,
                            MaxCapacity = 50,
                            ImageUrl = "/images/events/web-workshop.jpg",
                            ThumbnailUrl = "/images/events/thumbs/web-workshop.jpg"
                        },
                        
                        // تعليق: فعالية 3 - مؤتمر طبي (معتمدة)
                        new Event(_guidGenerator.Create(), 
                            "المؤتمر الطبي السوري السنوي 2025", 
                            "مؤتمر طبي شامل يجمع الأطباء والمختصين لمناقشة أحدث الأبحاث والعلاجات في مختلف المجالات الطبية. يشمل محاضرات علمية ومعارض طبية.", 
                            now.AddDays(21), 
                            now.AddDays(21).AddHours(7), 
                            "مشفى دمشق الجامعي - قاعة المحاضرات", 
                            medicalCat.Id, 
                            damascus.Id, 
                            organizer.Id)
                        {
                            TitleEn = "Syrian Annual Medical Conference 2025",
                            DescriptionEn = "Comprehensive medical conference bringing together doctors and specialists to discuss latest research and treatments.",
                            LocationEn = "Damascus University Hospital - Lecture Hall",
                            Status = EventStatus.Approved,
                            IsApproved = true,
                            MaxCapacity = 300,
                            ImageUrl = "/images/events/medical-conference.jpg",
                            ThumbnailUrl = "/images/events/thumbs/medical-conference.jpg"
                        },
                        
                        // تعليق: فعالية 4 - معرض تجاري (قيد المراجعة)
                        new Event(_guidGenerator.Create(), 
                            "معرض حمص التجاري الدولي", 
                            "معرض تجاري ضخم يضم أكثر من 200 عارض من مختلف القطاعات: المواد الغذائية، الألبسة، الإلكترونيات، والمزيد. فرصة ممتازة للتجار والمستثمرين.", 
                            now.AddDays(30), 
                            now.AddDays(32), 
                            "مدينة المعارض - حمص", 
                            businessCat.Id, 
                            homs.Id, 
                            organizer.Id)
                        {
                            TitleEn = "Homs International Trade Fair",
                            DescriptionEn = "Large trade fair featuring 200+ exhibitors from various sectors including food, clothing, electronics.",
                            LocationEn = "Exhibition City - Homs",
                            Status = EventStatus.Pending,
                            IsApproved = false,
                            MaxCapacity = 5000,
                            ImageUrl = "/images/events/trade-fair.jpg",
                            ThumbnailUrl = "/images/events/thumbs/trade-fair.jpg"
                        },
                        
                        // تعليق: فعالية 5 - مهرجان ثقافي (معتمدة)
                        new Event(_guidGenerator.Create(), 
                            "مهرجان اللاذقية الثقافي الصيفي", 
                            "مهرجان ثقافي فني يستمر 3 أيام يتضمن عروض موسيقية، مسرحيات، معارض فنية، وأمسيات شعرية. احتفال بالتنوع الثقافي السوري.", 
                            now.AddDays(45), 
                            now.AddDays(47), 
                            "مسرح دار الأسد للثقافة والفنون - اللاذقية", 
                            culturalCat.Id, 
                            latakia.Id, 
                            organizer.Id)
                        {
                            TitleEn = "Latakia Summer Cultural Festival",
                            DescriptionEn = "3-day cultural festival featuring music, theater, art exhibitions, and poetry evenings.",
                            LocationEn = "Assad Cultural Center - Latakia",
                            Status = EventStatus.Approved,
                            IsApproved = true,
                            MaxCapacity = 800,
                            ImageUrl = "/images/events/cultural-festival.jpg",
                            ThumbnailUrl = "/images/events/thumbs/cultural-festival.jpg"
                        },
                        
                        // تعليق: فعالية 6 - ندوة تعليمية (معتمدة)
                        new Event(_guidGenerator.Create(), 
                            "ندوة: التعليم الإلكتروني والمستقبل", 
                            "ندوة تعليمية تناقش مستقبل التعليم في العصر الرقمي، أساليب التعليم عن بعد، وكيفية دمج التكنولوجيا في التعليم التقليدي.", 
                            now.AddDays(10), 
                            now.AddDays(10).AddHours(4), 
                            "جامعة دمشق - كلية التربية", 
                            educationalCat.Id, 
                            damascus.Id, 
                            organizer.Id)
                        {
                            TitleEn = "Seminar: E-Learning and the Future",
                            DescriptionEn = "Educational seminar discussing the future of education in the digital age and distance learning methods.",
                            LocationEn = "Damascus University - Faculty of Education",
                            Status = EventStatus.Approved,
                            IsApproved = true,
                            MaxCapacity = 150,
                            ImageUrl = "/images/events/education-seminar.jpg",
                            ThumbnailUrl = "/images/events/thumbs/education-seminar.jpg"
                        },
                        
                        // تعليق: فعالية 7 - ورشة برمجة للأطفال (معتمدة)
                        new Event(_guidGenerator.Create(), 
                            "ورشة برمجة للأطفال - Scratch و Minecraft", 
                            "ورشة ممتعة وتفاعلية لتعليم الأطفال (8-14 سنة) أساسيات البرمجة من خلال ألعاب Scratch و Minecraft Education. تنمية مهارات التفكير المنطقي.", 
                            now.AddDays(5), 
                            now.AddDays(5).AddHours(3), 
                            "مركز الإبداع الشبابي - حلب", 
                            educationalCat.Id, 
                            aleppo.Id, 
                            organizer.Id)
                        {
                            TitleEn = "Kids Coding Workshop - Scratch & Minecraft",
                            DescriptionEn = "Fun interactive workshop teaching kids (8-14) programming basics through Scratch and Minecraft Education.",
                            LocationEn = "Youth Creativity Center - Aleppo",
                            Status = EventStatus.Approved,
                            IsApproved = true,
                            MaxCapacity = 30,
                            ImageUrl = "/images/events/kids-coding.jpg",
                            ThumbnailUrl = "/images/events/thumbs/kids-coding.jpg"
                        },
                        
                        // تعليق: فعالية 8 - لقاء ريادة أعمال (معتمدة)
                        new Event(_guidGenerator.Create(), 
                            "لقاء رواد الأعمال السوريين", 
                            "لقاء شهري يجمع رواد الأعمال والمستثمرين لتبادل الخبرات، عرض المشاريع الناشئة، وبناء شراكات استراتيجية. فرصة للتواصل والتعلم.", 
                            now.AddDays(3), 
                            now.AddDays(3).AddHours(3), 
                            "مركز دمشق للأعمال - الطابق الخامس", 
                            businessCat.Id, 
                            damascus.Id, 
                            organizer.Id)
                        {
                            TitleEn = "Syrian Entrepreneurs Meetup",
                            DescriptionEn = "Monthly gathering for entrepreneurs and investors to exchange experiences and build partnerships.",
                            LocationEn = "Damascus Business Center - 5th Floor",
                            Status = EventStatus.Approved,
                            IsApproved = true,
                            MaxCapacity = 100,
                            ImageUrl = "/images/events/entrepreneurs.jpg",
                            ThumbnailUrl = "/images/events/thumbs/entrepreneurs.jpg"
                        },
                        
                        // تعليق: فعالية 9 - دورة أمن سيبراني (قيد المراجعة)
                        new Event(_guidGenerator.Create(), 
                            "دورة تدريبية: أساسيات الأمن السيبراني", 
                            "دورة متقدمة تغطي مبادئ الأمن السيبراني، الحماية من الاختراقات، التشفير، والممارسات الأمنية. شهادة معتمدة بعد الانتهاء.", 
                            now.AddDays(20), 
                            now.AddDays(24), 
                            "معهد التدريب التقني - حمص", 
                            techCat.Id, 
                            homs.Id, 
                            organizer.Id)
                        {
                            TitleEn = "Training Course: Cybersecurity Fundamentals",
                            DescriptionEn = "Advanced course covering cybersecurity principles, encryption, and security best practices.",
                            LocationEn = "Technical Training Institute - Homs",
                            Status = EventStatus.Pending,
                            IsApproved = false,
                            MaxCapacity = 40,
                            ImageUrl = "/images/events/cybersecurity.jpg",
                            ThumbnailUrl = "/images/events/thumbs/cybersecurity.jpg"
                        },
                        
                        // تعليق: فعالية 10 - معرض كتاب (معتمدة)
                        new Event(_guidGenerator.Create(), 
                            "معرض دمشق الدولي للكتاب 2025", 
                            "معرض الكتاب السنوي الأكبر في سوريا، يضم دور نشر محلية وعربية وعالمية. آلاف العناوين في مختلف المجالات، مع ندوات وتوقيعات للكتاب.", 
                            now.AddDays(60), 
                            now.AddDays(75), 
                            "مدينة المعارض - باب توما، دمشق", 
                            culturalCat.Id, 
                            damascus.Id, 
                            organizer.Id)
                        {
                            TitleEn = "Damascus International Book Fair 2025",
                            DescriptionEn = "Syria's largest annual book fair featuring local, Arab, and international publishers.",
                            LocationEn = "Exhibition City - Bab Touma, Damascus",
                            Status = EventStatus.Approved,
                            IsApproved = true,
                            MaxCapacity = 10000,
                            ImageUrl = "/images/events/book-fair.jpg",
                            ThumbnailUrl = "/images/events/thumbs/book-fair.jpg"
                        },
                        
                        // تعليق: فعالية 11 - هاكاثون برمجي (معتمدة)
                        new Event(_guidGenerator.Create(), 
                            "هاكاثون سوريا للبرمجة 2025", 
                            "مسابقة برمجية لمدة 48 ساعة للمطورين والمبرمجين. تحديات حقيقية، جوائز قيمة، فرص للتوظيف. اعمل مع فريقك لبناء حلول مبتكرة.", 
                            now.AddDays(35), 
                            now.AddDays(37), 
                            "جامعة حلب - كلية الهندسة المعلوماتية", 
                            techCat.Id, 
                            aleppo.Id, 
                            organizer.Id)
                        {
                            TitleEn = "Syria Programming Hackathon 2025",
                            DescriptionEn = "48-hour coding competition for developers and programmers with real challenges and valuable prizes.",
                            LocationEn = "Aleppo University - Faculty of Informatics Engineering",
                            Status = EventStatus.Approved,
                            IsApproved = true,
                            MaxCapacity = 200,
                            ImageUrl = "/images/events/hackathon.jpg",
                            ThumbnailUrl = "/images/events/thumbs/hackathon.jpg"
                        },
                        
                        // تعليق: فعالية 12 - مؤتمر طاقة متجددة (معتمدة)
                        new Event(_guidGenerator.Create(), 
                            "مؤتمر الطاقة المتجددة والاستدامة", 
                            "مؤتمر متخصص يناقش حلول الطاقة المتجددة في سوريا، الطاقة الشمسية، طاقة الرياح، والممارسات المستدامة. خبراء محليون ودوليون.", 
                            now.AddDays(50), 
                            now.AddDays(50).AddHours(6), 
                            "فندق البحر الأبيض - اللاذقية", 
                            businessCat.Id, 
                            latakia.Id, 
                            organizer.Id)
                        {
                            TitleEn = "Renewable Energy & Sustainability Conference",
                            DescriptionEn = "Specialized conference discussing renewable energy solutions in Syria including solar and wind power.",
                            LocationEn = "Mediterranean Hotel - Latakia",
                            Status = EventStatus.Approved,
                            IsApproved = true,
                            MaxCapacity = 250,
                            ImageUrl = "/images/events/renewable-energy.jpg",
                            ThumbnailUrl = "/images/events/thumbs/renewable-energy.jpg"
                        }
                    });
                }
            }

            // إعدادات التطبيق الافتراضية
            if (await _settingsRepo.GetCountAsync() == 0)
            {
                await _settingsRepo.InsertAsync(
                    new AppSettings(_guidGenerator.Create())
                    {
                        SliderItemsCount = 3,
                        AutoApproveEvents = false
                    }
                );
            }

            // عناصر السلايدر الأولية
            if (await _sliderRepo.GetCountAsync() == 0)
            {
                var approvedEvents = await _eventRepo.GetListAsync(x => x.IsApproved);
                if (approvedEvents.Any())
                {
                    var firstEvent = approvedEvents.First();
                    await _sliderRepo.InsertManyAsync(new[]
                    {
                        new HomeSliderItem(
                            _guidGenerator.Create(),
                            1, // DisplayOrder
                            SliderItemType.Custom, // Type
                            true // IsActive
                        )
                        {
                            Title = firstEvent.Title,
                            TitleEn = firstEvent.TitleEn ?? "Event Slider",
                            ImageUrl = firstEvent.ImageUrl ?? "/images/slider/default1.jpg",
                            CustomEventId = firstEvent.Id
                        }
                    });
                }
            }

            // تعليق: المربعات الثلاث تحت السلايدر (FeaturedBoxes)
            if (await _featuredBoxRepo.GetCountAsync() == 0)
            {
                var approvedEvents = await _eventRepo.GetListAsync(x => x.IsApproved);
                if (approvedEvents.Any())
                {
                    await _featuredBoxRepo.InsertManyAsync(new[]
                    {
                        // المربع 1: أحدث الفعاليات
                        new FeaturedBox(
                            _guidGenerator.Create(),
                            1, // DisplayOrder
                            FeaturedBoxType.Latest,
                            true // IsActive
                        )
                        {
                            Title = "أحدث الفعاليات",
                            TitleEn = "Latest Events",
                            Description = "تصفح أحدث الفعاليات والأنشطة",
                            DescriptionEn = "Browse the latest events and activities"
                        },
                        // المربع 2: الأكثر شعبية
                        new FeaturedBox(
                            _guidGenerator.Create(),
                            2, // DisplayOrder
                            FeaturedBoxType.Popular,
                            true // IsActive
                        )
                        {
                            Title = "الأكثر شعبية",
                            TitleEn = "Most Popular",
                            Description = "الفعاليات الأكثر حجزاً ومتابعة",
                            DescriptionEn = "Most booked and followed events"
                        },
                        // المربع 3: القادمة قريباً
                        new FeaturedBox(
                            _guidGenerator.Create(),
                            3, // DisplayOrder
                            FeaturedBoxType.Upcoming,
                            true // IsActive
                        )
                        {
                            Title = "قادمة قريباً",
                            TitleEn = "Coming Soon",
                            Description = "الفعاليات القادمة في الأيام المقبلة",
                            DescriptionEn = "Upcoming events in the next days"
                        }
                    });
                }
            }
        }
    }
}


