using ONE.Abp.DataPermission.Localization;
using Volo.Abp.Application.Services;

namespace ONE.Abp.DataPermission;

public abstract class DataPermissionAppService : ApplicationService
{
    protected DataPermissionAppService()
    {
        LocalizationResource = typeof(DataPermissionResource);
        ObjectMapperContext = typeof(AbpDataPermissionApplicationModule);
    }
}
