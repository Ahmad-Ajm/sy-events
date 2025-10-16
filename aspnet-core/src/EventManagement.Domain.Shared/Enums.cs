namespace EventManagement.Enums
{
    public enum UserRole
    {
        Admin = 1,
        Organizer = 2,
        Editor = 3,
        Support = 4,
        Viewer = 5
    }

    public enum EventStatus
    {
        Draft = 1,
        Pending = 2,
        Approved = 3,
        Rejected = 4,
        Hidden = 5
    }

    public enum BookingStatus
    {
        Confirmed = 1,
        Cancelled = 2,
        Attended = 3,
        NoShow = 4
    }

    public enum ReminderTime
    {
        OneHour = 1,
        TwentyFourHours = 24,
        SeventyTwoHours = 72,
        OneWeek = 168
    }
}


