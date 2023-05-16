using Microsoft.EntityFrameworkCore;
using ONE.Abp.DataDictionary.EntityFrameworkCore;
using ONE.Abp.DataPermission.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace MyCompanyName.MyProjectName.EntityFrameworkCore
{
    [ConnectionStringName("AbpPermissionManagement")]
    public class SharedDbContext : AbpDbContext<SharedDbContext>
    {
        public SharedDbContext(
            DbContextOptions<SharedDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /* Include modules to your migration db context */
            builder.ConfigureSettingManagement();
            builder.ConfigureFeatureManagement();
            builder.ConfigureAuditLogging();
            builder.ConfigurePermissionManagement();
            builder.ConfigureDataDictionary();
            builder.ConfigureDataPermission();
        }
    }
}
