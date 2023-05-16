using Microsoft.Extensions.DependencyInjection;
using ONE.Abp.Data.Rules;
using ONE.Abp.Pagination;
using ONE.Abp.Shared.Rules;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ObjectExtending;

namespace ONE.Abp.DataPermission.Extension
{
    public static class RuleEngineExtension
    {
        //todo:性能优化，添加缓存
        public static async Task<List<RuleResult<TEntity, TResult>>> Execute<TEntity, TResult>(CancellationToken cancellationToken = default) where TEntity : class, IEntity where TResult : ExtensibleObject
        {
            using (var scope = ServiceContainer.ServiceProvider.CreateScope())
            {
                var ruleEngine = scope.ServiceProvider.GetRequiredService<IRuleEngine>();
                return await ruleEngine.ExecuteAsync<TEntity, TResult>();
            }
        }
    }
}
