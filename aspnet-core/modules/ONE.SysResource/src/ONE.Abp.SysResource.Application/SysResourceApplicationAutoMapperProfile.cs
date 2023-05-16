using AutoMapper;
using ONE.Abp.SysResource.RoleMenus;
using ONE.Abp.SysResource.SaleVersions;
using ONE.Abp.SysResource.SysApps;
using ONE.Abp.SysResource.SysMenus;
using Volo.Abp.AutoMapper;

namespace ONE.Abp.SysResource;

public class SysResourceApplicationAutoMapperProfile : Profile
{
    public SysResourceApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<SysApp,SysAppDto>();
        CreateMap<SysMenu,SysMenuDto>();
        CreateMap<SysMenuDto, MenuTree>()
            .Ignore(s=>s.Children);
        CreateMap<SaleVersion,SaleVersionDto>();
        CreateMap<SaleVersionMenu,SaleVersionMenuDto>();
    }
}
