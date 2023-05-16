using NuGet.Versioning;
using System;
using templates;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Building.Steps;
using Volo.Abp.Cli.ProjectBuilding.Templates.App;
using Volo.Abp.Cli.ProjectBuilding.Templates.Microservice;

namespace TemplateBuilders
{
    public static class ONETemplateProjectBuildPipelineBuilder
    {
        public static ProjectBuildPipeline Build(ProjectBuildContext context)
        {
            var pipeline = new ProjectBuildPipeline(context);

            pipeline.Steps.Add(new FileEntryListReadStep());

            if (SemanticVersion.Parse(context.TemplateFile.Version) > new SemanticVersion(4, 3, 99))
            {
                pipeline.Steps.Add(new CreateAppSettingsSecretsStep());
            }

            pipeline.Steps.AddRange(context.Template.GetCustomSteps(context));

            if (context.Template.Name == MicroTemplate.TemplateName ||context.Template.Name == OneAppTemplate.TemplateName)
            {
                pipeline.Steps.Add(new ONEProjectReferenceReplaceStep());
            }
            else
            {
                pipeline.Steps.Add(new ProjectReferenceReplaceStep());
            }
             
            pipeline.Steps.Add(new TemplateCodeDeleteStep());
            pipeline.Steps.Add(new SolutionRenameStep());

            if (context.Template.IsPro())
            {
                pipeline.Steps.Add(new LicenseCodeReplaceStep()); // todo: move to custom steps?
            }

            if (context.Template.Name == AppTemplate.TemplateName ||
                context.Template.Name == AppProTemplate.TemplateName)
            {
                pipeline.Steps.Add(new DatabaseManagementSystemChangeStep(context.Template.As<AppTemplateBase>().HasDbMigrations)); // todo: move to custom steps?
            }
            if (context.Template.Name == MicroTemplate.TemplateName)
            {
                pipeline.Steps.Add(new DatabaseManagementSystemChangeStep(context.Template.As<MicroTemplate>().HasDbMigrations)); // todo: move to custom steps?
            }
            if (context.Template.Name == OneAppTemplate.TemplateName)
            {
                pipeline.Steps.Add(new DatabaseManagementSystemChangeStep(context.Template.As<OneAppTemplate>().HasDbMigrations)); // todo: move to custom steps?
            }

            if (context.Template.Name == AppNoLayersTemplate.TemplateName ||
                context.Template.Name == AppNoLayersProTemplate.TemplateName)
            {
                pipeline.Steps.Add(new AppNoLayersDatabaseManagementSystemChangeStep()); // todo: move to custom steps?
            }

            if ((context.BuildArgs.UiFramework == UiFramework.Mvc || context.BuildArgs.UiFramework == UiFramework.Blazor || context.BuildArgs.UiFramework == UiFramework.BlazorServer)
                && context.BuildArgs.MobileApp == MobileApp.None && context.Template.Name != MicroserviceProTemplate.TemplateName
                && context.Template.Name != MicroserviceServiceProTemplate.TemplateName)
            {
                pipeline.Steps.Add(new RemoveRootFolderStep());
            }

            pipeline.Steps.Add(new CreateProjectResultZipStep());

            return pipeline;
        }
    }
}
