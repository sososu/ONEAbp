using Microsoft.EntityFrameworkCore;
using ONE.Abp.SysResource.SaleVersions;
using ONE.Abp.SysResource.SysApps;
using ONE.Abp.SysResource.SysMenus;
using System.Linq;

namespace ONE.Abp.SysResource.EntityFrameworkCore
{
    public static class SysResourceEfCoreQueryableExtensions
    {
        public static IQueryable<SaleVersion> IncludeDetails(this IQueryable<SaleVersion> queryable, bool include = true)
        {
            if (!include)
            {
                return queryable;
            }

            return queryable
                .Include(x => x.Menus);
        }

        public static IQueryable<SysApp> IncludeDetails(this IQueryable<SysApp> queryable, bool include = true)
        {
            return queryable;
        }

        public static IQueryable<SysMenu> IncludeDetails(this IQueryable<SysMenu> queryable, bool include = true)
        {
            return queryable;
        }
    }
}
