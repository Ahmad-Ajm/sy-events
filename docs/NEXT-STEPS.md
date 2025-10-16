# 🎯 الخطوات القادمة

## ✅ تم الآن: السلايدر الرئيسي (Home Slider)

**النسبة**: 100% مكتمل

- ✅ Backend API (8 endpoints)
- ✅ Database Migration
- ✅ Frontend Public Component
- ✅ Bootstrap Carousel Integration
- ✅ RTL Support

---

## 📋 المهام القادمة (بالترتيب)

### 1. Admin Panel للسلايدر ⭐ (أولوية قصوى)

**المدة المتوقعة**: 1-2 ساعة  
**الأهمية**: عالية جداً

#### الملفات المطلوبة:
```
angular/src/app/admin/home-slider/
├── home-slider.module.ts
├── home-slider-routing.module.ts
└── slider-management/
    ├── slider-management.component.ts
    ├── slider-management.component.html
    └── slider-management.component.scss
```

#### الميزات:
- [ ] جدول عرض عناصر السلايدر
- [ ] إضافة/تعديل/حذف عنصر
- [ ] تفعيل/تعطيل عنصر
- [ ] تحديد النوع (Latest/Popular/Custom)
- [ ] اختيار فعالية مخصصة
- [ ] تغيير عدد العناصر (2-6)
- [ ] Drag & Drop Reordering (اختياري)

#### API Endpoints الموجودة:
- ✅ `GET /api/app/home-slider` - List all
- ✅ `POST /api/app/home-slider` - Create
- ✅ `PUT /api/app/home-slider/{id}` - Update
- ✅ `DELETE /api/app/home-slider/{id}` - Delete
- ✅ `POST /api/app/home-slider/reorder` - Reorder
- ✅ `GET /api/app/home-slider/settings` - Get settings
- ✅ `PUT /api/app/home-slider/settings` - Update settings

---

### 2. Featured Boxes (المربعات المميزة)

**المدة المتوقعة**: 1-2 ساعة  
**الأهمية**: متوسطة

#### الوصف:
3 مربعات أسفل السلايدر، كل مربع يمكن تخصيصه.

#### Backend:
```csharp
public class FeaturedBox : Entity<Guid>
{
    public int Position { get; set; } // 1, 2, 3
    public BoxType Type { get; set; } // Latest, Popular, Custom
    public Guid? CustomEventId { get; set; }
    public string Title { get; set; }
    public string TitleEn { get; set; }
    public string ImageUrl { get; set; }
    public bool IsActive { get; set; }
}

public enum BoxType
{
    Latest = 1,
    Popular = 2,
    Custom = 3
}
```

#### Frontend:
- Public Component: عرض المربعات في Home Page
- Admin Component: إدارة المربعات

---

### 3. Calendar View (صفحة التقويم)

**المدة المتوقعة**: 3-4 ساعات  
**الأهمية**: عالية

#### الميزات:
- [ ] عرض تقويم شهري
- [ ] الفعاليات بألوان حسب الحالة:
  - 🟢 **أخضر**: حضرت (Attended)
  - 🔴 **أحمر**: تابعت وتغيبت (Followed but Missed)
  - 🟡 **أصفر**: انقضت ولم أتابع (Passed, Not Followed)
  - 🔵 **أزرق**: قادمة ولم أتابع (Upcoming, Not Followed)
  - 🟣 **بنفسجي**: قادمة وأتابع (Upcoming, Following)
- [ ] Color Legend
- [ ] Event details popup
- [ ] Navigation (السابق/التالي)

#### التقنيات المقترحة:
- FullCalendar.io
- أو Angular Calendar
- أو Custom implementation

---

### 4. نظام التسجيل المحسّن

**المدة المتوقعة**: 2-3 ساعات  
**الأهمية**: عالية

#### الميزات:
- [ ] Viewer-only registration
- [ ] Profile page
- [ ] Account customization
- [ ] Account deletion
- [ ] Complete profile data

---

### 5. Organizer Upgrade Flow

**المدة المتوقعة**: 2-3 ساعات  
**الأهمية**: عالية

#### السيناريو:
1. User مسجل كـ Viewer
2. يضغط على "إضافة فعالية"
3. رسالة: "هل تريد أن تصبح منظم؟"
4. إذا وافق → يتحول إلى Organizer
5. ينتقل إلى صفحة إضافة الفعالية (3 خطوات)

#### الخطوات الثلاث:
- **Step 1**: Basic Info (Title, Description, Category, City)
- **Step 2**: Details (Date, Location, Capacity, Price)
- **Step 3**: Media & Publishing (Images, Status)

---

### 6. نظام الموافقة على الفعاليات

**المدة المتوقعة**: 2-3 ساعات  
**الأهمية**: عالية

#### Admin Interface:
- [ ] قائمة الفعاليات المعلقة (Pending)
- [ ] زر "موافقة" لكل فعالية
- [ ] زر "الموافقة على الجميع" (Bulk Approve)
- [ ] Checkbox: "الموافقة التلقائية على جميع الفعاليات المستقبلية"
- [ ] إشعار للمنظم عند الموافقة/الرفض

---

### 7. نظام الحجز المحسّن

**المدة المتوقعة**: 2-3 ساعات  
**الأهمية**: متوسطة-عالية

#### الميزات:
- [ ] زر "متابعة الفعالية" للزوار (يقود للتسجيل)
- [ ] زر "متابعة الفعالية" للمسجلين (حجز مباشر)
- [ ] تأكيد الحجز
- [ ] إلغاء الحجز
- [ ] QR Code للحجز
- [ ] تذكير قبل الفعالية

---

### 8. Notifications System

**المدة المتوقعة**: 3-4 ساعات  
**الأهمية**: متوسطة

#### الأنواع:
- [ ] Email Notifications
- [ ] In-App Notifications
- [ ] Event reminders (1h, 24h, 72h, 1 week)
- [ ] Approval/Rejection notifications
- [ ] Booking confirmations

---

### 9. Reports & Analytics

**المدة المتوقعة**: 2-3 ساعات  
**الأهمية**: منخفضة-متوسطة

#### التقارير:
- [ ] عدد الفعاليات حسب الفئة
- [ ] عدد الحجوزات حسب الفعالية
- [ ] المستخدمين الأكثر نشاطاً
- [ ] الفعاليات الأكثر شعبية
- [ ] CSV Export

---

### 10. إعدادات إضافية

**المدة المتوقعة**: 2-3 ساعات  
**الأهمية**: منخفضة

#### الميزات:
- [ ] reCAPTCHA Integration
- [ ] Google Maps / OpenStreetMap
- [ ] SMTP Configuration
- [ ] Privacy Policy Editor (Rich Text)
- [ ] Social Media Links
- [ ] Site Logo & Favicon

---

## 🎨 تحسينات UI/UX (اختيارية)

### Public Site
- [ ] Animations & Transitions
- [ ] Loading Skeletons
- [ ] Better Error Messages
- [ ] Toast Notifications
- [ ] Dark Mode Support

### Admin Panel
- [ ] Dashboard with Statistics
- [ ] Quick Actions
- [ ] Better Table Sorting/Filtering
- [ ] Bulk Operations
- [ ] Export to Excel/PDF

---

## 📊 Progress Tracker

| المهمة | الأولوية | المدة | الحالة |
|--------|----------|-------|--------|
| Admin Panel - Slider | ⭐⭐⭐ | 1-2h | 🔜 القادم |
| Featured Boxes | ⭐⭐ | 1-2h | ⏳ انتظار |
| Calendar View | ⭐⭐⭐ | 3-4h | ⏳ انتظار |
| Registration Enhancement | ⭐⭐⭐ | 2-3h | ⏳ انتظار |
| Organizer Upgrade | ⭐⭐⭐ | 2-3h | ⏳ انتظار |
| Event Approval System | ⭐⭐⭐ | 2-3h | ⏳ انتظار |
| Booking Enhancement | ⭐⭐ | 2-3h | ⏳ انتظار |
| Notifications | ⭐⭐ | 3-4h | ⏳ انتظار |
| Reports | ⭐ | 2-3h | ⏳ انتظار |
| Additional Settings | ⭐ | 2-3h | ⏳ انتظار |

**إجمالي الوقت المتبقي**: 20-30 ساعة (~1 أسبوع بدوام كامل)

---

## 🚀 خطة العمل الموصى بها

### Week 1: Core Features
1. ✅ **يوم 1**: السلايدر (مكتمل)
2. **يوم 2**: Admin Panel + Featured Boxes
3. **يوم 3**: Calendar View
4. **يوم 4**: Registration + Organizer Flow
5. **يوم 5**: Event Approval + Booking

### Week 2: Enhancements
6. **يوم 6**: Notifications System
7. **يوم 7**: Reports & Analytics
8. **يوم 8**: Additional Settings
9. **يوم 9**: UI/UX Polish
10. **يوم 10**: Testing & Deployment

---

## 💡 نصائح التنفيذ

### للسرعة:
1. استخدم ABP Suite لتوليد CRUD (إذا كان متاحاً)
2. استخدم Bootstrap components الجاهزة
3. استخدم مكتبات جاهزة (FullCalendar, etc.)

### للجودة:
1. اكتب tests للميزات الحرجة
2. راجع التوثيق بعد كل ميزة
3. تأكد من RTL support
4. تأكد من Responsive design

### للصيانة:
1. استخدم تعليقات واضحة
2. التزم بـ naming conventions
3. وثّق الـ API endpoints
4. اكتب README لكل module

---

**جاهز للبدء؟ ابدأ بـ Admin Panel للسلايدر! 🎯**

