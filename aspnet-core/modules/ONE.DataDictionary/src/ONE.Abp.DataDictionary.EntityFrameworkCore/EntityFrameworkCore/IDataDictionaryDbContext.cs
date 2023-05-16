using Microsoft.EntityFrameworkCore;
using ONE.Abp.DataDictionary.DataItems;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace ONE.Abp.DataDictionary.EntityFrameworkCore;

[ConnectionStringName(DataDictionaryDbProperties.ConnectionStringName)]
public interface IDataDictionaryDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */

    public DbSet<DataItem> DataItems { get; set; }
}
