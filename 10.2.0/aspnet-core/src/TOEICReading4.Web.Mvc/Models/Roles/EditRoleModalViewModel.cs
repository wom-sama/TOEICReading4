using Abp.AutoMapper;
using TOEICReading4.Roles.Dto;
using TOEICReading4.Web.Models.Common;

namespace TOEICReading4.Web.Models.Roles;

[AutoMapFrom(typeof(GetRoleForEditOutput))]
public class EditRoleModalViewModel : GetRoleForEditOutput, IPermissionsEditViewModel
{
    public bool HasPermission(FlatPermissionDto permission)
    {
        return GrantedPermissionNames.Contains(permission.Name);
    }
}
