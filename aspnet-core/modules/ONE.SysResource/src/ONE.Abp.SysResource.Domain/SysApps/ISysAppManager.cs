using JetBrains.Annotations;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace ONE.Abp.SysResource.SysApps
{
    public interface ISysAppManager : IDomainService
    {
        [NotNull]
        public Task<SysApp> CreateAsync(string code);
        public Task ChangeCodeAsync(SysApp sysApp, string code);
    }
}
