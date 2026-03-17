using Abp.MultiTenancy;
using TOEICReading4.Authorization.Users;

namespace TOEICReading4.MultiTenancy;

public class Tenant : AbpTenant<User>
{
    public Tenant()
    {
    }

    public Tenant(string tenancyName, string name)
        : base(tenancyName, name)
    {
    }
}
