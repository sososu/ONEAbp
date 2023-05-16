using Microsoft.Extensions.Logging;
using System.Text;
using System.Threading.Tasks;
using System;
using Volo.Abp.Cli;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Bundling;
using Volo.Abp.Cli.Commands;
using Volo.Abp.Cli.Commands.Services;
using Volo.Abp.Cli.LIbs;
using Volo.Abp.Cli.ProjectBuilding;
using Volo.Abp.Cli.ProjectModification;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Cli.ProjectBuilding.Events;
using Volo.Abp.Cli.ProjectBuilding.Templates.App;
using Volo.Abp.Cli.ProjectBuilding.Templates.Microservice;
using Volo.Abp.Cli.ProjectBuilding.Templates.Module;
using templates;
using System.IO;
using Volo.Abp.Cli.ProjectBuilding.Building;
using System.Linq;
using System.Collections.Generic;

namespace TemplateBuilders
{
    public class ONENewCommand : ProjectCreationCommandBase, IConsoleCommand, ITransientDependency
    {
        public const string Name = "new";

        protected ONETemplateProjectBuilder TemplateProjectBuilder { get; }
        protected ONEInitialMigrationCreator ONEInitialMigrationCreator { get; }
        public ITemplateInfoProvider TemplateInfoProvider { get; }

        public ONENewCommand(
            ConnectionStringProvider connectionStringProvider,
            SolutionPackageVersionFinder solutionPackageVersionFinder,
            ICmdHelper cmdHelper,
            IInstallLibsService installLibsService,
            CliService cliService,
            AngularPwaSupportAdder angularPwaSupportAdder,
            InitialMigrationCreator initialMigrationCreator,
            ThemePackageAdder themePackageAdder,
            ILocalEventBus eventBus,
            IBundlingService bundlingService,
            ITemplateInfoProvider templateInfoProvider,
            ONETemplateProjectBuilder templateProjectBuilder,
            AngularThemeConfigurer angularThemeConfigurer,
            ONEInitialMigrationCreator oNEInitialMigrationCreator) :
            base(connectionStringProvider,
                solutionPackageVersionFinder,
                cmdHelper,
                installLibsService,
                cliService,
                angularPwaSupportAdder,
                initialMigrationCreator,
                themePackageAdder,
                eventBus,
                bundlingService,
                angularThemeConfigurer)
        {
            TemplateInfoProvider = templateInfoProvider;
            TemplateProjectBuilder = templateProjectBuilder;
            ONEInitialMigrationCreator = oNEInitialMigrationCreator;
        }

        public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            var projectName = NamespaceHelper.NormalizeNamespace(commandLineArgs.Target);
            if (string.IsNullOrWhiteSpace(projectName))
            {
                throw new CliUsageException("Project name is missing!" + Environment.NewLine + Environment.NewLine + GetUsageInfo());
            }

            ProjectNameValidator.Validate(projectName);

            Logger.LogInformation("Creating your project...");
            Logger.LogInformation("Project name: " + projectName);

            var template = commandLineArgs.Options.GetOrNull(Options.Template.Short, Options.Template.Long);
            if (template != null)
            {
                Logger.LogInformation("Template: " + template);
            }
            else
            {
                template = (await TemplateInfoProvider.GetDefaultAsync()).Name;
            }

            var isTiered = commandLineArgs.Options.ContainsKey(Options.Tiered.Long);
            if (isTiered)
            {
                Logger.LogInformation("Tiered: yes");
            }

            var projectArgs = await GetProjectBuildArgsAsync(commandLineArgs, template, projectName);

           // var tempBuilder = TemplateProjectBuilder as ONETemplateProjectBuilder;
            var result = await TemplateProjectBuilder.BuildAsync(
                projectArgs
            );

            ExtractProjectZip(result, projectArgs.OutputFolder);

            Logger.LogInformation($"'{projectName}' has been successfully created to '{projectArgs.OutputFolder}'");

            ConfigureAngularJsonForThemeSelection(projectArgs);
            ConfigureNpmPackagesForTheme(projectArgs);
            await RunGraphBuildForMicroserviceServiceTemplate(projectArgs);
            await CreateInitialMigrationsAsync(projectArgs, projectName);

            var skipInstallLibs = commandLineArgs.Options.ContainsKey(Options.SkipInstallingLibs.Long) || commandLineArgs.Options.ContainsKey(Options.SkipInstallingLibs.Short);
            if (!skipInstallLibs)
            {
                await RunInstallLibsForWebTemplateAsync(projectArgs);
            }

            //var skipBundling = commandLineArgs.Options.ContainsKey(Options.SkipBundling.Long) || commandLineArgs.Options.ContainsKey(Options.SkipBundling.Short);
            //if (!skipBundling)
            //{
            //    await RunBundleForBlazorWasmTemplateAsync(projectArgs);
            //}

            await ConfigurePwaSupportForAngular(projectArgs);

            OpenRelatedWebPage(projectArgs, template, isTiered, commandLineArgs);
        }

        protected async Task RunInstallLibsForWebTemplateAsync(ProjectBuildArgs projectArgs)
        {
            if (projectArgs.TemplateName == OneAppTemplate.TemplateName || AppTemplateBase.IsAppTemplate(projectArgs.TemplateName) || ModuleTemplateBase.IsModuleTemplate(projectArgs.TemplateName) || AppNoLayersTemplateBase.IsAppNoLayersTemplate(projectArgs.TemplateName) || MicroserviceServiceTemplateBase.IsMicroserviceTemplate(projectArgs.TemplateName))
            {
                Logger.LogInformation("Installing client-side packages...");
                await EventBus.PublishAsync(new ProjectCreationProgressEvent
                {
                    Message = "Installing client-side packages"
                }, onUnitOfWorkComplete: false).ConfigureAwait(continueOnCapturedContext: false);
                await InstallLibsService.InstallLibsAsync(projectArgs.OutputFolder).ConfigureAwait(continueOnCapturedContext: false);
            }
        }

        public string GetUsageInfo()
        {
            var sb = new StringBuilder();

            sb.AppendLine("");
            sb.AppendLine("Usage:");
            sb.AppendLine("");
            sb.AppendLine("  oneabp new <project-name> [options]");
            sb.AppendLine("");
            sb.AppendLine("Options:");
            sb.AppendLine("");
            sb.AppendLine("-t|--template <template-name>               (default: base)");
            sb.AppendLine("-d|--database-provider <database-provider>  (if supported by the template)");
            sb.AppendLine("-o|--output-folder <output-folder>          (default: current folder)");
            sb.AppendLine("-v|--version <version>                      (default: latest version)");
            sb.AppendLine("-cs|--connection-string <connection-string> (your database connection string)");
            sb.AppendLine("--dbms <database-management-system>         (your database management system)");
       
            //sb.AppendLine("-sib|--skip-installing-libs                      (Doesn't run `abp install-libs` command after project creation)");
            sb.AppendLine("");
            sb.AppendLine("Examples:");
            sb.AppendLine("");
            sb.AppendLine("oneabp new Acme.BookStore -t base -d ef -dbms postgresql");
            sb.AppendLine("oneabp new Acme.BookStore -t micro -d ef -dbms postgresql");
            sb.AppendLine("oneabp new Acme.BookStore -t micro --connection-string \"Server=myServerName\\myInstanceName;Database=myDatabase;User Id=myUsername;Password=myPassword\"");

            sb.AppendLine("");
            sb.AppendLine("See the documentation for more info: https://docs.abp.io/en/abp/latest/CLI");

            return sb.ToString();
        }


        public string GetShortDescription()
        {
            return "Generate a new solution based on the ABP startup templates.";
        }


        protected async Task CreateInitialMigrationsAsync(ProjectBuildArgs projectArgs,string projectName)
        {
            if (projectArgs.DatabaseProvider == DatabaseProvider.MongoDb)
            {
                return;
            }

            var efCoreProjectPath = string.Empty;
            bool isLayeredTemplate;

            switch (projectArgs.TemplateName)
            {
                case AppTemplate.TemplateName:
                case AppProTemplate.TemplateName:
                case MicroTemplate.TemplateName:
                case OneAppTemplate.TemplateName:
                    efCoreProjectPath = Directory.GetFiles(projectArgs.OutputFolder, "*EntityFrameworkCore.csproj", SearchOption.AllDirectories).FirstOrDefault();
                    isLayeredTemplate = true;
                    break;
                case AppNoLayersTemplate.TemplateName:
                case AppNoLayersProTemplate.TemplateName:
                    efCoreProjectPath = Directory.GetFiles(projectArgs.OutputFolder, "*.Host.csproj", SearchOption.AllDirectories).FirstOrDefault()
                        ?? Directory.GetFiles(projectArgs.OutputFolder, "*.csproj", SearchOption.AllDirectories).FirstOrDefault();
                    isLayeredTemplate = false;
                    break;
                default:
                    return;
            }

            if (string.IsNullOrWhiteSpace(efCoreProjectPath))
            {
                Logger.LogWarning("Couldn't find the project to create initial migrations!");
                return;
            }

            await EventBus.PublishAsync(new ProjectCreationProgressEvent
            {
                Message = "Creating the initial DB migration"
            }, false);

            if(projectArgs.TemplateName== OneAppTemplate.TemplateName) //特殊 包含2个DbContext
            {
                var pre = projectName.Split('.', StringSplitOptions.RemoveEmptyEntries)[1];
                await ONEInitialMigrationCreator.CreateAsync(Path.GetDirectoryName(efCoreProjectPath), isLayeredTemplate, new List<string> { "SharedDbContext", $"{pre}DbContext" });
            }
            else
                await InitialMigrationCreator.CreateAsync(Path.GetDirectoryName(efCoreProjectPath), isLayeredTemplate);
        }
    }
}
