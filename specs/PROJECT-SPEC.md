# 📐 مواصفات مشروع منصة إدارة الفعاليات (Project Specification)

## 1) النطاق (Scope)
- منصة لإدارة واستكشاف ومتابعة الفعاليات في سوريا.
- الأدوار: Admin / Organizer / Editor / Support / Viewer.
- الواجهات: عامة (الصفحة الرئيسية/قائمة الفعاليات/التفاصيل)، مستخدم (تقويم/حسابي)، إدارة (فعاليات/موافقات/إعدادات/CMS/SEO).
- التقنية: ASP.NET Core (ABP) + EF Core + PostgreSQL + Angular + Lepton X.

## 2) الافتراضات (Assumptions)
- العربية افتراضيًا (RTL) + دعم الإنجليزية للواجهة.
- المشروع الحالي مستقل؛ سيتم حذف مشروع Next.js لاحقاً بعد الاستقرار.
- التخزين محلي للملفات مع إمكانية الاستبدال لاحقًا.

## 3) المتطلبات اللاوظيفية (NFRs)
- الأداء: p95 الصفحة الرئيسية ≤ 200ms؛ Popular ≤ 250ms؛ Calendar ≤ 300ms.
- الأمان: AuthZ/Policies، Rate limiting، reCAPTCHA v3 (اختياري)، سياسات ملفات صارمة.
- القابلية للتوسع: فهارس DB على الحقول الساخنة، Cache/ETag، فصل طبقات، إمكان تبديل التخزين.
- إمكانية الوصول: RTL، تباين جيد، تنقل لوحة المفاتيح.

## 4) نموذج البيانات (مختصر)
- Event(Id, Title, Description, StartDate, EndDate, Location, CityId, CategoryId, OrganizerId, Status, IsApproved, ImageUrl, ThumbnailUrl, SeoMeta, Canonical, Tags)
- Booking(Id, UserId, EventId, Status, ReminderTime, AttendedAt)
- Category(Id, Name, NameEn)
- City(Id, Name, NameEn)
- EventFile(Id, EventId, FileName, FilePath, FileType, MimeType, Alt, ThumbnailPath, DisplayOrder)
- HomeSliderItem(Id, Type[Latest/Popular/Custom], CustomEventId?, DisplayOrder, IsActive, ActiveFrom?, ActiveTo?)
- FeaturedBox(Id, Type[Latest/Popular/Custom], Title, Order, CustomLink?, CustomEventId?)
- Page(Id, Slug, Title, ContentHtml, IsPublished, MenuPlacement[Main/Footer/None], Order)
- MenuItem(Id, Label, Url, Order, ParentId?, IsActive)
- AppSettings(SeoDefaults, SliderItemsCount, AutoApproveEvents, RecaptchaKeys, ...)

## 5) الواجهات (APIs) – أمثلة أساسية
- Events:
  - GET /api/app/event (فلاتر: city/category/date/organizer/isUpcoming/minAttendees/filter)
  - GET /api/app/event/{id}
  - POST /api/app/event
  - PUT /api/app/event/{id}
  - DELETE /api/app/event/{id}
  - POST /api/app/event/{id}/approve | reject | hide | publish
  - GET /api/app/event/popular?count=10 (شعبية 30 يوم)
  - GET /api/app/event/upcoming?count=10
- Bookings:
  - POST /api/app/booking/follow-event?eventId=
  - POST /api/app/booking/unfollow-event?eventId=
  - GET  /api/app/booking/is-following-event?eventId=
  - POST /api/app/booking/confirm-attendance?bookingId=
- Files:
  - POST /api/app/event/{eventId}/files/upload-multiple (3 صور + PDF + TXT)
  - GET  /api/app/event/{eventId}/files
  - DELETE /api/app/event/{eventId}/files/{fileId}
- Calendar:
  - GET /api/app/calendar/my-events (حالات لونية)
- CMS/SEO:
  - GET /sitemap.xml
  - GET /robots.txt
  - Admin: إدارة الصفحات والقوائم وإعدادات SEO
- Social:
  - GET  /api/app/social-share/facebook-link/{eventId}
  - GET  /api/app/social-share/whats-app-link/{eventId}
  - POST /api/app/social-share/share-to-telegram?eventId=&chatId=&botToken=

## 6) محرر ومساعد SEO (الواجهة)
- محرر WYSIWYG يدعم RTL؛ منع تعدد H1؛ اقتراح H2/H3؛ تنبيهات meta/alt؛ فحص طول العنوان والوصف.

## 7) القبول (Acceptance Criteria)
- تحكم كامل بالسلايدر/المربعات (تفعيل/تعطيل/مصادر/عدد 2–6) ينعكس فوراً.
- الشعبية لآخر 30 يوماً تُستخدم في Popular والفرز الافتراضي.
- CSV Export فعلي بفلاتر وترميز صحيح.
- reCAPTCHA v3 يعمل عند التفعيل ويُعطّل بسهولة.
- تذكيرات 1/24/72/168 ساعة بلا تكرار مع سجل إرسال.
- WebP/Thumbnail وalt إلزامي للصور.
- sitemap/robots حسب إعدادات الإدارة مع Cache وLast-Modified.
- Calendar API يعيد الحالات اللونية وتُعرض في FullCalendar.

## 8) خارج النطاق (حالياً)
- مدفوعات إلكترونية، شهادات/QR، تكامل Google Calendar/ICS، CDN (لاحقاً كخيار إعدادات).

## 9) الأداء والاختبارات
- Cache/ETag، Brotli، EF NoTracking؛ فهارس DB.
- تتبع p95/p99 عبر Interceptor.
- اختبارات: وحدة، تكامل، E2E للتدفقات الحرجة؛ تدقيق نهائي للكود (منطق/وظيفة/Syntax/أمان/تبعيات/تنظيم/قابلية التوسع/نظافة الكود).
