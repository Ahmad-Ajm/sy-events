-- ===================================================================
-- سكريبت إضافة فعاليات وهمية واقعية - منصة إدارة الفعاليات
-- ===================================================================

-- تعليق: حذف الفعاليات الموجودة (اختياري - للتجربة فقط)
-- DELETE FROM "Events";

-- تعليق: إضافة 15 فعالية وهمية واقعية
INSERT INTO "Events" 
  ("Id", "Title", "TitleEn", "Description", "DescriptionEn", "StartDate", "EndDate", 
   "Location", "LocationEn", "MaxCapacity", "CategoryId", "CityId", "OrganizerId", 
   "IsApproved", "CreationTime", "CreatorId", "ExtraProperties", "ConcurrencyStamp")
VALUES
  -- فعالية 1: مؤتمر التقنية السنوي
  (
    gen_random_uuid(),
    'مؤتمر التقنية السنوي 2025',
    'Annual Technology Conference 2025',
    'مؤتمر سنوي يجمع خبراء التقنية والمبرمجين والشركات الناشئة من جميع أنحاء سوريا. يتضمن ورش عمل تفاعلية، جلسات نقاشية، ومعرض للشركات التقنية.',
    'An annual conference bringing together tech experts, developers, and startups from across Syria.',
    (CURRENT_DATE + INTERVAL '30 days')::timestamp,
    (CURRENT_DATE + INTERVAL '32 days')::timestamp,
    'فندق الشام - قاعة المؤتمرات الكبرى - دمشق',
    'Al-Sham Hotel - Grand Conference Hall - Damascus',
    500,
    (SELECT "Id" FROM "Categories" WHERE "Name" = 'مؤتمرات' LIMIT 1),
    (SELECT "Id" FROM "Cities" WHERE "Name" = 'دمشق' LIMIT 1),
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    true,
    CURRENT_TIMESTAMP,
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    '{}',
    gen_random_uuid()::text
  ),
  
  -- فعالية 2: ورشة تطوير الويب
  (
    gen_random_uuid(),
    'ورشة تطوير الويب المتقدم',
    'Advanced Web Development Workshop',
    'ورشة عمل مكثفة لمدة 3 أيام تغطي React وAngular مع مشاريع عملية.',
    'Intensive 3-day workshop covering React and Angular with practical projects.',
    (CURRENT_DATE + INTERVAL '15 days')::timestamp,
    (CURRENT_DATE + INTERVAL '17 days')::timestamp,
    'مركز التدريب التقني - حلب',
    'Technical Training Center - Aleppo',
    50,
    (SELECT "Id" FROM "Categories" WHERE "Name" = 'ورش عمل' LIMIT 1),
    (SELECT "Id" FROM "Cities" WHERE "Name" = 'حلب' LIMIT 1),
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    true,
    CURRENT_TIMESTAMP,
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    '{}',
    gen_random_uuid()::text
  ),
  
  -- فعالية 3: معرض الفنون
  (
    gen_random_uuid(),
    'معرض الفنون التشكيلية السورية',
    'Syrian Fine Arts Exhibition',
    'معرض فني يضم أعمال أكثر من 40 فنان سوري معاصر، لوحات ومنحوتات.',
    'Art exhibition featuring works from over 40 contemporary Syrian artists.',
    (CURRENT_DATE + INTERVAL '7 days')::timestamp,
    (CURRENT_DATE + INTERVAL '21 days')::timestamp,
    'المركز الثقافي العربي - دمشق',
    'Arab Cultural Center - Damascus',
    200,
    (SELECT "Id" FROM "Categories" WHERE "Name" = 'معارض' LIMIT 1),
    (SELECT "Id" FROM "Cities" WHERE "Name" = 'دمشق' LIMIT 1),
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    true,
    CURRENT_TIMESTAMP,
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    '{}',
    gen_random_uuid()::text
  ),
  
  -- فعالية 4: حفل موسيقي
  (
    gen_random_uuid(),
    'ليلة موسيقية كلاسيكية',
    'Classical Music Night',
    'أمسية موسيقية راقية تقدمها أوركسترا دمشق الوطنية.',
    'Elegant musical evening presented by Damascus National Orchestra.',
    (CURRENT_DATE + INTERVAL '45 days')::timestamp,
    (CURRENT_DATE + INTERVAL '45 days' + INTERVAL '3 hours')::timestamp,
    'دار الأوبرا السورية - دمشق',
    'Syrian Opera House - Damascus',
    800,
    (SELECT "Id" FROM "Categories" WHERE "Name" = 'حفلات' LIMIT 1),
    (SELECT "Id" FROM "Cities" WHERE "Name" = 'دمشق' LIMIT 1),
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    true,
    CURRENT_TIMESTAMP,
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    '{}',
    gen_random_uuid()::text
  ),
  
  -- فعالية 5: دورة Python
  (
    gen_random_uuid(),
    'دورة البرمجة بلغة Python',
    'Python Programming Course',
    'دورة شاملة مدتها شهرين، من الصفر للاحتراف مع مشاريع عملية.',
    'Comprehensive 2-month course, from zero to professional with practical projects.',
    (CURRENT_DATE + INTERVAL '60 days')::timestamp,
    (CURRENT_DATE + INTERVAL '120 days')::timestamp,
    'أكاديمية البرمجة - حمص',
    'Programming Academy - Homs',
    30,
    (SELECT "Id" FROM "Categories" WHERE "Name" = 'دورات' LIMIT 1),
    (SELECT "Id" FROM "Cities" WHERE "Name" = 'حمص' LIMIT 1),
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    true,
    CURRENT_TIMESTAMP,
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    '{}',
    gen_random_uuid()::text
  ),
  
  -- فعالية 6: مهرجان الطعام
  (
    gen_random_uuid(),
    'مهرجان الطعام السوري التراثي',
    'Syrian Heritage Food Festival',
    'مهرجان طعام يحتفي بالمطبخ السوري التقليدي مع أكثر من 50 طاهٍ.',
    'Food festival celebrating traditional Syrian cuisine with over 50 chefs.',
    (CURRENT_DATE + INTERVAL '25 days')::timestamp,
    (CURRENT_DATE + INTERVAL '27 days')::timestamp,
    'حديقة تشرين - دمشق',
    'Tishreen Park - Damascus',
    2000,
    (SELECT "Id" FROM "Categories" WHERE "Name" = 'مهرجانات' LIMIT 1),
    (SELECT "Id" FROM "Cities" WHERE "Name" = 'دمشق' LIMIT 1),
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    true,
    CURRENT_TIMESTAMP,
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    '{}',
    gen_random_uuid()::text
  ),
  
  -- فعالية 7: ندوة ريادة الأعمال
  (
    gen_random_uuid(),
    'ندوة ريادة الأعمال',
    'Entrepreneurship Seminar',
    'ندوة تجمع رواد أعمال ناجحين ومستثمرين وخبراء في مجال الأعمال.',
    'Seminar gathering successful entrepreneurs, investors, and business experts.',
    (CURRENT_DATE + INTERVAL '10 days')::timestamp,
    (CURRENT_DATE + INTERVAL '10 days' + INTERVAL '6 hours')::timestamp,
    'غرفة تجارة حلب',
    'Aleppo Chamber of Commerce',
    150,
    (SELECT "Id" FROM "Categories" WHERE "Name" = 'ندوات' LIMIT 1),
    (SELECT "Id" FROM "Cities" WHERE "Name" = 'حلب' LIMIT 1),
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    true,
    CURRENT_TIMESTAMP,
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    '{}',
    gen_random_uuid()::text
  ),
  
  -- فعالية 8: حفل تخرج
  (
    gen_random_uuid(),
    'حفل تخرج دفعة 2025',
    'Class of 2025 Graduation Ceremony',
    'حفل تخرج رسمي للطلاب الخريجين من كلية الهندسة المعلوماتية.',
    'Official graduation ceremony for Computer Engineering graduates.',
    (CURRENT_DATE + INTERVAL '40 days')::timestamp,
    (CURRENT_DATE + INTERVAL '40 days' + INTERVAL '4 hours')::timestamp,
    'جامعة دمشق - المدرج الرئيسي',
    'Damascus University - Main Auditorium',
    1000,
    (SELECT "Id" FROM "Categories" WHERE "Name" = 'احتفالات' LIMIT 1),
    (SELECT "Id" FROM "Cities" WHERE "Name" = 'دمشق' LIMIT 1),
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    true,
    CURRENT_TIMESTAMP,
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    '{}',
    gen_random_uuid()::text
  ),
  
  -- فعالية 9: معرض الكتاب
  (
    gen_random_uuid(),
    'معرض دمشق الدولي للكتاب',
    'Damascus International Book Fair',
    'المعرض الأكبر للكتب في سوريا مع أكثر من 200 دار نشر عربية وعالمية.',
    'The largest book fair in Syria with over 200 Arab and international publishers.',
    (CURRENT_DATE + INTERVAL '50 days')::timestamp,
    (CURRENT_DATE + INTERVAL '64 days')::timestamp,
    'مدينة المعارض - دمشق',
    'Damascus Exhibition City',
    5000,
    (SELECT "Id" FROM "Categories" WHERE "Name" = 'معارض' LIMIT 1),
    (SELECT "Id" FROM "Cities" WHERE "Name" = 'دمشق' LIMIT 1),
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    true,
    CURRENT_TIMESTAMP,
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    '{}',
    gen_random_uuid()::text
  ),
  
  -- فعالية 10: بطولة الشطرنج
  (
    gen_random_uuid(),
    'بطولة سوريا المفتوحة للشطرنج',
    'Syria Open Chess Championship',
    'بطولة شطرنج رسمية مفتوحة لجميع الأعمار والمستويات مع جوائز قيمة.',
    'Official chess tournament open to all ages and levels with valuable prizes.',
    (CURRENT_DATE + INTERVAL '35 days')::timestamp,
    (CURRENT_DATE + INTERVAL '38 days')::timestamp,
    'النادي الرياضي - اللاذقية',
    'Sports Club - Latakia',
    100,
    (SELECT "Id" FROM "Categories" WHERE "Name" = 'رياضة' LIMIT 1),
    (SELECT "Id" FROM "Cities" WHERE "Name" = 'اللاذقية' LIMIT 1),
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    true,
    CURRENT_TIMESTAMP,
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    '{}',
    gen_random_uuid()::text
  );

-- تعليق: عرض النتائج
SELECT 
  'تم إضافة الفعاليات بنجاح!' as status,
  (SELECT COUNT(*) FROM "Events") as total_events;

