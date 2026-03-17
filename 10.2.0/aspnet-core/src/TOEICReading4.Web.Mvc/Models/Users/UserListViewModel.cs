using TOEICReading4.Roles.Dto;
using System.Collections.Generic;

namespace TOEICReading4.Web.Models.Users;

public class UserListViewModel
{
    public IReadOnlyList<RoleDto> Roles { get; set; }
}
