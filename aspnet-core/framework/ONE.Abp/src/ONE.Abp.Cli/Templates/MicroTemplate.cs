using NuGet.Versioning;
using System.Collections.Generic;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Building.Steps;
using Volo.Abp.Cli.ProjectBuilding.Templates;
using Volo.Abp.Cli.ProjectBuilding.Templates.App;

namespace templates
{
    public class MicroTemplate : TemplateInfo
    {
        public const string TemplateName = "micro";

        public const Theme DefaultTheme = Theme.LeptonXLite;

        public bool HasDbMigrations { get; set; }

        public MicroTemplate()
            : base("micro")
        {
            DocumentUrl = "https://docs.abp.io/en/abp/latest/Startup-Templates/Module";
        }

        public override IEnumerable<ProjectBuildPipelineStep> GetCustomSteps(ProjectBuildContext context)
        {
            var steps = new List<ProjectBuildPipelineStep>();

    
            SwitchDatabaseProvider(context, steps);
            RemoveMigrations(context, steps);
            RemoveUnnecessaryPorts(context, steps);
            RandomizeSslPorts(context, steps);
            RandomizeStringEncryption(context, steps);
            UpdateNuGetConfig(context, steps);
            ChangeConnectionString(context, steps);
            return steps;
        }


        protected void SwitchDatabaseProvider(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            if (context.BuildArgs.DatabaseProvider == DatabaseProvider.MongoDb)
            {
                steps.Add(new AppTemplateSwitchEntityFrameworkCoreToMongoDbStep(HasDbMigrations));
            }

            if (context.BuildArgs.DatabaseProvider != DatabaseProvider.EntityFrameworkCore)
            {
                if (HasDbMigrations)
                {
                    steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.EntityFrameworkCore"));
                    steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.EntityFrameworkCore.DbMigrations"));
                    steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.EntityFrameworkCore.Tests", projectFolderPath: "/aspnet-core/test/MyCompanyName.MyProjectName.EntityFrameworkCore.Tests"));
                }
                else
                {
                    steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.EntityFrameworkCore"));
                    steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.EntityFrameworkCore.Tests", projectFolderPath: "/aspnet-core/test/MyCompanyName.MyProjectName.EntityFrameworkCore.Tests"));
                }
            }
            else
            {
                context.Symbols.Add("EFCORE");
            }

            //if (context.BuildArgs.DatabaseProvider != DatabaseProvider.MongoDb)
            //{
            //    steps.Add(new AppTemplateRemoveMongodbCollectionFixtureStep());
            //    steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.MongoDB"));
            //    steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.MongoDB.Tests", projectFolderPath: "/aspnet-core/test/MyCompanyName.MyProjectName.MongoDB.Tests"));
            //}

            context.Symbols.Add($"dbms:{context.BuildArgs.DatabaseManagementSystem}");
        }


        protected void RemoveMigrations(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            if (string.IsNullOrWhiteSpace(context.BuildArgs.Version) ||
                SemanticVersion.Parse(context.BuildArgs.Version) > new SemanticVersion(4, 1, 99))
            {
                if (HasDbMigrations)
                {
                    steps.Add(new RemoveFolderStep("/aspnet-core/src/MyCompanyName.MyProjectName.EntityFrameworkCore.DbMigrations/Migrations"));
                }
                else
                {
                    steps.Add(new RemoveFolderStep("/aspnet-core/src/MyCompanyName.MyProjectName.EntityFrameworkCore/Migrations"));
                }
            }
        }

        protected void RemoveUnnecessaryPorts(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            steps.Add(new RemoveUnnecessaryPortsStep());
        }


        protected void RandomizeSslPorts(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            if (context.BuildArgs.ExtraProperties.ContainsKey("no-random-port"))
            {
                return;
            }

            //todo: discuss blazor ports
            steps.Add(new TemplateRandomSslPortStep(
                    new List<string>
                    {
                        "https://localhost:44300",
                        "https://localhost:44301",
                        "https://localhost:44302",
                        "https://localhost:44303",
                        "https://localhost:44304",
                        "https://localhost:44305",
                        "https://localhost:44306",
                        "https://localhost:44307",
                        "https://localhost:44308",
                        "https://localhost:44309"
                    }
                )
            );
        }

        protected void RandomizeStringEncryption(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            steps.Add(new RandomizeStringEncryptionStep());
        }

        protected void UpdateNuGetConfig(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            steps.Add(new UpdateNuGetConfigStep("/aspnet-core/NuGet.Config"));
        }

        protected void ChangeConnectionString(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            if (context.BuildArgs.ConnectionString != null)
            {
                steps.Add(new ConnectionStringChangeStep());
            }
        }
    }
}
