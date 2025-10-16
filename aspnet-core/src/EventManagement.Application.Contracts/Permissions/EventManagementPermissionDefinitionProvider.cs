using EventManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace EventManagement.Permissions;

public class EventManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
		var group = context.AddGroup(EventManagementPermissions.GroupName, L("Permission:EventManagement"));

		var events = group.AddPermission(EventManagementPermissions.Events.Default, L("Permission:Events"));
		events.AddChild(EventManagementPermissions.Events.Create, L("Permission:Events.Create"));
		events.AddChild(EventManagementPermissions.Events.Edit, L("Permission:Events.Edit"));
		events.AddChild(EventManagementPermissions.Events.Delete, L("Permission:Events.Delete"));
		events.AddChild(EventManagementPermissions.Events.Approve, L("Permission:Events.Approve"));

		var bookings = group.AddPermission(EventManagementPermissions.Bookings.Default, L("Permission:Bookings"));
		bookings.AddChild(EventManagementPermissions.Bookings.Create, L("Permission:Bookings.Create"));
		bookings.AddChild(EventManagementPermissions.Bookings.Cancel, L("Permission:Bookings.Cancel"));
		bookings.AddChild(EventManagementPermissions.Bookings.MarkAttended, L("Permission:Bookings.MarkAttended"));

		var categories = group.AddPermission(EventManagementPermissions.Categories.Default, L("Permission:Categories"));
		categories.AddChild(EventManagementPermissions.Categories.Create, L("Permission:Categories.Create"));
		categories.AddChild(EventManagementPermissions.Categories.Edit, L("Permission:Categories.Edit"));
		categories.AddChild(EventManagementPermissions.Categories.Delete, L("Permission:Categories.Delete"));

		var cities = group.AddPermission(EventManagementPermissions.Cities.Default, L("Permission:Cities"));
		cities.AddChild(EventManagementPermissions.Cities.Create, L("Permission:Cities.Create"));
		cities.AddChild(EventManagementPermissions.Cities.Edit, L("Permission:Cities.Edit"));
		cities.AddChild(EventManagementPermissions.Cities.Delete, L("Permission:Cities.Delete"));

		var reports = group.AddPermission(EventManagementPermissions.Reports.Default, L("Permission:Reports"));
		reports.AddChild(EventManagementPermissions.Reports.View, L("Permission:Reports.View"));
		reports.AddChild(EventManagementPermissions.Reports.Export, L("Permission:Reports.Export"));

		var admin = group.AddPermission(EventManagementPermissions.Admin.Default, L("Permission:Admin"));
		admin.AddChild(EventManagementPermissions.Admin.UserManagement, L("Permission:Admin.UserManagement"));
		admin.AddChild(EventManagementPermissions.Admin.Settings, L("Permission:Admin.Settings"));

		// تعليق: صلاحيات السلايدر الرئيسي
		var homeSlider = group.AddPermission(EventManagementPermissions.HomeSlider.Default, L("Permission:HomeSlider"));
		homeSlider.AddChild(EventManagementPermissions.HomeSlider.Create, L("Permission:HomeSlider.Create"));
		homeSlider.AddChild(EventManagementPermissions.HomeSlider.Edit, L("Permission:HomeSlider.Edit"));
		homeSlider.AddChild(EventManagementPermissions.HomeSlider.Delete, L("Permission:HomeSlider.Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<EventManagementResource>(name);
    }
}
