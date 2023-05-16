using Microsoft.EntityFrameworkCore;
using ONE.Abp.DataDictionary.DataItems;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace ONE.Abp.DataDictionary.EntityFrameworkCore;

public static class DataDictionaryDbContextModelCreatingExtensions
{
    public static void ConfigureDataDictionary(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<DataItem>(b =>
        {
            //Configure table &schema name
            b.ToTable(DataDictionaryDbProperties.DbTablePrefix + "DataItems", DataDictionaryDbProperties.DbSchema);

            b.ConfigureByConvention();

            //Properties
            b.Property(q => q.Name).IsRequired().HasMaxLength(64);

            //Indexes
            b.HasIndex(q => q.Name);
            //b.HasIndex(q => q.ParentId);
        });

    }
}
