# 📋 سجل الأخطاء - Event Management Platform

---

## خطأ: التقويم يظهر فقط للأدمن - تم الحل ✅

**- التاريخ:** 19-10-2025
**- النسخة:** 1
**- نوع الخطأ:** Authorization / Permission

### المشكلة:
التقويم كان يظهر فقط للمسؤول (Admin)، ولا يظهر للمستخدمين العاديين رغم أنه من المفترض أن يكون لكل مستخدم مسجل تقويمه الخاص.

### السبب الجذري:
في ملف `CS-SY-Events/angular/src/app/route.provider.ts` السطر 83، كان التقويم يتطلب صلاحية `'AbpIdentity.Users'` وهي صلاحية متاحة فقط للمدراء:
```typescript
requiredPolicy: 'AbpIdentity.Users', // صلاحية خاصة بالمدراء
```

### الحل:
تم إزالة `requiredPolicy` من route التقويم، مع الاحتفاظ بـ `authGuard` في `calendar.routes.ts` للتحقق من تسجيل الدخول فقط:
```typescript
{
  path: '/calendar',
  name: '::Menu:MyCalendar',
  iconClass: 'fas fa-calendar',
  order: 50,
  layout: eLayoutType.application,
  // تعليق: متاح لجميع المستخدمين المسجلين - authGuard في routes يكفي
},
```

### التأثير:
- أصبح التقويم متاحاً لجميع المستخدمين المسجلين
- كل مستخدم يمكنه رؤية تقويمه الخاص مع فعالياته بالألوان المناسبة
- الزوار غير المسجلين لا يستطيعون الوصول للتقويم (محمي بـ authGuard)

---

## خطأ: فشل عرض تفاصيل الفعالية (302/500) - تم الحل ✅

**- التاريخ:** 19-10-2025
**- النسخة:** 1
**- نوع الخطأ:** Authorization / NullReferenceException

### المشكلة:
عند محاولة الوصول إلى صفحة تفاصيل فعالية، كان يتم إعادة توجيه المستخدم إلى الصفحة الرئيسية. كشفت أدوات المطور عن استجابة `302 Found` (تتحول إلى `500 Internal Server Error` في Angular) من الـ API عند طلب تفاصيل الفعالية.

### السبب الجذري:
كانت هناك مشكلتان متتاليتان:
1.  **مشكلة الصلاحية:** كانت دالة `GetAsync` في `EventAppService` تتطلب صلاحية (`GetPolicyName` لم تكن `null`)، مما أدى إلى إعادة التوجيه إلى صفحة تسجيل الدخول للمستخدمين غير المسجلين.
2.  **مشكلة NullReferenceException:** بعد حل مشكلة الصلاحية، ظهر خطأ `NullReferenceException` لأن `GetAsync` لم تكن تقوم بتحميل `Bookings` المرتبطة بالفعالية، وهو ما تحتاجه دالة `GetAvailableCapacity` عند عمل Mapping إلى `EventDto`.

### الحل:
تم تنفيذ حل من خطوتين في ملف `CS-SY-Events/aspnet-core/src/EventManagement.Application/EventManagementAppService.cs`:
1.  تم تعديل `GetPolicyName = null;` في constructor للسماح بالوصول العام لتفاصيل الفعالية.
2.  تم عمل `override` لدالة `GetAsync` لتستخدم `Repository.WithDetailsAsync(...)` وتضمن تحميل `Bookings` مع بيانات الفعالية.

### التأثير:
- أصبح بإمكان الزوار الآن عرض تفاصيل الفعاليات بنجاح.
- تم حل مشكلة التنقل التي كانت تعيد المستخدم إلى الصفحة الرئيسية.

---

## تحذيرات CS8618 في Event.cs - ليست مشكلة حرجة ⚠️

**- التاريخ:** 19-10-2025
**- النسخة:** 68
**- نوع الخطأ:** Compiler Warnings (Nullable Reference Types)

### المشكلة:
20 تحذير من نوع `CS8618: Non-nullable property must contain a non-null value when exiting constructor` في ملف `Event.cs`.

### السبب:
في C# 8.0+، عندما تُعرف property كـ `string` (بدون `?`)، يتوقع المترجم أن تُعطى قيمة في الـ constructor. الكود يحتوي على:
- `protected Event()` فارغ (يستخدمه EF Core)
- بعض properties مثل `TitleEn`, `ImageUrl`, `LocationEn` لا تُعطى قيم في الـ constructor العام

### التقييم:
**لا تؤثر على العمل الحالي.** هذه تحذيرات compile-time فقط وليست أخطاء runtime. التطبيق يعمل بشكل طبيعي.

### التوصية (اختياري):
- جعل الـ properties التي قد تكون `null` قابلة للـ null: `string?`
- أو إعطاء قيم افتراضية في الـ constructor
- أو تعطيل التحذير على مستوى المشروع

---

## ملاحظات عامة

### الأخطاء التي تم حلها:
1. ✅ التقويم محصور بالأدمن
2. ✅ فشل عرض تفاصيل الفعاليات (302/500)
3. ✅ NullReferenceException عند Mapping
4. ✅ مشاكل Swagger conflicts

### التحذيرات الموجودة (غير حرجة):
- ⚠️ 20 تحذير CS8618 في Event.cs (لا تؤثر على العمل)

---

**آخر تحديث:** 19 أكتوبر 2025
