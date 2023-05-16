using Microsoft.EntityFrameworkCore;
using ONE.Abp.SysResource.RoleMenus;
using ONE.Abp.SysResource.SaleVersions;
using ONE.Abp.SysResource.SysApps;
using ONE.Abp.SysResource.SysMenus;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace ONE.Abp.SysResource.EntityFrameworkCore;

public static class SysResourceDbContextModelCreatingExtensions
{
    public static void ConfigureSysResource(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<SysApp>(b =>
        {
            b.ToTable(SysResourceDbProperties.DbTablePrefix + "SysApps", SysResourceDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.Property((u) => u.AppCode).IsRequired().HasMaxLength(SysResourceConst.MaxStrLength);

            b.Property((u) => u.AppName).HasMaxLength(SysResourceConst.MaxNameLength);

            b.Property((u) => u.AppSecret).HasMaxLength(SysResourceConst.MaxStrLength);

            b.Property((u) => u.AppUrl).HasMaxLength(SysResourceConst.MaxStrLength);

            b.HasIndex(u => u.AppName);
            b.HasIndex(u => u.AppCode);

            b.ApplyObjectExtensionMappings();
        });


        builder.Entity<SysMenu>(b =>
        {
            b.ToTable(SysResourceDbProperties.DbTablePrefix + "SysMenus", SysResourceDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.Property(x => x.Id).ValueGeneratedNever();

            b.Property((u) => u.Name).IsRequired().HasMaxLength(SysResourceConst.MaxStrLength);

            b.Property((u) => u.Code).HasMaxLength(SysResourceConst.MaxNameLength);
            b.Property((u) => u.ParentCode).HasMaxLength(SysResourceConst.MaxNameLength);

            b.HasIndex(uc => uc.Code);
            b.HasIndex(uc => uc.ParentCode);

            b.ApplyObjectExtensionMappings();
        });



        builder.Entity<SaleVersion>(b =>
        {
            b.ToTable(SysResourceDbProperties.DbTablePrefix + "SaleVersions", SysResourceDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.Property(x => x.Id).ValueGeneratedNever();

            b.Property((u) => u.Name).IsRequired().HasMaxLength(SysResourceConst.MaxStrLength);
            b.HasIndex(uc => uc.Name);

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<SaleVersionMenu>(b =>
        {
            b.ToTable(SysResourceDbProperties.DbTablePrefix + "SaleVersionMenus", SysResourceDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.HasKey(x => new
            {
                x.SaleVersonId,
                x.MenuCode
            });
        });

        builder.Entity<RoleMenu>(b =>
        {
            b.ToTable(SysResourceDbProperties.DbTablePrefix + "RoleMenus", SysResourceDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.Property(x => x.Id).ValueGeneratedNever();

            b.Property((u) => u.MenuCode).IsRequired().HasMaxLength(SysResourceConst.MaxNameLength);
            b.HasIndex(uc => uc.Role);
            b.HasIndex(uc => uc.MenuCode);
            b.HasIndex(uc => uc.AppId);

            b.ApplyObjectExtensionMappings();
        });

    }
}
