using Microsoft.EntityFrameworkCore;
using ONE.Abp.DataPermission.Rules;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace ONE.Abp.DataPermission.EntityFrameworkCore;

[ConnectionStringName(DataPermissionDbProperties.ConnectionStringName)]
public interface IDataPermissionDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */

    public DbSet<DataRule> DataRules { get;}
    public DbSet<DataTarget> DataTargets { get;}
    public DbSet<UserDataRule> UserDataRules { get;}
    public DbSet<UserRule> UserRules { get; }
}
