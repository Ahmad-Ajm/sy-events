# 🚀 تشغيل المشروع - دليل سريع

## ⚠️ المشاكل التي تم حلها:

### 1. مشكلة API لا يعمل
**السبب:** ConnectionString لم يُقرأ بشكل صحيح من appsettings.json  
**الحل:** تمرير ConnectionString كمتغير بيئة مباشرة

### 2. مشكلة Angular timeout
**السبب:** Angular يحتاج وقت طويل للبناء أول مرة  
**الحل:** انتظار كافٍ + تشغيل في نافذة منفصلة

### 3. مشكلة Terminal encoding في Cursor
**السبب:** مشاكل في encoding الأحرف  
**الحل:** سكريبتات PowerShell منفصلة تعمل خارج Cursor

---

## 🎯 التشغيل السريع

### الطريقة 1: استخدام السكريبت الآلي (موصى به)

1. افتح **PowerShell** (خارج Cursor)
2. انتقل لمجلد المشروع:
   ```powershell
   cd "D:\NBS-Venture\Event-Management-Platform\CS-SY-Events"
   ```

3. شغّل السكريبت:
   ```powershell
   .\run-all.ps1
   ```

4. انتظر 30-60 ثانية

5. افتح المتصفح:
   - Frontend: http://localhost:4200
   - Swagger: https://localhost:44388/swagger
   - Username: `admin`
   - Password: `1q2w3E*`

### لإيقاف المشروع:

```powershell
.\stop-all.ps1
```

---

## 🔧 الطريقة 2: التشغيل اليدوي

إذا فشل السكريبت، نفّذ هذه الأوامر بالترتيب في **PowerShell خارج Cursor**:

### 1. تشغيل Database
```powershell
cd "D:\NBS-Venture\Event-Management-Platform\CS-SY-Events"
docker compose up -d postgres pgadmin redis
```

### 2. تشغيل Backend API (نافذة PowerShell منفصلة)
```powershell
cd "D:\NBS-Venture\Event-Management-Platform\CS-SY-Events\aspnet-core\src\EventManagement.HttpApi.Host"
$env:ConnectionStrings__Default="Host=localhost;Port=5432;Database=EventManagementDb;Username=postgres;Password=postgres123"
$env:ASPNETCORE_URLS="https://localhost:44388"
dotnet run
```

انتظر حتى تظهر رسالة: `Now listening on: https://localhost:44388`

### 3. تشغيل Angular (نافذة PowerShell منفصلة)
```powershell
cd "D:\NBS-Venture\Event-Management-Platform\CS-SY-Events\angular"
ng serve --open
```

انتظر حتى تظهر: `✔ Compiled successfully`

---

## ✅ التحقق من نجاح التشغيل

### في PowerShell:
```powershell
# التحقق من المنافذ
netstat -ano | findstr ":44388"   # API
netstat -ano | findstr ":4200"    # Angular
netstat -ano | findstr ":5432"    # PostgreSQL
```

يجب أن ترى `LISTENING` لكل منفذ.

### في المتصفح:
1. **Swagger:** https://localhost:44388/swagger
   - يجب أن تظهر واجهة Swagger UI
   - يجب أن ترى Endpoints مثل `/api/abp/application-configuration`

2. **Angular:** http://localhost:4200
   - يجب أن تظهر صفحة تسجيل الدخول
   - ثيم LeptonX Side Menu يجب أن يظهر

---

## 🐛 حل المشاكل الشائعة

### مشكلة: "Port is already in use"
```powershell
# إيقاف العملية التي تستخدم المنفذ
$pid = (netstat -ano | findstr ":44388" | findstr "LISTENING" | ForEach-Object {($_ -split '\s+')[-1]})[0]
Stop-Process -Id $pid -Force
```

### مشكلة: "Docker is not running"
1. شغّل Docker Desktop
2. انتظر حتى يكتمل التشغيل
3. أعد تشغيل السكريبت

### مشكلة: "Cannot connect to database"
```powershell
# تحقق من عمل PostgreSQL
docker ps | findstr postgres

# إعادة تشغيل PostgreSQL
docker compose restart postgres
```

### مشكلة: Angular بطيء جداً
- **السبب:** أول بناء يأخذ وقت طويل
- **الحل:** انتظر 2-3 دقائق في أول مرة
- **بديل:** استخدم `ng serve --poll=2000` إذا كان لا يستجيب

### مشكلة: Swagger لا يفتح
```powershell
# تحقق من logs في نافذة API
# ابحث عن أخطاء في الاتصال بقاعدة البيانات
```

---

## 📊 الحالة المتوقعة

بعد التشغيل الناجح:

```
✓ Docker containers تعمل (postgres, pgadmin, redis)
✓ Backend API يستمع على https://localhost:44388
✓ Angular يستمع على http://localhost:4200
✓ Database: EventManagementDb موجودة وتحتوي بيانات
✓ Admin user موجود (admin / 1q2w3E*)
```

---

## 🎯 الخطوة التالية

بعد التشغيل الناجح:
1. سجّل دخول بـ admin / 1q2w3E*
2. استكشف الواجهة
3. افتح Swagger وجرّب APIs
4. انتقل لـ **Phase 2** - إضافة Domain Entities

راجع: `../examples/README.md` للخطوات التالية

---

## 📞 إذا استمرت المشاكل

1. تأكد من تشغيل PowerShell **خارج Cursor**
2. تأكد من Docker Desktop يعمل
3. تأكد من عدم وجود برامج أخرى تستخدم المنافذ 44388، 4200، 5432
4. أعد تشغيل الكمبيوتر إذا لزم الأمر

---

**آخر تحديث:** 12 أكتوبر 2025  
**الحالة:** الحلول مختبرة ✅

---

## 🧪 تحديث الحالة آلياً بعد إنهاء أي مرحلة

بعد إنهاء أي Phase، يمكنك تشغيل الهوك التالي لتحديث الملفات تلقائياً:

```powershell
# مثال: الإشارة إلى اكتمال Phase 2 مع ملاحظة
node hooks/whenPhaseFinish.js --phase phase2 --status completed --notes "Domain layer copied & build green"
```

ما الذي يفعله الهوك؟
- يتحقق من API: `https://localhost:44388/api/abp/application-configuration`
- يتحقق من الواجهة: `http://localhost:4200/`
- يضيف سطراً إلى قسم Automation Log داخل `STATUS.md`
- يحدّث سطر الحالة في عنوان المرحلة داخل `PLAN.md`
- يضيف سِجلاً بصيغة JSONL في `Logs/phase-updates.jsonl`

المتطلبات:
- Node.js مثبت
- API و Angular يعملان (استخدم `simple-run.ps1` أو `run-all.ps1`)

