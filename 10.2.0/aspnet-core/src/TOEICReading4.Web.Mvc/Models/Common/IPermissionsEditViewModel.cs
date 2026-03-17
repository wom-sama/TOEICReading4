using TOEICReading4.Roles.Dto;
using System.Collections.Generic;

namespace TOEICReading4.Web.Models.Common;

public interface IPermissionsEditViewModel
{
    List<FlatPermissionDto> Permissions { get; set; }
}