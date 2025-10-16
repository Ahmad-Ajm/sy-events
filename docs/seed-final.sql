-- ====================================================================
-- بيانات وهمية كاملة - الإصدار النهائي
-- ====================================================================

-- 1. المدن السورية (10 مدن)
INSERT INTO "Cities" ("Id", "Name", "NameEn", "ExtraProperties", "ConcurrencyStamp", "CreationTime", "IsDeleted") VALUES
(gen_random_uuid(), 'دمشق', 'Damascus', '{}', gen_random_uuid()::text, NOW(), false),
(gen_random_uuid(), 'حلب', 'Aleppo', '{}', gen_random_uuid()::text, NOW(), false),
(gen_random_uuid(), 'حمص', 'Homs', '{}', gen_random_uuid()::text, NOW(), false),
(gen_random_uuid(), 'حماة', 'Hama', '{}', gen_random_uuid()::text, NOW(), false),
(gen_random_uuid(), 'اللاذقية', 'Latakia', '{}', gen_random_uuid()::text, NOW(), false),
(gen_random_uuid(), 'طرطوس', 'Tartus', '{}', gen_random_uuid()::text, NOW(), false),
(gen_random_uuid(), 'السويداء', 'As-Suwayda', '{}', gen_random_uuid()::text, NOW(), false),
(gen_random_uuid(), 'درعا', 'Daraa', '{}', gen_random_uuid()::text, NOW(), false),
(gen_random_uuid(), 'دير الزور', 'Deir ez-Zor', '{}', gen_random_uuid()::text, NOW(), false),
(gen_random_uuid(), 'الرقة', 'Raqqa', '{}', gen_random_uuid()::text, NOW(), false);

-- 2. التصنيفات (12 تصنيف)
INSERT INTO "Categories" ("Id", "Name", "NameEn", "Description", "DescriptionEn", "ExtraProperties", "ConcurrencyStamp", "CreationTime", "IsDeleted") VALUES
(gen_random_uuid(), 'مؤتمرات', 'Conferences', 'مؤتمرات تقنية وعلمية', 'Technology conferences', '{}', gen_random_uuid()::text, NOW(), false),
(gen_random_uuid(), 'ورش عمل', 'Workshops', 'ورش عمل تدريبية', 'Training workshops', '{}', gen_random_uuid()::text, NOW(), false),
(gen_random_uuid(), 'ندوات', 'Seminars', 'ندوات ثقافية وعلمية', 'Cultural seminars', '{}', gen_random_uuid()::text, NOW(), false),
(gen_random_uuid(), 'معارض', 'Exhibitions', 'معارض فنية وتجارية', 'Art exhibitions', '{}', gen_random_uuid()::text, NOW(), false),
(gen_random_uuid(), 'احتفالات', 'Celebrations', 'احتفالات وفعاليات', 'Celebrations', '{}', gen_random_uuid()::text, NOW(), false),
(gen_random_uuid(), 'تقنية', 'Technology', 'فعاليات تقنية وبرمجية', 'Tech events', '{}', gen_random_uuid()::text, NOW(), false),
(gen_random_uuid(), 'فنون', 'Arts', 'فعاليات فنية وثقافية', 'Arts events', '{}', gen_random_uuid()::text, NOW(), false),
(gen_random_uuid(), 'رياضة', 'Sports', 'فعاليات رياضية', 'Sports events', '{}', gen_random_uuid()::text, NOW(), false),
(gen_random_uuid(), 'تعليم', 'Education', 'فعاليات تعليمية', 'Educational events', '{}', gen_random_uuid()::text, NOW(), false),
(gen_random_uuid(), 'صحة', 'Health', 'فعاليات صحية', 'Health events', '{}', gen_random_uuid()::text, NOW(), false),
(gen_random_uuid(), 'أعمال', 'Business', 'فعاليات ريادية', 'Business events', '{}', gen_random_uuid()::text, NOW(), false),
(gen_random_uuid(), 'موسيقى', 'Music', 'حفلات موسيقية', 'Music concerts', '{}', gen_random_uuid()::text, NOW(), false);

-- 3. إعدادات السلايدر
DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM app_settings) THEN
        INSERT INTO app_settings ("SliderItemsCount", "AutoApproveEvents") VALUES (5, false);
    END IF;
END $$;

-- 4. فعاليات (6 فعاليات قادمة)
DO $$
DECLARE
    admin_id uuid;
    damascus_id uuid;
    aleppo_id uuid;
    latakia_id uuid;
    tech_cat uuid;
    workshop_cat uuid;
    arts_cat uuid;
    music_cat uuid;
    business_cat uuid;
    exhibition_cat uuid;
    
    event1_id uuid;
    event2_id uuid;
    event3_id uuid;
    event4_id uuid;
    event5_id uuid;
BEGIN
    SELECT "Id" INTO admin_id FROM "AbpUsers" WHERE "UserName" = 'admin' LIMIT 1;
    SELECT "Id" INTO damascus_id FROM "Cities" WHERE "NameEn" = 'Damascus' LIMIT 1;
    SELECT "Id" INTO aleppo_id FROM "Cities" WHERE "NameEn" = 'Aleppo' LIMIT 1;
    SELECT "Id" INTO latakia_id FROM "Cities" WHERE "NameEn" = 'Latakia' LIMIT 1;
    
    SELECT "Id" INTO tech_cat FROM "Categories" WHERE "NameEn" = 'Technology' LIMIT 1;
    SELECT "Id" INTO workshop_cat FROM "Categories" WHERE "NameEn" = 'Workshops' LIMIT 1;
    SELECT "Id" INTO arts_cat FROM "Categories" WHERE "NameEn" = 'Arts' LIMIT 1;
    SELECT "Id" INTO music_cat FROM "Categories" WHERE "NameEn" = 'Music' LIMIT 1;
    SELECT "Id" INTO business_cat FROM "Categories" WHERE "NameEn" = 'Business' LIMIT 1;
    SELECT "Id" INTO exhibition_cat FROM "Categories" WHERE "NameEn" = 'Exhibitions' LIMIT 1;
    
    -- فعالية 1
    event1_id := gen_random_uuid();
    INSERT INTO "Events" ("Id", "Title", "TitleEn", "Description", "DescriptionEn", "StartDate", "EndDate", "Location", "LocationEn", 
                          "MaxCapacity", "IsApproved", "Status", "ImageUrl", "ThumbnailUrl", "CategoryId", "CityId", "OrganizerId", 
                          "ExtraProperties", "ConcurrencyStamp", "CreationTime", "IsDeleted")
    VALUES (event1_id, 
            'مؤتمر التقنية السوري 2025', 
            'Syrian Tech Conference 2025',
            'مؤتمر سنوي يجمع خبراء التقنية والمبرمجين من جميع أنحاء سوريا. يتضمن ورش عمل ومحاضرات ومعرض للشركات التقنية.',
            'Annual conference bringing together tech experts and developers from all over Syria.',
            NOW() + INTERVAL '15 days',
            NOW() + INTERVAL '17 days',
            'فندق الشام - قاعة المؤتمرات',
            'Al Sham Hotel - Conference Hall',
            500, true, 3,
            'https://images.unsplash.com/photo-1540575467063-178a50c2df87?w=1200&h=500&fit=crop',
            'https://images.unsplash.com/photo-1540575467063-178a50c2df87?w=400&h=200&fit=crop',
            tech_cat, damascus_id, admin_id,
            '{}', gen_random_uuid()::text, NOW(), false);
    
    -- فعالية 2
    event2_id := gen_random_uuid();
    INSERT INTO "Events" ("Id", "Title", "TitleEn", "Description", "DescriptionEn", "StartDate", "EndDate", "Location", "LocationEn", 
                          "MaxCapacity", "IsApproved", "Status", "ImageUrl", "ThumbnailUrl", "CategoryId", "CityId", "OrganizerId", 
                          "ExtraProperties", "ConcurrencyStamp", "CreationTime", "IsDeleted")
    VALUES (event2_id,
            'معرض الفنون المعاصرة',
            'Contemporary Art Exhibition',
            'معرض فني يستعرض أعمال 50 فنان سوري معاصر، يشمل اللوحات والمنحوتات والتصوير الفوتوغرافي.',
            'Art exhibition showcasing works of 50 contemporary Syrian artists.',
            NOW() + INTERVAL '7 days',
            NOW() + INTERVAL '30 days',
            'المركز الثقافي العربي',
            'Arab Cultural Center',
            200, true, 3,
            'https://images.unsplash.com/photo-1561214115-f2f134cc4912?w=1200&h=500&fit=crop',
            'https://images.unsplash.com/photo-1561214115-f2f134cc4912?w=400&h=200&fit=crop',
            arts_cat, damascus_id, admin_id,
            '{}', gen_random_uuid()::text, NOW(), false);
    
    -- فعالية 3
    event3_id := gen_random_uuid();
    INSERT INTO "Events" ("Id", "Title", "TitleEn", "Description", "DescriptionEn", "StartDate", "EndDate", "Location", "LocationEn", 
                          "MaxCapacity", "IsApproved", "Status", "ImageUrl", "ThumbnailUrl", "CategoryId", "CityId", "OrganizerId", 
                          "ExtraProperties", "ConcurrencyStamp", "CreationTime", "IsDeleted")
    VALUES (event3_id,
            'ورشة تطوير الويب الحديث',
            'Modern Web Development Workshop',
            'ورشة عمل مكثفة لمدة 3 أيام تغطي React, Angular, Vue وأحدث تقنيات تطوير الواجهات.',
            'Intensive 3-day workshop covering React, Angular, Vue and latest frontend technologies.',
            NOW() + INTERVAL '10 days',
            NOW() + INTERVAL '12 days',
            'مركز الابتكار التقني - حلب',
            'Tech Innovation Center - Aleppo',
            50, true, 3,
            'https://images.unsplash.com/photo-1531482615713-2afd69097998?w=1200&h=500&fit=crop',
            'https://images.unsplash.com/photo-1531482615713-2afd69097998?w=400&h=200&fit=crop',
            workshop_cat, aleppo_id, admin_id,
            '{}', gen_random_uuid()::text, NOW(), false);
    
    -- فعالية 4
    event4_id := gen_random_uuid();
    INSERT INTO "Events" ("Id", "Title", "TitleEn", "Description", "DescriptionEn", "StartDate", "EndDate", "Location", "LocationEn", 
                          "MaxCapacity", "IsApproved", "Status", "ImageUrl", "ThumbnailUrl", "CategoryId", "CityId", "OrganizerId", 
                          "ExtraProperties", "ConcurrencyStamp", "CreationTime", "IsDeleted")
    VALUES (event4_id,
            'مهرجان اللاذقية الموسيقي الصيفي',
            'Latakia Summer Music Festival',
            'مهرجان موسيقي كبير يستمر لمدة أسبوع مع عروض لفرق محلية وعالمية على شاطئ البحر المتوسط.',
            'Major music festival lasting one week with local and international bands by the Mediterranean.',
            NOW() + INTERVAL '25 days',
            NOW() + INTERVAL '32 days',
            'كورنيش اللاذقية',
            'Latakia Corniche',
            5000, true, 3,
            'https://images.unsplash.com/photo-1470229722913-7c0e2dbbafd3?w=1200&h=500&fit=crop',
            'https://images.unsplash.com/photo-1470229722913-7c0e2dbbafd3?w=400&h=200&fit=crop',
            music_cat, latakia_id, admin_id,
            '{}', gen_random_uuid()::text, NOW(), false);
    
    -- فعالية 5
    event5_id := gen_random_uuid();
    INSERT INTO "Events" ("Id", "Title", "TitleEn", "Description", "DescriptionEn", "StartDate", "EndDate", "Location", "LocationEn", 
                          "MaxCapacity", "IsApproved", "Status", "ImageUrl", "ThumbnailUrl", "CategoryId", "CityId", "OrganizerId", 
                          "ExtraProperties", "ConcurrencyStamp", "CreationTime", "IsDeleted")
    VALUES (event5_id,
            'ملتقى ريادة الأعمال السوري',
            'Syrian Entrepreneurship Forum',
            'ملتقى يجمع رواد الأعمال والمستثمرين لمناقشة فرص الاستثمار والمشاريع الناشئة في سوريا.',
            'Forum bringing together entrepreneurs and investors to discuss startups in Syria.',
            NOW() + INTERVAL '20 days',
            NOW() + INTERVAL '21 days',
            'غرفة تجارة دمشق',
            'Damascus Chamber of Commerce',
            300, true, 3,
            'https://images.unsplash.com/photo-1559136555-9303baea8ebd?w=1200&h=500&fit=crop',
            'https://images.unsplash.com/photo-1559136555-9303baea8ebd?w=400&h=200&fit=crop',
            business_cat, damascus_id, admin_id,
            '{}', gen_random_uuid()::text, NOW(), false);
    
    -- فعالية 6
    INSERT INTO "Events" ("Id", "Title", "TitleEn", "Description", "DescriptionEn", "StartDate", "EndDate", "Location", "LocationEn", 
                          "MaxCapacity", "IsApproved", "Status", "ImageUrl", "ThumbnailUrl", "CategoryId", "CityId", "OrganizerId", 
                          "ExtraProperties", "ConcurrencyStamp", "CreationTime", "IsDeleted")
    VALUES (gen_random_uuid(),
            'معرض دمشق الدولي للكتاب',
            'Damascus International Book Fair',
            'معرض الكتاب السنوي الأكبر في سوريا، يستضيف دور نشر عربية وعالمية.',
            'The largest annual book fair in Syria, hosting Arab and international publishers.',
            NOW() + INTERVAL '40 days',
            NOW() + INTERVAL '54 days',
            'مدينة المعارض - دمشق',
            'Exhibition City - Damascus',
            10000, true, 3,
            'https://images.unsplash.com/photo-1481627834876-b7833e8f5570?w=1200&h=500&fit=crop',
            'https://images.unsplash.com/photo-1481627834876-b7833e8f5570?w=400&h=200&fit=crop',
            exhibition_cat, damascus_id, admin_id,
            '{}', gen_random_uuid()::text, NOW(), false);
    
    -- 5. عناصر السلايدر (5 عناصر)
    INSERT INTO home_slider_items (display_order, type, custom_event_id, is_active, title, title_en, image_url, creation_time, is_deleted) VALUES
    (1, 3, event1_id, true, 
     'مؤتمر التقنية السوري 2025', 
     'Syrian Tech Conference 2025',
     'https://images.unsplash.com/photo-1540575467063-178a50c2df87?w=1200&h=500&fit=crop',
     NOW(), false),
    
    (2, 3, event2_id, true,
     'معرض الفنون المعاصرة',
     'Contemporary Art Exhibition',
     'https://images.unsplash.com/photo-1561214115-f2f134cc4912?w=1200&h=500&fit=crop',
     NOW(), false),
    
    (3, 3, event3_id, true,
     'ورشة تطوير الويب',
     'Web Development Workshop',
     'https://images.unsplash.com/photo-1531482615713-2afd69097998?w=1200&h=500&fit=crop',
     NOW(), false),
    
    (4, 3, event4_id, true,
     'مهرجان اللاذقية الموسيقي',
     'Latakia Music Festival',
     'https://images.unsplash.com/photo-1470229722913-7c0e2dbbafd3?w=1200&h=500&fit=crop',
     NOW(), false),
    
    (5, 3, event5_id, true,
     'ملتقى ريادة الأعمال',
     'Entrepreneurship Forum',
     'https://images.unsplash.com/photo-1559136555-9303baea8ebd?w=1200&h=500&fit=crop',
     NOW(), false);
    
    RAISE NOTICE '====================================';
    RAISE NOTICE 'تم إضافة البيانات بنجاح!';
    RAISE NOTICE 'المدن: %', (SELECT COUNT(*) FROM "Cities");
    RAISE NOTICE 'التصنيفات: %', (SELECT COUNT(*) FROM "Categories");
    RAISE NOTICE 'الفعاليات: %', (SELECT COUNT(*) FROM "Events");
    RAISE NOTICE 'السلايدر: %', (SELECT COUNT(*) FROM home_slider_items);
    RAISE NOTICE '====================================';
    RAISE NOTICE 'افتح: http://localhost:4200';
    RAISE NOTICE '====================================';
END $$;

