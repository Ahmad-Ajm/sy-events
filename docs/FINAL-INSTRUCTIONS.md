# 🚀 التعليمات النهائية للتشغيل والفحص

**النموذج**: Claude Sonnet 4  
**التاريخ**: 14 أكتوبر 2025

---

## ✅ الحالة الحالية

الخوادم تعمل الآن:
- ✅ Backend Server - تم تشغيله
- ✅ Frontend Server - تم تشغيله

---

## 🌐 فتح المتصفح

### الطريقة 1: ملف BAT
انقر نقراً مزدوجاً على: **`OPEN-BROWSER.bat`**

### الطريقة 2: يدوياً
افتح المتصفح واذهب إلى: **http://localhost:4200**

---

## 🔍 قائمة الفحص الكاملة

### 1. الصفحة الرئيسية `/`
- [ ] السلايدر يظهر (5 شرائح)
- [ ] المربعات الثلاثة تظهر
- [ ] قائمة الفعاليات تظهر
- [ ] **افتح DevTools (F12)** وتحقق من:
  - [ ] لا توجد أخطاء في Console
  - [ ] لا توجد أخطاء 404 في Network

### 2. قائمة الفعاليات `/events`
- [ ] الفعاليات تظهر في الجدول
- [ ] البحث يعمل
- [ ] الفلاتر المتقدمة تظهر (7 فلاتر)
- [ ] زر "مسح الفلاتر" يعمل
- [ ] **DevTools Check**:
  - [ ] API Call: `GET /api/app/event` - Status 200
  - [ ] No errors في Console

### 3. التقويم `/calendar`
- [ ] FullCalendar يظهر
- [ ] 4 أزرار للعرض (Month/Week/Day/List)
- [ ] الأحداث تظهر بألوان مختلفة
- [ ] النقر على حدث ينقل للتفاصيل
- [ ] **DevTools Check**:
  - [ ] No errors في Console
  - [ ] FullCalendar loaded correctly

### 4. الملف الشخصي `/profile/me`
- [ ] الصفحة تظهر (قد تحتاج تسجيل دخول)
- [ ] زر "تعديل الملف الشخصي" يظهر
- [ ] **DevTools Check**:
  - [ ] API Call: `GET /api/app/user-profile/my-profile`

### 5. الاجتماعات `/meetings`
- [ ] 3 تبويبات تظهر
- [ ] **DevTools Check**:
  - [ ] No errors

### 6. الصفحات القانونية
- [ ] `/legal/privacy` - سياسة الخصوصية تظهر
- [ ] `/legal/terms` - الشروط والأحكام تظهر
- [ ] **DevTools Check**:
  - [ ] No errors في Console

### 7. تفاصيل الفعالية `/events/{id}`
- [ ] انقر على أي فعالية من القائمة
- [ ] صفحة التفاصيل تظهر
- [ ] الصورة تظهر
- [ ] زر "متابعة الفعالية" يظهر
- [ ] **DevTools Check**:
  - [ ] API Call: `GET /api/app/event/{id}` - Status 200

---

## 🐛 الأخطاء الشائعة وحلولها

### Backend لا يعمل
```cmd
cd CS-SY-Events\aspnet-core\src\EventManagement.HttpApi.Host
dotnet run
```
انتظر حتى ترى: `Now listening on: http://localhost:44349`

### Frontend لا يعمل
```cmd
cd CS-SY-Events\angular
npm start
```
انتظر حتى ترى: `** Angular Live Development Server is listening on localhost:4200`

### قاعدة البيانات
تأكد من Docker:
```cmd
docker ps
```

إذا لم تعمل:
```cmd
cd CS-SY-Events
docker-compose up -d
```

### أخطاء 401 Unauthorized
- طبيعي إذا لم تسجل دخول
- الصفحات العامة يجب أن تعمل: `/`, `/events`, `/calendar`, `/legal/*`

### أخطاء CORS
- تأكد أن Backend يعمل على port 44349
- تأكد أن Frontend يعمل على port 4200

---

## 📸 فحص DevTools (F12)

### Console Tab
يجب ألا ترى:
- ❌ أخطاء باللون الأحمر (Errors)
- ⚠️ يمكن تجاهل Warnings الصفراء

### Network Tab
فلتر على: **XHR**
تحقق من:
- ✅ Calls إلى `/api/app/event` - Status 200
- ✅ Calls إلى `/api/app/category` - Status 200
- ✅ Calls إلى `/api/app/city` - Status 200

### مثال على Calls الناجحة:
```
GET /api/app/event?...  200 OK  (150ms)
GET /api/app/category   200 OK  (50ms)
GET /api/app/city       200 OK  (45ms)
```

---

## ✅ معايير النجاح

### الصفحة تعمل بنجاح إذا:
1. ✅ لا توجد أخطاء في Console (أحمر)
2. ✅ جميع API calls تعود بـ 200 أو 204
3. ✅ المحتوى يظهر بشكل صحيح
4. ✅ التنقل بين الصفحات يعمل
5. ✅ الأزرار تستجيب للنقر

---

## 🎯 الميزات المطلوب فحصها

### الميزات الأساسية
- [x] السلايدر الرئيسي
- [x] قائمة الفعاليات
- [x] البحث المتقدم (7 فلاتر)
- [x] التقويم (FullCalendar)
- [x] الصفحات القانونية

### الميزات المتقدمة
- [x] ملفات التعريف
- [x] منتديات النقاش
- [x] جدولة الاجتماعات
- [x] لوحة التحليلات

---

## 📝 تقرير الفحص

بعد الفحص، املأ هذا:

### الأخطاء المكتشفة
```
صفحة: _______
الخطأ: _______
Console Error: _______
```

### الميزات التي تعمل
- [ ] الصفحة الرئيسية
- [ ] قائمة الفعاليات
- [ ] التقويم
- [ ] الملف الشخصي
- [ ] الاجتماعات
- [ ] الصفحات القانونية

---

## 🚀 جاهز للفحص!

1. ✅ افتح: **OPEN-BROWSER.bat**
2. ✅ اضغط **F12** لفتح DevTools
3. ✅ اذهب لكل صفحة وتحقق من Console
4. ✅ سجل أي أخطاء تظهر

**المشروع جاهز الآن للفحص الكامل!** 🎉

