using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace ONE.Abp.Data
{
    public class ONEAbpDataModule:AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            ConfigureMagicodes(context);
        }

        /// <summary>
        /// 配置Magicodes.IE
        /// Excel导入导出
        /// </summary>
        /// <param name="context"></param>
        private void ConfigureMagicodes(ServiceConfigurationContext context)
        {
            context.Services.AddTransient<IExporter, ExcelExporter>();
            context.Services.AddTransient<IExcelExporter, ExcelExporter>();
            context.Services.AddTransient<IImporter, ExcelImporter>();
            context.Services.AddTransient<IExcelImporter, ExcelImporter>();
        }
    }
}
