using AutoMapper;
using Volo.Abp.OpenIddict.Application.Contracts.Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Scopes;

namespace Volo.Abp.OpenIddict.Application.Volo.Abp.OpenIddict
{
    public class AbpOpeniddictApplicationModuleAutoMapperProfile : Profile
    {
        public AbpOpeniddictApplicationModuleAutoMapperProfile()
        {
            CreateMap<OpenIddictScopeModel, OpenIddictScopeDto>();
            CreateMap<OpenIddictApplicationModel, OpenIddictApplicationDto>();
        }
    }
}
