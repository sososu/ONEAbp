using Microsoft.EntityFrameworkCore;
using ONE.Abp.DataDictionary.DataItems;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace ONE.Abp.DataDictionary.EntityFrameworkCore;

[ConnectionStringName(DataDictionaryDbProperties.ConnectionStringName)]
public class DataDictionaryDbContext : AbpDbContext<DataDictionaryDbContext>, IDataDictionaryDbContext
{
    public DbSet<DataItem> DataItems { get; set; }
    public DataDictionaryDbContext(DbContextOptions<DataDictionaryDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureDataDictionary();
    }
}
