using Abp.Authorization;
using Abp.Runtime.Session;
using TOEICReading4.Configuration.Dto;
using System.Threading.Tasks;

namespace TOEICReading4.Configuration;

[AbpAuthorize]
public class ConfigurationAppService : TOEICReading4AppServiceBase, IConfigurationAppService
{
    public async Task ChangeUiTheme(ChangeUiThemeInput input)
    {
        await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
    }
}
