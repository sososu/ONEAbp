using Volo.Abp.Data;

namespace ONE.Abp.DataPermission;

public static class DataPermissionDbProperties
{
    public static string DbTablePrefix { get; set; } = AbpCommonDbProperties.DbTablePrefix;

    public static string DbSchema { get; set; } = AbpCommonDbProperties.DbSchema;

    public const string ConnectionStringName = "AbpDataPermission";
}
