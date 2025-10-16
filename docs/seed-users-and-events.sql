-- ===================================================================
-- إضافة مستخدمين وفعاليات وهمية - النسخة الكاملة
-- ===================================================================

-- تعليق: الخطوة 1 - إضافة مستخدمين وهميين في جدول User المخصص
INSERT INTO "User" 
  ("Id", "Email", "Name", "PasswordHash", "Phone", "Profession", "CityId", 
   "Interests", "Reason", "Role", "ExtraProperties", "ConcurrencyStamp", 
   "CreationTime", "IsDeleted")
VALUES
  -- مستخدم 1: منظم فعاليات تقنية
  (
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    'organizer1@events.sy',
    'أحمد محمد - منظم فعاليات',
    'AQAAAAEAACcQAAAAEDummy',
    '+963991234567',
    'مطور برمجيات',
    (SELECT "Id" FROM "Cities" WHERE "Name" = 'دمشق' LIMIT 1),
    'تقنية، برمجة، ذكاء اصطناعي',
    'منظم فعاليات تقنية',
    1,
    '{}',
    gen_random_uuid()::text,
    CURRENT_TIMESTAMP,
    false
  ),
  
  -- مستخدم 2: منظم فعاليات ثقافية
  (
    gen_random_uuid(),
    'organizer2@events.sy',
    'فاطمة علي - منظمة فعاليات ثقافية',
    'AQAAAAEAACcQAAAAEDummy',
    '+963992345678',
    'منسقة فعاليات',
    (SELECT "Id" FROM "Cities" WHERE "Name" = 'حلب' LIMIT 1),
    'فنون، ثقافة، موسيقى',
    'منظمة فعاليات ثقافية وفنية',
    1,
    '{}',
    gen_random_uuid()::text,
    CURRENT_TIMESTAMP,
    false
  ),
  
  -- مستخدم 3: متابع
  (
    gen_random_uuid(),
    'follower1@events.sy',
    'خالد حسن - متابع',
    'AQAAAAEAACcQAAAAEDummy',
    '+963993456789',
    'طالب جامعي',
    (SELECT "Id" FROM "Cities" WHERE "Name" = 'دمشق' LIMIT 1),
    'تعلم، تطوير ذات',
    'مهتم بحضور الفعاليات التعليمية',
    0,
    '{}',
    gen_random_uuid()::text,
    CURRENT_TIMESTAMP,
    false
  );

-- تعليق: الخطوة 2 - إضافة الفعاليات الوهمية
INSERT INTO "Events" 
  ("Id", "Title", "TitleEn", "Description", "DescriptionEn", "StartDate", "EndDate", 
   "Location", "LocationEn", "MaxCapacity", "Status", "ImageUrl", "ThumbnailUrl",
   "CategoryId", "CityId", "OrganizerId", "IsApproved", "IsDeleted",
   "CreationTime", "CreatorId", "ExtraProperties", "ConcurrencyStamp")
VALUES
  -- فعالية 1: مؤتمر التقنية
  (
    gen_random_uuid(),
    'مؤتمر التقنية السنوي 2025',
    'Annual Technology Conference 2025',
    'مؤتمر سنوي يجمع خبراء التقنية والمبرمجين والشركات الناشئة من جميع أنحاء سوريا. يتضمن ورش عمل تفاعلية، جلسات نقاشية، ومعرض للشركات التقنية. فرصة مثالية للتواصل والتعلم من أفضل المختصين في المجال.',
    'An annual conference bringing together tech experts, developers, and startups from across Syria.',
    (CURRENT_DATE + INTERVAL '30 days')::timestamp,
    (CURRENT_DATE + INTERVAL '32 days')::timestamp,
    'فندق الشام - قاعة المؤتمرات الكبرى - دمشق',
    'Al-Sham Hotel - Grand Conference Hall - Damascus',
    500,
    0,
    'https://images.unsplash.com/photo-1540575467063-178a50c2df87?w=1200&h=600&fit=crop',
    'https://images.unsplash.com/photo-1540575467063-178a50c2df87?w=400&h=300&fit=crop',
    (SELECT "Id" FROM "Categories" WHERE "Name" = 'مؤتمرات' LIMIT 1),
    (SELECT "Id" FROM "Cities" WHERE "Name" = 'دمشق' LIMIT 1),
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    true,
    false,
    CURRENT_TIMESTAMP,
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    '{}',
    gen_random_uuid()::text
  ),
  
  -- فعالية 2: ورشة Web
  (
    gen_random_uuid(),
    'ورشة تطوير الويب المتقدم - React & Angular',
    'Advanced Web Development Workshop',
    'ورشة عمل مكثفة لمدة 3 أيام تغطي أحدث تقنيات تطوير الويب باستخدام React وAngular. تشمل مشاريع عملية وشهادة حضور معتمدة.',
    'Intensive 3-day workshop covering the latest web development technologies using React and Angular.',
    (CURRENT_DATE + INTERVAL '15 days')::timestamp,
    (CURRENT_DATE + INTERVAL '17 days')::timestamp,
    'مركز التدريب التقني - حلب',
    'Technical Training Center - Aleppo',
    50,
    0,
    'https://images.unsplash.com/photo-1531482615713-2afd69097998?w=1200&h=600&fit=crop',
    'https://images.unsplash.com/photo-1531482615713-2afd69097998?w=400&h=300&fit=crop',
    (SELECT "Id" FROM "Categories" WHERE "Name" = 'ورش عمل' LIMIT 1),
    (SELECT "Id" FROM "Cities" WHERE "Name" = 'حلب' LIMIT 1),
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    true,
    false,
    CURRENT_TIMESTAMP,
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    '{}',
    gen_random_uuid()::text
  ),
  
  -- فعالية 3: معرض فنون
  (
    gen_random_uuid(),
    'معرض الفنون التشكيلية السورية - رؤى معاصرة',
    'Syrian Fine Arts Exhibition',
    'معرض فني يضم أعمال أكثر من 40 فنان سوري معاصر، يستعرض لوحات زيتية ومنحوتات وأعمال رقمية. المعرض مفتوح للجمهور مجاناً مع جولات إرشادية.',
    'Art exhibition featuring works from over 40 contemporary Syrian artists.',
    (CURRENT_DATE + INTERVAL '7 days')::timestamp,
    (CURRENT_DATE + INTERVAL '21 days')::timestamp,
    'المركز الثقافي العربي - دمشق',
    'Arab Cultural Center - Damascus',
    200,
    0,
    'https://images.unsplash.com/photo-1561214115-f2f134cc4912?w=1200&h=600&fit=crop',
    'https://images.unsplash.com/photo-1561214115-f2f134cc4912?w=400&h=300&fit=crop',
    (SELECT "Id" FROM "Categories" WHERE "Name" = 'معارض' LIMIT 1),
    (SELECT "Id" FROM "Cities" WHERE "Name" = 'دمشق' LIMIT 1),
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    true,
    false,
    CURRENT_TIMESTAMP,
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    '{}',
    gen_random_uuid()::text
  ),
  
  -- فعالية 4: حفل موسيقي
  (
    gen_random_uuid(),
    'ليلة موسيقية كلاسيكية - أوركسترا دمشق الوطنية',
    'Classical Music Night',
    'أمسية موسيقية راقية تقدمها أوركسترا دمشق الوطنية، تتضمن مقطوعات لموتسارت، بيتهوفن، وتشايكوفسكي. عرض موسيقي استثنائي في أجواء ساحرة.',
    'Elegant musical evening presented by Damascus National Orchestra.',
    (CURRENT_DATE + INTERVAL '45 days')::timestamp,
    (CURRENT_DATE + INTERVAL '45 days' + INTERVAL '3 hours')::timestamp,
    'دار الأوبرا السورية - دمشق',
    'Syrian Opera House - Damascus',
    800,
    0,
    'https://images.unsplash.com/photo-1470229722913-7c0e2dbbafd3?w=1200&h=600&fit=crop',
    'https://images.unsplash.com/photo-1470229722913-7c0e2dbbafd3?w=400&h=300&fit=crop',
    (SELECT "Id" FROM "Categories" WHERE "Name" = 'حفلات' LIMIT 1),
    (SELECT "Id" FROM "Cities" WHERE "Name" = 'دمشق' LIMIT 1),
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    true,
    false,
    CURRENT_TIMESTAMP,
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    '{}',
    gen_random_uuid()::text
  ),
  
  -- فعالية 5: مهرجان طعام
  (
    gen_random_uuid(),
    'مهرجان الطعام السوري التراثي - نكهات من التراث',
    'Syrian Heritage Food Festival',
    'مهرجان طعام يحتفي بالمطبخ السوري التقليدي، يضم أكثر من 50 طاهٍ وطاهية، عروض طبخ حية، مسابقات، وتذوق مجاني.',
    'Food festival celebrating traditional Syrian cuisine with over 50 chefs.',
    (CURRENT_DATE + INTERVAL '25 days')::timestamp,
    (CURRENT_DATE + INTERVAL '27 days')::timestamp,
    'حديقة تشرين - دمشق',
    'Tishreen Park - Damascus',
    2000,
    0,
    'https://images.unsplash.com/photo-1555939594-58d7cb561ad1?w=1200&h=600&fit=crop',
    'https://images.unsplash.com/photo-1555939594-58d7cb561ad1?w=400&h=300&fit=crop',
    (SELECT "Id" FROM "Categories" WHERE "Name" = 'مهرجانات' LIMIT 1),
    (SELECT "Id" FROM "Cities" WHERE "Name" = 'دمشق' LIMIT 1),
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    true,
    false,
    CURRENT_TIMESTAMP,
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    '{}',
    gen_random_uuid()::text
  );

-- تعليق: عرض النتائج
SELECT 
  'تم إضافة البيانات بنجاح!' as status,
  (SELECT COUNT(*) FROM "User") as total_users,
  (SELECT COUNT(*) FROM "Events") as total_events;

