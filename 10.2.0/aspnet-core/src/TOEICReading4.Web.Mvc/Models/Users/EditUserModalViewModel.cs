using TOEICReading4.Roles.Dto;
using TOEICReading4.Users.Dto;
using System.Collections.Generic;
using System.Linq;

namespace TOEICReading4.Web.Models.Users;

public class EditUserModalViewModel
{
    public UserDto User { get; set; }

    public IReadOnlyList<RoleDto> Roles { get; set; }

    public bool UserIsInRole(RoleDto role)
    {
        return User.RoleNames != null && User.RoleNames.Any(r => r == role.NormalizedName);
    }
}
