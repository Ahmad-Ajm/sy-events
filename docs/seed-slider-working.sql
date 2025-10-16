-- إضافة عناصر سلايدر (5 عناصر)
INSERT INTO home_slider_items ("Id", "DisplayOrder", "Type", "CustomEventId", "IsActive", "Title", "TitleEn", "ImageUrl", "ExtraProperties", "ConcurrencyStamp", "CreationTime", "IsDeleted") VALUES
(gen_random_uuid(), 1, 1, NULL, true, 
 'مرحباً بك في منصة إدارة الفعاليات', 
 'Welcome to Event Management Platform',
 'https://images.unsplash.com/photo-1540575467063-178a50c2df87?w=1200&h=500&fit=crop',
 '{}', gen_random_uuid()::text, NOW(), false),

(gen_random_uuid(), 2, 1, NULL, true,
 'اكتشف أحدث الفعاليات في سوريا',
 'Discover Latest Events in Syria',
 'https://images.unsplash.com/photo-1561214115-f2f134cc4912?w=1200&h=500&fit=crop',
 '{}', gen_random_uuid()::text, NOW(), false),

(gen_random_uuid(), 3, 1, NULL, true,
 'انضم إلى مجتمع الفعاليات',
 'Join Our Events Community',
 'https://images.unsplash.com/photo-1531482615713-2afd69097998?w=1200&h=500&fit=crop',
 '{}', gen_random_uuid()::text, NOW(), false),

(gen_random_uuid(), 4, 1, NULL, true,
 'فعاليات تقنية ومؤتمرات',
 'Tech Events and Conferences',
 'https://images.unsplash.com/photo-1470229722913-7c0e2dbbafd3?w=1200&h=500&fit=crop',
 '{}', gen_random_uuid()::text, NOW(), false),

(gen_random_uuid(), 5, 1, NULL, true,
 'احجز مقعدك الآن',
 'Book Your Seat Now',
 'https://images.unsplash.com/photo-1559136555-9303baea8ebd?w=1200&h=500&fit=crop',
 '{}', gen_random_uuid()::text, NOW(), false);

-- إعدادات
DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM app_settings) THEN
        INSERT INTO app_settings ("SliderItemsCount", "AutoApproveEvents") VALUES (5, false);
    ELSE
        UPDATE app_settings SET "SliderItemsCount" = 5, "AutoApproveEvents" = false;
    END IF;
END $$;

SELECT '✅ تم إضافة ' || COUNT(*) || ' عنصر للسلايدر' as "النتيجة" FROM home_slider_items;

