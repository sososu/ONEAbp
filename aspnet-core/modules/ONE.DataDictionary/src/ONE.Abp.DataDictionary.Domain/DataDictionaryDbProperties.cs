using Volo.Abp.Data;

namespace ONE.Abp.DataDictionary;

public static class DataDictionaryDbProperties
{
    public static string DbTablePrefix { get; set; } = AbpCommonDbProperties.DbTablePrefix;

    public static string DbSchema { get; set; } = AbpCommonDbProperties.DbSchema;

    public const string ConnectionStringName = "AbpDataDictionary";
}
