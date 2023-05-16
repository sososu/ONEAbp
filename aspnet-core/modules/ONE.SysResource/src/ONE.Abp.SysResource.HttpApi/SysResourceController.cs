using ONE.Abp.SysResource.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace ONE.Abp.SysResource;

public abstract class SysResourceController : AbpControllerBase
{
    protected SysResourceController()
    {
        LocalizationResource = typeof(SysResourceResource);
    }
}
