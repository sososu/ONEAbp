using Comands;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Generic;
using TemplateBuilders;
using Volo.Abp.Autofac;
using Volo.Abp.Cli;
using Volo.Abp.Cli.Commands;
using Volo.Abp.Modularity;

namespace ONE.Abp.Cli
{
    [DependsOn(
     typeof(AbpCliCoreModule),
     typeof(AbpAutofacModule)
 )]
    public class AbpCliModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            //context.Services.Replace(new ServiceDescriptor(typeof(HelpCommand),typeof(ONEHelpCommand),ServiceLifetime.Transient));
            //context.Services.RemoveAll(descriptor => descriptor.ImplementationType==typeof(HelpCommand));
            //context.Services.RemoveAll(descriptor => descriptor.ImplementationType==typeof(NewCommand));
        }

        public override void PostConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpCliOptions>(option =>
            {
                option.Commands.Remove(HelpCommand.Name);
                option.Commands.Remove(NewCommand.Name);
                option.Commands.Add(ONEHelpCommand.Name, typeof(ONEHelpCommand));
                option.Commands.Add(ONENewCommand.Name, typeof(ONENewCommand));
            });
        }
    }
}
