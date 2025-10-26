# AI Builder Guide

هدف هذا القالب: إلزام أي نموذج ذكاء اصطناعي بطلب كل التفاصيل الناقصة قبل التوليد.


## قواعد إلزامية
1) لا تفترض أي قيمة غير موجودة؛ إن لم تتوفر قيمة صرّح بها واطلبها صراحة.
2) استخدم عبارة: "القيمة غير واضحة في المشروع" في الحقول المجهولة مؤقتًا.
3) قبل التوليد، أعرض لائحة أسئلة مقسمة (Backend, Frontend, Security, DevOps, Data, Integrations, i18n, UX, Performance) واستمر حتى تتم الإجابة أو وضع قرارات معقولة.
4) بعد تعبئة جميع القوالب `.example` قم بتوليد الشجرة والكود.

## قائمة أسئلة حدّية (Checklist)
- Backend: أسماء الحل/المشاريع/الـNamespaces؟ سياسات ABP؟ DTOs النهائية؟
- API: أي اختلافات عن `ana-docs/openapi.yaml`؟ فلاتر البحث المتقدمة؟
- Data: المدن/التصنيفات الأساسية؟ حدود الحجم للملفات؟ مسار التخزين؟
- Security: CORS origins, Auth issuer/clientId, headers policy؟
- Frontend: عناصر القائمة وrequiredPolicy؟ صفحات التقويم والسلايدر والمربعات؟
- Theme: متغيرات LeptonX والتخصيص/الوضع الليلي؟
- i18n: مفاتيح أساسية، لغة افتراضية؟
- Performance: أهداف p95/p99 لكل مسار حرج؟
- Integrations: تفاصيل Weaviate/n8n إن وُجدت؟
- DevOps: Docker/CI/CD وSecrets strategy؟

بمجرد الإجابة، املأ placeholders ثم ولّد المشروع.

