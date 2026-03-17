using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using TOEICReading4.Authorization.Users;
using TOEICReading4.MultiTenancy;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace TOEICReading4;

/// <summary>
/// Derive your application services from this class.
/// </summary>
public abstract class TOEICReading4AppServiceBase : ApplicationService
{
    public TenantManager TenantManager { get; set; }

    public UserManager UserManager { get; set; }

    protected TOEICReading4AppServiceBase()
    {
        LocalizationSourceName = TOEICReading4Consts.LocalizationSourceName;
    }

    protected virtual async Task<User> GetCurrentUserAsync()
    {
        var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
        if (user == null)
        {
            throw new Exception("There is no current user!");
        }

        return user;
    }

    protected virtual Task<Tenant> GetCurrentTenantAsync()
    {
        return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
    }

    protected virtual void CheckErrors(IdentityResult identityResult)
    {
        identityResult.CheckErrors(LocalizationManager);
    }
}
