# المتطلبات غير الوظيفية (NFR)

- الأداء (Performance):
  - الأهداف: Home p95 ≤ 200ms، Popular p95 ≤ 250ms، Calendar p95 ≤ 300ms.
  - عام: API p95 للعمليات الشائعة ≤ 300ms، p99 ≤ 600ms. "القيمة غير واضحة في المشروع" لأي هدف إضافي.
  - فهارس DB على الحقول الساخنة، EF NoTracking للقراءة، Cache/ETag.

- الأمان (Security):
  - مصادقة OpenIddict + JWT Bearer، تفويض بالسياسات/الصلاحيات ABP.
  - CORS مضبوطة، رؤوس أمنية، حماية الملفات (MIME/الحجم)، CSRF (مضمن ABP).
  - reCAPTCHA v3 (اختياري بالإعدادات). تخزين أسرار آمن (appsettings.secrets.json).

- الاعتمادية (Reliability):
  - التوافر المستهدف: "القيمة غير واضحة في المشروع".
  - نسخ احتياطي دوري لقاعدة البيانات، استعادة مجرّبة، مراقبة الأخطاء.

- القابلية للتوسع (Scalability):
  - قابلية أفقية لخدمات الويب، Redis Distributed Cache (اختياري)، فصل concern.

- القابلية للصيانة (Maintainability):
  - DDD + Modular Monolith، ADRs موثّقة، تغطية اختبارات أساسية، أسلوب ترميز واضح.

- القابلية للمراقبة (Observability):
  - سجلات: Serilog مع Enrichers.
  - قياسات: OpenTelemetry/Prometheus.
  - تتبّع: Jaeger. لوحات: Grafana.

- الترجمة وRTL:
  - ملفات i18n (ar/en)، RTL متوافق مع LeptonX، تدقيق وصول (a11y).
