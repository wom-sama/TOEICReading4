using Abp.AspNetCore.Mvc.Authorization;
using TOEICReading4.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace TOEICReading4.Web.Controllers;

[AbpMvcAuthorize]
public class AboutController : TOEICReading4ControllerBase
{
    public ActionResult Index()
    {
        return View();
    }
}
