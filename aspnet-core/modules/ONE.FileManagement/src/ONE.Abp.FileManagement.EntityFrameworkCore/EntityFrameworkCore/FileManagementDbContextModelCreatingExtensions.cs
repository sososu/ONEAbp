using Microsoft.EntityFrameworkCore;
using ONE.Abp.FileManagement.FileRecords;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace ONE.Abp.FileManagement.EntityFrameworkCore;

public static class FileManagementDbContextModelCreatingExtensions
{
    public static void ConfigureFileManagement(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<FileRecord>(b =>
        {
            //Configure table & schema name
            b.ToTable(FileManagementDbProperties.DbTablePrefix + "FileRecords", FileManagementDbProperties.DbSchema);

            b.ConfigureByConvention();

            //Properties
            b.Property(q => q.FileName).IsRequired().HasMaxLength(FileManagementConst.MaxStrLength); 
            b.Property(q => q.OriginalFileName).HasMaxLength(FileManagementConst.MaxNameLength);
            b.Property(q => q.Tag).HasMaxLength(FileManagementConst.MaxTagLength);

            //Indexes
            b.HasIndex(q => q.CreationTime);
            b.HasIndex(q => q.OriginalFileName);
            b.HasIndex(q => q.FileName);
        });

    }
}
