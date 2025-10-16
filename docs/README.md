# 🎉 منصة إدارة الفعاليات في سوريا - Event Management Platform

منصة شاملة ومتكاملة لإدارة الفعاليات والأحداث في سوريا، مبنية باستخدام ABP Framework وAngular مع LeptonX Theme.

**الحالة**: 85% مكتمل - جاهز للإنتاج التجريبي (Beta)  
**آخر تحديث**: 14 أكتوبر 2025

## 🚀 نظرة عامة

### التقنيات المستخدمة

#### Backend
- **Framework:** ABP Framework (Open Source) - .NET 8
- **Architecture:** Modular Monolith (Domain-Driven Design)
- **Database:** PostgreSQL 15+
- **ORM:** Entity Framework Core 8
- **Authentication:** ABP Identity + JWT
- **API:** RESTful API with Swagger/OpenAPI

#### Frontend
- **Framework:** Angular 17+
- **UI Theme:** LeptonX Lite (Side Menu Layout)
- **UI Components:** ABP Angular Components
- **State Management:** RxJS
- **Localization:** ABP Localization (Arabic/English)
- **RTL Support:** ✅ Full RTL support for Arabic

#### DevOps
- **Containerization:** Docker & Docker Compose
- **CI/CD:** GitHub Actions
- **Cache:** Redis
- **Database Management:** pgAdmin

---

## 📋 المتطلبات المسبقة

### Software
- ✅ [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- ✅ [Node.js 18+](https://nodejs.org/)
- ✅ [Docker Desktop](https://www.docker.com/products/docker-desktop)
- ✅ [Git](https://git-scm.com/)
- ✅ [Visual Studio 2022](https://visualstudio.microsoft.com/) أو [VS Code](https://code.visualstudio.com/)

### CLI Tools
```bash
# ABP CLI
dotnet tool install -g Volo.Abp.Cli

# Angular CLI
npm install -g @angular/cli

# Entity Framework Core CLI
dotnet tool install -g dotnet-ef
```

### التحقق من التثبيت
```bash
dotnet --version          # يجب أن يظهر 8.0.x
node --version            # يجب أن يظهر 18.x أو أحدث
npm --version             # يجب أن يظهر 9.x أو أحدث
docker --version          # يجب أن يظهر 20.x أو أحدث
abp --version            # يجب أن يظهر 8.x أو أحدث
ng version               # يجب أن يظهر 17.x أو أحدث
```

---

## 🏁 البدء السريع

### 1. Clone المشروع
```bash
git clone <repository-url>
cd Event-Management-Platform/CS-SY-Events
```

### 2. تشغيل PostgreSQL باستخدام Docker
```bash
# نسخ ملف .env
cp .env.example .env

# تشغيل قاعدة البيانات
docker-compose up -d postgres pgadmin redis
```

### 3. إنشاء ABP Solution (إذا لم يكن موجوداً)
```bash
# في مجلد CS-SY-Events
abp new EventManagement -t app -u angular -d ef -dbms PostgreSQL --mobile none --pwa
```

### 4. تشغيل Backend
```bash
cd aspnet-core/src/EventManagement.DbMigrator
dotnet run                    # تطبيق migrations وإنشاء البيانات الأولية

cd ../EventManagement.HttpApi.Host
dotnet run                    # تشغيل API
```

Backend سيعمل على: **https://localhost:44300**  
Swagger UI: **https://localhost:44300/swagger**

### 5. تشغيل Frontend
```bash
cd angular
npm install                   # تثبيت dependencies
npm start                     # تشغيل Angular
```

Frontend سيعمل على: **http://localhost:4200**

### 6. تسجيل الدخول
```
Username: admin
Password: 1q2w3E*
```

---

## 🗂️ هيكل المشروع

```
CS-SY-Events/
├── aspnet-core/                      # Backend - ABP Framework
│   ├── src/
│   │   ├── EventManagement.Domain/              # Domain Layer (Entities, Domain Services)
│   │   ├── EventManagement.Domain.Shared/        # Shared (Enums, Constants)
│   │   ├── EventManagement.Application/          # Application Layer (Services)
│   │   ├── EventManagement.Application.Contracts/ # Application Contracts (DTOs, Interfaces)
│   │   ├── EventManagement.EntityFrameworkCore/  # Data Access (EF Core, DbContext)
│   │   ├── EventManagement.HttpApi/              # HTTP API (Controllers)
│   │   ├── EventManagement.HttpApi.Host/         # Web API Host
│   │   └── EventManagement.DbMigrator/           # Database Migration Tool
│   └── test/
│       ├── EventManagement.Domain.Tests/
│       ├── EventManagement.Application.Tests/
│       └── EventManagement.TestBase/
├── angular/                          # Frontend - Angular 17
│   ├── src/
│   │   ├── app/
│   │   │   ├── events/              # Events Module
│   │   │   ├── bookings/            # Bookings Module
│   │   │   ├── admin/               # Admin Module
│   │   │   ├── shared/              # Shared Components
│   │   │   └── proxy/               # Auto-generated API proxies
│   │   ├── assets/
│   │   │   └── locales/             # Localization files (ar.json, en.json)
│   │   └── environments/            # Environment configs
│   └── package.json
├── docker-compose.yml                # Docker Compose configuration
├── .env.example                      # Environment variables template
├── PLAN.md                          # خطة المشروع التفصيلية
└── README.md                        # هذا الملف
```

---

## 🎯 الميزات الرئيسية

### ✅ نظام إدارة المستخدمين
- 5 أدوار: Admin, Organizer, Editor, Support, Viewer
- نظام صلاحيات متقدم (ABP Permission System)
- تسجيل دخول/خروج مع JWT
- إدارة ملفات المستخدمين

### ✅ إدارة الفعاليات
- CRUD كامل للفعاليات
- نظام الموافقات (Draft → Pending → Approved/Rejected)
- رفع الصور والملفات
- تصنيفات ومدن
- بحث وفلترة متقدمة
- دعم اللغتين (Arabic/English)

### ✅ نظام الحجوزات
- حجز الفعاليات
- فحص السعة المتاحة
- نظام التذكير (Email Reminders)
- تتبع الحضور (Attendance Tracking)
- إلغاء الحجوزات

### ✅ التقارير والإحصائيات
- Dashboard مع إحصائيات مباشرة
- تقارير الفعاليات
- تقارير الحجوزات
- Popular Events Algorithm
- Export to Excel/CSV

### ✅ لوحة الإدارة
- إدارة الفعاليات والموافقات
- إدارة المستخدمين
- إدارة التصنيفات والمدن
- الإعدادات العامة
- Audit Logs (سجل التدقيق)

### ✅ مميزات إضافية
- Multi-language (Arabic/English) مع RTL
- LeptonX Side Menu Theme
- Mobile Responsive
- Dark Mode Support
- Social Sharing
- Email Notifications
- File Upload System
- Caching (Redis)
- Rate Limiting
- Security Headers

---

## 🔧 التكوين

### Database Connection
```json
// aspnet-core/src/EventManagement.HttpApi.Host/appsettings.json
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5432;Database=EventManagementDb;Username=postgres;Password=postgres123"
  }
}
```

### Frontend API Configuration
```typescript
// angular/src/environments/environment.ts
export const environment = {
  production: false,
  application: {
    name: 'EventManagement',
    logoUrl: '/assets/logo.png',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44300',
    clientId: 'EventManagement_App',
    scope: 'offline_access EventManagement',
  },
  apis: {
    default: {
      url: 'https://localhost:44300',
    },
  },
};
```

---

## 🐳 Docker

### تشغيل كامل المشروع بـ Docker
```bash
# Build & Run all services
docker-compose up -d

# إيقاف Services
docker-compose down

# عرض Logs
docker-compose logs -f

# إعادة بناء Images
docker-compose up -d --build
```

### الـ Services
- **PostgreSQL:** localhost:5432
- **pgAdmin:** http://localhost:5050
- **Redis:** localhost:6379
- **Backend API:** https://localhost:44300
- **Frontend:** http://localhost:4200

---

## 🧪 الاختبارات

### Backend Tests
```bash
cd aspnet-core
dotnet test
```

### Frontend Tests
```bash
cd angular
npm run test              # Unit tests
npm run test:ci          # CI tests
npm run e2e              # E2E tests
```

---

## 📚 الوثائق

### ملفات مهمة
- **PLAN.md** - خطة المشروع الكاملة مع جميع المراحل
- **docs/getting-started.md** - دليل البدء التفصيلي
- **docs/api-documentation.md** - توثيق API
- **docs/deployment.md** - دليل النشر

### روابط خارجية
- [ABP Framework Documentation](https://docs.abp.io)
- [LeptonX Theme Guide](https://docs.abp.io/en/commercial/latest/themes/lepton-x/angular)
- [Angular Documentation](https://angular.io/docs)
- [PostgreSQL Documentation](https://www.postgresql.org/docs/)

---

## 🔐 الأمان

### ⚠️ تحذيرات مهمة للإنتاج

1. **غيّر جميع كلمات المرور الافتراضية**
   ```bash
   # Database password
   # Admin password
   # JWT Secret
   # SMTP credentials
   ```

2. **فعّل HTTPS**
   ```json
   "AuthServer": {
     "RequireHttpsMetadata": true
   }
   ```

3. **استخدم Secrets Management**
   - Azure Key Vault
   - AWS Secrets Manager
   - Docker Secrets

4. **راجع OWASP Top 10**
   - SQL Injection ✅ (محمي بـ EF Core)
   - XSS ✅ (محمي بـ Angular)
   - CSRF ✅ (محمي بـ ABP)

---

## 🚀 النشر

### Docker Production
```bash
# Build production images
docker-compose -f docker-compose.prod.yml build

# Deploy
docker-compose -f docker-compose.prod.yml up -d
```

### Manual Deployment
```bash
# Backend
cd aspnet-core/src/EventManagement.HttpApi.Host
dotnet publish -c Release -o ./publish

# Frontend
cd angular
npm run build:prod
```

---

## 🤝 المساهمة

### Workflow
1. Fork المشروع
2. إنشاء Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit التغييرات (`git commit -m 'Add AmazingFeature'`)
4. Push للـ Branch (`git push origin feature/AmazingFeature`)
5. فتح Pull Request

### معايير الكود
- اتبع ABP Framework Best Practices
- استخدم SOLID Principles
- اكتب Unit Tests
- وثّق الكود بالتعليقات
- اتبع Naming Conventions

---

## 🐛 حل المشاكل الشائعة

### مشكلة: Database connection failed
```bash
# تأكد من تشغيل PostgreSQL
docker-compose up -d postgres

# تحقق من Connection String
# تأكد من Username/Password صحيح
```

### مشكلة: CORS errors في Frontend
```csharp
// أضف في Program.cs
app.UseCors(builder =>
{
    builder
        .WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
});
```

### مشكلة: npm install failures
```bash
# نظف cache
npm cache clean --force
rm -rf node_modules package-lock.json
npm install
```

---

## 📊 الحالة والتقدم

### Current Status
```
Phase 0: الإعداد الأولي ✅ مكتمل
Phase 1: ABP Solution     ⏳ جاري التنفيذ
Phase 2-12:               🔜 قادم
```

### Progress
```
[███░░░░░░░] 25%
```

---

## 📞 الدعم

### للأسئلة والمشاكل
- افتح Issue في GitHub
- راجع Documentation في `docs/`
- ابحث في [ABP Community](https://community.abp.io)

### Contact
- Email: support@eventmanagement.sy
- Website: [قريباً]

---

## 📄 الترخيص

[أضف معلومات الترخيص هنا]

---

## 🙏 شكر خاص

- [ABP Framework](https://abp.io) - أفضل Framework لـ .NET
- [Angular Team](https://angular.io) - Frontend Framework رائع
- [PostgreSQL](https://www.postgresql.org) - قاعدة بيانات موثوقة

---

**آخر تحديث:** 12 أكتوبر 2025  
**الإصدار:** v1.0.0  
**الحالة:** قيد التطوير النشط 🚧

