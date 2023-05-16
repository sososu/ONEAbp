using Volo.Abp.Application.Services;
using Volo.Abp.OpenIddict.Localization;

namespace Volo.Abp.OpenIddict.Application.Volo.Abp.OpenIddict
{
    public abstract class AbpOpenIddictAppService : ApplicationService
    {
        protected AbpOpenIddictAppService()
        {
            LocalizationResource = typeof(AbpOpenIddictResource);
        }
    }
}
