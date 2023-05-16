using JetBrains.Annotations;
using ONE.Abp.Shared.Rules;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace ONE.Abp.Data.Rules
{
    public interface IRuleStore
    {
        Task<List<UserDataRuleResult>> GetUserDataRulesAsync(string dataTarget, CancellationToken cancellationToken = default);

        [CanBeNull]
        Task<UserRuleResult> GetUserRuleAsync(Guid id, CancellationToken cancellationToken = default);

        [CanBeNull]
        Task<DataRuleResult> GetDataRuleAsync(Guid id, CancellationToken cancellationToken = default);
    }

    public class NullRuleStore : IRuleStore, ITransientDependency
    {
        public Task<DataRuleResult> GetDataRuleAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<DataRuleResult>(null);
        }

        public Task<List<UserDataRuleResult>> GetUserDataRulesAsync(string dataTarget, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<List<UserDataRuleResult>>(new List<UserDataRuleResult>());
        }

        public Task<UserRuleResult> GetUserRuleAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<UserRuleResult>(null);
        }
    }
}
