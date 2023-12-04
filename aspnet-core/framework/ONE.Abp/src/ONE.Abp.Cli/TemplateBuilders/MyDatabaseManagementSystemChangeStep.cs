using System;
using System.Linq;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Files;

namespace TemplateBuilders
{
    public class MyDatabaseManagementSystemChangeStep : ProjectBuildPipelineStep
    {
        private readonly bool _hasDbMigrations;

        public MyDatabaseManagementSystemChangeStep(bool hasDbMigrations)
        {
            _hasDbMigrations = hasDbMigrations;
        }

        public override void Execute(ProjectBuildContext context)
        {
            switch (context.BuildArgs.DatabaseManagementSystem)
            {
                case DatabaseManagementSystem.MySQL:
                    ChangeEntityFrameworkCoreDependency(context, "Volo.Abp.EntityFrameworkCore.MySQL",
                        "Volo.Abp.EntityFrameworkCore.MySQL",
                        "AbpEntityFrameworkCoreMySQLModule");
                    AddMySqlServerVersion(context);
                    ChangeUseSqlServer(context, "UseMySQL", "UseMySql");
                    break;

                case DatabaseManagementSystem.PostgreSQL:
                    ChangeEntityFrameworkCoreDependency(context, "Volo.Abp.EntityFrameworkCore.PostgreSql",
                        "Volo.Abp.EntityFrameworkCore.PostgreSql",
                        "AbpEntityFrameworkCorePostgreSqlModule");
                    ChangeUseSqlServer(context, "UseNpgsql");
                    break;

                case DatabaseManagementSystem.Oracle:
                    ChangeEntityFrameworkCoreDependency(context, "Volo.Abp.EntityFrameworkCore.Oracle",
                        "Volo.Abp.EntityFrameworkCore.Oracle",
                        "AbpEntityFrameworkCoreOracleModule");
                    AdjustOracleDbContextOptionsBuilder(context);
                    ChangeUseSqlServer(context, "UseOracle");
                    break;

                case DatabaseManagementSystem.OracleDevart:
                    ChangeEntityFrameworkCoreDependency(context, "Volo.Abp.EntityFrameworkCore.Oracle.Devart",
                        "Volo.Abp.EntityFrameworkCore.Oracle.Devart",
                        "AbpEntityFrameworkCoreOracleDevartModule");
                    AdjustOracleDbContextOptionsBuilder(context);
                    ChangeUseSqlServer(context, "UseOracle");
                    break;

                case DatabaseManagementSystem.SQLite:
                    ChangeEntityFrameworkCoreDependency(context, "Volo.Abp.EntityFrameworkCore.Sqlite",
                        "Volo.Abp.EntityFrameworkCore.Sqlite",
                        "AbpEntityFrameworkCoreSqliteModule");
                    ChangeUseSqlServer(context, "UseSqlite");
                    break;

                default:
                    return;
            }
        }

        private void AdjustOracleDbContextOptionsBuilder(ProjectBuildContext context)
        {
            var dbContextFactoryFile = context.Files.FirstOrDefault(f => f.Name.EndsWith($"{(_hasDbMigrations ? "Migrations" : string.Empty)}DbContextFactoryBase.cs", StringComparison.OrdinalIgnoreCase))
                                       ?? context.Files.FirstOrDefault(f => f.Name.EndsWith($"{(_hasDbMigrations ? "Migrations" : string.Empty)}DbContextFactory.cs", StringComparison.OrdinalIgnoreCase));

            dbContextFactoryFile?.ReplaceText("new DbContextOptionsBuilder",
                $"(DbContextOptionsBuilder<{context.BuildArgs.SolutionName.ProjectName}{(_hasDbMigrations ? "Migrations" : string.Empty)}DbContext>) new DbContextOptionsBuilder");
        }

        private void AddMySqlServerVersion(ProjectBuildContext context)
        {
            var dbContextFactoryFiles = context.Files.Where(f => f.Name.EndsWith($"{(_hasDbMigrations ? "Migrations" : string.Empty)}DbContextFactory.cs", StringComparison.OrdinalIgnoreCase));

            foreach (var dbContextFactoryFile in dbContextFactoryFiles)
            {
                if (dbContextFactoryFile.Name.EndsWith("SharedDbContextFactory.cs"))
                {
                    dbContextFactoryFile?.ReplaceText("configuration.GetConnectionString(\"AbpPermissionManagement\")",
          "configuration.GetConnectionString(\"AbpPermissionManagement\"), MySqlServerVersion.LatestSupportedServerVersion");
                }
                else
                {
                    dbContextFactoryFile?.ReplaceText("configuration.GetConnectionString(\"Default\")",
           "configuration.GetConnectionString(\"Default\"), MySqlServerVersion.LatestSupportedServerVersion");
                }
            }
        }

        private void ChangeEntityFrameworkCoreDependency(ProjectBuildContext context, string newPackageName, string newModuleNamespace, string newModuleClass)
        {
            var efCoreProjectFile = context.Files.FirstOrDefault(f => f.Name.EndsWith("EntityFrameworkCore.csproj", StringComparison.OrdinalIgnoreCase));
            efCoreProjectFile?.ReplaceText("Volo.Abp.EntityFrameworkCore.SqlServer", newPackageName);

            var efCoreModuleClass = context.Files.FirstOrDefault(f => f.Name.EndsWith("EntityFrameworkCoreModule.cs", StringComparison.OrdinalIgnoreCase));
            efCoreModuleClass?.ReplaceText("Volo.Abp.EntityFrameworkCore.SqlServer", newModuleNamespace);
            efCoreModuleClass?.ReplaceText("AbpEntityFrameworkCoreSqlServerModule", newModuleClass);
        }

        private void ChangeUseSqlServer(ProjectBuildContext context, string newUseMethodForEfModule, string newUseMethodForDbContext = null)
        {
            if (newUseMethodForDbContext == null)
            {
                newUseMethodForDbContext = newUseMethodForEfModule;
            }

            var oldUseMethod = "UseSqlServer";

            var efCoreModuleClass = context.Files.FirstOrDefault(f => f.Name.EndsWith("EntityFrameworkCoreModule.cs", StringComparison.OrdinalIgnoreCase));

            if (efCoreModuleClass == null)
            {
                return;
            }

            efCoreModuleClass.ReplaceText(oldUseMethod, newUseMethodForEfModule);

            var dbContextFactoryFiles = context.Files.Where(f => f.Name.EndsWith($"{(_hasDbMigrations ? "Migrations" : string.Empty)}DbContextFactory.cs", StringComparison.OrdinalIgnoreCase));

            foreach (var dbContextFactoryFile in dbContextFactoryFiles)
            {
                dbContextFactoryFile?.ReplaceText(oldUseMethod, newUseMethodForDbContext);
            }
        }
    }
}
