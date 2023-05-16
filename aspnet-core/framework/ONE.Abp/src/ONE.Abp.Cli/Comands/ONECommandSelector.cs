using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using Volo.Abp.Cli;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Commands;
using Volo.Abp.DependencyInjection;

namespace Comands
{
    [Dependency(ReplaceServices = true)]
    public class ONECommandSelector : ICommandSelector, ITransientDependency
    {
        protected AbpCliOptions Options { get; }

        public ONECommandSelector(IOptions<AbpCliOptions> options)
        {
            Options = options.Value;
        }

        public Type Select(CommandLineArgs commandLineArgs)
        {
            if (commandLineArgs.Command.IsNullOrWhiteSpace())
            {
                return typeof(ONEHelpCommand);
            }

            return Options.Commands.GetOrDefault(commandLineArgs.Command)
                   ?? typeof(ONEHelpCommand);
        }
    }
}
