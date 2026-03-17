using TOEICReading4.Roles.Dto;
using System.Collections.Generic;

namespace TOEICReading4.Web.Models.Roles;

public class RoleListViewModel
{
    public IReadOnlyList<PermissionDto> Permissions { get; set; }
}
