-- ===================================================================
-- سكريبت إضافة بيانات وهمية كاملة لمنصة إدارة الفعاليات
-- يحاكي بيئة إنتاج حقيقية مع فعاليات ومستخدمين واقعيين
-- ===================================================================

-- تعليق: إضافة فعاليات وهمية واقعية
INSERT INTO "Events" 
  ("Id", "Title", "TitleEn", "Description", "DescriptionEn", "StartDate", "EndDate", "Location", "MaxCapacity", 
   "CategoryId", "CityId", "OrganizerId", "IsApproved", "CreationTime", "CreatorId",
   "ExtraProperties", "ConcurrencyStamp")
VALUES
  -- فعالية 1: مؤتمر التقنية السنوي
  (
    gen_random_uuid(),
    'مؤتمر التقنية السنوي 2025',
    'Annual Technology Conference 2025',
    'مؤتمر سنوي يجمع خبراء التقنية والمبرمجين والشركات الناشئة من جميع أنحاء سوريا. يتضمن ورش عمل تفاعلية، جلسات نقاشية، ومعرض للشركات التقنية. فرصة مثالية للتواصل والتعلم من أفضل المختصين في المجال.',
    'An annual conference bringing together tech experts, developers, and startups from across Syria.',
    (CURRENT_DATE + INTERVAL '30 days')::timestamp,
    (CURRENT_DATE + INTERVAL '32 days')::timestamp,
    'فندق الشام - قاعة المؤتمرات الكبرى - دمشق',
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
  
  -- فعالية 2: ورشة تطوير الويب المتقدم
  (
    gen_random_uuid(),
    'ورشة تطوير الويب المتقدم - React & Angular',
    'ورشة عمل مكثفة لمدة 3 أيام تغطي أحدث تقنيات تطوير الويب باستخدام React وAngular. تشمل مشاريع عملية، أفضل الممارسات، والنشر على السحابة. مناسبة للمطورين ذوي الخبرة المتوسطة.',
    (CURRENT_DATE + INTERVAL '15 days')::timestamp,
    (CURRENT_DATE + INTERVAL '17 days')::timestamp,
    'مركز التدريب التقني - حلب',
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
  
  -- فعالية 3: معرض الفنون التشكيلية السورية
  (
    gen_random_uuid(),
    'معرض الفنون التشكيلية السورية - رؤى معاصرة',
    'معرض فني يضم أعمال أكثر من 40 فنان سوري معاصر، يستعرض لوحات زيتية، منحوتات، وأعمال رقمية. المعرض مفتوح للجمهور مجاناً. يتضمن جولات إرشادية يومية ولقاءات مع الفنانين.',
    (CURRENT_DATE + INTERVAL '7 days')::timestamp,
    (CURRENT_DATE + INTERVAL '21 days')::timestamp,
    'المركز الثقافي العربي - دمشق',
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
  
  -- فعالية 4: حفل موسيقي كلاسيكي
  (
    gen_random_uuid(),
    'ليلة موسيقية كلاسيكية - أوركسترا دمشق الوطنية',
    'أمسية موسيقية راقية تقدمها أوركسترا دمشق الوطنية، تتضمن مقطوعات لموتسارت، بيتهوفن، وتشايكوفسكي. عرض موسيقي استثنائي في أجواء ساحرة مع مشاركة عازفين محترفين.',
    (CURRENT_DATE + INTERVAL '45 days')::timestamp,
    (CURRENT_DATE + INTERVAL '45 days' + INTERVAL '3 hours')::timestamp,
    'دار الأوبرا السورية - دمشق',
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
  
  -- فعالية 5: دورة البرمجة المتقدمة بلغة Python
  (
    gen_random_uuid(),
    'دورة البرمجة المتقدمة بلغة Python - من الصفر للاحتراف',
    'دورة تدريبية شاملة مدتها شهرين، تغطي أساسيات Python، البرمجة الكائنية، تطوير الويب بـ Django، علم البيانات، والتعلم الآلي. تشمل مشاريع عملية وشهادة معتمدة. مناسبة للمبتدئين والمحترفين.',
    (CURRENT_DATE + INTERVAL '60 days')::timestamp,
    (CURRENT_DATE + INTERVAL '120 days')::timestamp,
    'أكاديمية البرمجة - حمص',
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
  
  -- فعالية 6: مهرجان الطعام السوري
  (
    gen_random_uuid(),
    'مهرجان الطعام السوري التراثي - نكهات من التراث',
    'مهرجان طعام يحتفي بالمطبخ السوري التقليدي، يضم أكثر من 50 طاهٍ وطاهية، عروض طبخ حية، مسابقات، وتذوق مجاني. فعالية عائلية ممتعة للجميع مع برنامج ترفيهي للأطفال.',
    (CURRENT_DATE + INTERVAL '25 days')::timestamp,
    (CURRENT_DATE + INTERVAL '27 days')::timestamp,
    'حديقة تشرين - دمشق',
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
    'ندوة ريادة الأعمال - بناء مشروعك الناجح',
    'ندوة تفاعلية تجمع رواد أعمال ناجحين، مستثمرين، وخبراء في مجال الأعمال. تتضمن محاضرات ملهمة، ورش عمل عملية، وجلسات تواصل. فرصة ذهبية لرواد الأعمال الطموحين.',
    (CURRENT_DATE + INTERVAL '10 days')::timestamp,
    (CURRENT_DATE + INTERVAL '10 days' + INTERVAL '6 hours')::timestamp,
    'غرفة تجارة حلب - قاعة الاجتماعات',
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
  
  -- فعالية 8: حفل تخرج دفعة 2025
  (
    gen_random_uuid(),
    'حفل تخرج دفعة 2025 - كلية الهندسة المعلوماتية',
    'حفل تخرج رسمي للطلاب الخريجين من كلية الهندسة المعلوماتية. يتضمن توزيع الشهادات، كلمات تكريمية، وحفل استقبال للخريجين وعائلاتهم. لحظة احتفالية مميزة لتكريم إنجازات الطلاب.',
    (CURRENT_DATE + INTERVAL '40 days')::timestamp,
    (CURRENT_DATE + INTERVAL '40 days' + INTERVAL '4 hours')::timestamp,
    'جامعة دمشق - المدرج الرئيسي',
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
  
  -- فعالية 9: معرض الكتاب السوري
  (
    gen_random_uuid(),
    'معرض دمشق الدولي للكتاب - الدورة الـ 35',
    'المعرض الأكبر للكتب في سوريا، يضم أكثر من 200 دار نشر عربية وعالمية، ندوات أدبية يومية، توقيع كتب مع المؤلفين، وفعاليات ثقافية متنوعة. حدث ثقافي بارز يستمر أسبوعين.',
    (CURRENT_DATE + INTERVAL '50 days')::timestamp,
    (CURRENT_DATE + INTERVAL '64 days')::timestamp,
    'مدينة المعارض - دمشق',
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
  
  -- فعالية 10: بطولة الشطرنج الوطنية
  (
    gen_random_uuid(),
    'بطولة سوريا المفتوحة للشطرنج 2025',
    'بطولة شطرنج رسمية مفتوحة لجميع الأعمار والمستويات. تشمل فئات مختلفة للمحترفين والهواة والناشئين. جوائز قيمة للفائزين وشهادات مشاركة. التسجيل المبكر مطلوب.',
    (CURRENT_DATE + INTERVAL '35 days')::timestamp,
    (CURRENT_DATE + INTERVAL '38 days')::timestamp,
    'النادي الرياضي - اللاذقية',
    100,
    (SELECT "Id" FROM "Categories" WHERE "Name" = 'رياضة' LIMIT 1),
    (SELECT "Id" FROM "Cities" WHERE "Name" = 'اللاذقية' LIMIT 1),
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    true,
    CURRENT_TIMESTAMP,
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    '{}',
    gen_random_uuid()::text
  ),
  
  -- فعالية 11: ملتقى المصورين السوريين
  (
    gen_random_uuid(),
    'ملتقى المصورين السوريين - عدسة على الوطن',
    'ملتقى سنوي يجمع مصوري فوتوغرافيا محترفين وهواة. معرض صور، محاضرات تقنية، ورش عمل عملية عن التصوير الفوتوغرافي والإضاءة والمونتاج. مسابقة أفضل صورة مع جوائز قيمة.',
    (CURRENT_DATE + INTERVAL '20 days')::timestamp,
    (CURRENT_DATE + INTERVAL '22 days')::timestamp,
    'صالة الشعب - حمص',
    120,
    (SELECT "Id" FROM "Categories" WHERE "Name" = 'فنون' LIMIT 1),
    (SELECT "Id" FROM "Cities" WHERE "Name" = 'حمص' LIMIT 1),
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    true,
    CURRENT_TIMESTAMP,
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    '{}',
    gen_random_uuid()::text
  ),
  
  -- فعالية 12: يوم التطوع البيئي
  (
    gen_random_uuid(),
    'يوم التطوع البيئي - معاً لبيئة أنظف',
    'فعالية تطوعية بيئية تشمل تنظيف الشواطئ والحدائق، زراعة أشجار، وتوعية بيئية. مفتوحة للجميع مع توفير أدوات ومعدات التنظيف. وجبة غداء مجانية للمتطوعين.',
    (CURRENT_DATE + INTERVAL '12 days')::timestamp,
    (CURRENT_DATE + INTERVAL '12 days' + INTERVAL '8 hours')::timestamp,
    'كورنيش اللاذقية',
    300,
    (SELECT "Id" FROM "Categories" WHERE "Name" = 'اجتماعية' LIMIT 1),
    (SELECT "Id" FROM "Cities" WHERE "Name" = 'اللاذقية' LIMIT 1),
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    true,
    CURRENT_TIMESTAMP,
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    '{}',
    gen_random_uuid()::text
  ),
  
  -- فعالية 13: ماراثون دمشق الخيري
  (
    gen_random_uuid(),
    'ماراثون دمشق الخيري - نركض من أجل الأمل',
    'ماراثون خيري بمسافات مختلفة: 5 كم، 10 كم، 21 كم (نصف ماراثون). الأرباح تذهب لدعم الأطفال الأيتام. تتضمن الفعالية فحوصات طبية مجانية، جوائز للفائزين، وميداليات لجميع المشاركين.',
    (CURRENT_DATE + INTERVAL '55 days')::timestamp,
    (CURRENT_DATE + INTERVAL '55 days' + INTERVAL '5 hours')::timestamp,
    'شارع بغداد - دمشق',
    1000,
    (SELECT "Id" FROM "Categories" WHERE "Name" = 'رياضة' LIMIT 1),
    (SELECT "Id" FROM "Cities" WHERE "Name" = 'دمشق' LIMIT 1),
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    true,
    CURRENT_TIMESTAMP,
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    '{}',
    gen_random_uuid()::text
  ),
  
  -- فعالية 14: مؤتمر التعليم الإلكتروني
  (
    gen_random_uuid(),
    'مؤتمر التعليم الإلكتروني - مستقبل التعليم في سوريا',
    'مؤتمر يناقش مستقبل التعليم الإلكتروني في سوريا، يضم خبراء تربويين، مطورين تقنيين، ومسؤولين حكوميين. يتضمن عروض تقديمية، تجارب ناجحة، وحلول عملية لتحديات التعليم عن بُعد.',
    (CURRENT_DATE + INTERVAL '70 days')::timestamp,
    (CURRENT_DATE + INTERVAL '71 days')::timestamp,
    'جامعة حلب - مركز المؤتمرات',
    300,
    (SELECT "Id" FROM "Categories" WHERE "Name" = 'مؤتمرات' LIMIT 1),
    (SELECT "Id" FROM "Cities" WHERE "Name" = 'حلب' LIMIT 1),
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    true,
    CURRENT_TIMESTAMP,
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    '{}',
    gen_random_uuid()::text
  ),
  
  -- فعالية 15: معرض الصناعات اليدوية
  (
    gen_random_uuid(),
    'معرض الصناعات اليدوية السورية - إبداع أصيل',
    'معرض يستعرض أفضل الصناعات اليدوية التقليدية السورية: الموزاييك الدمشقي، المنسوجات، الخزف، والنحاسيات. فرصة لشراء منتجات أصيلة مباشرة من الحرفيين ودعم الصناعات المحلية.',
    (CURRENT_DATE + INTERVAL '18 days')::timestamp,
    (CURRENT_DATE + INTERVAL '25 days')::timestamp,
    'سوق البزورية - دمشق القديمة',
    400,
    (SELECT "Id" FROM "Categories" WHERE "Name" = 'معارض' LIMIT 1),
    (SELECT "Id" FROM "Cities" WHERE "Name" = 'دمشق' LIMIT 1),
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    true,
    CURRENT_TIMESTAMP,
    '3a1ce96f-b9f8-a2e1-9128-176b5805b988',
    '{}',
    gen_random_uuid()::text
  );

-- تعليق: إضافة حجوزات وهمية للتقويم
-- ملاحظة: يتطلب ذلك وجود جدول Bookings في قاعدة البيانات
-- سيتم إضافتها عند إنشاء Migration للحجوزات

-- ===================================================================
-- تقرير النتائج
-- ===================================================================

SELECT 
  'تم إضافة البيانات الوهمية بنجاح!' as status,
  (SELECT COUNT(*) FROM "Events") as total_events,
  (SELECT COUNT(*) FROM "Cities") as total_cities,
  (SELECT COUNT(*) FROM "Categories") as total_categories,
  (SELECT COUNT(*) FROM "HomeSliderItems") as total_slider_items;

