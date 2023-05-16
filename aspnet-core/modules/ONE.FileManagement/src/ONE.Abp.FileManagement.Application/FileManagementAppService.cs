using ONE.Abp.FileManagement.Localization;
using Volo.Abp.Application.Services;

namespace ONE.Abp.FileManagement;

public abstract class FileManagementAppService : ApplicationService
{
    protected FileManagementAppService()
    {
        LocalizationResource = typeof(FileManagementResource);
        ObjectMapperContext = typeof(AbpFileManagementApplicationModule);
    }
}
