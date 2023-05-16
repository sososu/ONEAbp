using System.Threading.Tasks;

namespace ONE.Admin.Data;

public interface IAdminDbSchemaMigrator
{
    Task MigrateAsync();
}
