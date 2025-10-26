# 🛠️ أدوات قاعدة البيانات - دليل الاستخدام

**التاريخ:** 17 أكتوبر 2025  
**الغرض:** توفير أدوات لتسهيل التعامل مع قاعدة بيانات PostgreSQL

---

## 📁 الملفات المتوفرة

### 1. `test-db-connection.ps1`
سكريبت PowerShell لاختبار الاتصال بقاعدة البيانات وعرض معلومات مفيدة.

### 2. `common-db-queries.sql`
مجموعة من الاستعلامات SQL الشائعة والمفيدة.

### 3. `.cursor/mcp_config.json`
ملف إعداد MCP Server للاتصال المباشر بقاعدة البيانات.

---

## 🚀 الاستخدام

### الطريقة 1: سكريبت PowerShell

#### تشغيل بسيط:
```powershell
cd CS-SY-Events
.\scripts\test-db-connection.ps1
```

#### تشغيل مع معاملات مخصصة:
```powershell
.\scripts\test-db-connection.ps1 `
    -DbHost "localhost" `
    -DbPort 5432 `
    -Database "EventManagementDb" `
    -Username "postgres" `
    -Password "postgres123"
```

#### ماذا يفعل السكريبت؟
- ✅ يتحقق من تثبيت `psql`
- ✅ يختبر الاتصال بقاعدة البيانات
- ✅ يعرض إصدار PostgreSQL
- ✅ يعرض قائمة الجداول
- ✅ يعرض الترحيلات المطبقة
- ✅ يعرض إحصائيات البيانات
- ✅ يفحص وجود عمود `Kind` في جدول `Events`

---

### الطريقة 2: استعلامات SQL المباشرة

#### إذا كان لديك `psql` مثبت:

**الاتصال بقاعدة البيانات:**
```bash
psql -h localhost -U postgres -d EventManagementDb
```

**تنفيذ استعلام واحد:**
```bash
psql -h localhost -U postgres -d EventManagementDb -c "SELECT COUNT(*) FROM \"Events\";"
```

**تنفيذ ملف SQL كامل:**
```bash
psql -h localhost -U postgres -d EventManagementDb -f scripts/common-db-queries.sql
```

---

### الطريقة 3: MCP Server (الموصى به للـ AI)

#### الإعداد:

1. تم إنشاء ملف `.cursor/mcp_config.json` تلقائياً
2. أعد تشغيل Cursor IDE
3. سيكون لدى الـ AI القدرة على الاتصال المباشر بقاعدة البيانات

#### الفوائد:
- ✅ الاتصال المباشر بقاعدة البيانات
- ✅ تنفيذ استعلامات SQL من الـ AI مباشرة
- ✅ فحص البيانات الفعلية
- ✅ تشخيص المشاكل بشكل أسرع

---

## 📊 استعلامات شائعة

### فحص الترحيلات المطبقة:
```sql
SELECT "MigrationId", "ProductVersion" 
FROM "__EFMigrationsHistory" 
ORDER BY "MigrationId";
```

### عرض بنية جدول:
```sql
SELECT 
    column_name,
    data_type,
    is_nullable
FROM information_schema.columns 
WHERE table_name = 'Events'
ORDER BY ordinal_position;
```

### إحصائيات البيانات:
```sql
SELECT 
    'Cities' as table_name, COUNT(*) as count FROM "Cities"
UNION ALL
SELECT 'Categories', COUNT(*) FROM "Categories"
UNION ALL
SELECT 'Users', COUNT(*) FROM "Users"
UNION ALL
SELECT 'Events', COUNT(*) FROM "Events";
```

### فحص عمود Kind:
```sql
SELECT 
    column_name,
    data_type,
    is_nullable
FROM information_schema.columns 
WHERE table_name = 'Events' 
  AND column_name = 'Kind';
```

### عرض الفعاليات:
```sql
SELECT 
    "Id",
    "Title",
    "Kind",
    "Status",
    "IsApproved",
    "StartDate"
FROM "Events"
WHERE NOT "IsDeleted"
ORDER BY "StartDate" DESC
LIMIT 10;
```

---

## 🔧 تثبيت الأدوات

### تثبيت PostgreSQL Client Tools

#### Windows:
1. **الطريقة الأولى - مع PostgreSQL كامل:**
   - حمّل من: https://www.postgresql.org/download/windows/
   - اختر مكونات "Command Line Tools" أثناء التثبيت

2. **الطريقة الثانية - Client Tools فقط:**
   ```powershell
   # باستخدام Chocolatey
   choco install postgresql-client
   
   # أو باستخدام Scoop
   scoop install postgresql
   ```

#### Linux:
```bash
# Ubuntu/Debian
sudo apt-get install postgresql-client

# CentOS/RHEL
sudo yum install postgresql
```

#### macOS:
```bash
# باستخدام Homebrew
brew install libpq
```

---

### تثبيت MCP Server

#### متطلبات:
- Node.js (مثبت بالفعل)
- npm (مثبت بالفعل)

#### التثبيت:
```bash
npm install -g @modelcontextprotocol/server-postgres
```

أو سيتم تثبيته تلقائياً عند أول استخدام (npx -y).

---

## 🐛 حل المشاكل

### المشكلة: `psql: command not found`

**الحل:**
1. تحقق من تثبيت PostgreSQL Client
2. أضف مسار `psql` إلى PATH:
   ```powershell
   $env:PATH += ";C:\Program Files\PostgreSQL\16\bin"
   ```

### المشكلة: `FATAL: password authentication failed`

**الحل:**
1. تحقق من كلمة المرور في `appsettings.json`
2. تأكد من أن المستخدم `postgres` موجود
3. تحقق من `pg_hba.conf` لتأكيد السماح بالاتصالات المحلية

### المشكلة: `connection refused`

**الحل:**
1. تأكد من تشغيل PostgreSQL:
   ```powershell
   Get-Service postgresql*
   ```
2. تحقق من المنفذ (5432 افتراضياً):
   ```powershell
   netstat -ano | findstr :5432
   ```

### المشكلة: MCP Server لا يعمل

**الحل:**
1. تحقق من ملف `.cursor/mcp_config.json`
2. تأكد من صحة `POSTGRES_CONNECTION_STRING`
3. أعد تشغيل Cursor IDE
4. تحقق من logs في Cursor

---

## 📚 موارد إضافية

### أدوات واجهة مرئية:

1. **pgAdmin 4** (موصى به)
   - https://www.pgadmin.org/
   - واجهة رسومية شاملة

2. **DBeaver** (مفتوح المصدر)
   - https://dbeaver.io/
   - يدعم قواعد بيانات متعددة

3. **TablePlus** (مدفوع)
   - https://tableplus.com/
   - واجهة أنيقة وسريعة

### امتدادات VS Code:

1. **PostgreSQL** by Chris Kolkman
   ```
   ext install cweijan.vscode-postgresql-client2
   ```

2. **SQLTools**
   ```
   ext install mtxr.sqltools
   ext install mtxr.sqltools-driver-pg
   ```

---

## 💡 نصائح

### للتطوير:
- استخدم MCP Server للفحص السريع
- استخدم `test-db-connection.ps1` للتحقق اليومي
- احفظ الاستعلامات المفيدة في `common-db-queries.sql`

### للإنتاج:
- لا تُخزّن كلمات المرور في الملفات
- استخدم متغيرات البيئة
- قم بعمل نسخ احتياطية منتظمة:
  ```bash
  pg_dump -h localhost -U postgres EventManagementDb > backup_$(date +%Y%m%d).sql
  ```

---

## 🎯 الخلاصة

الآن لديك ثلاث طرق للتعامل مع قاعدة البيانات:

1. ✅ **السكريبت (test-db-connection.ps1)** → للفحص السريع
2. ✅ **الاستعلامات (common-db-queries.sql)** → للعمليات المتقدمة
3. ✅ **MCP Server** → للتفاعل المباشر مع الـ AI

اختر الطريقة المناسبة حسب احتياجك!

---

*تم إنشاء هذا الدليل بواسطة: Claude Sonnet 4.5*  
*التاريخ: 17 أكتوبر 2025*

