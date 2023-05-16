using Microsoft.Extensions.DependencyInjection;
using ONE.Abp.DataDictionary.EntityFrameworkCore;
using ONE.Abp.DataPermission.EntityFrameworkCore;
using ONE.Abp.FileManagement.EntityFrameworkCore;
using ONE.Abp.SysResource.EntityFrameworkCore;
using System;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Volo.Abp.Uow;

namespace MyCompanyName.MyProjectName.EntityFrameworkCore;

[DependsOn(
    typeof(MyProjectNameDomainModule),
    typeof(AbpIdentityEntityFrameworkCoreModule),
    typeof(AbpOpenIddictEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCorePostgreSqlModule),
    typeof(AbpBackgroundJobsEntityFrameworkCoreModule),
    typeof(AbpAuditLoggingEntityFrameworkCoreModule),
    typeof(AbpTenantManagementEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),
    typeof(AbpSysResourceEntityFrameworkCoreModule),
    typeof(AbpDataDictionaryEntityFrameworkCoreModule),
    typeof(AbpDataPermissionEntityFrameworkCoreModule),
    typeof(AbpFileManagementEntityFrameworkCoreModule)
    )]
public class MyProjectNameEntityFrameworkCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        //<TEMPLATE-REMOVE IF-NOT='dbms:PostgreSQL'>
        // https://www.npgsql.org/efcore/release-notes/6.0.html#opting-out-of-the-new-timestamp-mapping-logic
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        //</TEMPLATE-REMOVE>
        MyProjectNameEfCoreEntityExtensionMappings.Configure();
    }


    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<MyProjectNameDbContext>(options =>
        {
            /* Remove "includeAllEntities: true" to create
             * default repositories only for aggregate roots */
            options.AddDefaultRepositories(includeAllEntities: true);
        });

        context.Services.AddAbpDbContext<SharedDbContext>();

        Configure<AbpDbContextOptions>(options =>
        {
            /* The main point to change your DBMS.
             * See also MyProjectNameMigrationsDbContextFactory for EF Core tooling. */
            options.UseNpgsql();
        });

        //<TEMPLATE-REMOVE IF-NOT='dbms:SQLite'>
        Configure<AbpUnitOfWorkDefaultOptions>(options =>
        {
            options.TransactionBehavior = UnitOfWorkTransactionBehavior.Disabled;
        });
        //</TEMPLATE-REMOVE>
    }
}
