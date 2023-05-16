using Microsoft.Extensions.Options;
using ONE.Abp.Data.DataDics;
using ONE.Abp.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace ONE.Abp.DataDictionary.DataItems
{
    public class EnumDicDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        protected IDataItemManager DataItemManager { get; }
        protected IRepository<DataItem, Guid> Repository { get; }
        protected AbpEnumDicOption EnumDicOption { get; }
        protected IEnumerable<IEnumDataItemRegisterion> Registerions { get; }
        protected IUnitOfWorkManager UnitOfWorkManager { get; }
        public EnumDicDataSeedContributor(IUnitOfWorkManager unitOfWorkManager, IOptions<AbpEnumDicOption> option, IEnumerable<IEnumDataItemRegisterion> registerions, IRepository<DataItem, Guid> repository, IDataItemManager dataItemManager)
        {
            EnumDicOption = option.Value;
            Repository = repository;
            DataItemManager = dataItemManager;
            Registerions = registerions;
            UnitOfWorkManager = unitOfWorkManager;
        }

        [UnitOfWork]
        public async Task SeedAsync(DataSeedContext context)
        {
            //await CleareEnumDataAsync();
            //await UnitOfWorkManager.Current.SaveChangesAsync();

            foreach (var registerion in Registerions)
            {
                registerion.Register(EnumDicOption);
            }

            await AddEnumDataAsync();
        }

        //private async Task CleareEnumDataAsync()
        //{
        //    var categorys = await Repository.GetListAsync(r => r.Source == DataItemSource.Enum && !r.ParentId.HasValue);
        //    if (categorys.Any())
        //    {
        //        foreach (var item in categorys)
        //        {
        //            await DataItemManager.DeleteAsync(item.Id, true);
        //        }
        //    }
        //}
        private async Task AddEnumDataAsync()
        {
            var list = new List<DataItem>();
            foreach (var item in EnumDicOption.GetInfos())
            {
                var category = await Repository.FindAsync(x=>x.Name==item.Name||x.Code==item.EnmuType.Name);
                if (category != null)
                    continue;

                category = await DataItemManager.CreateCategoryAsync(item.Name, item.EnmuType.Name);
                category.Source = DataItemSource.Enum;
                category.SetStatus(DataItemStatus.Enable);
                await Repository.InsertAsync(category);

                foreach (var arr in item.EnmuType.GetEnumValues())
                {
                    var data = await DataItemManager.CreateAsync(((Enum)arr).DisplayName() ?? arr.ToString(), arr.ToString(), arr.To<int>(), category.Id);

                    data.Source = DataItemSource.Enum;
                    data.SetStatus(DataItemStatus.Enable);
                    data.Description = ((Enum)arr).DisplayDescription();
                    data.Order = ((Enum)arr).DisplayOrder() ?? arr.To<int>();

                    list.Add(data);
                }
            }

            await Repository.InsertManyAsync(list);
        }

    }
}
