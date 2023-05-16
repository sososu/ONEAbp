using AutoMapper;

namespace Volo.Abp.Identity;

public class AbpIdentityApplicationModuleAutoMapperProfile : Profile
{
    public AbpIdentityApplicationModuleAutoMapperProfile()
    {
        CreateMap<IdentityUser, IdentityUserDto>()
            .MapExtraProperties();

        CreateMap<IdentityUser, IdentityUserExtDto>()
         .MapExtraProperties()
          .ForMember(d => d.RoleNames, opt => opt.Ignore())
      .ForMember(d => d.OrganizationUnitId, opt => opt.Ignore())
      .ForMember(d => d.OrganizationUnitName, opt => opt.Ignore());

        CreateMap<IdentityRole, IdentityRoleDto>()
            .MapExtraProperties();

        CreateMap<OrganizationUnit, OrganizationUnitDto>()
         .MapExtraProperties();
    }
}
