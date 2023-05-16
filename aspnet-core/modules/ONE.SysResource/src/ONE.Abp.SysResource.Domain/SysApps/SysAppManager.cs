using ONE.Abp.SysResource.SysApps;
using System;
using System.Threading.Tasks;
using Volo.Abp;

namespace ONE.Abp.SysResource.SysApps
{
    public class SysAppManager : SysResourceDomainService, ISysAppManager
    {
        protected ISysAppRepository SysAppRepository { get; }
        public SysAppManager(ISysAppRepository sysAppRepository)
        {
            SysAppRepository = sysAppRepository;
        }

        public async Task<SysApp> CreateAsync(string code)
        {
            Check.NotNull(code, nameof(code));

            await ValidateCodeAsync(code);
            return new SysApp(GuidGenerator.Create(), code);
        }
        public virtual async Task ChangeCodeAsync(SysApp sysApp, string code)
        {
            Check.NotNull(sysApp, nameof(sysApp));
            Check.NotNull(code, nameof(code));

            await ValidateCodeAsync(code, sysApp.Id);
            sysApp.SetAppCode(code);
        }

        protected virtual async Task ValidateCodeAsync(string code, Guid? expectedId = null)
        {
            var sysApp = await SysAppRepository.FindByCodeAsync(code);
            if (sysApp != null && sysApp.Id != expectedId)
            {
                throw new BusinessException(SysResourceErrorCodes.DuplicateSysAppCode).WithData("Code", code);
            }
        }
    }
}
