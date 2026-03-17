using Abp.MultiTenancy;

namespace TOEICReading4.Web.Models.Account;

public class LoginFormViewModel
{
    public string ReturnUrl { get; set; }

    public string UsernameOrEmailAddress { get; set; }

    public bool RememberMe { get; set; }

    public bool IsMultiTenancyEnabled { get; set; }

    public bool IsSelfRegistrationAllowed { get; set; }

    public MultiTenancySides MultiTenancySide { get; set; }
}
