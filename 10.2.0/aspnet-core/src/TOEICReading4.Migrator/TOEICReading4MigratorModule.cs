using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using TOEICReading4.Configuration;
using TOEICReading4.EntityFrameworkCore;
using TOEICReading4.Migrator.DependencyInjection;
using Castle.MicroKernel.Registration;
using Microsoft.Extensions.Configuration;

namespace TOEICReading4.Migrator;

[DependsOn(typeof(TOEICReading4EntityFrameworkModule))]
public class TOEICReading4MigratorModule : AbpModule
{
    private readonly IConfigurationRoot _appConfiguration;

    public TOEICReading4MigratorModule(TOEICReading4EntityFrameworkModule abpProjectNameEntityFrameworkModule)
    {
        abpProjectNameEntityFrameworkModule.SkipDbSeed = true;

        _appConfiguration = AppConfigurations.Get(
            typeof(TOEICReading4MigratorModule).GetAssembly().GetDirectoryPathOrNull()
        );
    }

    public override void PreInitialize()
    {
        Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
            TOEICReading4Consts.ConnectionStringName
        );

        Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        Configuration.ReplaceService(
            typeof(IEventBus),
            () => IocManager.IocContainer.Register(
                Component.For<IEventBus>().Instance(NullEventBus.Instance)
            )
        );
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(TOEICReading4MigratorModule).GetAssembly());
        ServiceCollectionRegistrar.Register(IocManager);
    }
}
