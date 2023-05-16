using JetBrains.Annotations;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using ONE.Abp.SysResource.Menus;
using ONE.Abp.SysResource.SysApps;
using ONE.Abp.SysResource.SysMenus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace ONE.Admin.Data
{
    public class SysMenuSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<SysMenu> _repository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IImporter _importer;
        public SysMenuSeedContributor(IRepository<SysMenu> repository, IGuidGenerator guidGenerator, IImporter importer)
        {
            _repository = repository;
            _guidGenerator = guidGenerator;
            _importer = importer;
        }
        public async Task SeedAsync(DataSeedContext context)
        {
            if (await _repository.CountAsync() > 0) return;

            var menus = new List<SysMenu>();

            var appId = Guid.Parse("3a08cbf7-7321-dd8b-91f6-b4761aa85c0d");
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "SysMenus.xlsx");
            if (!File.Exists(filePath))
                return;

            var importResult = await _importer.Import<MenuImport>(filePath);

            if (importResult == null || importResult.HasError) return;

            foreach (var item in importResult.Data)
            {
                var menu = new SysMenu(_guidGenerator.Create(), appId, item.Code);
                menu.SetBasicInfo(item.Name,item.Order,item.Path,item.Component,item.MenuType,item.ParentCode,item.Icon,item.Query);
                menu.SetEnable(item.IsFrame,item.IsCache,item.IsEnable,item.Visible);
                menu.SetPerms(item.Perms);
                menus.Add(menu);
            }

        
            await _repository.InsertManyAsync(menus);
        }
    }


}
