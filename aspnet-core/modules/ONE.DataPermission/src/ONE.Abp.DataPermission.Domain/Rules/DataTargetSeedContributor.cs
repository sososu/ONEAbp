using Microsoft.Extensions.Options;
using ONE.Abp.Data.Rules;
using ONE.Abp.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Values;
using Volo.Abp.Uow;

namespace ONE.Abp.DataPermission.Rules
{
    public class DataTargetSeedContributor : IDataSeedContributor, ITransientDependency
    {
        protected DataTargetOption DataTargetOption { get; }
        protected IUnitOfWorkManager UnitOfWorkManager { get; }
        protected IRepository<DataTarget> DataTargetInfoRepository { get; }
        public DataTargetSeedContributor(IUnitOfWorkManager unitOfWorkManager, IOptions<AbpRuleOptions> option, IRepository<DataTarget> dataTargetInfoRepository)
        {
            DataTargetOption = option.Value?.DataTargetOption;
            DataTargetInfoRepository = dataTargetInfoRepository;
            UnitOfWorkManager = unitOfWorkManager;
        }

        [UnitOfWork]
        public async Task SeedAsync(DataSeedContext context)
        {
            await UnitOfWorkManager.Current.SaveChangesAsync();

            var targets = await DataTargetInfoRepository.GetListAsync();

            var entities = new List<DataTarget>();

            foreach (var item in DataTargetOption.GetInfos())
            {
                if (targets.Any(t => t.Name == item.DataTargetType.Name))
                {
                    //await DataTargetInfoRepository.DeleteAsync(t => t.Name == item.DataTargetType.Name);
                    continue;
                }
                  

                var entity = new DataTarget(item.DataTargetType.Name);
                entity.DisplayName = item.DisplayName?? entity.Name;
                entity.Description = item.Description;

                foreach (var property in item.DataTargetType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (IsComplexType(property.PropertyType)) //复杂类型
                    {
                        if (!property.PropertyType.IsSubclassOf(typeof(ValueObject))) //不是值对象则忽略
                            continue;


                        foreach (var cproperty in property.PropertyType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                        {
                            if (IsComplexType(cproperty.PropertyType)) continue;

                            var field = new DataTargetField(entity.Name, $"{property.Name}.{cproperty.Name}", cproperty.PropertyType.ToString());
                            field.DisplayName = cproperty.DisplayName()??field.Name;
                            field.Description = cproperty.DisplayDescription();
                            entity.AddField(field);

                        }
                    }
                    else
                    {
                        var field = new DataTargetField(entity.Name, property.Name, property.PropertyType.ToString());
                        field.DisplayName = property.DisplayName();
                        field.Description = property.DisplayDescription();
                        entity.AddField(field);
                    }
                }

                entities.Add(entity);
            }

            await DataTargetInfoRepository.InsertManyAsync(entities);
        }


        private bool IsComplexType(Type type)
        {
            var nonNullableType= type.GetNonNullableType();

            return nonNullableType != typeof(Guid) && Type.GetTypeCode(nonNullableType) == TypeCode.Object;
        }
    }
}
