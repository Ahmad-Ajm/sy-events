-- ====================================================================
-- بيانات وهمية شاملة - إصدار محدث
-- تاريخ: 13 أكتوبر 2025
-- ====================================================================

-- 1. المدن السورية
INSERT INTO "Cities" ("Id", "Name", "NameEn", "CreationTime", "IsDeleted") VALUES
(gen_random_uuid(), 'دمشق', 'Damascus', NOW(), false),
(gen_random_uuid(), 'حلب', 'Aleppo', NOW(), false),
(gen_random_uuid(), 'حمص', 'Homs', NOW(), false),
(gen_random_uuid(), 'حماة', 'Hama', NOW(), false),
(gen_random_uuid(), 'اللاذقية', 'Latakia', NOW(), false),
(gen_random_uuid(), 'طرطوس', 'Tartus', NOW(), false),
(gen_random_uuid(), 'السويداء', 'As-Suwayda', NOW(), false),
(gen_random_uuid(), 'درعا', 'Daraa', NOW(), false),
(gen_random_uuid(), 'دير الزور', 'Deir ez-Zor', NOW(), false),
(gen_random_uuid(), 'الرقة', 'Raqqa', NOW(), false)
ON CONFLICT DO NOTHING;

-- 2. التصنيفات
INSERT INTO "Categories" ("Id", "Name", "NameEn", "Description", "DescriptionEn", "CreationTime", "IsDeleted") VALUES
(gen_random_uuid(), 'مؤتمرات', 'Conferences', 'مؤتمرات تقنية وعلمية', 'Technology and scientific conferences', NOW(), false),
(gen_random_uuid(), 'ورش عمل', 'Workshops', 'ورش عمل تدريبية', 'Training workshops', NOW(), false),
(gen_random_uuid(), 'ندوات', 'Seminars', 'ندوات ثقافية وعلمية', 'Cultural and scientific seminars', NOW(), false),
(gen_random_uuid(), 'معارض', 'Exhibitions', 'معارض فنية وتجارية', 'Art and trade exhibitions', NOW(), false),
(gen_random_uuid(), 'احتفالات', 'Celebrations', 'احتفالات وفعاليات اجتماعية', 'Social celebrations and events', NOW(), false),
(gen_random_uuid(), 'تقنية', 'Technology', 'فعاليات تقنية وبرمجية', 'Tech and programming events', NOW(), false),
(gen_random_uuid(), 'فنون', 'Arts', 'فعاليات فنية وثقافية', 'Arts and cultural events', NOW(), false),
(gen_random_uuid(), 'رياضة', 'Sports', 'فعاليات وبطولات رياضية', 'Sports events and championships', NOW(), false),
(gen_random_uuid(), 'تعليم', 'Education', 'فعاليات تعليمية وأكاديمية', 'Educational and academic events', NOW(), false),
(gen_random_uuid(), 'صحة', 'Health', 'فعاليات صحية وتوعوية', 'Health and awareness events', NOW(), false),
(gen_random_uuid(), 'أعمال', 'Business', 'فعاليات ريادة الأعمال', 'Entrepreneurship events', NOW(), false),
(gen_random_uuid(), 'موسيقى', 'Music', 'حفلات ومهرجانات موسيقية', 'Music concerts and festivals', NOW(), false)
ON CONFLICT DO NOTHING;

-- 3. إعدادات التطبيق
INSERT INTO app_settings (slider_items_count, auto_approve_events) 
SELECT 5, false 
WHERE NOT EXISTS (SELECT 1 FROM app_settings LIMIT 1);

-- 4. الحصول على admin user ID
DO $$
DECLARE
    admin_user_id uuid;
    damascus_id uuid;
    aleppo_id uuid;
    latakia_id uuid;
    tech_cat uuid;
    workshop_cat uuid;
    arts_cat uuid;
    music_cat uuid;
    business_cat uuid;
BEGIN
    -- جلب admin user
    SELECT "Id" INTO admin_user_id FROM "AbpUsers" WHERE "UserName" = 'admin' LIMIT 1;
    
    -- جلب IDs المدن
    SELECT "Id" INTO damascus_id FROM "Cities" WHERE "NameEn" = 'Damascus' LIMIT 1;
    SELECT "Id" INTO aleppo_id FROM "Cities" WHERE "NameEn" = 'Aleppo' LIMIT 1;
    SELECT "Id" INTO latakia_id FROM "Cities" WHERE "NameEn" = 'Latakia' LIMIT 1;
    
    -- جلب IDs التصنيفات
    SELECT "Id" INTO tech_cat FROM "Categories" WHERE "NameEn" = 'Technology' LIMIT 1;
    SELECT "Id" INTO workshop_cat FROM "Categories" WHERE "NameEn" = 'Workshops' LIMIT 1;
    SELECT "Id" INTO arts_cat FROM "Categories" WHERE "NameEn" = 'Arts' LIMIT 1;
    SELECT "Id" INTO music_cat FROM "Categories" WHERE "NameEn" = 'Music' LIMIT 1;
    SELECT "Id" INTO business_cat FROM "Categories" WHERE "NameEn" = 'Business' LIMIT 1;
    
    -- فعاليات قادمة
    INSERT INTO "Events" ("Id", "Title", "TitleEn", "Description", "DescriptionEn", "StartDate", "EndDate", "Location", "LocationEn", 
                          "MaxCapacity", "IsApproved", "Status", "ImageUrl", "ThumbnailUrl", "CategoryId", "CityId", "OrganizerId", 
                          "CreationTime", "IsDeleted") VALUES
    
    (gen_random_uuid(), 
     'مؤتمر التقنية السوري 2025', 
     'Syrian Tech Conference 2025',
     'مؤتمر سنوي يجمع خبراء التقنية والمبرمجين من جميع أنحاء سوريا.',
     'Annual conference bringing together tech experts and developers.',
     NOW() + INTERVAL '15 days',
     NOW() + INTERVAL '17 days',
     'فندق الشام',
     'Al Sham Hotel',
     500, true, 3,
     'https://images.unsplash.com/photo-1540575467063-178a50c2df87?w=1200&h=500&fit=crop',
     'https://images.unsplash.com/photo-1540575467063-178a50c2df87?w=400&h=200&fit=crop',
     tech_cat, damascus_id, admin_user_id, NOW(), false),
    
    (gen_random_uuid(),
     'معرض الفنون المعاصرة',
     'Contemporary Art Exhibition',
     'معرض فني يستعرض أعمال 50 فنان سوري معاصر.',
     'Art exhibition showcasing 50 contemporary Syrian artists.',
     NOW() + INTERVAL '7 days',
     NOW() + INTERVAL '30 days',
     'المركز الثقافي العربي',
     'Arab Cultural Center',
     200, true, 3,
     'https://images.unsplash.com/photo-1561214115-f2f134cc4912?w=1200&h=500&fit=crop',
     'https://images.unsplash.com/photo-1561214115-f2f134cc4912?w=400&h=200&fit=crop',
     arts_cat, damascus_id, admin_user_id, NOW(), false),
    
    (gen_random_uuid(),
     'ورشة تطوير الويب الحديث',
     'Modern Web Development Workshop',
     'ورشة عمل مكثفة لمدة 3 أيام تغطي React, Angular, وأحدث تقنيات.',
     'Intensive 3-day workshop covering React, Angular, and latest technologies.',
     NOW() + INTERVAL '10 days',
     NOW() + INTERVAL '12 days',
     'مركز الابتكار التقني',
     'Tech Innovation Center',
     50, true, 3,
     'https://images.unsplash.com/photo-1531482615713-2afd69097998?w=1200&h=500&fit=crop',
     'https://images.unsplash.com/photo-1531482615713-2afd69097998?w=400&h=200&fit=crop',
     workshop_cat, aleppo_id, admin_user_id, NOW(), false),
    
    (gen_random_uuid(),
     'مهرجان اللاذقية الموسيقي الصيفي',
     'Latakia Summer Music Festival',
     'مهرجان موسيقي كبير يستمر لمدة أسبوع مع عروض لفرق محلية وعالمية.',
     'Major music festival with local and international bands.',
     NOW() + INTERVAL '25 days',
     NOW() + INTERVAL '32 days',
     'كورنيش اللاذقية',
     'Latakia Corniche',
     5000, true, 3,
     'https://images.unsplash.com/photo-1470229722913-7c0e2dbbafd3?w=1200&h=500&fit=crop',
     'https://images.unsplash.com/photo-1470229722913-7c0e2dbbafd3?w=400&h=200&fit=crop',
     music_cat, latakia_id, admin_user_id, NOW(), false),
    
    (gen_random_uuid(),
     'ملتقى ريادة الأعمال السوري',
     'Syrian Entrepreneurship Forum',
     'ملتقى يجمع رواد الأعمال والمستثمرين لمناقشة فرص الاستثمار.',
     'Forum bringing together entrepreneurs and investors.',
     NOW() + INTERVAL '20 days',
     NOW() + INTERVAL '21 days',
     'غرفة تجارة دمشق',
     'Damascus Chamber of Commerce',
     300, true, 3,
     'https://images.unsplash.com/photo-1559136555-9303baea8ebd?w=1200&h=500&fit=crop',
     'https://images.unsplash.com/photo-1559136555-9303baea8ebd?w=400&h=200&fit=crop',
     business_cat, damascus_id, admin_user_id, NOW(), false),
    
    (gen_random_uuid(),
     'معرض دمشق الدولي للكتاب',
     'Damascus International Book Fair',
     'معرض الكتاب السنوي الأكبر في سوريا.',
     'The largest annual book fair in Syria.',
     NOW() + INTERVAL '40 days',
     NOW() + INTERVAL '54 days',
     'مدينة المعارض',
     'Exhibition City',
     10000, true, 3,
     'https://images.unsplash.com/photo-1481627834876-b7833e8f5570?w=1200&h=500&fit=crop',
     'https://images.unsplash.com/photo-1481627834876-b7833e8f5570?w=400&h=200&fit=crop',
     (SELECT "Id" FROM "Categories" WHERE "NameEn" = 'Exhibitions' LIMIT 1), damascus_id, admin_user_id, NOW(), false);
    
    -- عناصر السلايدر
    INSERT INTO home_slider_items (display_order, type, custom_event_id, is_active, title, title_en, image_url, creation_time, is_deleted) VALUES
    (1, 3, (SELECT "Id" FROM "Events" WHERE "Title" LIKE '%مؤتمر التقنية%' LIMIT 1), true, 
     'مؤتمر التقنية السوري 2025', 
     'Syrian Tech Conference 2025',
     'https://images.unsplash.com/photo-1540575467063-178a50c2df87?w=1200&h=500&fit=crop',
     NOW(), false),
    
    (2, 3, (SELECT "Id" FROM "Events" WHERE "Title" LIKE '%معرض الفنون%' LIMIT 1), true,
     'معرض الفنون المعاصرة',
     'Contemporary Art Exhibition',
     'https://images.unsplash.com/photo-1561214115-f2f134cc4912?w=1200&h=500&fit=crop',
     NOW(), false),
    
    (3, 3, (SELECT "Id" FROM "Events" WHERE "Title" LIKE '%تطوير الويب%' LIMIT 1), true,
     'ورشة تطوير الويب الحديث',
     'Modern Web Development Workshop',
     'https://images.unsplash.com/photo-1531482615713-2afd69097998?w=1200&h=500&fit=crop',
     NOW(), false),
    
    (4, 3, (SELECT "Id" FROM "Events" WHERE "Title" LIKE '%مهرجان اللاذقية%' LIMIT 1), true,
     'مهرجان اللاذقية الموسيقي',
     'Latakia Music Festival',
     'https://images.unsplash.com/photo-1470229722913-7c0e2dbbafd3?w=1200&h=500&fit=crop',
     NOW(), false),
    
    (5, 3, (SELECT "Id" FROM "Events" WHERE "Title" LIKE '%ريادة الأعمال%' LIMIT 1), true,
     'ملتقى ريادة الأعمال',
     'Entrepreneurship Forum',
     'https://images.unsplash.com/photo-1559136555-9303baea8ebd?w=1200&h=500&fit=crop',
     NOW(), false)
    ON CONFLICT DO NOTHING;
    
    RAISE NOTICE '====================================';
    RAISE NOTICE 'تم إضافة البيانات بنجاح!';
    RAISE NOTICE 'المدن: %', (SELECT COUNT(*) FROM "Cities");
    RAISE NOTICE 'التصنيفات: %', (SELECT COUNT(*) FROM "Categories");
    RAISE NOTICE 'الفعاليات: %', (SELECT COUNT(*) FROM "Events");
    RAISE NOTICE 'السلايدر: %', (SELECT COUNT(*) FROM home_slider_items);
    RAISE NOTICE '====================================';
END $$;

