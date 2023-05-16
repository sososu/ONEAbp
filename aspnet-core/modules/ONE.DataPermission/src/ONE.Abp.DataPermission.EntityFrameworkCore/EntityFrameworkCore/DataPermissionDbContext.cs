using Microsoft.EntityFrameworkCore;
using ONE.Abp.DataPermission.Rules;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace ONE.Abp.DataPermission.EntityFrameworkCore;

[ConnectionStringName(DataPermissionDbProperties.ConnectionStringName)]
public class DataPermissionDbContext : AbpDbContext<DataPermissionDbContext>, IDataPermissionDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

      public DbSet<DataRule> DataRules { get; set; }
      public DbSet<DataTarget> DataTargets { get; set; }
      public DbSet<UserDataRule> UserDataRules { get; set; }
      public DbSet<UserRule> UserRules { get; set; }
    public DataPermissionDbContext(DbContextOptions<DataPermissionDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureDataPermission();
    }
}
