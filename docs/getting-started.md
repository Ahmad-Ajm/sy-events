# 🚀 دليل البدء - Event Management Platform

دليل شامل لبدء العمل على منصة إدارة الفعاليات.

---

## 📋 المحتويات

1. [الإعداد الأولي](#الإعداد-الأولي)
2. [إنشاء ABP Solution](#إنشاء-abp-solution)
3. [تشغيل المشروع](#تشغيل-المشروع)
4. [فهم الهيكل](#فهم-الهيكل)
5. [التطوير اليومي](#التطوير-اليومي)
6. [حل المشاكل](#حل-المشاكل)

---

## 1. الإعداد الأولي

### 1.1 تثبيت المتطلبات

#### Windows
```powershell
# تثبيت .NET 8 SDK
winget install Microsoft.DotNet.SDK.8

# تثبيت Node.js
winget install OpenJS.NodeJS.LTS

# تثبيت Docker Desktop
winget install Docker.DockerDesktop

# تثبيت Git
winget install Git.Git
```

#### macOS
```bash
# استخدم Homebrew
brew install dotnet-sdk
brew install node
brew install --cask docker
```

#### Linux (Ubuntu/Debian)
```bash
# تثبيت .NET 8
wget https://dot.net/v1/dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --channel 8.0

# تثبيت Node.js
curl -fsSL https://deb.nodesource.com/setup_18.x | sudo -E bash -
sudo apt-get install -y nodejs

# تثبيت Docker
curl -fsSL https://get.docker.com -o get-docker.sh
sudo sh get-docker.sh
```

### 1.2 تثبيت CLI Tools

```bash
# ABP CLI
dotnet tool install -g Volo.Abp.Cli

# Angular CLI
npm install -g @angular/cli

# Entity Framework CLI
dotnet tool install -g dotnet-ef

# تحديث الأدوات (إذا كانت مثبتة مسبقاً)
dotnet tool update -g Volo.Abp.Cli
dotnet tool update -g dotnet-ef
```

### 1.3 التحقق من التثبيت

```bash
# يجب أن تعرض جميع الأوامر الإصدارات بنجاح
dotnet --version          # 8.0.x
node --version            # v18.x.x أو أحدث
npm --version             # 9.x.x أو أحدث
docker --version          # 20.x.x أو أحدث
abp --version            # 8.x.x
ng version               # 17.x.x
dotnet ef                # EF Core CLI
```

---

## 2. إنشاء ABP Solution

### 2.1 Clone المشروع

```bash
# Clone repository
git clone <repository-url>
cd Event-Management-Platform

# الانتقال لمجلد المشروع
cd CS-SY-Events
```

### 2.2 إنشاء ABP Application

```bash
# في مجلد CS-SY-Events
abp new EventManagement \
  -t app \
  -u angular \
  -d ef \
  -dbms PostgreSQL \
  --mobile none \
  --pwa

# الخيارات:
# -t app              : نوع Template (application)
# -u angular          : UI Framework (Angular)
# -d ef               : Database Provider (Entity Framework Core)
# -dbms PostgreSQL    : Database Management System
# --mobile none       : بدون mobile app
# --pwa               : بدون Progressive Web App
```

### 2.3 ما يحدث بعد التنفيذ

ABP CLI سينشئ:
```
CS-SY-Events/
├── EventManagement.sln
├── aspnet-core/
│   ├── src/
│   │   ├── EventManagement.Domain/
│   │   ├── EventManagement.Domain.Shared/
│   │   ├── EventManagement.Application/
│   │   ├── EventManagement.Application.Contracts/
│   │   ├── EventManagement.EntityFrameworkCore/
│   │   ├── EventManagement.HttpApi/
│   │   ├── EventManagement.HttpApi.Host/
│   │   └── EventManagement.DbMigrator/
│   └── test/
└── angular/
    ├── src/
    ├── package.json
    └── angular.json
```

---

## 3. تشغيل المشروع

### 3.1 إعداد قاعدة البيانات

#### الطريقة 1: استخدام Docker (موصى به)

```bash
# نسخ ملف .env
cp .env.example .env

# تشغيل PostgreSQL + pgAdmin + Redis
docker-compose up -d postgres pgadmin redis

# التحقق من عمل Containers
docker-compose ps

# يجب أن تظهر:
# eventmanagement-postgres   Up   5432/tcp
# eventmanagement-pgadmin    Up   5050/tcp
# eventmanagement-redis      Up   6379/tcp
```

#### الطريقة 2: تثبيت PostgreSQL محلياً

```bash
# Windows (Chocolatey)
choco install postgresql

# macOS (Homebrew)
brew install postgresql@15
brew services start postgresql@15

# Linux (Ubuntu)
sudo apt-get install postgresql-15
sudo systemctl start postgresql
```

**إنشاء Database:**
```bash
# فتح PostgreSQL CLI
psql -U postgres

# في psql:
CREATE DATABASE "EventManagementDb";
\q
```

### 3.2 تكوين Connection String

```json
// aspnet-core/src/EventManagement.HttpApi.Host/appsettings.json
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5432;Database=EventManagementDb;Username=postgres;Password=postgres123"
  }
}

// aspnet-core/src/EventManagement.DbMigrator/appsettings.json
// نفس Connection String
```

### 3.3 تطبيق Database Migrations

```bash
# الطريقة 1: استخدام DbMigrator (موصى به)
cd aspnet-core/src/EventManagement.DbMigrator
dotnet run

# سيقوم بـ:
# 1. تطبيق Migrations
# 2. Seed البيانات الأولية
# 3. إنشاء Admin user

# الطريقة 2: استخدام EF Core CLI
cd aspnet-core/src/EventManagement.EntityFrameworkCore
dotnet ef database update
```

### 3.4 تشغيل Backend API

```bash
cd aspnet-core/src/EventManagement.HttpApi.Host
dotnet restore
dotnet run

# أو للتطوير مع Hot Reload:
dotnet watch run
```

**Backend Endpoints:**
- API: https://localhost:44300
- Swagger UI: https://localhost:44300/swagger
- Health Check: https://localhost:44300/health

### 3.5 تشغيل Frontend

```bash
cd angular
npm install                # تثبيت dependencies (أول مرة فقط)
npm start                  # تشغيل dev server

# أو
ng serve
```

**Frontend:**
- URL: http://localhost:4200
- Hot Reload: enabled

### 3.6 تسجيل الدخول

```
Username: admin
Password: 1q2w3E*
```

---

## 4. فهم الهيكل

### 4.1 Backend Structure

#### Domain Layer
```
EventManagement.Domain/
├── Events/
│   ├── Event.cs                    # Entity
│   ├── EventManager.cs             # Domain Service
│   └── IEventRepository.cs         # Repository Interface
├── Users/
├── Categories/
└── Shared/
    └── EventManagementConsts.cs
```

**متى تستخدم Domain Layer:**
- Entities (الكيانات الأساسية)
- Value Objects
- Domain Services (منطق الأعمال المعقد)
- Domain Events
- Repository Interfaces

#### Application Layer
```
EventManagement.Application/
├── Events/
│   ├── EventAppService.cs          # Application Service
│   └── EventAppService.Tests.cs
└── EventManagementApplicationAutoMapperProfile.cs
```

**متى تستخدم Application Layer:**
- Application Services (CRUD operations)
- Business Logic
- DTOs Mapping
- Validation
- Authorization

#### Infrastructure Layer (EF Core)
```
EventManagement.EntityFrameworkCore/
├── EntityFrameworkCore/
│   ├── EventManagementDbContext.cs
│   └── EventManagementDbContextFactory.cs
├── Migrations/
└── Repositories/
    └── EventRepository.cs
```

**متى تستخدم Infrastructure Layer:**
- DbContext Configuration
- Entity Configurations (Fluent API)
- Repositories Implementation
- Database Migrations

#### HTTP API Layer
```
EventManagement.HttpApi/
└── Controllers/
    └── EventController.cs
```

**ملاحظة:** ABP يولّد Controllers تلقائياً من Application Services، نادراً ما نحتاج لإنشاء Controllers يدوياً.

### 4.2 Frontend Structure

```
angular/src/app/
├── events/                         # Events Feature Module
│   ├── components/
│   │   ├── event-list/
│   │   ├── event-detail/
│   │   ├── event-create/
│   │   └── event-edit/
│   ├── services/
│   │   └── event.service.ts
│   ├── models/
│   │   └── event.model.ts
│   ├── events-routing.module.ts
│   └── events.module.ts
├── bookings/                       # Bookings Feature Module
├── admin/                          # Admin Feature Module
├── shared/                         # Shared Components/Services
│   ├── components/
│   ├── pipes/
│   └── directives/
└── proxy/                          # Auto-generated API Proxies
    ├── events/
    └── bookings/
```

---

## 5. التطوير اليومي

### 5.1 Workflow المعتاد

```bash
# صباحاً - تشغيل البيئة
cd CS-SY-Events

# 1. تشغيل Database
docker-compose up -d postgres redis

# 2. تشغيل Backend (في terminal منفصل)
cd aspnet-core/src/EventManagement.HttpApi.Host
dotnet watch run

# 3. تشغيل Frontend (في terminal منفصل)
cd angular
npm start

# 4. فتح المتصفحات
# Frontend: http://localhost:4200
# Swagger: https://localhost:44300/swagger
# pgAdmin: http://localhost:5050
```

### 5.2 إضافة Entity جديد

#### الخطوة 1: إنشاء Entity في Domain
```csharp
// aspnet-core/src/EventManagement.Domain/Categories/Category.cs
using Volo.Abp.Domain.Entities.Auditing;

namespace EventManagement.Categories
{
    public class Category : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public string NameEn { get; set; }
        public string Description { get; set; }
        
        protected Category() { }
        
        public Category(Guid id, string name, string nameEn) : base(id)
        {
            Name = name;
            NameEn = nameEn;
        }
    }
}
```

#### الخطوة 2: إضافة للـ DbContext
```csharp
// EventManagement.EntityFrameworkCore/EventManagementDbContext.cs
public DbSet<Category> Categories { get; set; }

protected override void OnModelCreating(ModelBuilder builder)
{
    base.OnModelCreating(builder);
    
    builder.Entity<Category>(b =>
    {
        b.ToTable("categories");
        b.ConfigureByConvention();
        
        b.Property(x => x.Name).IsRequired().HasMaxLength(150);
        b.Property(x => x.NameEn).IsRequired().HasMaxLength(150);
        
        b.HasIndex(x => x.Name).IsUnique();
    });
}
```

#### الخطوة 3: إنشاء Migration
```bash
cd aspnet-core/src/EventManagement.EntityFrameworkCore
dotnet ef migrations add AddedCategory
dotnet ef database update
```

#### الخطوة 4: إنشاء DTOs
```csharp
// Application.Contracts/Categories/CategoryDto.cs
public class CategoryDto : EntityDto<Guid>
{
    public string Name { get; set; }
    public string NameEn { get; set; }
    public string Description { get; set; }
}

// Application.Contracts/Categories/CreateUpdateCategoryDto.cs
public class CreateUpdateCategoryDto
{
    [Required]
    [StringLength(150)]
    public string Name { get; set; }
    
    [Required]
    [StringLength(150)]
    public string NameEn { get; set; }
    
    public string Description { get; set; }
}
```

#### الخطوة 5: إنشاء Application Service
```csharp
// Application/Categories/CategoryAppService.cs
public class CategoryAppService : 
    CrudAppService<Category, CategoryDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateCategoryDto>,
    ICategoryAppService
{
    public CategoryAppService(IRepository<Category, Guid> repository) 
        : base(repository)
    {
    }
}
```

#### الخطوة 6: تولي Angular Proxies
```bash
cd angular
abp generate-proxy -t ng
```

#### الخطوة 7: إنشاء Angular Component
```bash
ng generate module categories --routing
ng generate component categories/category-list
```

### 5.3 إضافة Permission جديد

```csharp
// Application.Contracts/Permissions/EventManagementPermissions.cs
public static class Categories
{
    public const string Default = GroupName + ".Categories";
    public const string Create = Default + ".Create";
    public const string Edit = Default + ".Edit";
    public const string Delete = Default + ".Delete";
}

// في PermissionDefinitionProvider
var categoriesPermission = eventManagementGroup.AddPermission(
    EventManagementPermissions.Categories.Default,
    L("Permission:Categories"));

categoriesPermission.AddChild(
    EventManagementPermissions.Categories.Create,
    L("Permission:Categories.Create"));

// استخدام في Service
[Authorize(EventManagementPermissions.Categories.Create)]
public async Task<CategoryDto> CreateAsync(CreateUpdateCategoryDto input)
{
    // ...
}
```

### 5.4 إضافة Localization

```json
// angular/src/assets/locales/ar.json
{
  "Categories": "التصنيفات",
  "CreateCategory": "إنشاء تصنيف",
  "CategoryName": "اسم التصنيف",
  "Description": "الوصف"
}

// angular/src/assets/locales/en.json
{
  "Categories": "Categories",
  "CreateCategory": "Create Category",
  "CategoryName": "Category Name",
  "Description": "Description"
}
```

---

## 6. حل المشاكل

### مشكلة: "Cannot connect to database"

```bash
# تحقق من عمل PostgreSQL
docker-compose ps postgres

# إذا لم يعمل، أعد تشغيله
docker-compose restart postgres

# تحقق من Connection String
cat aspnet-core/src/EventManagement.HttpApi.Host/appsettings.json | grep ConnectionStrings

# اختبر الاتصال يدوياً
psql -h localhost -U postgres -d EventManagementDb
```

### مشكلة: "Migration pending"

```bash
# تطبيق Migrations
cd aspnet-core/src/EventManagement.DbMigrator
dotnet run

# أو
cd aspnet-core/src/EventManagement.EntityFrameworkCore
dotnet ef database update
```

### مشكلة: "CORS error" في Angular

```csharp
// في Program.cs أو Startup
app.UseCors(builder =>
{
    builder
        .WithOrigins(
            "http://localhost:4200",
            "https://localhost:4200"
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
});
```

### مشكلة: "Port already in use"

```bash
# Windows - إيجاد Process على Port 44300
netstat -ano | findstr :44300
taskkill /PID <process_id> /F

# Linux/Mac
lsof -i :44300
kill -9 <process_id>

# أو غيّر Port في launchSettings.json
```

### مشكلة: "npm install fails"

```bash
# نظف npm cache
npm cache clean --force
rm -rf node_modules package-lock.json
npm install

# إذا فشل، استخدم --legacy-peer-deps
npm install --legacy-peer-deps
```

### مشكلة: "Swagger not showing endpoints"

```csharp
// تأكد من تكوين Auto API Controllers
Configure<AbpAspNetCoreMvcOptions>(options =>
{
    options
        .ConventionalControllers
        .Create(typeof(EventManagementApplicationModule).Assembly);
});

// أعد compile وشغل من جديد
dotnet clean
dotnet build
dotnet run
```

---

## 📚 الخطوات التالية

1. ✅ اقرأ `PLAN.md` للفهم الشامل
2. ✅ راجع [ABP Documentation](https://docs.abp.io)
3. ✅ جرب إنشاء Entity جديد
4. ✅ تعلم LeptonX Theme customization
5. ✅ اطلع على Best Practices في `docs/best-practices.md`

---

**تم آخر تحديث:** 12 أكتوبر 2025  
**الحالة:** قيد التطوير

