using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace ONE.Abp.DataPermission.Tests.Datas
{
    public class DataPermissionTestDbContext : AbpDbContext<DataPermissionTestDbContext>
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */
        public DbSet<Customer> Customers { get; set; }

        public DataPermissionTestDbContext(DbContextOptions<DataPermissionTestDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Customer>(b =>
            {
                //Configure table & schema name
                b.ToTable("Test" + "Customers");

                b.ConfigureByConvention();
                b.OwnsOne(x => x.Address, y =>
                {
                    y.Property(x => x.Detial).IsRequired().HasColumnName("Detial").HasMaxLength(128);
                    y.Property(x => x.City).HasColumnName("City").HasMaxLength(32);
                    y.Property(x => x.PostalCode).HasColumnName("PostalCode").HasMaxLength(32);
                    y.Property(x => x.Region).HasColumnName("Region").HasMaxLength(32);
                    y.Property(x => x.Country).HasColumnName("Country").HasMaxLength(32);
                });
            });

            builder.Entity<Team>(b =>
            {
                //Configure table & schema name
                b.ToTable("Test" + "Teams");

                b.ConfigureByConvention();
                b.HasKey(x => new
                {
                    x.CustomerId,
                    x.Name
                });
            });

        }
    }

}
