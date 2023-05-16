using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Cli;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Commands;
using Volo.Abp.DependencyInjection;

namespace Comands
{
    public class ONEHelpCommand : IConsoleCommand, ITransientDependency
    {
        public const string Name = "help";

        public ILogger<HelpCommand> Logger { get; set; }

        protected AbpCliOptions AbpCliOptions { get; }

        protected IServiceScopeFactory ServiceScopeFactory { get; }

        public ONEHelpCommand(IOptions<AbpCliOptions> cliOptions, IServiceScopeFactory serviceScopeFactory)
        {
            ServiceScopeFactory = serviceScopeFactory;
            Logger = NullLogger<HelpCommand>.Instance;
            AbpCliOptions = cliOptions.Value;
        }

        public Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            if (string.IsNullOrWhiteSpace(commandLineArgs.Target))
            {
                Logger.LogInformation(GetUsageInfo());
                return Task.CompletedTask;
            }

            if (!AbpCliOptions.Commands.ContainsKey(commandLineArgs.Target))
            {
                Logger.LogWarning("There is no command named " + commandLineArgs.Target + ".");
                Logger.LogInformation(GetUsageInfo());
                return Task.CompletedTask;
            }

            Type serviceType = AbpCliOptions.Commands[commandLineArgs.Target];
            using (IServiceScope serviceScope = ServiceScopeFactory.CreateScope())
            {
                IConsoleCommand consoleCommand = (IConsoleCommand)serviceScope.ServiceProvider.GetRequiredService(serviceType);
                Logger.LogInformation(consoleCommand.GetUsageInfo());
            }

            return Task.CompletedTask;
        }

        public string GetUsageInfo()
        {
            var sb = new StringBuilder();

            sb.AppendLine("");
            sb.AppendLine("Usage:");
            sb.AppendLine("");
            sb.AppendLine("    oneabp <command> <target> [options]");
            sb.AppendLine("");
            sb.AppendLine("Command List:");
            sb.AppendLine("");

            foreach (var command in AbpCliOptions.Commands.ToArray())
            {
                string shortDescription;

                using (var scope = ServiceScopeFactory.CreateScope())
                {
                    shortDescription = ((IConsoleCommand)scope.ServiceProvider
                            .GetRequiredService(command.Value)).GetShortDescription();

                    shortDescription = shortDescription.Replace("abp", "oneabp");
                }

                sb.Append("    > ");
                sb.Append(command.Key);
                sb.Append(string.IsNullOrWhiteSpace(shortDescription) ? "" : ":");
                sb.Append(" ");
                sb.AppendLine(shortDescription);
            }

            sb.AppendLine("");
            sb.AppendLine("To get a detailed help for a command:");
            sb.AppendLine("");
            sb.AppendLine("    oneabp help <command>");
            sb.AppendLine("");
            sb.AppendLine("See the documentation for more info: https://docs.abp.io/en/abp/latest/CLI");

            return sb.ToString();
        }

        public string GetShortDescription()
        {
            return "Show command line help. Write ` abp help <command> `";
        }
    }
}
