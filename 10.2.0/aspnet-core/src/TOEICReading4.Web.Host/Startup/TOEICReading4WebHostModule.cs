using Abp.Modules;
using Abp.Reflection.Extensions;
using TOEICReading4.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace TOEICReading4.Web.Host.Startup
{
    [DependsOn(
       typeof(TOEICReading4WebCoreModule))]
    public class TOEICReading4WebHostModule : AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public TOEICReading4WebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TOEICReading4WebHostModule).GetAssembly());
        }
    }
}
