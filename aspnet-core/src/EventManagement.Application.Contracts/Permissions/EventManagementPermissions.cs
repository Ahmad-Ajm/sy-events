namespace EventManagement.Permissions;

public static class EventManagementPermissions
{
	public const string GroupName = "EventManagement";

	public static class Events
	{
		public const string Default = GroupName + ".Events";
		public const string Create = Default + ".Create";
		public const string Edit = Default + ".Edit";
		public const string Delete = Default + ".Delete";
		public const string Approve = Default + ".Approve";
	}

	public static class Bookings
	{
		public const string Default = GroupName + ".Bookings";
		public const string Create = Default + ".Create";
		public const string Cancel = Default + ".Cancel";
		public const string MarkAttended = Default + ".MarkAttended";
	}

	public static class Categories
	{
		public const string Default = GroupName + ".Categories";
		public const string Create = Default + ".Create";
		public const string Edit = Default + ".Edit";
		public const string Delete = Default + ".Delete";
	}

	public static class Cities
	{
		public const string Default = GroupName + ".Cities";
		public const string Create = Default + ".Create";
		public const string Edit = Default + ".Edit";
		public const string Delete = Default + ".Delete";
	}

	public static class Reports
	{
		public const string Default = GroupName + ".Reports";
		public const string View = Default + ".View";
		public const string Export = Default + ".Export";
	}

	public static class Admin
	{
		public const string Default = GroupName + ".Admin";
		public const string UserManagement = Default + ".UserManagement";
		public const string Settings = Default + ".Settings";
	}

	// تعليق: صلاحيات السلايدر الرئيسي
	public static class HomeSlider
	{
		public const string Default = GroupName + ".HomeSlider";
		public const string Create = Default + ".Create";
		public const string Edit = Default + ".Edit";
		public const string Delete = Default + ".Delete";
	}
}
