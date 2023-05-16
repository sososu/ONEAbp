using System;
using System.Threading.Tasks;
using Volo.Abp;

namespace ONE.Abp.SysResource.SysMenus
{
    public class SysMenuManager: SysResourceDomainService,ISysMenuManager
    {
        protected ISysMenuRepository SysMenuRepository { get; }
        public SysMenuManager(ISysMenuRepository sysMenuRepository)
        {
            SysMenuRepository = sysMenuRepository;
        }

        public async Task<SysMenu> CreateAsync(Guid appId,string code)
        {
            Check.NotNull(code, nameof(code));

            await ValidateCodeAsync(code);
            return new SysMenu(GuidGenerator.Create(),appId,code);
        }
        public virtual async Task ChangeCodeAsync(SysMenu sysMenu, string code)
        {
            Check.NotNull(sysMenu, nameof(sysMenu));
            Check.NotNull(code, nameof(code));

            await ValidateCodeAsync(code, sysMenu.Id);
            sysMenu.SetCode(code);
        }

        protected virtual async Task ValidateCodeAsync(string code, Guid? expectedId = null)
        {
            var sysMenu = await SysMenuRepository.FindByCodeAsync(code);
            if (sysMenu != null && sysMenu.Id != expectedId)
            {
                throw new BusinessException(SysResourceErrorCodes.DuplicateSysMenuCode).WithData("Code", code);
            }
        }
    }
}
