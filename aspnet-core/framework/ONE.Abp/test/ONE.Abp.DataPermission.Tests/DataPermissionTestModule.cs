using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using ONE.Abp.Data.Rules;
using ONE.Abp.DataPermission.Extension;
using ONE.Abp.DataPermission.Tests.Datas;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Modularity;

namespace ONE.Abp.DataPermission.Tests
{
    [DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpTestBaseModule),
    typeof(AbpDataPermissionExtensionModule),
    typeof(AbpEntityFrameworkCoreSqliteModule),
    typeof(AbpDddDomainModule)
    )]
    public class DataPermissionTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

            Configure<AbpRuleOptions>(option =>
            {
                option.DataTargetOption.Add<Customer>(needUpdateShadowProperty:true);

            });

            context.Services.AddAbpDbContext<DataPermissionTestDbContext>(options =>
            {
                options.AddDefaultRepositories();
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
            });

            var sqliteConnection = CreateDatabaseAndGetConnection();

            Configure<AbpDbContextOptions>(options =>
            {
                options.Configure(abpDbContextConfigurationContext =>
                {
                    abpDbContextConfigurationContext.DbContextOptions.UseSqlite(sqliteConnection);
                });
            });

            context.Services.AddAutoMapperObjectMapper<DataPermissionTestModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<DataPermissionTestModule>(validate: true);
            });
        }

        private static SqliteConnection CreateDatabaseAndGetConnection()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            new DataPermissionTestDbContext(
                new DbContextOptionsBuilder<DataPermissionTestDbContext>().UseSqlite(connection).Options
            ).GetService<IRelationalDatabaseCreator>().CreateTables();

            return connection;
        }
    }
}
