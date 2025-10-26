# ملاحظات مخطط الكيانات ER
- العلاقات:
  - City 1—N Users, City 1—N Events
  - Category 1—N Events
  - User 1—N Events(Organized), User 1—N Bookings
  - Event 1—N Bookings, Event 1—N EventFiles

- الفهارس:
  - Users: unique(email)
  - Categories/Cities: unique(name), unique(nameEn)
  - Events: idx(StartDate), idx(CityId,CategoryId)
  - Bookings: unique(UserId,EventId), idx(EventId)

- مفاتيح:
  - جميع الكيانات GUID/UUID كمفاتيح بديلة.
  - مفاتيح طبيعية (name/nameEn) تُفهرس فقط.
