using Microsoft.EntityFrameworkCore;
using ONE.Abp.FileManagement.FileRecords;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace ONE.Abp.FileManagement.EntityFrameworkCore;

[ConnectionStringName(FileManagementDbProperties.ConnectionStringName)]
public interface IFileManagementDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */

    public DbSet<FileRecord> FileRecords { get;}
}
