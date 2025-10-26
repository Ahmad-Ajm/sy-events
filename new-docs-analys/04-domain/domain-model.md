# نموذج المجال (Domain Model)

## الكيانات (Entities)
- User: Email, Name, PasswordHash, Phone?, Profession?, CityId?, Interests?, Reason?, Role.
- City: Name, NameEn.
- Category: Name, NameEn, Description?, DescriptionEn?.
- Event: Title, TitleEn?, Description, DescriptionEn?, StartDate, EndDate, Location, LocationEn?, MaxCapacity?, IsApproved, Status, ImageUrl?, ThumbnailUrl?, CategoryId, CityId, OrganizerId.
- Booking: UserId, EventId, Status, ReminderTime?, AttendedAt?.
- EventFile: EventId, FileName, FilePath, FileType, MimeType, Alt?, ThumbnailPath?, DisplayOrder.
- HomeSliderItem: Type[Latest/Popular/Custom], CustomEventId?, DisplayOrder, IsActive, ActiveFrom?, ActiveTo?.
- FeaturedBox: Type[Latest/Popular/Custom], Title, Order, CustomLink?, CustomEventId?.

## القيم (Value Objects)
- Address: "القيمة غير واضحة في المشروع"
- Money: "القيمة غير واضحة في المشروع"

## التجميـعات (Aggregates)
- Event كـ Aggregate Root ويضم Bookings وFiles.
- User كـ Aggregate Root ويضم OrganizedEvents وBookings.
- Category، City كجذور لتجميعات بسيطة.

## العلاقات الرئيسية
- Event (N) — (1) Category
- Event (N) — (1) City
- Event (N) — (1) Organizer(User)
- Booking (N) — (1) User، Booking (N) — (1) Event
- City (1) — (N) Users

## قواعد العمل (Business Rules)
- لا يمكن إلغاء حجز Attended أو الملغي مسبقًا.
- الموافقة مطلوبة قبل ظهور الفعالية للعامة؛ Approve/Reject تغيّر الحالة وتعلَم IsApproved.
- سعة الفعالية القصوى تحدّ من Confirmed Bookings؛ التفاصيل العددية: "القيمة غير واضحة في المشروع".
