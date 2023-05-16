using ONE.Abp.DataPermission.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace ONE.Abp.DataPermission;

public abstract class DataPermissionController : AbpControllerBase
{
    protected DataPermissionController()
    {
        LocalizationResource = typeof(DataPermissionResource);
    }
}
