# 📚 أمثلة الكود - Event Management Platform

هذا المجلد يحتوي على أمثلة كاملة وجاهزة للاستخدام لجميع مراحل المشروع.

---

## 📁 هيكل المجلد

```
examples/
├── README.md                          # هذا الملف
├── phase2-domain/                     # Phase 2 - Domain Entities ✅
│   ├── User.cs
│   ├── Event.cs
│   ├── Category.cs
│   ├── City.cs
│   ├── Booking.cs
│   └── Enums.cs
├── phase3-ef-config/                  # Phase 3 - EF Core Configuration (قريباً)
├── phase4-application/                # Phase 4 - Application Services (قريباً)
├── phase5-permissions/                # Phase 5 - Permissions (قريباً)
└── phase7-angular/                    # Phase 7 - Angular Components (قريباً)
```

---

## Phase 2: Domain Entities ✅

### الملفات المتوفرة

#### 1. `Enums.cs`
**الموقع:** `aspnet-core/src/EventManagement.Domain.Shared/Enums.cs`

يحتوي على:
- `UserRole` - أدوار المستخدمين (Admin, Organizer, Editor, Support, Viewer)
- `EventStatus` - حالات الفعاليات (Draft, Pending, Approved, Rejected, Hidden)
- `BookingStatus` - حالات الحجوزات (Confirmed, Cancelled, Attended, NoShow)
- `ReminderTime` - أوقات التذكير (OneHour, TwentyFourHours, SeventyTwoHours, OneWeek)

#### 2. `User.cs`
**الموقع:** `aspnet-core/src/EventManagement.Domain/Users/User.cs`

الـ Entity الخاص بالمستخدمين، يحتوي على:
- **Properties:** Email, Name, PasswordHash, Phone, Profession, CityId, Interests, Reason, Role
- **Navigation Properties:** City, OrganizedEvents, Bookings
- **Domain Methods:** 
  - `ChangeRole(UserRole newRole)`
  - `UpdateProfile(string name, string phone, string profession)`
  - `SetCity(Guid? cityId)`
  - `CanOrganizeEvents()`
  - `CanApproveEvents()`

#### 3. `Event.cs`
**الموقع:** `aspnet-core/src/EventManagement.Domain/Events/Event.cs`

الـ Entity الخاص بالفعاليات، يحتوي على:
- **Properties:** Title, TitleEn, Description, DescriptionEn, StartDate, EndDate, Location, LocationEn, MaxCapacity, IsApproved, Status, ImageUrl, ThumbnailUrl
- **Foreign Keys:** CategoryId, CityId, OrganizerId
- **Navigation Properties:** Category, City, Organizer, Bookings, Files, SocialShares
- **Domain Methods:**
  - `Approve()` - اعتماد الفعالية
  - `Reject()` - رفض الفعالية
  - `SubmitForApproval()` - إرسال للاعتماد
  - `Publish()` - نشر الفعالية
  - `Hide()` - إخفاء الفعالية
  - `HasAvailableCapacity()` - فحص السعة المتاحة
  - `GetAvailableCapacity()` - الحصول على السعة المتاحة
  - `HasPassed()` - فحص إذا انتهت الفعالية
  - `IsUpcoming()` - فحص إذا كانت قادمة
  - `IsOngoing()` - فحص إذا كانت جارية

#### 4. `Category.cs`
**الموقع:** `aspnet-core/src/EventManagement.Domain/Categories/Category.cs`

الـ Entity الخاص بالتصنيفات، يحتوي على:
- **Properties:** Name, NameEn, Description, DescriptionEn
- **Navigation Properties:** Events
- **Domain Methods:**
  - `UpdateNames(string name, string nameEn)`
  - `UpdateDescriptions(string description, string descriptionEn)`

#### 5. `City.cs`
**الموقع:** `aspnet-core/src/EventManagement.Domain/Cities/City.cs`

الـ Entity الخاص بالمدن، يحتوي على:
- **Properties:** Name, NameEn
- **Navigation Properties:** Events, Users
- **Domain Methods:**
  - `UpdateNames(string name, string nameEn)`

#### 6. `Booking.cs`
**الموقع:** `aspnet-core/src/EventManagement.Domain/Bookings/Booking.cs`

الـ Entity الخاص بالحجوزات، يحتوي على:
- **Properties:** UserId, EventId, Status, ReminderTime, AttendedAt
- **Navigation Properties:** User, Event
- **Domain Methods:**
  - `Cancel()` - إلغاء الحجز
  - `MarkAsAttended()` - تحديد كحاضر
  - `MarkAsNoShow()` - تحديد كغائب
  - `SetReminderTime(ReminderTime reminderTime)`
  - `ShouldSendReminder(DateTime eventStartTime)`

---

## كيفية الاستخدام

### الخطوة 1: إنشاء ABP Solution

أولاً، يجب إنشاء ABP Solution باتباع `QUICK-SETUP.md` أو `MANUAL-SETUP-REQUIRED.md`

### الخطوة 2: نسخ الـ Entities

#### 2.1 نسخ Enums
```bash
# انسخ الملف
cp examples/phase2-domain/Enums.cs aspnet-core/src/EventManagement.Domain.Shared/

# أو انسخ المحتوى يدوياً
```

#### 2.2 نسخ Domain Entities

انسخ الملفات إلى المواقع الصحيحة:

```bash
# User
mkdir -p aspnet-core/src/EventManagement.Domain/Users
cp examples/phase2-domain/User.cs aspnet-core/src/EventManagement.Domain/Users/

# Event
mkdir -p aspnet-core/src/EventManagement.Domain/Events
cp examples/phase2-domain/Event.cs aspnet-core/src/EventManagement.Domain/Events/

# Category
mkdir -p aspnet-core/src/EventManagement.Domain/Categories
cp examples/phase2-domain/Category.cs aspnet-core/src/EventManagement.Domain/Categories/

# City
mkdir -p aspnet-core/src/EventManagement.Domain/Cities
cp examples/phase2-domain/City.cs aspnet-core/src/EventManagement.Domain/Cities/

# Booking
mkdir -p aspnet-core/src/EventManagement.Domain/Bookings
cp examples/phase2-domain/Booking.cs aspnet-core/src/EventManagement.Domain/Bookings/
```

### الخطوة 3: إصلاح References

بعد نسخ الملفات، قد تحتاج لإصلاح بعض الـ using statements:

```csharp
// في كل ملف، تأكد من:
using EventManagement.Enums;        // للـ Enums
using EventManagement.Users;        // للـ User
using EventManagement.Events;       // للـ Event
using EventManagement.Categories;   // للـ Category
using EventManagement.Cities;       // للـ City
using EventManagement.Bookings;     // للـ Booking
```

### الخطوة 4: Build المشروع

```bash
cd aspnet-core
dotnet restore
dotnet build
```

إذا كان هناك أخطاء، راجع الـ using statements والـ namespaces.

### الخطوة 5: الانتقال لـ Phase 3

بعد نجاح الـ build، انتقل لـ Phase 3 (EF Core Configuration & Migrations).

---

## 📝 ملاحظات مهمة

### 1. ABP Base Classes

جميع الـ Entities تستخدم:
- `FullAuditedAggregateRoot<Guid>` - يوفر تلقائياً:
  - `Id` (Guid)
  - `CreationTime` (DateTime)
  - `CreatorId` (Guid?)
  - `LastModificationTime` (DateTime?)
  - `LastModifierId` (Guid?)
  - `IsDeleted` (bool) - Soft Delete
  - `DeletionTime` (DateTime?)
  - `DeleterId` (Guid?)

### 2. Domain-Driven Design Principles

الـ Entities تتبع DDD principles:
- ✅ Protected constructors (EF Core)
- ✅ Public constructors with validation
- ✅ Domain methods (Business logic)
- ✅ No public setters for collections
- ✅ Aggregate roots

### 3. Navigation Properties

استخدمنا `virtual` للـ navigation properties لـ:
- Lazy Loading (إذا فعّلته)
- Proxies (EF Core)
- Unit Testing (Mocking)

### 4. Validation

الـ validation على مستويين:
- **Domain Level:** في الـ Domain Methods (مثل `Approve()`, `Cancel()`)
- **Application Level:** في الـ DTOs (سيأتي في Phase 4)

---

## 🚀 الخطوات التالية

بعد إكمال Phase 2، انتقل لـ:

### Phase 3: EF Core Configuration
1. إنشاء DbContext
2. Configure Entity relationships
3. Add indexes
4. Create migrations
5. Seed data

### Phase 4: Application Layer
1. إنشاء DTOs
2. إنشاء Application Services
3. AutoMapper profiles
4. Input validation

### Phase 5: Permissions
1. Define permissions
2. Permission localization
3. Apply [Authorize] attributes

راجع `PLAN.md` للتفاصيل الكاملة.

---

## 💡 نصائح

### Custom Properties
إذا احتجت لإضافة properties جديدة:
```csharp
public class Event : FullAuditedAggregateRoot<Guid>
{
    // ... existing properties
    
    // Add your custom property
    public string CustomField { get; set; }
}
```

### Custom Domain Methods
أضف business logic في Domain Methods:
```csharp
public void YourCustomMethod()
{
    // Validate
    if (someCondition)
    {
        throw new InvalidOperationException("Error message");
    }
    
    // Apply changes
    SomeProperty = newValue;
}
```

### Relations
لإضافة علاقة جديدة:
```csharp
// في Entity الأول
public Guid RelatedEntityId { get; set; }
public virtual RelatedEntity RelatedEntity { get; set; }

// في Entity الثاني
public virtual ICollection<FirstEntity> FirstEntities { get; set; }
```

---

## 🆘 المساعدة

إذا واجهت مشاكل:
1. راجع `PLAN.md` - Phase 2
2. راجع `docs/getting-started.md`
3. تحقق من ABP Documentation: https://docs.abp.io/en/abp/latest/Domain-Entities

---

**آخر تحديث:** 12 أكتوبر 2025  
**الحالة:** Phase 2 Examples - Complete ✅

