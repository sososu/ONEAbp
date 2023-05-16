using Microsoft.EntityFrameworkCore;
using ONE.Abp.Pagination.Contracts;
using ONE.Abp.Pagination.Contracts.Dtos;
using ONE.Abp.Data.Rules;
using ONE.Abp.Pagination;
using ONE.Abp.Shared.Rules;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectExtending;
namespace ONE.Abp.DataPermission.Extension
{
    public static class RuleQueryableExtension
    {

        /// <summary>
        /// 启用数据权限返回分页的数据数据
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="source"></param>
        /// <param name="page">分页信息</param>
        /// <param name="defaultSort">默认排序</param>
        /// <param name="cancellationToken"></param>
        public static Task<PagedResult<TDto>> ToPagedResultForRuleAsync<TEntity, TDto>(this IQueryable<TEntity> source,
             IPagedQuery query, string? defaultSort = null, CancellationToken cancellationToken = default)
             where TEntity : class, IEntity where TDto : ExtensibleObject
        {
            return ToPagedResultForRuleAsync<TEntity, TDto>(source, query, query, defaultSort, cancellationToken);
        }


        public static Task<ExtensiblePagedResult<TDto>> ToExtensiblePagedResultForRuleAsync<TEntity, TDto>(this IQueryable<TEntity> source,
        IPagedQuery query, string? defaultSort = null, CancellationToken cancellationToken = default)
         where TEntity : class, IEntity where TDto : ExtensibleObject
        {
            return ToExtensiblePagedResultForRuleAsync<TEntity, TDto>(source, query, query, defaultSort, cancellationToken);
        }


        /// <summary>
        /// 启用数据权限返回分页的数据数据
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="source"></param>
        /// <param name="page">分页信息</param>
        /// <param name="defaultSort">默认排序</param>
        /// <param name="cancellationToken"></param>
        public static async Task<PagedResult<TDto>> ToPagedResultForRuleAsync<TEntity, TDto>(this IQueryable<TEntity> source,
           IQuery query, IPagedSortInfo page, string? defaultSort = null, CancellationToken cancellationToken = default)
             where TEntity : class, IEntity where TDto : ExtensibleObject
        {
            page = page ?? new PagedSortInfo();
            var pageIndex = Math.Max(1, page.PageIndex);
            var pageSize = Math.Max(1, page.PageSize);

            var result = new PagedResult<TDto> { PageIndex = pageIndex, PageSize = pageSize };

            var rules = await RuleEngineExtension.Execute<TEntity, TDto>(cancellationToken);

            if (rules.Count == 1) //只有一条，提高查询效率
            {
                var data = source.Where(query).WhereIf(rules[0].ConditionExpression!=null,rules[0].ConditionExpression);
                result.TotalCount = await data.CountAsync(cancellationToken);
                if (result.TotalCount > 0)
                {
                    var mapData = data.OrderBy(page, defaultSort);

                    result.Items = await mapData.Skip(pageSize * (pageIndex - 1))
                      .Take(pageSize)
                      //.Select(rules[0].SelectExpression)
                      .ProjectTo<TDto>()
                      .ToListAsync(cancellationToken);
                }

                foreach (var item in result.Items)
                {
                    rules[0].ChangeTargetAction?.Invoke(item);
                    item.ExtraProperties.AddIfNotContains(new KeyValuePair<string, object>(RuleDataOperationNameConst.CanRemove, rules[0].RuleDataOperation.HasFlag(RuleDataOperation.Delete)));
                    item.ExtraProperties.AddIfNotContains(new KeyValuePair<string, object>(RuleDataOperationNameConst.CanEdit, rules[0].RuleDataOperation.HasFlag(RuleDataOperation.Edit)));
                }
                return result;
            }
            else
            {
                var conditions = rules.Where(r => r.ConditionExpression != null).ToList();
       
                var entitys = new List<TEntity>();
                var listResult = new List<TDto>();

                IQueryable<TEntity> data = null;
                if (conditions.Any())
                {
                   var condition = conditions.Select(r => r.ConditionExpression).Aggregate((a, b) => a.Or(b)); //todo:求并集，条件是或运算，待验证
                   data = source.Where(query).Where(condition);
                }
                else
                {
                   data = source.Where(query);
                }
               
                result.TotalCount = await data.CountAsync(cancellationToken);
                if (result.TotalCount > 0)
                {
                    var mapData = data.OrderBy(page, defaultSort);
                    entitys = await mapData.Skip(pageSize * (pageIndex - 1))
                        .Take(pageSize)
                        .ToListAsync(cancellationToken);
                }

                foreach (var entity in entitys)
                {
                    foreach (var rule in rules)
                    {
                        if (rule.ConditionExpression.Compile().Invoke(entity))
                        {
                            ///筛选字段是否显示
                            //var item = entitys.Where(e => e == entity).Select(rule.SelectExpression.Compile()).FirstOrDefault(); //todo:验证e==entity是否可以，不行则尝试e.getkey()==entity.getkey()

                            var item = MapperContainer.Mapper.Map<TEntity, TDto>(entity);
                            rule.ChangeTargetAction?.Invoke(item);

                            //操作权限
                            item.ExtraProperties.AddIfNotContains(new KeyValuePair<string, object>(RuleDataOperationNameConst.CanRemove, rule.RuleDataOperation.HasFlag(RuleDataOperation.Delete)));
                            item.ExtraProperties.AddIfNotContains(new KeyValuePair<string, object>(RuleDataOperationNameConst.CanEdit, rule.RuleDataOperation.HasFlag(RuleDataOperation.Edit)));
                            listResult.Add(item);

                            break;
                        }
                    }
                }

                result.Items = listResult;
                return result;
            }
        }


        /// <summary>
        /// 启用数据权限返回分页的数据数据
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="source"></param>
        /// <param name="page">分页信息</param>
        /// <param name="defaultSort">默认排序</param>
        /// <param name="cancellationToken"></param>
        public static async Task<ExtensiblePagedResult<TDto>> ToExtensiblePagedResultForRuleAsync<TEntity, TDto>(this IQueryable<TEntity> source,
           IQuery query, IPagedSortInfo page, string? defaultSort = null, CancellationToken cancellationToken = default)
             where TEntity : class, IEntity where TDto : ExtensibleObject
        {
            page = page ?? new PagedSortInfo();
            var pageIndex = Math.Max(1, page.PageIndex);
            var pageSize = Math.Max(1, page.PageSize);

            var result = new ExtensiblePagedResult<TDto> { PageIndex = pageIndex, PageSize = pageSize };

            var rules = await RuleEngineExtension.Execute<TEntity, TDto>(cancellationToken);

            if (rules.Count == 1) //只有一条，提高查询效率
            {
                var data = source.Where(query).Where(rules[0].ConditionExpression);
                result.TotalCount = await data.CountAsync(cancellationToken);
                if (result.TotalCount > 0)
                {
                    var mapData = data.OrderBy(page, defaultSort);

                    result.Items = await mapData.Skip(pageSize * (pageIndex - 1))
                      .Take(pageSize)
                      //.Select(rules[0].SelectExpression)
                      .ProjectTo<TDto>()
                      .ToListAsync(cancellationToken);
                }

                foreach (var item in result.Items)
                {
                    rules[0].ChangeTargetAction?.Invoke(item);
                    item.ExtraProperties.AddIfNotContains(new KeyValuePair<string, object>(RuleDataOperationNameConst.CanRemove, rules[0].RuleDataOperation.HasFlag(RuleDataOperation.Delete)));
                    item.ExtraProperties.AddIfNotContains(new KeyValuePair<string, object>(RuleDataOperationNameConst.CanEdit, rules[0].RuleDataOperation.HasFlag(RuleDataOperation.Edit)));
                }
                return result;
            }
            else
            {
                var condition = rules.Select(r => r.ConditionExpression).Aggregate((a, b) => a.Or(b)); //todo:求并集，条件是或运算，待验证
                var entitys = new List<TEntity>();
                var listResult = new List<TDto>();

                var data = source.Where(query).Where(condition);
                result.TotalCount = await data.CountAsync(cancellationToken);
                if (result.TotalCount > 0)
                {
                    var mapData = data.OrderBy(page, defaultSort);
                    entitys = await mapData.Skip(pageSize * (pageIndex - 1))
                        .Take(pageSize)
                        .ToListAsync(cancellationToken);
                }

                foreach (var entity in entitys)
                {
                    foreach (var rule in rules)
                    {
                        if (rule.ConditionExpression.Compile().Invoke(entity))
                        {
                            //筛选字段是否显示
                            //var item = entitys.Where(e => e == entity).Select(rule.SelectExpression.Compile()).FirstOrDefault(); //todo:验证e==entity是否可以，不行则尝试e.getkey()==entity.getkey()
                            var item = MapperContainer.Mapper.Map<TEntity, TDto>(entity);
                            rule.ChangeTargetAction?.Invoke(item);
                            //操作权限
                            item.ExtraProperties.AddIfNotContains(new KeyValuePair<string, object>(RuleDataOperationNameConst.CanRemove, rule.RuleDataOperation.HasFlag(RuleDataOperation.Delete)));
                            item.ExtraProperties.AddIfNotContains(new KeyValuePair<string, object>(RuleDataOperationNameConst.CanEdit, rule.RuleDataOperation.HasFlag(RuleDataOperation.Edit)));
                            listResult.Add(item);

                            break;
                        }
                    }
                }

                result.Items = listResult;
                return result;
            }
        }
    }
}
