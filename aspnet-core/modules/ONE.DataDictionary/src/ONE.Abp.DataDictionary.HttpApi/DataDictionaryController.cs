using ONE.Abp.DataDictionary.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace ONE.Abp.DataDictionary;

public abstract class DataDictionaryController : AbpControllerBase
{
    protected DataDictionaryController()
    {
        LocalizationResource = typeof(DataDictionaryResource);
    }
}
