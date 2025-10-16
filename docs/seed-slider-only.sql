-- إضافة عناصر سلايدر بسيطة (بدون ربط بفعاليات)
INSERT INTO home_slider_items (display_order, type, custom_event_id, is_active, title, title_en, image_url, creation_time, is_deleted) VALUES
(1, 1, NULL, true, 
 'مرحباً بك في منصة إدارة الفعاليات', 
 'Welcome to Event Management Platform',
 'https://images.unsplash.com/photo-1540575467063-178a50c2df87?w=1200&h=500&fit=crop',
 NOW(), false),

(2, 1, NULL, true,
 'اكتشف أحدث الفعاليات في سوريا',
 'Discover Latest Events in Syria',
 'https://images.unsplash.com/photo-1561214115-f2f134cc4912?w=1200&h=500&fit=crop',
 NOW(), false),

(3, 1, NULL, true,
 'انضم إلى مجتمع الفعاليات',
 'Join Our Events Community',
 'https://images.unsplash.com/photo-1531482615713-2afd69097998?w=1200&h=500&fit=crop',
 NOW(), false),

(4, 1, NULL, true,
 'فعاليات تقنية ومؤتمرات',
 'Tech Events and Conferences',
 'https://images.unsplash.com/photo-1470229722913-7c0e2dbbafd3?w=1200&h=500&fit=crop',
 NOW(), false),

(5, 1, NULL, true,
 'احجز مقعدك الآن',
 'Book Your Seat Now',
 'https://images.unsplash.com/photo-1559136555-9303baea8ebd?w=1200&h=500&fit=crop',
 NOW(), false)
ON CONFLICT DO NOTHING;

SELECT 'تم إضافة ' || COUNT(*) || ' عنصر للسلايدر' as result FROM home_slider_items;

