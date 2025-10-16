# ⚡ Quick Setup Guide - Manual Steps

بسبب مشاكل في encoding الأحرف في Terminal، يرجى تنفيذ هذه الخطوات يدوياً.

---

## الخطوات المطلوبة

### 1. تثبيت ABP CLI

افتح PowerShell كـ Administrator وانفذ:

```powershell
dotnet tool install -g Volo.Abp.Cli
```

إذا كان مثبت مسبقاً، حدّثه:

```powershell
dotnet tool update -g Volo.Abp.Cli
```

تحقق من التثبيت:

```powershell
abp --version
```

---

### 2. إنشاء ABP Solution

افتح PowerShell في مجلد `CS-SY-Events` وانفذ:

```powershell
abp new EventManagement -t app -u angular -d ef -dbms PostgreSQL --mobile none --pwa
```

هذا الأمر سيستغرق 5-10 دقائق لإنشاء المشروع وتنزيل الـ packages.

---

### 3. تشغيل PostgreSQL

```powershell
docker-compose up -d postgres pgadmin redis
```

تحقق من عمل الـ containers:

```powershell
docker-compose ps
```

---

### 4. تطبيق Database Migrations

```powershell
cd aspnet-core\src\EventManagement.DbMigrator
dotnet run
```

انتظر حتى تكتمل العملية (ستظهر "Seeded successfully").

---

### 5. تشغيل Backend

في terminal جديد:

```powershell
cd aspnet-core\src\EventManagement.HttpApi.Host
dotnet run
```

Backend سيعمل على: **https://localhost:44300**  
Swagger: **https://localhost:44300/swagger**

---

### 6. تشغيل Frontend

في terminal جديد:

```powershell
cd angular
npm install
npm start
```

Frontend سيعمل على: **http://localhost:4200**

---

### 7. تسجيل الدخول

```
Username: admin
Password: 1q2w3E*
```

---

## بعد الإعداد الأولي

### تشغيل يومي سريع

```powershell
# Terminal 1 - Database
docker-compose up -d postgres redis

# Terminal 2 - Backend
cd aspnet-core\src\EventManagement.HttpApi.Host
dotnet watch run

# Terminal 3 - Frontend
cd angular
npm start
```

---

## ملاحظات مهمة

### إذا واجهت مشكلة في CORS

أضف في `Program.cs`:

```csharp
app.UseCors(builder =>
{
    builder
        .WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
});
```

### إذا واجهت مشكلة في Database Connection

تأكد من Connection String في `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5432;Database=EventManagementDb;Username=postgres;Password=postgres123"
  }
}
```

### إذا احتجت لحذف Database وإعادة إنشائه

```powershell
# في PostgreSQL
docker exec -it eventmanagement-postgres psql -U postgres -c "DROP DATABASE IF EXISTS \"EventManagementDb\""
docker exec -it eventmanagement-postgres psql -U postgres -c "CREATE DATABASE \"EventManagementDb\""

# ثم نفذ DbMigrator مرة أخرى
cd aspnet-core\src\EventManagement.DbMigrator
dotnet run
```

---

## للمساعدة

- راجع `README.md` للوثائق الكاملة
- راجع `docs/getting-started.md` للتفاصيل
- راجع `PLAN.md` لخطة المشروع

---

**بعد إكمال هذه الخطوات، سيكون لديك:**

✅ ABP Solution جاهز  
✅ PostgreSQL يعمل  
✅ Backend API يعمل على https://localhost:44300  
✅ Frontend Angular يعمل على http://localhost:4200  
✅ Swagger UI جاهز للاختبار  

**الخطوة التالية:** Phase 2 - إضافة Domain Entities

