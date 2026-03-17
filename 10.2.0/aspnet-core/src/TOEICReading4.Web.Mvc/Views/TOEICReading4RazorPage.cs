using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace TOEICReading4.Web.Views;

public abstract class TOEICReading4RazorPage<TModel> : AbpRazorPage<TModel>
{
    [RazorInject]
    public IAbpSession AbpSession { get; set; }

    protected TOEICReading4RazorPage()
    {
        LocalizationSourceName = TOEICReading4Consts.LocalizationSourceName;
    }
}
