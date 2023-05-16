using Microsoft.EntityFrameworkCore;
using ONE.Abp.SysResource.RoleMenus;
using ONE.Abp.SysResource.SaleVersions;
using ONE.Abp.SysResource.SysApps;
using ONE.Abp.SysResource.SysMenus;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace ONE.Abp.SysResource.EntityFrameworkCore;

[ConnectionStringName(SysResourceDbProperties.ConnectionStringName)]
public interface ISysResourceDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */


    public DbSet<SaleVersion> SaleVersions { get; }
    public DbSet<SysApp> SysApps { get; }
    public DbSet<SysMenu> SysMenus { get; }
    public DbSet<RoleMenu> RoleMenus { get; }
}
