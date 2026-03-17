using Abp.AspNetCore.Mvc.ViewComponents;

namespace TOEICReading4.Web.Views;

public abstract class TOEICReading4ViewComponent : AbpViewComponent
{
    protected TOEICReading4ViewComponent()
    {
        LocalizationSourceName = TOEICReading4Consts.LocalizationSourceName;
    }
}
