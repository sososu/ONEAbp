using ONE.Abp.SysResource.Localization;
using Volo.Abp.Application.Services;

namespace ONE.Abp.SysResource;

public abstract class SysResourceAppService : ApplicationService
{
    protected SysResourceAppService()
    {
        LocalizationResource = typeof(SysResourceResource);
        ObjectMapperContext = typeof(AbpSysResourceApplicationModule);
    }
}
