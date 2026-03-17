using Abp.AutoMapper;
using TOEICReading4.Sessions.Dto;

namespace TOEICReading4.Web.Views.Shared.Components.TenantChange;

[AutoMapFrom(typeof(GetCurrentLoginInformationsOutput))]
public class TenantChangeViewModel
{
    public TenantLoginInfoDto Tenant { get; set; }
}
