using Abp.Application.Features;
using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using TOEICReading4.Authorization.Users;
using TOEICReading4.Editions;

namespace TOEICReading4.MultiTenancy;

public class TenantManager : AbpTenantManager<Tenant, User>
{
    public TenantManager(
        IRepository<Tenant> tenantRepository,
        IRepository<TenantFeatureSetting, long> tenantFeatureRepository,
        EditionManager editionManager,
        IAbpZeroFeatureValueStore featureValueStore)
        : base(
            tenantRepository,
            tenantFeatureRepository,
            editionManager,
            featureValueStore)
    {
    }
}
