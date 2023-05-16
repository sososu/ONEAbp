using ONE.Abp.DataDictionary.Localization;
using Volo.Abp.Application.Services;

namespace ONE.Abp.DataDictionary;

public abstract class DataDictionaryAppService : ApplicationService
{
    protected DataDictionaryAppService()
    {
        LocalizationResource = typeof(DataDictionaryResource);
        ObjectMapperContext = typeof(AbpDataDictionaryApplicationModule);
    }
}
