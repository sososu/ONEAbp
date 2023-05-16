using Microsoft.EntityFrameworkCore;
using ONE.Abp.DataPermission.Rules;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace ONE.Abp.DataPermission.EntityFrameworkCore;

public static class DataPermissionDbContextModelCreatingExtensions
{
    public static void ConfigureDataPermission(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));


        builder.Entity<UserDataRule>(b =>
        {
            //Configure table & schema name
            b.ToTable(DataPermissionDbProperties.DbTablePrefix + "UserDataRules", DataPermissionDbProperties.DbSchema);

            b.ConfigureByConvention();

            ////Properties
            //b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);

            ////Relations
            //b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

            ////Indexes
            //b.HasIndex(q => q.CreationTime);
        });

        builder.Entity<UserRule>(b =>
        {
            //Configure table & schema name
            b.ToTable(DataPermissionDbProperties.DbTablePrefix + "UserRules", DataPermissionDbProperties.DbSchema);

            b.ConfigureByConvention();

            ////Properties
            //b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);

            ////Relations
            //b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

            ////Indexes
            //b.HasIndex(q => q.CreationTime);
        });

        builder.Entity<DataRule>(b =>
        {
            //Configure table & schema name
            b.ToTable(DataPermissionDbProperties.DbTablePrefix + "DataRules", DataPermissionDbProperties.DbSchema);

            b.ConfigureByConvention();

            ////Properties
            //b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);

            ////Relations
            //b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

            ////Indexes
            //b.HasIndex(q => q.CreationTime);
        });

        builder.Entity<DataTarget>(b =>
        {
            //Configure table & schema name
            b.ToTable(DataPermissionDbProperties.DbTablePrefix + "DataTargets", DataPermissionDbProperties.DbSchema);

            b.ConfigureByConvention();
            b.HasKey(x => x.Name);
            ////Properties
            //b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);

            ////Relations
            //b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

            ////Indexes
            //b.HasIndex(q => q.CreationTime);
        });

        builder.Entity<DataTargetField>(b =>
        {
            //Configure table & schema name
            b.ToTable(DataPermissionDbProperties.DbTablePrefix + "DataTargetFields", DataPermissionDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.HasKey(qt =>new{ qt.DataTargetName, qt.Name});
            ////Properties
            //b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);

            ////Relations
            //b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

            ////Indexes
            //b.HasIndex(q => q.CreationTime);
        });

    }
}
