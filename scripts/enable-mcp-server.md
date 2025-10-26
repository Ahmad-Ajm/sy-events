# 🔌 تفعيل MCP Server للـ PostgreSQL في Cursor

## ❌ المشكلة الحالية

MCP Server غير مُفعّل لأن Cursor يحتاج إعداد يدوي في الإعدادات.

---

## ✅ الحل (خطوات التفعيل اليدوي)

### الطريقة 1: عبر إعدادات Cursor (الموصى به)

1. **افتح إعدادات Cursor:**
   - اضغط `Ctrl + ,` (أو `Cmd + ,` على Mac)
   - أو من القائمة: `File > Preferences > Settings`

2. **ابحث عن "MCP":**
   - في شريط البحث اكتب: `mcp`
   - أو `Model Context Protocol`

3. **أضف PostgreSQL Server:**
   
   في قسم **"Cursor: MCP Servers"**، أضف الإعداد التالي:

   ```json
   {
     "postgres": {
       "command": "npx",
       "args": [
         "-y",
         "@modelcontextprotocol/server-postgres",
         "postgresql://postgres:postgres123@localhost:5432/EventManagementDb"
       ]
     }
   }
   ```

4. **أعد تشغيل Cursor:**
   - `Ctrl + Shift + P` → `Developer: Reload Window`
   - أو أغلق وافتح Cursor مرة أخرى

5. **تحقق من التفعيل:**
   - سترى إشعار في أسفل الشاشة
   - أو يمكنك فحص الـ logs

---

### الطريقة 2: عبر ملف settings.json

1. **افتح ملف الإعدادات:**
   - `Ctrl + Shift + P` → `Preferences: Open User Settings (JSON)`

2. **أضف الإعداد:**
   ```json
   {
     // ... إعداداتك الأخرى ...
     
     "cursor.mcpServers": {
       "postgres": {
         "command": "npx",
         "args": [
           "-y",
           "@modelcontextprotocol/server-postgres",
           "postgresql://postgres:postgres123@localhost:5432/EventManagementDb"
         ]
       }
     }
   }
   ```

3. **احفظ وأعد التشغيل**

---

### الطريقة 3: استخدام psql مباشرة (البديل الأسرع)

إذا كان `psql` مثبت، يمكنك استخدامه مباشرة:

```powershell
# الاتصال
$env:PGPASSWORD="postgres123"
psql -h localhost -U postgres -d EventManagementDb

# أو تنفيذ استعلام مباشر
psql -h localhost -U postgres -d EventManagementDb -c "SELECT * FROM \"Events\" LIMIT 5;"
```

---

## 🧪 التحقق من التفعيل

بعد التفعيل، يجب أن تتمكن من:

### في Cursor Chat:
```
سؤال: "ما هي الجداول الموجودة في قاعدة البيانات؟"

الجواب (سيستخدم MCP):
"الجداول الموجودة:
- Cities
- Categories  
- Users
- Events
- Bookings
- ..."
```

### أو طلب استعلام مباشر:
```
"قم بتنفيذ: SELECT COUNT(*) FROM \"Events\""
```

---

## ⚠️ إذا لم يعمل

### المشاكل الشائعة:

**1. Package غير مثبت:**
```bash
# ثبّت يدوياً
npm install -g @modelcontextprotocol/server-postgres
```

**2. المنفذ 5432 مغلق:**
```powershell
# تحقق من PostgreSQL
Get-Service postgresql*
netstat -ano | findstr :5432
```

**3. كلمة المرور خاطئة:**
```powershell
# اختبر يدوياً
$env:PGPASSWORD="postgres123"
psql -h localhost -U postgres -d EventManagementDb -c "SELECT 1;"
```

**4. Connection String خاطئ:**
تحقق من `appsettings.json`:
```json
"ConnectionStrings": {
  "Default": "Host=localhost;Port=5432;Database=EventManagementDb;Username=postgres;Password=postgres123"
}
```

---

## 🎯 البدائل الفورية

### إذا لم يعمل MCP، استخدم:

**1. السكريبت الموجود:**
```powershell
.\scripts\test-db-connection.ps1
```

**2. psql مباشرة:**
```bash
psql -h localhost -U postgres -d EventManagementDb
```

**3. pgAdmin أو DBeaver:**
- واجهة مرئية
- سهل الاستخدام
- يعرض البيانات بشكل جدولي

**4. استعلامات عبر .NET:**
```powershell
cd aspnet-core/src/EventManagement.EntityFrameworkCore
dotnet ef database update --context EventManagementMigrationsDbContext
```

---

## 📚 موارد إضافية

- [MCP Documentation](https://modelcontextprotocol.io/)
- [Cursor MCP Guide](https://docs.cursor.com/context/model-context-protocol)
- [PostgreSQL MCP Server](https://github.com/modelcontextprotocol/servers/tree/main/src/postgres)

---

## ✅ الخلاصة

**الحالة الحالية:** ⚠️ MCP غير مُفعّل

**الحل:**
1. أضف الإعداد يدوياً في إعدادات Cursor
2. أو استخدم البدائل المتاحة (psql, pgAdmin, السكريبتات)

**ملاحظة:** MCP مفيد جداً لكنه **ليس ضرورياً** - البدائل متوفرة وتعمل بشكل ممتاز!

---

*تم التحديث: 17 أكتوبر 2025*



