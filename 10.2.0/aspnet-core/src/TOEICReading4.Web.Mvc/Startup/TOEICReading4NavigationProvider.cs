using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using TOEICReading4.Authorization;

namespace TOEICReading4.Web.Startup;

/// <summary>
/// This class defines menus for the application.
/// </summary>
public class TOEICReading4NavigationProvider : NavigationProvider
{
    public override void SetNavigation(INavigationProviderContext context)
    {
        context.Manager.MainMenu
            .AddItem(
                new MenuItemDefinition(
                    PageNames.Dashboard,
                    L("DashboardPage"),
                    url: "Home/Dashboard",
                    icon: "fas fa-home",
                    requiresAuthentication: true
                )
            )
            .AddItem(
                new MenuItemDefinition(
                    PageNames.UploadExam,
                    L("UploadExam"),
                    url: "",
                    icon: "fas fa-cloud-upload-alt",
                    requiresAuthentication: true
                )
            )
            .AddItem(
                new MenuItemDefinition(
                    PageNames.ExamLibrary,
                    L("ExamLibrary"),
                    url: "/Exam/List",
                    icon: "fas fa-book-open",
                    requiresAuthentication: true
                )
            )
            .AddItem(
                new MenuItemDefinition(
                    PageNames.Tenants,
                    L("Tenants"),
                    url: "Tenants",
                    icon: "fas fa-building",
                    permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Tenants)
                )
            )
            .AddItem(
                new MenuItemDefinition(
                    PageNames.Users,
                    L("Users"),
                    url: "Users",
                    icon: "fas fa-users",
                    permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Users)
                )
            ).AddItem(
                new MenuItemDefinition(
                    PageNames.Roles,
                    L("Roles"),
                    url: "Roles",
                    icon: "fas fa-theater-masks",
                    permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Roles)
                )
            )
            .AddItem(
                new MenuItemDefinition(
                    PageNames.About,
                    L("About"),
                    url: "About",
                    icon: "fas fa-info-circle"
                )
            );
    }

    private static ILocalizableString L(string name)
    {
        return new LocalizableString(name, TOEICReading4Consts.LocalizationSourceName);
    }
}
