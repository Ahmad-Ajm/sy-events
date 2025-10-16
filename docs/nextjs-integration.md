# 🔗 Next.js Integration مع ABP Framework

## 📋 نظرة عامة

هذا المستند يوضح كيفية دمج مشروع Next.js الحالي مع ABP Framework Event Management Platform عبر **Shared Database**.

---

## 🎯 استراتيجية التكامل

### الخيار المُختار: Shared Database

- **قاعدة بيانات واحدة** يستخدمها كل من Next.js و ABP
- **نفس الجداول** مع أسماء متوافقة
- **تزامن تلقائي** للبيانات بين التطبيقين

### مقارنة الـ Schemas

#### Next.js (Prisma)
```
Database: event_management (PostgreSQL)
Port: 5432
Tables: users, events, categories, cities, bookings, etc.
IDs: String (CUID)
```

#### ABP Framework
```
Database: EventManagementDb (PostgreSQL)  
Port: 5432
Tables: users, events, categories, cities, bookings, etc.
IDs: Guid (UUID)
```

---

## 🔧 خطوات التكامل

### Phase 11.2: إعداد Shared Database Configuration

#### 1. توحيد اسم قاعدة البيانات

**Option A: استخدام نفس اسم Next.js**
```json
// CS-SY-Events/aspnet-core/src/EventManagement.HttpApi.Host/appsettings.json
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5432;Database=event_management;Username=postgres;Password=postgres123"
  }
}
```

**Option B: استخدام اسم ABP وتحديث Next.js**
```env
# project0.0.2/.env
DATABASE_URL=postgresql://postgres:postgres123@localhost:5432/EventManagementDb
```

**✅ التوصية:** استخدام Option A (اسم Next.js) لأنه المشروع الموجود أولاً.

#### 2. توحيد أسماء الجداول

ABP يستخدم أسماء جداول افتراضية، نحتاج مطابقتها مع Prisma:

```csharp
// EventManagementDbContext.cs - تكوين أسماء الجداول
builder.Entity<User>(b =>
{
    b.ToTable("users"); // ✅ متطابق مع Prisma
});

builder.Entity<Event>(b =>
{
    b.ToTable("events"); // ✅ متطابق
});

builder.Entity<Category>(b =>
{
    b.ToTable("categories"); // ✅ متطابق
});

builder.Entity<City>(b =>
{
    b.ToTable("cities"); // ✅ متطابق
});

builder.Entity<Booking>(b =>
{
    b.ToTable("bookings"); // ✅ متطابق
});
```

#### 3. توحيد نوع الـ IDs

**المشكلة:** 
- Prisma يستخدم `String (CUID)`: `ckf3j4k5l0000abc123`
- ABP يستخدم `Guid (UUID)`: `123e4567-e89b-12d3-a456-426614174000`

**الحل:**

**Option A: تحويل ABP لاستخدام String IDs**
```csharp
// تغيير جميع Entities من Guid إلى string
public class Event : FullAuditedAggregateRoot<string>
{
    // ...
}
```

**Option B: تحويل Next.js لاستخدام UUID** ✅ (الأفضل)
```prisma
// schema.prisma
model User {
  id String @id @default(uuid()) // بدلاً من cuid()
  // ...
}
```

**Option C: Migration تدريجي**
- إنشاء جداول جديدة بـ UUID
- Migration التدريجي من CUID إلى UUID
- الاحتفاظ بـ mapping table

---

### Phase 11.3: إنشاء API Integration Layer

#### 1. Next.js API يستدعي ABP

```typescript
// project0.0.2/lib/abp-client.ts
import axios from 'axios';

const abpClient = axios.create({
  baseURL: process.env.ABP_API_URL || 'https://localhost:44388/api',
  headers: {
    'Content-Type': 'application/json',
  },
});

// مثال: استدعاء Events من ABP
export async function getEventsFromABP() {
  const response = await abpClient.get('/app/event');
  return response.data.items;
}

export async function createEventInABP(eventData: any) {
  const response = await abpClient.post('/app/event', eventData);
  return response.data;
}
```

#### 2. ABP يقرأ من نفس قاعدة البيانات التي كتب فيها Next.js

```csharp
// لا حاجة لـ Integration Layer - ABP سيقرأ مباشرة من الجداول
// فقط نحتاج التأكد من توافق الـ schema
```

#### 3. Shared Authentication (اختياري)

```typescript
// Next.js يستخدم ABP للـ Authentication
export async function loginWithABP(username: string, password: string) {
  const response = await abpClient.post('/connect/token', {
    grant_type: 'password',
    client_id: 'EventManagement_App',
    username,
    password,
    scope: 'offline_access EventManagement',
  });
  
  return response.data.access_token;
}
```

---

### Phase 11.4: Data Migration

#### 1. تحليل البيانات الموجودة

```sql
-- فحص البيانات الحالية في Next.js
SELECT 
  (SELECT COUNT(*) FROM users) as users_count,
  (SELECT COUNT(*) FROM events) as events_count,
  (SELECT COUNT(*) FROM categories) as categories_count,
  (SELECT COUNT(*) FROM cities) as cities_count,
  (SELECT COUNT(*) FROM bookings) as bookings_count;
```

#### 2. Migration Script

```sql
-- CS-SY-Events/docs/migration-script.sql

-- 1. Backup البيانات الحالية
CREATE TABLE users_backup AS SELECT * FROM users;
CREATE TABLE events_backup AS SELECT * FROM events;

-- 2. تحويل IDs من CUID إلى UUID (إذا لزم)
-- هذا يتطلب إنشاء mapping table
CREATE TABLE id_mapping (
  old_id VARCHAR(255) PRIMARY KEY,
  new_id UUID NOT NULL
);

-- 3. إضافة أعمدة ABP الإضافية
ALTER TABLE users ADD COLUMN IF NOT EXISTS ExtraProperties JSONB;
ALTER TABLE users ADD COLUMN IF NOT EXISTS ConcurrencyStamp VARCHAR(40);
ALTER TABLE users ADD COLUMN IF NOT EXISTS CreationTime TIMESTAMP;
ALTER TABLE users ADD COLUMN IF NOT EXISTS CreatorId UUID;
ALTER TABLE users ADD COLUMN IF NOT EXISTS LastModificationTime TIMESTAMP;
ALTER TABLE users ADD COLUMN IF NOT EXISTS LastModifierId UUID;
ALTER TABLE users ADD COLUMN IF NOT EXISTS DeletionTime TIMESTAMP;
ALTER TABLE users ADD COLUMN IF NOT EXISTS DeleterId UUID;
ALTER TABLE users ADD COLUMN IF NOT EXISTS IsDeleted BOOLEAN DEFAULT FALSE;

-- 4. تحديث البيانات الموجودة
UPDATE users SET 
  CreationTime = createdAt,
  LastModificationTime = updatedAt,
  IsDeleted = FALSE
WHERE CreationTime IS NULL;

-- 5. إعادة تسمية الأعمدة (إذا لزم)
-- Prisma: createdAt -> ABP: CreationTime
-- لكن يمكن تكوين ABP لاستخدام أسماء Prisma
```

#### 3. EF Core Configuration للتوافق مع Prisma

```csharp
// EventManagementDbContextModelCreatingExtensions.cs

builder.Entity<User>(b =>
{
    b.ToTable("users");
    
    // تكوين أسماء الأعمدة لتطابق Prisma
    b.Property(x => x.CreationTime)
        .HasColumnName("createdAt"); // استخدام اسم Prisma
    
    b.Property(x => x.LastModificationTime)
        .HasColumnName("updatedAt");
    
    b.Property(x => x.PasswordHash)
        .HasColumnName("password"); // Prisma: password, ABP: PasswordHash
    
    // ABP Audit Properties - إضافة كأعمدة جديدة
    b.Property(x => x.CreatorId).HasColumnName("CreatorId");
    b.Property(x => x.LastModifierId).HasColumnName("LastModifierId");
    b.Property(x => x.IsDeleted).HasColumnName("IsDeleted").HasDefaultValue(false);
    b.Property(x => x.DeleterId).HasColumnName("DeleterId");
    b.Property(x => x.DeletionTime).HasColumnName("DeletionTime");
});
```

---

### Phase 11.5: اختبار التكامل

#### 1. Test Scenario 1: إنشاء Event من Next.js

```typescript
// project0.0.2/app/api/events/create/route.ts
import { prisma } from '@/lib/prisma';

export async function POST(request: Request) {
  const eventData = await request.json();
  
  // إنشاء Event عبر Prisma
  const event = await prisma.event.create({
    data: {
      id: crypto.randomUUID(), // UUID بدلاً من CUID
      title: eventData.title,
      description: eventData.description,
      startDate: new Date(eventData.startDate),
      endDate: new Date(eventData.endDate),
      location: eventData.location,
      categoryId: eventData.categoryId,
      cityId: eventData.cityId,
      organizerId: eventData.organizerId,
    },
  });
  
  return Response.json(event);
}
```

**التحقق من ABP:**
```bash
# استدعاء ABP API للحصول على Events
curl https://localhost:44388/api/app/event
# يجب أن يظهر الـ Event الذي أُنشئ من Next.js
```

#### 2. Test Scenario 2: إنشاء Event من ABP

```bash
# إنشاء Event عبر ABP API
curl -X POST https://localhost:44388/api/app/event \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Test Event from ABP",
    "description": "Testing integration",
    "startDate": "2025-11-01T10:00:00Z",
    "endDate": "2025-11-01T12:00:00Z",
    "location": "Damascus",
    "categoryId": "...",
    "cityId": "..."
  }'
```

**التحقق من Next.js:**
```typescript
// يجب أن يظهر الـ Event في Next.js
const events = await prisma.event.findMany();
console.log(events); // يتضمن Event من ABP
```

#### 3. Test Scenario 3: Real-time Sync

```typescript
// Next.js - WebSocket للإشعارات الفورية
import { io } from 'socket.io-client';

const socket = io('https://localhost:44388');

socket.on('event:created', (event) => {
  console.log('New event from ABP:', event);
  // تحديث UI
});

socket.on('event:updated', (event) => {
  console.log('Event updated from ABP:', event);
  // تحديث UI
});
```

---

### Phase 11.6: التوثيق

#### Architecture Diagram

```
┌─────────────────┐         ┌──────────────────┐
│   Next.js App   │         │   ABP Angular    │
│  (Frontend 1)   │         │  (Frontend 2)    │
└────────┬────────┘         └────────┬─────────┘
         │                           │
         │ API Calls                 │ API Calls
         │                           │
         ▼                           ▼
┌─────────────────┐         ┌──────────────────┐
│  Next.js API    │         │  ABP Backend     │
│  Routes         │◄───────►│  (C# + EF Core)  │
└────────┬────────┘         └────────┬─────────┘
         │                           │
         │ Prisma Client             │ EF Core
         │                           │
         ▼                           ▼
    ┌────────────────────────────────┐
    │  PostgreSQL                    │
    │  event_management Database     │
    │  (Shared Database)             │
    └────────────────────────────────┘
```

#### API Endpoints Mapping

| Next.js Route | ABP Endpoint | Purpose |
|--------------|--------------|---------|
| `/api/events` | `/api/app/event` | CRUD Events |
| `/api/bookings` | `/api/app/booking` | CRUD Bookings |
| `/api/categories` | `/api/app/category` | CRUD Categories |
| `/api/users` | `/api/app/user` | User Management |

#### Environment Variables

**Next.js (.env)**
```env
# Database - نفس قاعدة ABP
DATABASE_URL=postgresql://postgres:postgres123@localhost:5432/event_management

# ABP API Integration (optional)
ABP_API_URL=https://localhost:44388/api
ABP_CLIENT_ID=EventManagement_App
ABP_CLIENT_SECRET=...
```

**ABP (appsettings.json)**
```json
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5432;Database=event_management;Username=postgres;Password=postgres123"
  },
  "App": {
    "AllowedOrigins": "http://localhost:3000,http://localhost:4200"
  }
}
```

---

## ⚠️ التحديات والحلول

### 1. اختلاف نوع IDs

**التحدي:** Prisma (CUID) vs ABP (UUID)

**الحل:**
- ✅ استخدام UUID في كل من Prisma و ABP
- Migration تدريجي للبيانات الموجودة

### 2. Audit Columns

**التحدي:** ABP يضيف أعمدة audit إضافية

**الحل:**
```sql
-- إضافة الأعمدة للجداول الموجودة
ALTER TABLE users ADD COLUMN CreatorId UUID;
ALTER TABLE users ADD COLUMN LastModifierId UUID;
ALTER TABLE users ADD COLUMN IsDeleted BOOLEAN DEFAULT FALSE;
```

### 3. Authentication

**التحدي:** نظامين authentication منفصلين

**الحل:**
- **Option A:** استخدام ABP Identity كـ single source
- **Option B:** Sync users بين النظامين
- **Option C:** SSO/OAuth integration

### 4. Soft Delete

**التحدي:** ABP يستخدم soft delete (IsDeleted)

**الحل:**
```prisma
// إضافة soft delete لـ Prisma
model User {
  // ... existing fields
  isDeleted Boolean @default(false)
  deletedAt DateTime?
}
```

---

## 🚀 خطة التنفيذ الموصى بها

### أسبوع 1: إعداد البنية التحتية
1. ✅ توحيد أسماء قواعد البيانات
2. ✅ تحديث Prisma Schema لاستخدام UUID
3. ✅ إضافة ABP audit columns

### أسبوع 2: Migration البيانات
1. Backup البيانات الحالية
2. تشغيل Migration Scripts
3. اختبار Data Integrity

### أسبوع 3: Integration Layer
1. إنشاء Next.js ABP Client
2. تكوين CORS في ABP
3. اختبار API calls

### أسبوع 4: Testing & Documentation
1. E2E Testing
2. Performance Testing
3. توثيق كامل

---

## 📝 Checklist

### Database Setup
- [ ] توحيد اسم قاعدة البيانات
- [ ] تحديث Prisma لاستخدام UUID
- [ ] إضافة ABP audit columns
- [ ] Migration البيانات الموجودة
- [ ] اختبار Data Integrity

### API Integration
- [ ] إنشاء ABP Client في Next.js
- [ ] تكوين CORS في ABP
- [ ] اختبار CRUD operations
- [ ] إضافة Error Handling
- [ ] إضافة Logging

### Authentication
- [ ] تحديد استراتيجية Auth
- [ ] تطبيق User Sync (إذا لزم)
- [ ] اختبار Login/Logout
- [ ] اختبار Permissions

### Testing
- [ ] Unit Tests
- [ ] Integration Tests
- [ ] E2E Tests
- [ ] Performance Tests
- [ ] Security Tests

### Documentation
- [ ] API Documentation
- [ ] Database Schema
- [ ] Deployment Guide
- [ ] Troubleshooting Guide

---

## 🔗 المراجع

- [ABP Framework Docs](https://docs.abp.io)
- [Prisma Docs](https://www.prisma.io/docs)
- [PostgreSQL UUID](https://www.postgresql.org/docs/current/datatype-uuid.html)
- [Next.js API Routes](https://nextjs.org/docs/api-routes/introduction)

---

**آخر تحديث:** 13 أكتوبر 2025  
**الحالة:** جاهز للتنفيذ  
**النموذج:** Claude Sonnet 4.5

