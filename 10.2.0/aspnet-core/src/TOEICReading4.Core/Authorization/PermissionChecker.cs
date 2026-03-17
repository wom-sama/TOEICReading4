using Abp.Authorization;
using TOEICReading4.Authorization.Roles;
using TOEICReading4.Authorization.Users;

namespace TOEICReading4.Authorization;

public class PermissionChecker : PermissionChecker<Role, User>
{
    public PermissionChecker(UserManager userManager)
        : base(userManager)
    {
    }
}
