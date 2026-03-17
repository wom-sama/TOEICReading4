using System;
using System.Threading.Tasks;
using Abp;
using Abp.Authorization;
using Abp.Configuration;
using Abp.Localization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using TOEICReading4.Controllers;

namespace TOEICReading4.Web.Controllers;

[AbpAllowAnonymous]
public class LanguageController : TOEICReading4ControllerBase
{
    private readonly ILanguageManager _languageManager;

    public LanguageController(ILanguageManager languageManager)
    {
        _languageManager = languageManager;
    }

    [HttpGet]
    public async Task<IActionResult> Change(string cultureName, string returnUrl = "/")
    {
        if (string.IsNullOrWhiteSpace(cultureName))
        {
            cultureName = _languageManager.CurrentLanguage.Name;
        }

        if (AbpSession.UserId.HasValue)
        {
            await SettingManager.ChangeSettingForUserAsync(
                new UserIdentifier(AbpSession.TenantId, AbpSession.UserId.Value),
                LocalizationSettingNames.DefaultLanguage,
                cultureName
            );
        }

        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cultureName)),
            new Microsoft.AspNetCore.Http.CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                IsEssential = true,
                Path = "/"
            }
        );

        if (!Url.IsLocalUrl(returnUrl))
        {
            returnUrl = Url.Content("~/");
        }

        return LocalRedirect(returnUrl);
    }
}
