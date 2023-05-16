using JetBrains.Annotations;
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace ONE.Abp.SysResource.SysMenus
{
    public interface ISysMenuManager : IDomainService
    {
        [NotNull]
        public Task<SysMenu> CreateAsync(Guid appId,string code);
        public Task ChangeCodeAsync(SysMenu sysApp, string code);
    }
}
