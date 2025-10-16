# 🎨 Home Slider - دليل شامل

## نظرة عامة

السلايدر الرئيسي هو أول ميزة تفاعلية في الصفحة الرئيسية للمنصة. يعرض الفعاليات بطريقة جذابة وتفاعلية.

---

## ⚡ البدء السريع

### 1. التشغيل
```powershell
# Backend
cd CS-SY-Events\aspnet-core
dotnet run --project src\EventManagement.HttpApi.Host

# Frontend (نافذة جديدة)
cd CS-SY-Events\angular
npm start
```

### 2. إضافة بيانات
افتح Swagger: https://localhost:44388/swagger

```json
POST /api/app/home-slider
{
  "displayOrder": 1,
  "type": 1,
  "isActive": true,
  "title": "أحدث الفعاليات",
  "imageUrl": "https://picsum.photos/1200/500?random=1"
}
```

### 3. المشاهدة
افتح: http://localhost:4200/home

---

## 📐 البنية

### Backend API
```
/api/app/home-slider
├── GET    /                          → List all (Admin)
├── POST   /                          → Create (Admin)
├── GET    /{id}                      → Get one (Admin)
├── PUT    /{id}                      → Update (Admin)
├── DELETE /{id}                      → Delete (Admin)
├── GET    /active-slider-items       → Public ⭐
├── POST   /reorder                   → Reorder (Admin)
└── /settings
    ├── GET  /                        → Get settings (Public)
    └── PUT  /                        → Update settings (Admin)
```

### Frontend Structure
```
angular/src/app/
├── home/                    → Public Home
│   ├── home.module.ts
│   ├── home-routing.module.ts
│   └── home/
│       ├── home.component.ts
│       ├── home.component.html
│       └── home.component.scss
└── proxy/
    └── home-slider/         → API Integration
        ├── models.ts
        ├── home-slider.service.ts
        └── index.ts
```

---

## 🎨 أنواع السلايدر

### 1. Latest (أحدث)
```typescript
{
  type: SliderItemType.Latest,  // = 1
  displayOrder: 1,
  isActive: true
}
```
يعرض تلقائياً أحدث فعالية معتمدة.

### 2. Popular (الأكثر شعبية)
```typescript
{
  type: SliderItemType.Popular,  // = 2
  displayOrder: 2,
  isActive: true
}
```
يعرض الفعالية بأكبر عدد حجوزات.

### 3. Custom (مخصص)
```typescript
{
  type: SliderItemType.Custom,  // = 3
  customEventId: 'event-guid-here',
  displayOrder: 3,
  isActive: true
}
```
يعرض فعالية محددة يدوياً.

---

## ⚙️ الإعدادات

### عدد العناصر
```typescript
// يمكن تحديد عدد العناصر المعروضة (2-6)
{
  sliderItemsCount: 3,  // default
  autoApproveEvents: false
}
```

### التحديث
```typescript
PUT /api/app/home-slider/settings
{
  "sliderItemsCount": 4,
  "autoApproveEvents": false
}
```

---

## 🎭 المميزات

### ✅ مُنفذ
- Bootstrap 5 Carousel
- Auto-slide (5 ثوانٍ)
- Manual controls
- Indicators (dots)
- RTL Support
- Responsive Design
- Loading state
- Empty state
- Error handling
- Image fallback

### ⏳ قادم (Admin Panel)
- CRUD Interface
- Drag & Drop
- Image upload
- Preview mode
- Bulk operations

---

## 🔒 الأمان والصلاحيات

### Public Endpoints
```csharp
[AllowAnonymous]
Task<List<HomeSliderItemDto>> GetActiveSliderItemsAsync();

[AllowAnonymous]
Task<AppSettingsDto> GetSettingsAsync();
```

### Admin Endpoints
```csharp
[Authorize(EventManagementPermissions.Admin.Settings)]
// All CRUD operations
```

---

## 🎨 التخصيص

### الألوان
```scss
// home.component.scss
.carousel-caption {
  background: rgba(0, 0, 0, 0.6);  // شفافية الخلفية
}
```

### الارتفاع
```scss
.carousel-item {
  height: 500px;  // ارتفاع السلايدر
}
```

### المدة
```html
<!-- data-bs-interval بالميلي ثانية -->
<div class="carousel slide" data-bs-interval="5000">
```

---

## 🐛 استكشاف الأخطاء

### لا يوجد سلايدر؟
1. ✅ Backend يعمل؟
2. ✅ Migration مطبق؟
3. ✅ بيانات موجودة؟
4. ✅ Console خالي من الأخطاء؟

### الصور لا تظهر؟
```typescript
// استخدم روابط حقيقية
imageUrl: 'https://picsum.photos/1200/500'
```

### CORS Error؟
```json
// appsettings.json
"CorsOrigins": "http://localhost:4200"
```

---

## 📝 API Examples

### Create
```bash
POST /api/app/home-slider
Content-Type: application/json

{
  "displayOrder": 1,
  "type": 1,
  "isActive": true,
  "title": "أحدث الفعاليات",
  "titleEn": "Latest Events"
}
```

### Update
```bash
PUT /api/app/home-slider/{id}
Content-Type: application/json

{
  "displayOrder": 1,
  "type": 2,
  "isActive": true,
  "title": "الأكثر شعبية"
}
```

### Reorder
```bash
POST /api/app/home-slider/reorder
Content-Type: application/json

[
  "guid-1",
  "guid-2",
  "guid-3"
]
```

---

## 🧪 Testing

### Manual Testing
1. افتح `/home`
2. يجب أن يظهر السلايدر
3. اضغط Next/Previous
4. اضغط على Indicators
5. انتظر Auto-slide

### API Testing
```bash
# Get active items
GET https://localhost:44388/api/app/home-slider/active-slider-items

# Expected: Array of slider items
```

---

## 📦 Dependencies

### Backend
- ABP Framework 9.3.5
- Entity Framework Core 9.0
- AutoMapper

### Frontend
- Angular 17+
- Bootstrap 5
- RxJS

---

## 📚 الملفات ذات الصلة

| الملف | الغرض |
|-------|-------|
| `HomeSliderItem.cs` | Domain Entity |
| `HomeSliderAppService.cs` | Business Logic |
| `home.component.ts` | UI Component |
| `home-slider.service.ts` | API Client |

---

## 🔄 دورة الحياة

```
1. User opens /home
   ↓
2. HomeComponent.ngOnInit()
   ↓
3. sliderService.getActiveSliderItems()
   ↓
4. HTTP GET → /api/app/home-slider/active-slider-items
   ↓
5. Backend: GetActiveSliderItemsAsync()
   ↓
6. Database query + Business logic
   ↓
7. Return HomeSliderItemDto[]
   ↓
8. Angular displays in Carousel
```

---

## 💡 نصائح

### الأداء
- استخدم CDN للصور
- قلل حجم الصور
- استخدم lazy loading

### UX
- استخدم صور عالية الجودة
- اكتب عناوين واضحة
- تأكد من RTL للعربية

### SEO
- أضف alt text للصور
- استخدم semantic HTML
- أضف structured data

---

## 📞 الدعم

مشكلة؟ راجع:
1. `QUICK-START-SLIDER.md`
2. `SLIDER-IMPLEMENTATION.md`
3. Swagger Documentation
4. Console Errors (F12)

---

**Happy Coding! 🚀**

