using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using ONE.Abp.Data.Rules;
using ONE.Abp.Shared.Rules;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;
using Volo.Abp.Users;

namespace ONE.Abp.DataPermission
{
    public class UserChangeEventHandler : IDistributedEventHandler<EntityUpdatedEto<UserEto>>, ITransientDependency
    {
        protected ICurrentTenant CurrentTenant { get; }
        protected IUnitOfWorkManager UnitOfWorkManager { get; }
        protected IServiceProvider ServiceProvider { get; }
        protected AbpRuleOptions AbpRuleOptions { get; }
        protected IList<Type> Entities => AbpRuleOptions.DataTargetOption.GetNeedUpdateShadowPropertyType();

        public UserChangeEventHandler(ICurrentTenant currentTenant, IUnitOfWorkManager unitOfWorkManager, IOptions<AbpRuleOptions> abpRuleOptions, IServiceProvider serviceProvider)
        {
            CurrentTenant = currentTenant;
            UnitOfWorkManager = unitOfWorkManager;
            AbpRuleOptions = abpRuleOptions.Value;
            ServiceProvider = serviceProvider;
        }

        readonly MethodInfo _createFilterConditionExpressionMethod =
typeof(UserChangeEventHandler).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).FirstOrDefault(m => m.Name ==nameof(CreateFilterConditionExpression));


        readonly MethodInfo _getRequiredServiceMethod =
typeof(ServiceProviderServiceExtensions).GetMethods(BindingFlags.Static | BindingFlags.Public).FirstOrDefault(m => m.Name == nameof(ServiceProviderServiceExtensions.GetRequiredService)&&m.IsGenericMethod);

        public async Task HandleEventAsync(EntityUpdatedEto<UserEto> eventData)
        {
            eventData.IsMultiTenant(out var tenantId);

            using (var scope = ServiceProvider.CreateScope())
            {
                using (CurrentTenant.Change(tenantId))
                {
                    foreach (var item in Entities)
                    {

                        var shadowPropertyInterfaces = item.GetInterfaces().Where(t => t != typeof(IShadowProperty)).Where(typeof(IShadowProperty).IsAssignableFrom).ToList();
                        if (!shadowPropertyInterfaces.Any())
                            continue;

                        //获取动态仓储
                        var repoType = typeof(IRepository<>).MakeGenericType(item);

                       // var getService = _getRequiredServiceMethod.MakeGenericMethod(repoType);

                        //var repo = getService.Invoke(null,new object[1] { ServiceProvider });

                        var repo = scope.ServiceProvider.GetRequiredService(repoType);
                        var getlistMethod = repo.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(m => m.Name == "GetListAsync" && m.GetParameters().Count() == 3).FirstOrDefault();
                        var updateMethod = repo.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(m => m.Name == "UpdateManyAsync" && m.GetParameters().Count() == 3).FirstOrDefault();


                        //构建表达式树  x=>x.id=
                        var expression = _createFilterConditionExpressionMethod.MakeGenericMethod(item).Invoke(this, new object[1] { eventData.Entity.Id });


                        //获取创建人数据
                        var entities = await getlistMethod.InvokeAsync(repo, new object[3] { expression, false, default(CancellationToken) }) as IEnumerable;


                        var isEmpty = true;

                        //给实体赋值
                        foreach (var entity in entities)
                        {
                            isEmpty = false;
                            foreach (var inter in shadowPropertyInterfaces)
                            {
                                //有影子接口
                                var p = inter.GetProperties(BindingFlags.Instance | BindingFlags.Public).FirstOrDefault();

                                //扩展属性有这个值
                                if (!eventData.Entity.ExtraProperties.ContainsKey(p.Name))
                                    continue;

                                var value = eventData.Entity.ExtraProperties[p.Name];

                                var property = entity.GetType().GetProperty(p.Name);
                                property.SetValue(entity, value);
                            }
                        }

                        if (!isEmpty)
                        {
                            using (var uow = UnitOfWorkManager.Begin(true))
                            {
                                //todo:禁止更新时间跟更新人字段？
                                await updateMethod.InvokeNoResultAsync(repo, new object[3] { entities, true, default(CancellationToken) }); //批量更新
                            }
                        }
                           
                    }

                }
            }
        }

        private Expression<Func<TEntity,bool>> CreateFilterConditionExpression<TEntity>(Guid id)
        {
            var entityParam = Expression.Parameter(typeof(TEntity), "o");

            var member = Expression.Property(entityParam, nameof(IMayHaveCreator.CreatorId));

            var val = Expression.Constant(id, member.Type);
            var body=Expression.Equal(member, val);

            return Expression.Lambda<Func<TEntity, bool>>(body, entityParam);
        }
    }

    public static class ExtensionMethods
    {
        public static async Task<object> InvokeAsync(this MethodInfo method, object obj, params object[] parameters)
        {
            dynamic awaitable = method.Invoke(obj, parameters);
            await awaitable;
            return awaitable.GetAwaiter().GetResult();
        }

        public static async Task InvokeNoResultAsync(this MethodInfo method, object obj, params object[] parameters)
        {
            dynamic awaitable = method.Invoke(obj, parameters);
            await awaitable;
        }
    }
}

