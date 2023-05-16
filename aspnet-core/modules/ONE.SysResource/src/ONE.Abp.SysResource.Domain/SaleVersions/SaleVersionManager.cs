using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;

namespace ONE.Abp.SysResource.SaleVersions
{
    public class SaleVersionManager: SysResourceDomainService, ISaleVersionManager
    {
        protected ISaleVersionRepository SaleVersionRepository { get; }
        public SaleVersionManager(ISaleVersionRepository saleVersionRepository)
        {
            SaleVersionRepository = saleVersionRepository;
        }

        public async Task<SaleVersion> CreateAsync(string name)
        {
            Check.NotNull(name, nameof(name));

            await ValidateNameAsync(name);
            return new SaleVersion(GuidGenerator.Create(), name);
        }
        public virtual async Task ChangeNameAsync(SaleVersion saleVersion, string name)
        {
            Check.NotNull(saleVersion, nameof(saleVersion));
            Check.NotNull(name, nameof(name));

            await ValidateNameAsync(name, saleVersion.Id);
            saleVersion.SetName(name);
        }


        public virtual async Task<List<SaleVersionMenu>> GetMenusAsync(Guid id,Guid? appId=null)
        {
            var saleVersion = await SaleVersionRepository.GetAsync(id);
            var menus = saleVersion.Menus.ToList();
            if (appId.HasValue)
                menus = menus.Where(m => m.AppId == appId).ToList();

            return menus;
        }

        public virtual async Task<List<Guid>> GetAppsAsync(Guid id)
        {
            var saleVersion = await SaleVersionRepository.FindAsync(id);
            if(saleVersion == null)
                return new List<Guid>();

            var menus = saleVersion.Menus.ToList();
            if(!menus.Any())
                return new List<Guid>();
            return menus.Select(m => m.AppId).Distinct().ToList();
        }

        public virtual async Task SetMenuAsync(SaleVersion saleVersion, Guid appId,IList<string> menuCodes)
        {
            Check.NotNull(saleVersion, nameof(saleVersion));
            Check.NotNull(appId, nameof(appId));

            if(menuCodes==null|| menuCodes.Count==0)
            {
                saleVersion.RemoveAllMenu();
            }

            var codes=saleVersion.Menus.Select(m => m.MenuCode);
            var addMenuCodes = menuCodes.Except(codes);
            var rmMenuCodes = codes.Except(menuCodes);

            if(addMenuCodes.Any()) {
                foreach (var item in addMenuCodes)
                {
                    saleVersion.AddMenu(appId, item);
                }
            }

            if (rmMenuCodes.Any())
            {
                foreach (var item in rmMenuCodes)
                {
                    saleVersion.RemoveMenu(item);
                }
            }
        }


        protected virtual async Task ValidateNameAsync(string name, Guid? expectedId = null)
        {
            var saleVersion = await SaleVersionRepository.FindByNameAsync(name);
            if (saleVersion != null && saleVersion.Id != expectedId)
            {
                throw new BusinessException(SysResourceErrorCodes.DuplicateSaleVersionName).WithData("Name", name);
            }
        }
    }
}
