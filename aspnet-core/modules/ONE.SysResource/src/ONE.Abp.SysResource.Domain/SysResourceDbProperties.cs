using Volo.Abp.Data;

namespace ONE.Abp.SysResource;

public static class SysResourceDbProperties
{
    public static string DbTablePrefix { get; set; } = AbpCommonDbProperties.DbTablePrefix;

    public static string DbSchema { get; set; } = AbpCommonDbProperties.DbSchema;

    public const string ConnectionStringName = "AbpSysResource";
}
