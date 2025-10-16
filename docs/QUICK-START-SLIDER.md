# 🚀 دليل التشغيل السريع - السلايدر

## خطوات سريعة للبدء (5 دقائق)

### 1️⃣ تشغيل Backend
```powershell
cd CS-SY-Events\aspnet-core
dotnet run --project src\EventManagement.HttpApi.Host
```
✅ انتظر حتى ترى: `Now listening on: https://localhost:44388`

---

### 2️⃣ تشغيل Frontend
```powershell
# نافذة PowerShell جديدة
cd CS-SY-Events\angular
npm start
```
✅ انتظر حتى يفتح المتصفح تلقائياً على `http://localhost:4200`

---

### 3️⃣ إضافة بيانات تجريبية

افتح Swagger: https://localhost:44388/swagger

**أضف 3 عناصر سلايدر:**

#### عنصر 1: أحدث الفعاليات
```json
POST /api/app/home-slider

{
  "displayOrder": 1,
  "type": 1,
  "isActive": true,
  "title": "أحدث الفعاليات",
  "titleEn": "Latest Events",
  "imageUrl": "https://picsum.photos/1200/500?random=1"
}
```

#### عنصر 2: الأكثر شعبية
```json
POST /api/app/home-slider

{
  "displayOrder": 2,
  "type": 2,
  "isActive": true,
  "title": "الأكثر شعبية",
  "titleEn": "Most Popular",
  "imageUrl": "https://picsum.photos/1200/500?random=2"
}
```

#### عنصر 3: عرض مميز
```json
POST /api/app/home-slider

{
  "displayOrder": 3,
  "type": 1,
  "isActive": true,
  "title": "عرض خاص",
  "titleEn": "Special Offer",
  "imageUrl": "https://picsum.photos/1200/500?random=3"
}
```

---

### 4️⃣ عرض السلايدر

افتح: **http://localhost:4200/home**

🎉 يجب أن ترى السلايدر يعمل!

---

## ⚡ أوامر مختصرة

### Windows PowerShell
```powershell
# تشغيل كل شيء في نافذة واحدة (Background)
cd CS-SY-Events
.\run-all.ps1
```

### إيقاف كل شيء
```powershell
cd CS-SY-Events
.\stop-all.ps1
```

---

## 🔍 تحقق من عمل السلايدر

### ✅ علامات النجاح:
1. السلايدر يظهر في الصفحة الرئيسية
2. يمكن التنقل بين الشرائح
3. الصور تظهر بشكل صحيح
4. النص واضح ومقروء
5. الأزرار تعمل

### ❌ إذا لم يظهر السلايدر:
1. تحقق من Console (F12)
2. تأكد من Backend يعمل: https://localhost:44388/swagger
3. تحقق من وجود بيانات: `GET /api/app/home-slider/active-slider-items`

---

## 🎨 تخصيص السلايدر

### تغيير عدد العناصر (2-6)
```json
PUT /api/app/home-slider/settings

{
  "sliderItemsCount": 4,
  "autoApproveEvents": false
}
```

### تعطيل عنصر
```json
PUT /api/app/home-slider/{id}

{
  "displayOrder": 1,
  "type": 1,
  "isActive": false,  ← تغيير إلى false
  "title": "...",
  ...
}
```

---

## 📋 Checklist سريع

- [ ] PostgreSQL يعمل (Port 5432)
- [ ] Backend يعمل (Port 44388)
- [ ] Frontend يعمل (Port 4200)
- [ ] Migrations مطبقة
- [ ] بيانات تجريبية موجودة
- [ ] السلايدر يظهر في /home

---

## 🆘 حل المشاكل الشائعة

### المشكلة: Backend لا يبدأ
```powershell
# تحقق من PostgreSQL
docker ps | findstr postgres

# إذا لم يكن يعمل:
docker-compose up -d
```

### المشكلة: Migration خطأ
```powershell
cd CS-SY-Events\aspnet-core\src\EventManagement.DbMigrator
dotnet run
```

### المشكلة: CORS Error
تحقق من `appsettings.json`:
```json
"CorsOrigins": "http://localhost:4200"
```

### المشكلة: الصور لا تظهر
استخدم روابط صور حقيقية:
- https://picsum.photos/1200/500
- https://source.unsplash.com/1200x500/?event

---

## 📱 اختبار Mobile

1. افتح Chrome DevTools (F12)
2. اضغط على Device Toolbar (Ctrl+Shift+M)
3. اختر iPhone/iPad
4. تحقق من السلايدر responsive

---

**جاهز؟ ابدأ الآن! 🎯**

```powershell
cd CS-SY-Events\aspnet-core
dotnet run --project src\EventManagement.HttpApi.Host
```

