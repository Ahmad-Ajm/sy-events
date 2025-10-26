-- ============================================
-- الاستعلامات الشائعة لقاعدة البيانات
-- ============================================
-- التاريخ: 17 أكتوبر 2025
-- الغرض: مجموعة من الاستعلامات المفيدة للتطوير والصيانة

-- ============================================
-- 1. معلومات عامة
-- ============================================

-- عرض إصدار PostgreSQL
SELECT version();

-- عرض جميع الجداول
SELECT table_name 
FROM information_schema.tables 
WHERE table_schema = 'public' 
ORDER BY table_name;

-- عرض حجم قاعدة البيانات
SELECT pg_size_pretty(pg_database_size('EventManagementDb')) as database_size;

-- ============================================
-- 2. فحص الترحيلات (Migrations)
-- ============================================

-- عرض جميع الترحيلات المطبقة
SELECT "MigrationId", "ProductVersion" 
FROM "__EFMigrationsHistory" 
ORDER BY "MigrationId";

-- آخر ترحيل مطبق
SELECT "MigrationId", "ProductVersion" 
FROM "__EFMigrationsHistory" 
ORDER BY "MigrationId" DESC 
LIMIT 1;

-- عدد الترحيلات المطبقة
SELECT COUNT(*) as total_migrations 
FROM "__EFMigrationsHistory";

-- ============================================
-- 3. فحص بنية الجداول
-- ============================================

-- عرض أعمدة جدول Events
SELECT 
    column_name,
    data_type,
    is_nullable,
    column_default
FROM information_schema.columns 
WHERE table_name = 'Events'
ORDER BY ordinal_position;

-- فحص وجود عمود Kind في جدول Events
SELECT 
    column_name,
    data_type,
    is_nullable
FROM information_schema.columns 
WHERE table_name = 'Events' 
  AND column_name = 'Kind';

-- عرض جميع Foreign Keys
SELECT
    tc.table_name, 
    kcu.column_name,
    ccu.table_name AS foreign_table_name,
    ccu.column_name AS foreign_column_name
FROM information_schema.table_constraints AS tc
JOIN information_schema.key_column_usage AS kcu
  ON tc.constraint_name = kcu.constraint_name
JOIN information_schema.constraint_column_usage AS ccu
  ON ccu.constraint_name = tc.constraint_name
WHERE tc.constraint_type = 'FOREIGN KEY'
ORDER BY tc.table_name;

-- عرض Indexes
SELECT
    tablename,
    indexname,
    indexdef
FROM pg_indexes
WHERE schemaname = 'public'
ORDER BY tablename, indexname;

-- ============================================
-- 4. إحصائيات البيانات
-- ============================================

-- عدد السجلات في كل جدول
SELECT 
    'Cities' as table_name, 
    COUNT(*) as record_count 
FROM "Cities"
UNION ALL
SELECT 
    'Categories', 
    COUNT(*) 
FROM "Categories"
UNION ALL
SELECT 
    'Users', 
    COUNT(*) 
FROM "Users"
UNION ALL
SELECT 
    'Events', 
    COUNT(*) 
FROM "Events"
UNION ALL
SELECT 
    'Bookings', 
    COUNT(*) 
FROM "Bookings"
UNION ALL
SELECT 
    'AppSettings', 
    COUNT(*) 
FROM "AppSettings"
UNION ALL
SELECT 
    'HomeSliderItems', 
    COUNT(*) 
FROM "HomeSliderItems"
UNION ALL
SELECT 
    'FeaturedBoxes', 
    COUNT(*) 
FROM "FeaturedBoxes";

-- ============================================
-- 5. فحص البيانات الأساسية
-- ============================================

-- عرض جميع المدن
SELECT "Id", "Name", "NameEn" 
FROM "Cities" 
ORDER BY "Name";

-- عرض جميع التصنيفات
SELECT "Id", "Name", "NameEn", "Description" 
FROM "Categories" 
ORDER BY "DisplayOrder";

-- عرض المستخدمين حسب الدور
SELECT 
    "Role",
    COUNT(*) as user_count
FROM "Users"
WHERE NOT "IsDeleted"
GROUP BY "Role";

-- عرض أول 5 مستخدمين
SELECT 
    "Id", 
    "Email", 
    "Name", 
    "Role", 
    "IsActive" 
FROM "Users" 
WHERE NOT "IsDeleted"
ORDER BY "CreationTime" DESC
LIMIT 5;

-- ============================================
-- 6. فحص الفعاليات
-- ============================================

-- عرض جميع الفعاليات
SELECT 
    "Id",
    "Title",
    "TitleEn",
    "Kind",
    "Status",
    "IsApproved",
    "StartDate",
    "EndDate"
FROM "Events"
WHERE NOT "IsDeleted"
ORDER BY "StartDate" DESC;

-- عدد الفعاليات حسب النوع (Kind)
SELECT 
    "Kind",
    COUNT(*) as event_count
FROM "Events"
WHERE NOT "IsDeleted"
GROUP BY "Kind";

-- عدد الفعاليات حسب الحالة (Status)
SELECT 
    "Status",
    COUNT(*) as event_count
FROM "Events"
WHERE NOT "IsDeleted"
GROUP BY "Status";

-- الفعاليات المعتمدة
SELECT 
    "Title",
    "StartDate",
    "Location"
FROM "Events"
WHERE "IsApproved" = true 
  AND NOT "IsDeleted"
ORDER BY "StartDate";

-- الفعاليات القادمة
SELECT 
    "Title",
    "TitleEn",
    "StartDate",
    "Location"
FROM "Events"
WHERE "StartDate" > NOW() 
  AND "IsApproved" = true
  AND NOT "IsDeleted"
ORDER BY "StartDate"
LIMIT 10;

-- ============================================
-- 7. فحص الحجوزات
-- ============================================

-- إحصائيات الحجوزات
SELECT 
    "Status",
    COUNT(*) as booking_count
FROM "Bookings"
WHERE NOT "IsDeleted"
GROUP BY "Status";

-- آخر 10 حجوزات
SELECT 
    b."Id",
    u."Name" as user_name,
    e."Title" as event_title,
    b."Status",
    b."CreationTime"
FROM "Bookings" b
JOIN "Users" u ON b."UserId" = u."Id"
JOIN "Events" e ON b."EventId" = e."Id"
WHERE NOT b."IsDeleted"
ORDER BY b."CreationTime" DESC
LIMIT 10;

-- ============================================
-- 8. فحص إعدادات التطبيق
-- ============================================

-- عرض إعدادات التطبيق
SELECT 
    "Id",
    "SliderItemsCount",
    "AutoApproveEvents",
    "CreationTime",
    "LastModificationTime"
FROM "AppSettings";

-- ============================================
-- 9. فحص السلايدر والمربعات المميزة
-- ============================================

-- عناصر السلايدر النشطة
SELECT 
    "Id",
    "Title",
    "TitleEn",
    "Type",
    "DisplayOrder",
    "IsActive"
FROM "HomeSliderItems"
WHERE "IsActive" = true
ORDER BY "DisplayOrder";

-- المربعات المميزة النشطة
SELECT 
    "Id",
    "Title",
    "TitleEn",
    "Type",
    "DisplayOrder",
    "IsActive"
FROM "FeaturedBoxes"
WHERE "IsActive" = true
ORDER BY "DisplayOrder";

-- ============================================
-- 10. استعلامات الصيانة
-- ============================================

-- حذف جميع البيانات (احذر!)
-- DELETE FROM "Bookings";
-- DELETE FROM "Events";
-- DELETE FROM "Users" WHERE "Email" != 'admin@example.com';
-- DELETE FROM "Categories";
-- DELETE FROM "Cities";
-- DELETE FROM "HomeSliderItems";
-- DELETE FROM "FeaturedBoxes";
-- DELETE FROM "AppSettings";

-- إعادة تعيين sequences
-- SELECT setval('"Cities_Id_seq"', 1, false);
-- SELECT setval('"Categories_Id_seq"', 1, false);

-- ============================================
-- 11. استعلامات التشخيص
-- ============================================

-- فحص الاتصالات النشطة
SELECT 
    datname,
    usename,
    application_name,
    client_addr,
    state,
    query
FROM pg_stat_activity
WHERE datname = 'EventManagementDb';

-- فحص حجم الجداول
SELECT
    tablename,
    pg_size_pretty(pg_total_relation_size(schemaname||'.'||tablename)) AS size
FROM pg_tables
WHERE schemaname = 'public'
ORDER BY pg_total_relation_size(schemaname||'.'||tablename) DESC;

-- فحص آخر تحديث للإحصائيات
SELECT
    schemaname,
    tablename,
    last_vacuum,
    last_autovacuum,
    last_analyze,
    last_autoanalyze
FROM pg_stat_user_tables
WHERE schemaname = 'public'
ORDER BY last_autoanalyze DESC NULLS LAST;

-- ============================================
-- 12. استعلامات مفيدة للتطوير
-- ============================================

-- البحث عن مستخدم بالبريد الإلكتروني
SELECT * 
FROM "Users" 
WHERE "Email" = 'admin@example.com';

-- البحث عن فعالية بالعنوان
SELECT * 
FROM "Events" 
WHERE "Title" LIKE '%تقنية%' 
   OR "TitleEn" LIKE '%Tech%';

-- فحص صلاحيات المستخدم
SELECT 
    u."Name",
    u."Email",
    u."Role",
    COUNT(e."Id") as organized_events_count
FROM "Users" u
LEFT JOIN "Events" e ON u."Id" = e."OrganizerId"
WHERE NOT u."IsDeleted"
GROUP BY u."Id", u."Name", u."Email", u."Role"
ORDER BY organized_events_count DESC;

-- ============================================
-- نهاية الملف
-- ============================================

-- ملاحظات الاستخدام:
-- 1. استخدم psql لتنفيذ هذه الاستعلامات:
--    psql -h localhost -U postgres -d EventManagementDb -f common-db-queries.sql
--
-- 2. أو استخدم MCP Server للتفاعل المباشر
--
-- 3. للاستعلامات الخطرة (DELETE, DROP), احذف التعليق بحذر!

