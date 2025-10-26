# نظرة معمارية (Architecture Overview)
- ABP Layering: Domain, Domain.Shared, Application, Application.Contracts, HttpApi, HttpApi.Host, EntityFrameworkCore, DbMigrator, Angular.
- النمط: DDD + Modular Monolith مع وحدات ABP القياسية (Identity, TenantManagement, Setting, Permission, Feature, OpenIddict).
- قاعدة البيانات: PostgreSQL عبر EF Core/Npgsql، Migration Assembly موحّد.
- التعددية (Multi-tenancy): Enabled عبر `MultiTenancyConsts.IsEnabled = true`، Middleware مفعل في Host.
- الثيم والواجهة: Angular + LeptonX Side Menu، i18n (ar/en) + RTL.
- التخزين: BlobStoring FileSystem لحفظ صور/ملفات الفعاليات (قابل للاستبدال).
- المراقبة: Serilog + OpenTelemetry/Prometheus + Jaeger + Grafana (وصف).
- الرسم التخطيطي النصي: 
  - Client (Angular/LeptonX) → HttpApi.Host (Auth + Swagger + MultiTenancy) → Application (Policies/DTO/Mappers) → Domain (Entities/Rules) → EF Core (Repositories/DbContext) → PostgreSQL.
