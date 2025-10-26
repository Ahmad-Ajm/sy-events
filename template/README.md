# Event Management Template (ABP .NET + Angular + LeptonX + PostgreSQL)

- املأ القوالب في هذا المجلد ثم اتبع الخطوات لإنشاء مشروع جاهز للتشغيل بدون تخمين.

## 1) المتطلبات (Prerequisites)
- .NET 8 SDK
- Node.js 20+ و npm/yarn
- Angular CLI 17+
- ABP CLI (install: dotnet tool install -g Volo.Abp.Cli)
- PostgreSQL 15+ (أو Docker)
- (اختياري) Redis للتخزين الموزع

## 2) القوالب الواجب تعبئتها
- backend/appsettings.Development.json.example
- backend/DbMigrator.appsettings.json.example
- backend/abp-cli-commands.md
- frontend/environment.example.ts
- frontend/leptonx-setup.md
- frontend/locales/ar.example.json, en.example.json
- devops/docker-compose.yml.example
- devops/github-actions-ci.yml.example
- testing/postman.collection.json.example
- telemetry/appsettings.logging.json.example
- security/cors-and-headers.md
- integrations/weaviate.schema.json.example (اختياري)
- integrations/n8n.flow.json.example (اختياري)
- migration/seed.sql.example (اختياري)
- api/openapi.yaml.example (أو انسخ من ana-docs/openapi.yaml)

املأ جميع حقول {{PLACEHOLDER}} قبل البدء. أي حقل غير واضح اكتب: "القيمة غير واضحة في المشروع".

## 3) إنشاء الحل عبر ABP CLI
- راجع backend/abp-cli-commands.md واضبط القيم ثم شغّل الأوامر.

## 4) نسخ الإعدادات إلى مواقعها بعد الإنشاء
- backend/appsettings.Development.json → aspnet-core/src/EventManagement.HttpApi.Host/appsettings.Development.json
- backend/DbMigrator.appsettings.json → aspnet-core/src/EventManagement.DbMigrator/appsettings.json
- frontend/environment.ts → angular/src/environments/environment.ts

## 5) قاعدة البيانات والبذور (اختياري)
- شغّل DbMigrator: dotnet run -p aspnet-core/src/EventManagement.DbMigrator
- أضف بذور من migration/seed.sql.example إذا رغبت.

## 6) التشغيل والتحقق
- Backend: dotnet run -p aspnet-core/src/EventManagement.HttpApi.Host
- Frontend: cd angular && npm install && npm start
- Swagger: https://localhost:44388/swagger
- Angular: http://localhost:4200

## 7) الثيم والأداء (Rules)
- Theme Integration: طبق LeptonX حسب frontend/leptonx-setup.md.
- Perf Metrics: راقب p95/p99 لأهم المسارات بعد أي تعديل.

## 8) ملاحظات
- يمكنك استيراد ana-docs/openapi.yaml كمرجع نهائي للـ API.
- للمزيد، راجع وثائق التحليل في ana-docs/ و project-analysis/.
