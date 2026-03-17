using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using TOEICReading4.Authorization;

namespace TOEICReading4;

[DependsOn(
    typeof(TOEICReading4CoreModule),
    typeof(AbpAutoMapperModule))]
public class TOEICReading4ApplicationModule : AbpModule
{
    public override void PreInitialize()
    {
        Configuration.Authorization.Providers.Add<TOEICReading4AuthorizationProvider>();
    }

    public override void Initialize()
    {
        var thisAssembly = typeof(TOEICReading4ApplicationModule).GetAssembly();

        IocManager.RegisterAssemblyByConvention(thisAssembly);

        Configuration.Modules.AbpAutoMapper().Configurators.Add(
            // Scan the assembly for classes which inherit from AutoMapper.Profile
            cfg => cfg.AddMaps(thisAssembly)
        );
    }
}
