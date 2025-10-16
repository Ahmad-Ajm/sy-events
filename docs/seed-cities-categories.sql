-- المدن والتصنيفات
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
(gen_random_uuid(), 'الرقة', 'Raqqa', '{}', gen_random_uuid()::text, NOW(), false)
ON CONFLICT DO NOTHING;

INSERT INTO "Categories" ("Id", "Name", "NameEn", "Description", "DescriptionEn", "ExtraProperties", "ConcurrencyStamp", "CreationTime", "IsDeleted") VALUES
(gen_random_uuid(), 'مؤتمرات', 'Conferences', 'مؤتمرات تقنية وعلمية', 'Tech conferences', '{}', gen_random_uuid()::text, NOW(), false),
(gen_random_uuid(), 'ورش عمل', 'Workshops', 'ورش عمل تدريبية', 'Workshops', '{}', gen_random_uuid()::text, NOW(), false),
(gen_random_uuid(), 'ندوات', 'Seminars', 'ندوات ثقافية', 'Seminars', '{}', gen_random_uuid()::text, NOW(), false),
(gen_random_uuid(), 'معارض', 'Exhibitions', 'معارض فنية', 'Exhibitions', '{}', gen_random_uuid()::text, NOW(), false),
(gen_random_uuid(), 'تقنية', 'Technology', 'فعاليات تقنية', 'Tech events', '{}', gen_random_uuid()::text, NOW(), false),
(gen_random_uuid(), 'فنون', 'Arts', 'فعاليات فنية', 'Arts events', '{}', gen_random_uuid()::text, NOW(), false),
(gen_random_uuid(), 'رياضة', 'Sports', 'فعاليات رياضية', 'Sports', '{}', gen_random_uuid()::text, NOW(), false),
(gen_random_uuid(), 'تعليم', 'Education', 'فعاليات تعليمية', 'Education', '{}', gen_random_uuid()::text, NOW(), false),
(gen_random_uuid(), 'موسيقى', 'Music', 'حفلات موسيقية', 'Music', '{}', gen_random_uuid()::text, NOW(), false),
(gen_random_uuid(), 'أعمال', 'Business', 'فعاليات أعمال', 'Business', '{}', gen_random_uuid()::text, NOW(), false)
ON CONFLICT DO NOTHING;

SELECT '✅ المدن: ' || (SELECT COUNT(*) FROM "Cities") || ', التصنيفات: ' || (SELECT COUNT(*) FROM "Categories") || ', السلايدر: ' || (SELECT COUNT(*) FROM home_slider_items) as "النتيجة";

