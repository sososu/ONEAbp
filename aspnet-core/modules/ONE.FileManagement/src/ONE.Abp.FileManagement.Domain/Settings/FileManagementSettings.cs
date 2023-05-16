namespace ONE.Abp.FileManagement.Settings;

public static class FileManagementSettings
{
    public const string GroupName = "FileManagement";

    /* Add constants for setting names. Example:
     * public const string MySettingName = GroupName + ".MySettingName";
     */

     public const string LimitSizeSettingName = GroupName + ".LimitSize";
     public const string TotalLimitSizeSettingName = GroupName + ".TotalLimitSize";
     public const string SupportMimeType = GroupName + ".SupportMIMEType";
}
