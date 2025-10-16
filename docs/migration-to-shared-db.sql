-- =============================================
-- Migration Script: Next.js + ABP Shared Database
-- Database: event_management (PostgreSQL)
-- Date: 2025-10-13
-- Purpose: إعداد قاعدة البيانات المشتركة بين Next.js و ABP
-- =============================================

-- =============================================
-- الخطوة 1: Backup البيانات الحالية
-- =============================================

-- إنشاء جداول backup
CREATE TABLE IF NOT EXISTS users_backup AS SELECT * FROM users;
CREATE TABLE IF NOT EXISTS events_backup AS SELECT * FROM events;
CREATE TABLE IF NOT EXISTS categories_backup AS SELECT * FROM categories;
CREATE TABLE IF NOT EXISTS cities_backup AS SELECT * FROM cities;
CREATE TABLE IF NOT EXISTS bookings_backup AS SELECT * FROM bookings;

-- تأكيد نجاح Backup
SELECT 
    (SELECT COUNT(*) FROM users_backup) as users_backup_count,
    (SELECT COUNT(*) FROM events_backup) as events_backup_count,
    (SELECT COUNT(*) FROM categories_backup) as categories_backup_count,
    (SELECT COUNT(*) FROM cities_backup) as cities_backup_count,
    (SELECT COUNT(*) FROM bookings_backup) as bookings_backup_count;

-- =============================================
-- الخطوة 2: إضافة ABP Audit Columns
-- =============================================

-- Users Table
ALTER TABLE users ADD COLUMN IF NOT EXISTS "ExtraProperties" JSONB;
ALTER TABLE users ADD COLUMN IF NOT EXISTS "ConcurrencyStamp" VARCHAR(40);
ALTER TABLE users ADD COLUMN IF NOT EXISTS "CreatorId" UUID;
ALTER TABLE users ADD COLUMN IF NOT EXISTS "CreationTime" TIMESTAMP DEFAULT NOW();
ALTER TABLE users ADD COLUMN IF NOT EXISTS "LastModifierId" UUID;
ALTER TABLE users ADD COLUMN IF NOT EXISTS "LastModificationTime" TIMESTAMP;
ALTER TABLE users ADD COLUMN IF NOT EXISTS "DeleterId" UUID;
ALTER TABLE users ADD COLUMN IF NOT EXISTS "DeletionTime" TIMESTAMP;
ALTER TABLE users ADD COLUMN IF NOT EXISTS "IsDeleted" BOOLEAN DEFAULT FALSE;

-- Events Table
ALTER TABLE events ADD COLUMN IF NOT EXISTS "ExtraProperties" JSONB;
ALTER TABLE events ADD COLUMN IF NOT EXISTS "ConcurrencyStamp" VARCHAR(40);
ALTER TABLE events ADD COLUMN IF NOT EXISTS "CreatorId" UUID;
ALTER TABLE events ADD COLUMN IF NOT EXISTS "CreationTime" TIMESTAMP DEFAULT NOW();
ALTER TABLE events ADD COLUMN IF NOT EXISTS "LastModifierId" UUID;
ALTER TABLE events ADD COLUMN IF NOT EXISTS "LastModificationTime" TIMESTAMP;
ALTER TABLE events ADD COLUMN IF NOT EXISTS "DeleterId" UUID;
ALTER TABLE events ADD COLUMN IF NOT EXISTS "DeletionTime" TIMESTAMP;
ALTER TABLE events ADD COLUMN IF NOT EXISTS "IsDeleted" BOOLEAN DEFAULT FALSE;

-- Categories Table
ALTER TABLE categories ADD COLUMN IF NOT EXISTS "ExtraProperties" JSONB;
ALTER TABLE categories ADD COLUMN IF NOT EXISTS "ConcurrencyStamp" VARCHAR(40);
ALTER TABLE categories ADD COLUMN IF NOT EXISTS "CreatorId" UUID;
ALTER TABLE categories ADD COLUMN IF NOT EXISTS "CreationTime" TIMESTAMP DEFAULT NOW();
ALTER TABLE categories ADD COLUMN IF NOT EXISTS "LastModifierId" UUID;
ALTER TABLE categories ADD COLUMN IF NOT EXISTS "LastModificationTime" TIMESTAMP;
ALTER TABLE categories ADD COLUMN IF NOT EXISTS "DeleterId" UUID;
ALTER TABLE categories ADD COLUMN IF NOT EXISTS "DeletionTime" TIMESTAMP;
ALTER TABLE categories ADD COLUMN IF NOT EXISTS "IsDeleted" BOOLEAN DEFAULT FALSE;

-- Cities Table
ALTER TABLE cities ADD COLUMN IF NOT EXISTS "ExtraProperties" JSONB;
ALTER TABLE cities ADD COLUMN IF NOT EXISTS "ConcurrencyStamp" VARCHAR(40);
ALTER TABLE cities ADD COLUMN IF NOT EXISTS "CreatorId" UUID;
ALTER TABLE cities ADD COLUMN IF NOT EXISTS "CreationTime" TIMESTAMP DEFAULT NOW();
ALTER TABLE cities ADD COLUMN IF NOT EXISTS "LastModifierId" UUID;
ALTER TABLE cities ADD COLUMN IF NOT EXISTS "LastModificationTime" TIMESTAMP;
ALTER TABLE cities ADD COLUMN IF NOT EXISTS "DeleterId" UUID;
ALTER TABLE cities ADD COLUMN IF NOT EXISTS "DeletionTime" TIMESTAMP;
ALTER TABLE cities ADD COLUMN IF NOT EXISTS "IsDeleted" BOOLEAN DEFAULT FALSE;

-- Bookings Table
ALTER TABLE bookings ADD COLUMN IF NOT EXISTS "ExtraProperties" JSONB;
ALTER TABLE bookings ADD COLUMN IF NOT EXISTS "ConcurrencyStamp" VARCHAR(40);
ALTER TABLE bookings ADD COLUMN IF NOT EXISTS "CreatorId" UUID;
ALTER TABLE bookings ADD COLUMN IF NOT EXISTS "CreationTime" TIMESTAMP DEFAULT NOW();
ALTER TABLE bookings ADD COLUMN IF NOT EXISTS "LastModifierId" UUID;
ALTER TABLE bookings ADD COLUMN IF NOT EXISTS "LastModificationTime" TIMESTAMP;
ALTER TABLE bookings ADD COLUMN IF NOT EXISTS "DeleterId" UUID;
ALTER TABLE bookings ADD COLUMN IF NOT EXISTS "DeletionTime" TIMESTAMP;
ALTER TABLE bookings ADD COLUMN IF NOT EXISTS "IsDeleted" BOOLEAN DEFAULT FALSE;

-- =============================================
-- الخطوة 3: تحديث البيانات الموجودة
-- =============================================

-- Users
UPDATE users SET 
    "CreationTime" = "createdAt",
    "LastModificationTime" = "updatedAt",
    "IsDeleted" = FALSE,
    "ConcurrencyStamp" = substr(md5(random()::text), 1, 40)
WHERE "CreationTime" IS NULL;

-- Events
UPDATE events SET 
    "CreationTime" = "createdAt",
    "LastModificationTime" = "updatedAt",
    "IsDeleted" = FALSE,
    "ConcurrencyStamp" = substr(md5(random()::text), 1, 40)
WHERE "CreationTime" IS NULL;

-- Categories
UPDATE categories SET 
    "CreationTime" = "createdAt",
    "LastModificationTime" = "updatedAt",
    "IsDeleted" = FALSE,
    "ConcurrencyStamp" = substr(md5(random()::text), 1, 40)
WHERE "CreationTime" IS NULL;

-- Cities
UPDATE cities SET 
    "CreationTime" = "createdAt",
    "LastModificationTime" = "updatedAt",
    "IsDeleted" = FALSE,
    "ConcurrencyStamp" = substr(md5(random()::text), 1, 40)
WHERE "CreationTime" IS NULL;

-- Bookings
UPDATE bookings SET 
    "CreationTime" = "createdAt",
    "LastModificationTime" = "updatedAt",
    "IsDeleted" = FALSE,
    "ConcurrencyStamp" = substr(md5(random()::text), 1, 40)
WHERE "CreationTime" IS NULL;

-- =============================================
-- الخطوة 4: تحويل IDs من CUID إلى UUID (إذا لزم)
-- =============================================

-- ملاحظة: هذه الخطوة اختيارية وتعتمد على:
-- 1. إذا كانت البيانات الموجودة تستخدم CUID
-- 2. إذا قررت استخدام UUID في كلا النظامين

-- إنشاء mapping table (اختياري)
CREATE TABLE IF NOT EXISTS id_mapping (
    old_id VARCHAR(255) PRIMARY KEY,
    new_id UUID NOT NULL UNIQUE,
    table_name VARCHAR(100) NOT NULL,
    created_at TIMESTAMP DEFAULT NOW()
);

-- مثال: تحويل IDs (يتطلب تنفيذ مخصص حسب البيانات)
-- INSERT INTO id_mapping (old_id, new_id, table_name)
-- SELECT id, gen_random_uuid(), 'users' FROM users
-- WHERE id NOT SIMILAR TO '[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}';

-- =============================================
-- الخطوة 5: إنشاء Indexes للأداء
-- =============================================

-- Users
CREATE INDEX IF NOT EXISTS idx_users_email ON users(email);
CREATE INDEX IF NOT EXISTS idx_users_role ON users(role);
CREATE INDEX IF NOT EXISTS idx_users_createdAt ON users("createdAt");
CREATE INDEX IF NOT EXISTS idx_users_IsDeleted ON users("IsDeleted") WHERE "IsDeleted" = FALSE;

-- Events
CREATE INDEX IF NOT EXISTS idx_events_status ON events(status);
CREATE INDEX IF NOT EXISTS idx_events_categoryId ON events("categoryId");
CREATE INDEX IF NOT EXISTS idx_events_cityId ON events("cityId");
CREATE INDEX IF NOT EXISTS idx_events_organizerId ON events("organizerId");
CREATE INDEX IF NOT EXISTS idx_events_startDate ON events("startDate");
CREATE INDEX IF NOT EXISTS idx_events_IsDeleted ON events("IsDeleted") WHERE "IsDeleted" = FALSE;

-- Bookings
CREATE INDEX IF NOT EXISTS idx_bookings_userId ON bookings("userId");
CREATE INDEX IF NOT EXISTS idx_bookings_eventId ON bookings("eventId");
CREATE INDEX IF NOT EXISTS idx_bookings_status ON bookings(status);
CREATE INDEX IF NOT EXISTS idx_bookings_IsDeleted ON bookings("IsDeleted") WHERE "IsDeleted" = FALSE;

-- =============================================
-- الخطوة 6: التحقق من النتائج
-- =============================================

-- فحص الأعمدة الجديدة
SELECT 
    column_name, 
    data_type, 
    is_nullable
FROM information_schema.columns
WHERE table_name = 'users'
  AND column_name IN ('ExtraProperties', 'ConcurrencyStamp', 'CreatorId', 'IsDeleted')
ORDER BY ordinal_position;

-- فحص البيانات
SELECT 
    (SELECT COUNT(*) FROM users WHERE "IsDeleted" = FALSE) as active_users,
    (SELECT COUNT(*) FROM events WHERE "IsDeleted" = FALSE) as active_events,
    (SELECT COUNT(*) FROM categories WHERE "IsDeleted" = FALSE) as active_categories,
    (SELECT COUNT(*) FROM cities WHERE "IsDeleted" = FALSE) as active_cities,
    (SELECT COUNT(*) FROM bookings WHERE "IsDeleted" = FALSE) as active_bookings;

-- فحص Audit Data
SELECT 
    id,
    email,
    name,
    "CreationTime",
    "LastModificationTime",
    "IsDeleted"
FROM users
LIMIT 5;

-- =============================================
-- الخطوة 7: تعليقات وملاحظات
-- =============================================

-- ✅ تم إضافة جميع الأعمدة المطلوبة لـ ABP
-- ✅ تم تحديث البيانات الموجودة
-- ✅ تم إنشاء Backup للبيانات
-- ✅ تم إضافة Indexes للأداء

-- ⚠️ ملاحظات مهمة:
-- 1. قبل التشغيل على Production، اختبر على بيئة Development
-- 2. تأكد من وجود Backup كامل لقاعدة البيانات
-- 3. راجع التوافق بين Prisma Schema و EF Core Configuration
-- 4. اختبر CRUD operations من كلا التطبيقين بعد Migration

-- 🔗 الخطوات التالية:
-- 1. تحديث Prisma Schema لتضمين الأعمدة الجديدة
-- 2. تحديث EF Core DbContext لاستخدام أسماء الأعمدة الصحيحة
-- 3. اختبار التكامل بين Next.js و ABP
-- 4. مراقبة الأداء

-- تاريخ الإنشاء: 2025-10-13
-- النموذج: Claude Sonnet 4.5

