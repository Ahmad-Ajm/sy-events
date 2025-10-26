# نطاق المشروع (Project Scope)
## ضمن النطاق
- منصة ABP (.NET 8) مع Angular وثيم LeptonX Side Menu.
- قاعدة بيانات PostgreSQL عبر EF Core/Npgsql.
- تعددية المستأجرين Multi-Tenancy: Enabled (Host/Tenant).
- إدارة الفعاليات: CRUD، موافقات (Approve/Reject/Publish/Hide)، شعبية/قادمة.
- مدن/تصنيفات: CRUD وربط مع الفعاليات.
- حجوزات/متابعة: متابعة الفعالية، إلغاء، تأكيد حضور، تذكير اختياري.
- رفع ملفات الفعاليات وصور مصغّرة WebP، تخزين FileSystem (قابل للاستبدال لاحقًا).
- تقويم ملوّن للحالات (حضر/تغيب/قادم/منقضي…).
- تقارير أساسية وتصدير CSV.
- توثيق Swagger/OpenAPI وتعريب كامل RTL (ar/en).
- نظام صلاحيات ABP Permissions وسياسات على الخدمات.
- مراقبة/قياس: Serilog + OpenTelemetry/Prometheus + Jaeger + Grafana (وصف إعادي).

## خارج النطاق (النسخة الحالية)
- مدفوعات إلكترونية وQR Tickets.
- تكامل Google Calendar/ICS.
- CDN لتخزين الوسائط.
- مزامنة مع مشاريع خارجية (Next.js) — المنصة مستقلة.

## الافتراضات والقيود
- الأداء: p95 Home ≤ 200ms، Popular ≤ 250ms، Calendar ≤ 300ms.
- الأمان: JWT + OpenIddict، سياسات وصلاحيات، Rate limiting (لاحقًا)، reCAPTCHA v3 (اختياري).
- البنية: Modular Monolith مع وحدات ABP قياسية.
- الوقت/الميزانية: "القيمة غير واضحة في المشروع".
