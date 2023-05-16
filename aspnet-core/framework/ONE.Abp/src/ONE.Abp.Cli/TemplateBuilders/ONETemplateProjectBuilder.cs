﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using templates;
using Volo.Abp;
using Volo.Abp.Cli;
using Volo.Abp.Cli.Commands;
using Volo.Abp.Cli.Licensing;
using Volo.Abp.Cli.ProjectBuilding;
using Volo.Abp.Cli.ProjectBuilding.Analyticses;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Events;
using Volo.Abp.Cli.ProjectBuilding.Templates.App;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Json;

namespace TemplateBuilders
{
    [Dependency(ReplaceServices =true)]
    [ExposeServices(typeof(TemplateProjectBuilder),typeof(ONETemplateProjectBuilder))]
    public class ONETemplateProjectBuilder : TemplateProjectBuilder, IProjectBuilder, ITransientDependency
    { 
        public ILogger<ONETemplateProjectBuilder> Logger { get; set; }

        private readonly IConfiguration _configuration;

        public ONETemplateProjectBuilder(ISourceCodeStore sourceCodeStore, ITemplateInfoProvider templateInfoProvider, ICliAnalyticsCollect cliAnalyticsCollect, IOptions<AbpCliOptions> options, IJsonSerializer jsonSerializer, IApiKeyService apiKeyService, IConfiguration configuration, ILocalEventBus eventBus) : base(sourceCodeStore, templateInfoProvider, cliAnalyticsCollect, options, jsonSerializer, apiKeyService, configuration, eventBus)
        {
            _configuration= configuration;
        }

        public new async Task<ProjectBuildResult> BuildAsync(ProjectBuildArgs args)
        {
            var templateInfo = await GetTemplateInfoAsync(args);

            NormalizeArgs(args, templateInfo);

            await EventBus.PublishAsync(new ProjectCreationProgressEvent
            {
                Message = "Downloading the solution template"
            }, false);

            var templateFile = await SourceCodeStore.GetAsync(
                args.TemplateName,
                SourceCodeTypes.Template,
                args.Version,
                args.TemplateSource,
                args.ExtraProperties.ContainsKey(NewCommand.Options.Preview.Long)
            );

            ConfigureThemeOptions(args, templateFile.Version);

            DeveloperApiKeyResult apiKeyResult = null;

#if DEBUG
            try
            {
                var apiKeyResultSection = _configuration.GetSection("apiKeyResult");
                if (apiKeyResultSection.Exists())
                {
                    apiKeyResult = apiKeyResultSection.Get<DeveloperApiKeyResult>(); //you can use user secrets
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            if (apiKeyResult == null)
            {
                apiKeyResult = await ApiKeyService.GetApiKeyOrNullAsync();
            }
#else
            apiKeyResult = await ApiKeyService.GetApiKeyOrNullAsync();
#endif

            if (apiKeyResult != null)
            {
                if (apiKeyResult.ApiKey != null)
                {
                    args.ExtraProperties["api-key"] = apiKeyResult.ApiKey;
                }
                else if (templateInfo.Name == AppProTemplate.TemplateName)
                {
                    throw new UserFriendlyException(apiKeyResult.ErrorMessage);
                }
            }

            if (apiKeyResult?.LicenseCode != null)
            {
                args.ExtraProperties["license-code"] = apiKeyResult.LicenseCode;
            }

            var context = new ProjectBuildContext(
                templateInfo,
                null,
                null,
                null,
                templateFile,
                args
            );

            if (context.Template is AppTemplateBase appTemplateBase)
            {
                appTemplateBase.HasDbMigrations = SemanticVersion.Parse(templateFile.Version) < new SemanticVersion(4, 3, 99);
            }
            if (context.Template is OneAppTemplate oneTemplateBase)
            {
                oneTemplateBase.HasDbMigrations = SemanticVersion.Parse(templateFile.Version) < new SemanticVersion(4, 3, 99);
            }
            if (context.Template is MicroTemplate microTemplateBase)
            {
                microTemplateBase.HasDbMigrations = SemanticVersion.Parse(templateFile.Version) < new SemanticVersion(4, 3, 99);
            }

            await EventBus.PublishAsync(new ProjectCreationProgressEvent
            {
                Message = "Customizing the solution template"
            }, false);

            var pine = ONETemplateProjectBuildPipelineBuilder.Build(context);
            pine.Execute();

            if (!templateInfo.DocumentUrl.IsNullOrEmpty())
            {
                Logger.LogInformation("Check out the documents at " + templateInfo.DocumentUrl);
            }

            // Exclude unwanted or known options.
            var options = args.ExtraProperties
                .Where(x => !x.Key.Equals(CliConsts.Command, StringComparison.InvariantCultureIgnoreCase))
                .Where(x => !x.Key.Equals(NewCommand.Options.Tiered.Long, StringComparison.InvariantCultureIgnoreCase))
                .Where(x => !x.Key.Equals(NewCommand.Options.Preview.Long, StringComparison.InvariantCultureIgnoreCase))
                .Where(x => !x.Key.Equals(NewCommand.Options.DatabaseProvider.Long, StringComparison.InvariantCultureIgnoreCase) &&
                            !x.Key.Equals(NewCommand.Options.DatabaseProvider.Short, StringComparison.InvariantCultureIgnoreCase))
                .Where(x => !x.Key.Equals(NewCommand.Options.OutputFolder.Long, StringComparison.InvariantCultureIgnoreCase) &&
                            !x.Key.Equals(NewCommand.Options.OutputFolder.Short, StringComparison.InvariantCultureIgnoreCase))
                .Where(x => !x.Key.Equals(NewCommand.Options.UiFramework.Long, StringComparison.InvariantCultureIgnoreCase) &&
                            !x.Key.Equals(NewCommand.Options.UiFramework.Short, StringComparison.InvariantCultureIgnoreCase))
                .Where(x => !x.Key.Equals(NewCommand.Options.Mobile.Long, StringComparison.InvariantCultureIgnoreCase) &&
                            !x.Key.Equals(NewCommand.Options.Mobile.Short, StringComparison.InvariantCultureIgnoreCase))
                .Where(x => !x.Key.Equals(NewCommand.Options.Version.Long, StringComparison.InvariantCultureIgnoreCase) &&
                            !x.Key.Equals(NewCommand.Options.Version.Short, StringComparison.InvariantCultureIgnoreCase))
                .Where(x => !x.Key.Equals(NewCommand.Options.TemplateSource.Short, StringComparison.InvariantCultureIgnoreCase) &&
                            !x.Key.Equals(NewCommand.Options.TemplateSource.Long, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Key).ToList();

            await CliAnalyticsCollect.CollectAsync(new CliAnalyticsCollectInputDto
            {
                Tool = Options.ToolName,
                Command = args.ExtraProperties.ContainsKey(CliConsts.Command) ? args.ExtraProperties[CliConsts.Command] : "",
                DatabaseProvider = args.DatabaseProvider.ToProviderName(),
                IsTiered = args.ExtraProperties.ContainsKey(NewCommand.Options.Tiered.Long),
                UiFramework = args.UiFramework.ToFrameworkName(),
                Options = JsonSerializer.Serialize(options),
                ProjectName = args.SolutionName.FullName,
                TemplateName = args.TemplateName,
                TemplateVersion = templateFile.Version
            });

            return new ProjectBuildResult(context.Result.ZipContent, args.SolutionName.ProjectName);
        }

        private static void NormalizeArgs(ProjectBuildArgs args, TemplateInfo templateInfo)
        {
            if (args.TemplateName.IsNullOrEmpty())
            {
                args.TemplateName = templateInfo.Name;
            }

            if (args.DatabaseProvider == DatabaseProvider.NotSpecified)
            {
                if (templateInfo.DefaultDatabaseProvider != DatabaseProvider.NotSpecified)
                {
                    args.DatabaseProvider = templateInfo.DefaultDatabaseProvider;
                }
            }

            if (args.UiFramework == UiFramework.NotSpecified)
            {
                if (templateInfo.DefaultUiFramework != UiFramework.NotSpecified)
                {
                    args.UiFramework = templateInfo.DefaultUiFramework;
                }
            }
        }

        private async Task<TemplateInfo> GetTemplateInfoAsync(ProjectBuildArgs args)
        {
            if (args.TemplateName.IsNullOrWhiteSpace())
            {
                return await TemplateInfoProvider.GetDefaultAsync();
            }
            else
            {
                return TemplateInfoProvider.Get(args.TemplateName);
            }
        }

        private bool IsThemeOptionEnabled(ProjectBuildArgs args, string templateVersion)
        {
            var version = string.IsNullOrWhiteSpace(args.Version)
                ? templateVersion
                : args.Version;

            if (!SemanticVersion.TryParse(version, out var semanticVersion))
            {
                return false;
            }

            return semanticVersion >= SemanticVersion.Parse("6.0.0-rc.1");
        }

        private void ConfigureThemeOptions(ProjectBuildArgs args, string templateVersion)
        {
            if (!IsThemeOptionEnabled(args, templateVersion))
            {
                args.Theme = null;
                args.ThemeStyle = null;
            }

            if (args.Theme.HasValue)
            {
                Logger.LogInformation("Theme: " + args.Theme);

                var isProTemplate = !args.TemplateName.IsNullOrEmpty() && args.TemplateName.EndsWith("-pro", StringComparison.OrdinalIgnoreCase);

                if (args.UiFramework == UiFramework.Angular && ((isProTemplate && args.Theme != AppProTemplate.DefaultTheme) ||
                                                                (!isProTemplate && args.Theme != AppTemplate.DefaultTheme)))
                {
                    Logger.LogWarning("You may need to make some additional changes for this theme. " +
                                      "See the documentation for more information: " +
                                      "https://docs.abp.io/en/abp/latest/UI/Angular/Theme-Configurations");
                }
            }

            if (args.ThemeStyle.HasValue)
            {
                Logger.LogInformation("Theme Style: " + args.ThemeStyle);
            }
        }

    }
}
