using Microsoft.EntityFrameworkCore;
using ONE.Abp.SysResource.RoleMenus;
using ONE.Abp.SysResource.SaleVersions;
using ONE.Abp.SysResource.SysApps;
using ONE.Abp.SysResource.SysMenus;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace ONE.Abp.SysResource.EntityFrameworkCore;

[ConnectionStringName(SysResourceDbProperties.ConnectionStringName)]
public class SysResourceDbContext : AbpDbContext<SysResourceDbContext>, ISysResourceDbContext
{
    public DbSet<SaleVersion> SaleVersions { get; set; }
    public DbSet<SysApp> SysApps { get; set; }
    public DbSet<SysMenu> SysMenus { get; set; }
    public DbSet<RoleMenu> RoleMenus { get; set; }



    public SysResourceDbContext(DbContextOptions<SysResourceDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureSysResource();
    }
}
