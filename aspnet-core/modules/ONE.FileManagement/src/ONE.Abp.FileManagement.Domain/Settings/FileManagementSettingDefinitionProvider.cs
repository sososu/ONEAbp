using Volo.Abp.Settings;

namespace ONE.Abp.FileManagement.Settings;

public class FileManagementSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        /* Define module settings here.
         * Use names from FileManagementSettings class.
         */

        context.Add(new SettingDefinition(FileManagementSettings.TotalLimitSizeSettingName, "10737418240")); //10g
        context.Add(new SettingDefinition(FileManagementSettings.LimitSizeSettingName, "5242880")); //5mb

        context.Add(new SettingDefinition(FileManagementSettings.SupportMimeType, @"image/jpeg,image/png,image/bmp,image/gif,text/plain,application/msword,application/vnd.openxmlformats-officedocument.wordprocessingml.document，
application/vnd.ms-excel,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet,application/pdf"));
    }
}
