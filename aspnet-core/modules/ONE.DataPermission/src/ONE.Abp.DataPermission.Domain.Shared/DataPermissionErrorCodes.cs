namespace ONE.Abp.DataPermission;

public static class DataPermissionErrorCodes
{
    //Add your business exception error codes here...

    public static string ExistRule = "DataPermission:1001";

    public static string DataTargetNameInconsistent = "DataPermission:1002";
    public const string DuplicateUserDataRule = "DataPermission:1003";

    public static string ExistDataRuleName = "DataPermission:1004";

    public static string ExistUserRuleName = "DataPermission:1005";
}
