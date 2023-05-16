using ONE.Abp.FileManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace ONE.Abp.FileManagement;

public abstract class FileManagementController : AbpControllerBase
{
    protected FileManagementController()
    {
        LocalizationResource = typeof(FileManagementResource);
    }
}
