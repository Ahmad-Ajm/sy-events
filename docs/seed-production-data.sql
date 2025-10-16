-- ====================================================================
-- بيانات وهمية شاملة لمنصة إدارة الفعاليات
-- تاريخ: 13 أكتوبر 2025
-- الهدف: جعل المنصة تبدو كموقع إنتاج حقيقي
-- ====================================================================

-- تنظيف البيانات القديمة (اختياري - احذر في الإنتاج!)
-- DELETE FROM bookings;
-- DELETE FROM home_slider_items;
-- DELETE FROM events;
-- DELETE FROM categories;
-- DELETE FROM cities;

-- ====================================================================
-- 1. المدن السورية (10 مدن)
-- ====================================================================
INSERT INTO "Cities" ("Id", "Name", "NameEn", "CreationTime", "CreatorId", "IsDeleted") VALUES
(gen_random_uuid(), 'دمشق', 'Damascus', NOW(), NULL, false),
(gen_random_uuid(), 'حلب', 'Aleppo', NOW(), NULL, false),
(gen_random_uuid(), 'حمص', 'Homs', NOW(), NULL, false),
(gen_random_uuid(), 'حماة', 'Hama', NOW(), NULL, false),
(gen_random_uuid(), 'اللاذقية', 'Latakia', NOW(), NULL, false),
(gen_random_uuid(), 'طرطوس', 'Tartus', NOW(), NULL, false),
(gen_random_uuid(), 'السويداء', 'As-Suwayda', NOW(), NULL, false),
(gen_random_uuid(), 'درعا', 'Daraa', NOW(), NULL, false),
(gen_random_uuid(), 'دير الزور', 'Deir ez-Zor', NOW(), NULL, false),
(gen_random_uuid(), 'الرقة', 'Raqqa', NOW(), NULL, false)
ON CONFLICT DO NOTHING;

-- ====================================================================
-- 2. التصنيفات (12 تصنيف)
-- ====================================================================
INSERT INTO "Categories" ("Id", "Name", "NameEn", "Description", "DescriptionEn", "CreationTime", "CreatorId", "IsDeleted") VALUES
(gen_random_uuid(), 'مؤتمرات', 'Conferences', 'مؤتمرات تقنية وعلمية', 'Technology and scientific conferences', NOW(), NULL, false),
(gen_random_uuid(), 'ورش عمل', 'Workshops', 'ورش عمل تدريبية', 'Training workshops', NOW(), NULL, false),
(gen_random_uuid(), 'ندوات', 'Seminars', 'ندوات ثقافية وعلمية', 'Cultural and scientific seminars', NOW(), NULL, false),
(gen_random_uuid(), 'معارض', 'Exhibitions', 'معارض فنية وتجارية', 'Art and trade exhibitions', NOW(), NULL, false),
(gen_random_uuid(), 'احتفالات', 'Celebrations', 'احتفالات وفعاليات اجتماعية', 'Social celebrations and events', NOW(), NULL, false),
(gen_random_uuid(), 'تقنية', 'Technology', 'فعاليات تقنية وبرمجية', 'Tech and programming events', NOW(), NULL, false),
(gen_random_uuid(), 'فنون', 'Arts', 'فعاليات فنية وثقافية', 'Arts and cultural events', NOW(), NULL, false),
(gen_random_uuid(), 'رياضة', 'Sports', 'فعاليات وبطولات رياضية', 'Sports events and championships', NOW(), NULL, false),
(gen_random_uuid(), 'تعليم', 'Education', 'فعاليات تعليمية وأكاديمية', 'Educational and academic events', NOW(), NULL, false),
(gen_random_uuid(), 'صحة', 'Health', 'فعاليات صحية وتوعوية', 'Health and awareness events', NOW(), NULL, false),
(gen_random_uuid(), 'أعمال', 'Business', 'فعاليات ريادة الأعمال', 'Entrepreneurship events', NOW(), NULL, false),
(gen_random_uuid(), 'موسيقى', 'Music', 'حفلات ومهرجانات موسيقية', 'Music concerts and festivals', NOW(), NULL, false)
ON CONFLICT DO NOTHING;

-- ====================================================================
-- 3. إعدادات التطبيق
-- ====================================================================
INSERT INTO app_settings (slider_items_count, auto_approve_events) VALUES
(5, false)
ON CONFLICT DO NOTHING;

-- ====================================================================
-- 4. فعاليات (20 فعالية متنوعة - ماضية وقادمة)
-- ====================================================================

-- متغيرات للمدن والتصنيفات (سنستخدم IDs عشوائية)
DO $$
DECLARE
    damascus_id uuid;
    aleppo_id uuid;
    homs_id uuid;
    latakia_id uuid;
    tartus_id uuid;
    
    tech_cat uuid;
    workshop_cat uuid;
    conference_cat uuid;
    exhibition_cat uuid;
    arts_cat uuid;
    music_cat uuid;
    business_cat uuid;
    education_cat uuid;
    
    admin_user uuid := '39f15738-c2fe-7b9d-0f41-5bd8b074ce50'; -- Admin default ID
BEGIN
    -- جلب IDs المدن
    SELECT "Id" INTO damascus_id FROM "Cities" WHERE "NameEn" = 'Damascus' LIMIT 1;
    SELECT "Id" INTO aleppo_id FROM "Cities" WHERE "NameEn" = 'Aleppo' LIMIT 1;
    SELECT "Id" INTO homs_id FROM "Cities" WHERE "NameEn" = 'Homs' LIMIT 1;
    SELECT "Id" INTO latakia_id FROM "Cities" WHERE "NameEn" = 'Latakia' LIMIT 1;
    SELECT "Id" INTO tartus_id FROM "Cities" WHERE "NameEn" = 'Tartus' LIMIT 1;
    
    -- جلب IDs التصنيفات
    SELECT "Id" INTO tech_cat FROM "Categories" WHERE "NameEn" = 'Technology' LIMIT 1;
    SELECT "Id" INTO workshop_cat FROM "Categories" WHERE "NameEn" = 'Workshops' LIMIT 1;
    SELECT "Id" INTO conference_cat FROM "Categories" WHERE "NameEn" = 'Conferences' LIMIT 1;
    SELECT "Id" INTO exhibition_cat FROM "Categories" WHERE "NameEn" = 'Exhibitions' LIMIT 1;
    SELECT "Id" INTO arts_cat FROM "Categories" WHERE "NameEn" = 'Arts' LIMIT 1;
    SELECT "Id" INTO music_cat FROM "Categories" WHERE "NameEn" = 'Music' LIMIT 1;
    SELECT "Id" INTO business_cat FROM "Categories" WHERE "NameEn" = 'Business' LIMIT 1;
    SELECT "Id" INTO education_cat FROM "Categories" WHERE "NameEn" = 'Education' LIMIT 1;
    
    -- فعاليات قادمة (10 فعاليات)
    INSERT INTO events (id, title, "title_en", description, "description_en", start_date, end_date, location, "location_en", 
                        max_capacity, is_approved, status, image_url, thumbnail_url, category_id, city_id, organizer_id, 
                        creation_time, creator_id, is_deleted) VALUES
    
    -- 1. مؤتمر التقنية 2025
    (gen_random_uuid(), 
     'مؤتمر التقنية السوري 2025', 
     'Syrian Tech Conference 2025',
     'مؤتمر سنوي يجمع خبراء التقنية والمبرمجين من جميع أنحاء سوريا. يتضمن ورش عمل، محاضرات، ومعرض للشركات التقنية.',
     'Annual conference bringing together tech experts and developers from all over Syria. Includes workshops, lectures, and tech company exhibition.',
     NOW() + INTERVAL '15 days',
     NOW() + INTERVAL '17 days',
     'فندق الشام - قاعة المؤتمرات الكبرى',
     'Al Sham Hotel - Grand Conference Hall',
     500,
     true,
     3, -- Approved
     'https://images.unsplash.com/photo-1540575467063-178a50c2df87?w=1200&h=500&fit=crop',
     'https://images.unsplash.com/photo-1540575467063-178a50c2df87?w=400&h=200&fit=crop',
     tech_cat, damascus_id, admin_user, NOW(), NULL, false),
    
    -- 2. معرض الفنون المعاصرة
    (gen_random_uuid(),
     'معرض الفنون المعاصرة - دمشق',
     'Contemporary Art Exhibition - Damascus',
     'معرض فني يستعرض أعمال 50 فنان سوري معاصر، يشمل اللوحات، المنحوتات، والتصوير الفوتوغرافي.',
     'Art exhibition showcasing works of 50 contemporary Syrian artists, including paintings, sculptures, and photography.',
     NOW() + INTERVAL '7 days',
     NOW() + INTERVAL '30 days',
     'المركز الثقافي العربي',
     'Arab Cultural Center',
     200,
     true,
     3,
     'https://images.unsplash.com/photo-1561214115-f2f134cc4912?w=1200&h=500&fit=crop',
     'https://images.unsplash.com/photo-1561214115-f2f134cc4912?w=400&h=200&fit=crop',
     arts_cat, damascus_id, admin_user, NOW(), NULL, false),
    
    -- 3. ورشة تطوير الويب
    (gen_random_uuid(),
     'ورشة تطوير الويب الحديث',
     'Modern Web Development Workshop',
     'ورشة عمل مكثفة لمدة 3 أيام تغطي React, Angular, وأحدث تقنيات تطوير الواجهات.',
     'Intensive 3-day workshop covering React, Angular, and latest frontend technologies.',
     NOW() + INTERVAL '10 days',
     NOW() + INTERVAL '12 days',
     'مركز الابتكار التقني - حلب',
     'Tech Innovation Center - Aleppo',
     50,
     true,
     3,
     'https://images.unsplash.com/photo-1531482615713-2afd69097998?w=1200&h=500&fit=crop',
     'https://images.unsplash.com/photo-1531482615713-2afd69097998?w=400&h=200&fit=crop',
     workshop_cat, aleppo_id, admin_user, NOW(), NULL, false),
    
    -- 4. مهرجان الموسيقى الصيفي
    (gen_random_uuid(),
     'مهرجان اللاذقية الموسيقي الصيفي',
     'Latakia Summer Music Festival',
     'مهرجان موسيقي كبير يستمر لمدة أسبوع مع عروض لفرق محلية وعالمية على شاطئ البحر.',
     'Major music festival lasting one week with local and international bands performing by the seaside.',
     NOW() + INTERVAL '25 days',
     NOW() + INTERVAL '32 days',
     'كورنيش اللاذقية',
     'Latakia Corniche',
     5000,
     true,
     3,
     'https://images.unsplash.com/photo-1470229722913-7c0e2dbbafd3?w=1200&h=500&fit=crop',
     'https://images.unsplash.com/photo-1470229722913-7c0e2dbbafd3?w=400&h=200&fit=crop',
     music_cat, latakia_id, admin_user, NOW(), NULL, false),
    
    -- 5. ملتقى ريادة الأعمال
    (gen_random_uuid(),
     'ملتقى ريادة الأعمال السوري',
     'Syrian Entrepreneurship Forum',
     'ملتقى يجمع رواد الأعمال والمستثمرين لمناقشة فرص الاستثمار والمشاريع الناشئة في سوريا.',
     'Forum bringing together entrepreneurs and investors to discuss investment opportunities and startups in Syria.',
     NOW() + INTERVAL '20 days',
     NOW() + INTERVAL '21 days',
     'غرفة تجارة دمشق',
     'Damascus Chamber of Commerce',
     300,
     true,
     3,
     'https://images.unsplash.com/photo-1559136555-9303baea8ebd?w=1200&h=500&fit=crop',
     'https://images.unsplash.com/photo-1559136555-9303baea8ebd?w=400&h=200&fit=crop',
     business_cat, damascus_id, admin_user, NOW(), NULL, false),
    
    -- 6. معرض الكتاب السنوي
    (gen_random_uuid(),
     'معرض دمشق الدولي للكتاب',
     'Damascus International Book Fair',
     'معرض الكتاب السنوي الأكبر في سوريا، يستضيف دور نشر عربية وعالمية ويتضمن ندوات أدبية.',
     'The largest annual book fair in Syria, hosting Arab and international publishers with literary seminars.',
     NOW() + INTERVAL '40 days',
     NOW() + INTERVAL '54 days',
     'مدينة المعارض - دمشق',
     'Exhibition City - Damascus',
     10000,
     true,
     3,
     'https://images.unsplash.com/photo-1481627834876-b7833e8f5570?w=1200&h=500&fit=crop',
     'https://images.unsplash.com/photo-1481627834876-b7833e8f5570?w=400&h=200&fit=crop',
     exhibition_cat, damascus_id, admin_user, NOW(), NULL, false),
    
    -- 7. ورشة الذكاء الاصطناعي
    (gen_random_uuid(),
     'ورشة الذكاء الاصطناعي وتعلم الآلة',
     'AI and Machine Learning Workshop',
     'ورشة متقدمة تغطي أساسيات وتطبيقات الذكاء الاصطناعي باستخدام Python وTensorFlow.',
     'Advanced workshop covering AI fundamentals and applications using Python and TensorFlow.',
     NOW() + INTERVAL '18 days',
     NOW() + INTERVAL '19 days',
     'جامعة دمشق - كلية الهندسة المعلوماتية',
     'Damascus University - IT Engineering Faculty',
     80,
     true,
     3,
     'https://images.unsplash.com/photo-1677442136019-21780ecad995?w=1200&h=500&fit=crop',
     'https://images.unsplash.com/photo-1677442136019-21780ecad995?w=400&h=200&fit=crop',
     workshop_cat, damascus_id, admin_user, NOW(), NULL, false),
    
    -- 8. مؤتمر التعليم الإلكتروني
    (gen_random_uuid(),
     'مؤتمر التعليم الإلكتروني والتعلم عن بعد',
     'E-Learning and Distance Education Conference',
     'مؤتمر يناقش مستقبل التعليم الإلكتروني وأفضل الممارسات في التعلم عن بعد.',
     'Conference discussing the future of e-learning and best practices in distance education.',
     NOW() + INTERVAL '35 days',
     NOW() + INTERVAL '36 days',
     'مركز المؤتمرات - حمص',
     'Conference Center - Homs',
     250,
     true,
     3,
     'https://images.unsplash.com/photo-1501504905252-473c47e087f8?w=1200&h=500&fit=crop',
     'https://images.unsplash.com/photo-1501504905252-473c47e087f8?w=400&h=200&fit=crop',
     education_cat, homs_id, admin_user, NOW(), NULL, false),
    
    -- 9. بطولة الشطرنج الوطنية
    (gen_random_uuid(),
     'بطولة سوريا المفتوحة للشطرنج',
     'Syria Open Chess Championship',
     'بطولة شطرنج وطنية مفتوحة لجميع الأعمار والمستويات مع جوائز قيمة للفائزين.',
     'National open chess championship for all ages and levels with valuable prizes for winners.',
     NOW() + INTERVAL '28 days',
     NOW() + INTERVAL '30 days',
     'نادي الشرق الرياضي',
     'Al Sharq Sports Club',
     100,
     true,
     3,
     'https://images.unsplash.com/photo-1529699211952-734e80c4d42b?w=1200&h=500&fit=crop',
     'https://images.unsplash.com/photo-1529699211952-734e80c4d42b?w=400&h=200&fit=crop',
     (SELECT id FROM categories WHERE "name_en" = 'Sports' LIMIT 1), damascus_id, admin_user, NOW(), NULL, false),
    
    -- 10. ندوة الأمن السيبراني
    (gen_random_uuid(),
     'ندوة الأمن السيبراني وحماية البيانات',
     'Cybersecurity and Data Protection Seminar',
     'ندوة متخصصة تناقش أحدث تهديدات الأمن السيبراني وطرق الحماية للشركات والأفراد.',
     'Specialized seminar discussing latest cybersecurity threats and protection methods for companies and individuals.',
     NOW() + INTERVAL '22 days',
     NOW() + INTERVAL '22 days',
     'فندق داما روز',
     'Dama Rose Hotel',
     150,
     true,
     3,
     'https://images.unsplash.com/photo-1550751827-4bd374c3f58b?w=1200&h=500&fit=crop',
     'https://images.unsplash.com/photo-1550751827-4bd374c3f58b?w=400&h=200&fit=crop',
     (SELECT id FROM categories WHERE "name_en" = 'Seminars' LIMIT 1), damascus_id, admin_user, NOW(), NULL, false);
    
    -- فعاليات ماضية (10 فعاليات)
    INSERT INTO events (id, title, "title_en", description, "description_en", start_date, end_date, location, "location_en", 
                        max_capacity, is_approved, status, image_url, thumbnail_url, category_id, city_id, organizer_id, 
                        creation_time, creator_id, is_deleted) VALUES
    
    (gen_random_uuid(),
     'هاكاثون دمشق للبرمجة 2024',
     'Damascus Coding Hackathon 2024',
     'مسابقة برمجية استمرت 48 ساعة مع مشاركة 100 مطور من مختلف أنحاء سوريا.',
     '48-hour coding competition with 100 developers from across Syria.',
     NOW() - INTERVAL '60 days',
     NOW() - INTERVAL '58 days',
     'حاضنة أعمال دمشق',
     'Damascus Business Incubator',
     100,
     true,
     3,
     'https://images.unsplash.com/photo-1504384308090-c894fdcc538d?w=1200&h=500&fit=crop',
     'https://images.unsplash.com/photo-1504384308090-c894fdcc538d?w=400&h=200&fit=crop',
     tech_cat, damascus_id, admin_user, NOW() - INTERVAL '90 days', NULL, false),
    
    (gen_random_uuid(),
     'مهرجان الأفلام القصيرة',
     'Short Films Festival',
     'مهرجان سينمائي عرض 50 فيلم قصير من إنتاج سوري وعربي.',
     'Film festival screening 50 short films from Syrian and Arab production.',
     NOW() - INTERVAL '45 days',
     NOW() - INTERVAL '42 days',
     'دار الأوبرا السورية',
     'Syrian Opera House',
     400,
     true,
     3,
     'https://images.unsplash.com/photo-1489599849927-2ee91cede3ba?w=1200&h=500&fit=crop',
     'https://images.unsplash.com/photo-1489599849927-2ee91cede3ba?w=400&h=200&fit=crop',
     arts_cat, damascus_id, admin_user, NOW() - INTERVAL '75 days', NULL, false),
    
    (gen_random_uuid(),
     'ورشة التسويق الرقمي',
     'Digital Marketing Workshop',
     'ورشة عملية مكثفة حول استراتيجيات التسويق الرقمي ووسائل التواصل الاجتماعي.',
     'Intensive practical workshop on digital marketing strategies and social media.',
     NOW() - INTERVAL '30 days',
     NOW() - INTERVAL '29 days',
     'مركز التدريب المهني',
     'Professional Training Center',
     60,
     true,
     3,
     'https://images.unsplash.com/photo-1460925895917-afdab827c52f?w=1200&h=500&fit=crop',
     'https://images.unsplash.com/photo-1460925895917-afdab827c52f?w=400&h=200&fit=crop',
     workshop_cat, aleppo_id, admin_user, NOW() - INTERVAL '60 days', NULL, false),
    
    (gen_random_uuid(),
     'معرض التكنولوجيا الصحية',
     'Health Technology Exhibition',
     'معرض متخصص في التقنيات الطبية والصحية الحديثة.',
     'Specialized exhibition in modern medical and health technologies.',
     NOW() - INTERVAL '20 days',
     NOW() - INTERVAL '18 days',
     'مستشفى المواساة الجامعي',
     'Al Muwasa University Hospital',
     200,
     true,
     3,
     'https://images.unsplash.com/photo-1576091160399-112ba8d25d1d?w=1200&h=500&fit=crop',
     'https://images.unsplash.com/photo-1576091160399-112ba8d25d1d?w=400&h=200&fit=crop',
     (SELECT id FROM categories WHERE "name_en" = 'Health' LIMIT 1), damascus_id, admin_user, NOW() - INTERVAL '50 days', NULL, false),
    
    (gen_random_uuid(),
     'حفل موسيقى كلاسيكية',
     'Classical Music Concert',
     'حفل موسيقي كلاسيكي من أداء الأوركسترا السورية.',
     'Classical music concert performed by Syrian Orchestra.',
     NOW() - INTERVAL '15 days',
     NOW() - INTERVAL '15 days',
     'دار الأوبرا السورية',
     'Syrian Opera House',
     800,
     true,
     3,
     'https://images.unsplash.com/photo-1465847899084-d164df4dedc6?w=1200&h=500&fit=crop',
     'https://images.unsplash.com/photo-1465847899084-d164df4dedc6?w=400&h=200&fit=crop',
     music_cat, damascus_id, admin_user, NOW() - INTERVAL '45 days', NULL, false),
    
    (gen_random_uuid(),
     'يوم التوظيف المهني',
     'Career Fair Day',
     'يوم مفتوح للتوظيف مع مشاركة 40 شركة سورية.',
     'Open career fair day with 40 Syrian companies participating.',
     NOW() - INTERVAL '10 days',
     NOW() - INTERVAL '10 days',
     'فندق الفورسيزون',
     'Four Seasons Hotel',
     500,
     true,
     3,
     'https://images.unsplash.com/photo-1521737604893-d14cc237f11d?w=1200&h=500&fit=crop',
     'https://images.unsplash.com/photo-1521737604893-d14cc237f11d?w=400&h=200&fit=crop',
     business_cat, damascus_id, admin_user, NOW() - INTERVAL '40 days', NULL, false),
    
    (gen_random_uuid(),
     'ورشة التصوير الفوتوغرافي',
     'Photography Workshop',
     'ورشة عملية في التصوير الفوتوغرافي للمبتدئين والمحترفين.',
     'Practical photography workshop for beginners and professionals.',
     NOW() - INTERVAL '25 days',
     NOW() - INTERVAL '24 days',
     'المركز الثقافي الفرنسي',
     'French Cultural Center',
     40,
     true,
     3,
     'https://images.unsplash.com/photo-1452587925148-ce544e77e70d?w=1200&h=500&fit=crop',
     'https://images.unsplash.com/photo-1452587925148-ce544e77e70d?w=400&h=200&fit=crop',
     arts_cat, damascus_id, admin_user, NOW() - INTERVAL '55 days', NULL, false),
    
    (gen_random_uuid(),
     'ماراثون دمشق الخيري',
     'Damascus Charity Marathon',
     'ماراثون خيري بمشاركة 2000 عداء لدعم الأطفال المحتاجين.',
     'Charity marathon with 2000 runners supporting children in need.',
     NOW() - INTERVAL '35 days',
     NOW() - INTERVAL '35 days',
     'ساحة الأمويين',
     'Umayyad Square',
     2000,
     true,
     3,
     'https://images.unsplash.com/photo-1552674605-db6ffd4facb5?w=1200&h=500&fit=crop',
     'https://images.unsplash.com/photo-1552674605-db6ffd4facb5?w=400&h=200&fit=crop',
     (SELECT id FROM categories WHERE "name_en" = 'Sports' LIMIT 1), damascus_id, admin_user, NOW() - INTERVAL '65 days', NULL, false),
    
    (gen_random_uuid(),
     'مؤتمر الطاقة المتجددة',
     'Renewable Energy Conference',
     'مؤتمر متخصص في حلول الطاقة النظيفة والمستدامة.',
     'Specialized conference in clean and sustainable energy solutions.',
     NOW() - INTERVAL '50 days',
     NOW() - INTERVAL '49 days',
     'مركز المؤتمرات - اللاذقية',
     'Conference Center - Latakia',
     300,
     true,
     3,
     'https://images.unsplash.com/photo-1473341304170-971dccb5ac1e?w=1200&h=500&fit=crop',
     'https://images.unsplash.com/photo-1473341304170-971dccb5ac1e?w=400&h=200&fit=crop',
     conference_cat, latakia_id, admin_user, NOW() - INTERVAL '80 days', NULL, false),
    
    (gen_random_uuid(),
     'معرض الصناعات اليدوية',
     'Handicrafts Exhibition',
     'معرض للمنتجات والصناعات اليدوية السورية التقليدية.',
     'Exhibition of traditional Syrian handicrafts and products.',
     NOW() - INTERVAL '40 days',
     NOW() - INTERVAL '33 days',
     'سوق الحميدية',
     'Hamidiyah Souq',
     1000,
     true,
     3,
     'https://images.unsplash.com/photo-1513519245088-0e12902e35ca?w=1200&h=500&fit=crop',
     'https://images.unsplash.com/photo-1513519245088-0e12902e35ca?w=400&h=200&fit=crop',
     exhibition_cat, damascus_id, admin_user, NOW() - INTERVAL '70 days', NULL, false);
    
END $$;

-- ====================================================================
-- 5. عناصر السلايدر (6 عناصر نشطة)
-- ====================================================================

DO $$
DECLARE
    event1_id uuid;
    event2_id uuid;
    event3_id uuid;
    event4_id uuid;
    event5_id uuid;
    event6_id uuid;
BEGIN
    -- جلب IDs أول 6 فعاليات قادمة
    SELECT id INTO event1_id FROM events WHERE title LIKE '%مؤتمر التقنية السوري%' LIMIT 1;
    SELECT id INTO event2_id FROM events WHERE title LIKE '%معرض الفنون المعاصرة%' LIMIT 1;
    SELECT id INTO event3_id FROM events WHERE title LIKE '%تطوير الويب%' LIMIT 1;
    SELECT id INTO event4_id FROM events WHERE title LIKE '%مهرجان اللاذقية%' LIMIT 1;
    SELECT id INTO event5_id FROM events WHERE title LIKE '%ريادة الأعمال%' LIMIT 1;
    SELECT id INTO event6_id FROM events WHERE title LIKE '%معرض دمشق الدولي للكتاب%' LIMIT 1;
    
    INSERT INTO home_slider_items (id, display_order, type, custom_event_id, is_active, title, title_en, image_url, creation_time, is_deleted) VALUES
    (gen_random_uuid(), 1, 3, event1_id, true, 
     'مؤتمر التقنية السوري 2025', 
     'Syrian Tech Conference 2025',
     'https://images.unsplash.com/photo-1540575467063-178a50c2df87?w=1200&h=500&fit=crop',
     NOW(), false),
    
    (gen_random_uuid(), 2, 3, event2_id, true,
     'معرض الفنون المعاصرة',
     'Contemporary Art Exhibition',
     'https://images.unsplash.com/photo-1561214115-f2f134cc4912?w=1200&h=500&fit=crop',
     NOW(), false),
    
    (gen_random_uuid(), 3, 3, event3_id, true,
     'ورشة تطوير الويب الحديث',
     'Modern Web Development Workshop',
     'https://images.unsplash.com/photo-1531482615713-2afd69097998?w=1200&h=500&fit=crop',
     NOW(), false),
    
    (gen_random_uuid(), 4, 3, event4_id, true,
     'مهرجان اللاذقية الموسيقي',
     'Latakia Music Festival',
     'https://images.unsplash.com/photo-1470229722913-7c0e2dbbafd3?w=1200&h=500&fit=crop',
     NOW(), false),
    
    (gen_random_uuid(), 5, 3, event5_id, true,
     'ملتقى ريادة الأعمال',
     'Entrepreneurship Forum',
     'https://images.unsplash.com/photo-1559136555-9303baea8ebd?w=1200&h=500&fit=crop',
     NOW(), false),
    
    (gen_random_uuid(), 6, 1, NULL, true,
     'الفعاليات الأحدث',
     'Latest Events',
     'https://images.unsplash.com/photo-1505373877841-8d25f7d46678?w=1200&h=500&fit=crop',
     NOW(), false)
    ON CONFLICT DO NOTHING;
END $$;

-- ====================================================================
-- 6. إحصائيات نهائية
-- ====================================================================

DO $$
BEGIN
    RAISE NOTICE '====================================';
    RAISE NOTICE 'تم إضافة البيانات الوهمية بنجاح!';
    RAISE NOTICE '====================================';
    RAISE NOTICE 'المدن: %', (SELECT COUNT(*) FROM cities);
    RAISE NOTICE 'التصنيفات: %', (SELECT COUNT(*) FROM categories);
    RAISE NOTICE 'الفعاليات: %', (SELECT COUNT(*) FROM events);
    RAISE NOTICE 'عناصر السلايدر: %', (SELECT COUNT(*) FROM home_slider_items);
    RAISE NOTICE '====================================';
    RAISE NOTICE 'الصفحة الرئيسية: http://localhost:4200';
    RAISE NOTICE 'إدارة السلايدر: http://localhost:4200/admin/home-slider';
    RAISE NOTICE 'قائمة الفعاليات: http://localhost:4200/events';
    RAISE NOTICE '====================================';
END $$;

