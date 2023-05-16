using ONE.Admin.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace ONE.Admin.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class AdminController : AbpControllerBase
{
    protected AdminController()
    {
        LocalizationResource = typeof(AdminResource);
    }
}
