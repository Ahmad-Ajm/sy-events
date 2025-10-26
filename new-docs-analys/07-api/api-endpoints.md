# مواصفات الـ API (ملخص)
- Events
  - GET /api/app/event
  - GET /api/app/event/{id}
  - POST /api/app/event
  - PUT /api/app/event/{id}
  - DELETE /api/app/event/{id}
  - POST /api/app/event/{id}/approve | reject | publish | hide
  - GET /api/app/event/popular?count=10
  - GET /api/app/event/upcoming?count=10

- Bookings
  - POST /api/app/booking/follow-event?eventId=
  - POST /api/app/booking/unfollow-event?eventId=
  - GET  /api/app/booking/is-following-event?eventId=
  - POST /api/app/booking/confirm-attendance?bookingId=

- Files
  - POST /api/app/event/{eventId}/files/upload-multiple
  - GET  /api/app/event/{eventId}/files
  - DELETE /api/app/event/{eventId}/files/{fileId}

- Calendar
  - GET /api/app/calendar/my-events — "القيمة غير واضحة في المشروع" (المخطط النهائي)

- CMS/SEO (لاحقًا)
  - GET /sitemap.xml — "القيمة غير واضحة في المشروع"
  - GET /robots.txt — "القيمة غير واضحة في المشروع"
