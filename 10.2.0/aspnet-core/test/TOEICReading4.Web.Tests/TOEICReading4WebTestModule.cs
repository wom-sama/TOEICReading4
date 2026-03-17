using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using TOEICReading4.EntityFrameworkCore;
using TOEICReading4.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace TOEICReading4.Web.Tests;

[DependsOn(
    typeof(TOEICReading4WebMvcModule),
    typeof(AbpAspNetCoreTestBaseModule)
)]
public class TOEICReading4WebTestModule : AbpModule
{
    public TOEICReading4WebTestModule(TOEICReading4EntityFrameworkModule abpProjectNameEntityFrameworkModule)
    {
        abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
    }

    public override void PreInitialize()
    {
        Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(TOEICReading4WebTestModule).GetAssembly());
    }

    public override void PostInitialize()
    {
        IocManager.Resolve<ApplicationPartManager>()
            .AddApplicationPartsIfNotAddedBefore(typeof(TOEICReading4WebMvcModule).Assembly);
    }
}