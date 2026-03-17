using TOEICReading4.Configuration.Dto;
using System.Threading.Tasks;

namespace TOEICReading4.Configuration;

public interface IConfigurationAppService
{
    Task ChangeUiTheme(ChangeUiThemeInput input);
}
