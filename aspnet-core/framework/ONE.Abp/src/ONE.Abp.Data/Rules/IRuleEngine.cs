using ONE.Abp.Data.Rules;
using ONE.Abp.Shared.Rules;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ObjectExtending;

namespace ONE.Abp.Data.Rules
{
    public interface IRuleEngine
    {
        Task<List<RuleResult<TEntity, TResult>>> ExecuteAsync<TEntity, TResult>(CancellationToken cancellationToken = default) where TEntity : class, IEntity where TResult : ExtensibleObject;
    }

    public class NullRuleEngine : IRuleEngine, ITransientDependency
    {
        public Task<List<RuleResult<TEntity, TResult>>> ExecuteAsync<TEntity, TResult>(CancellationToken cancellationToken) where TEntity : class, IEntity where TResult : ExtensibleObject
        {
            return Task.FromResult(new List<RuleResult<TEntity, TResult>>());
        }
    }
}
