using ONE.Abp.Data.Rules;
using ONE.Abp.DataPermission.Tests.Datas;
using ONE.Abp.Shared.Rules;
using Volo.Abp.DependencyInjection;

namespace ONE.Abp.DataPermission.Tests
{
    [Dependency(ReplaceServices = true)]
    public class MockRuleStore : IRuleStore,ITransientDependency
    {
        public Task<DataRuleResult> GetDataRuleAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<DataRuleResult>(RuleData.DataRuleResults.FirstOrDefault(c=>c.Id==id));
        }

        public Task<List<UserDataRuleResult>> GetUserDataRulesAsync(string dataTarget, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(RuleData.UserDataRuleResults);
        }

        public Task<UserRuleResult> GetUserRuleAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(RuleData.UserRuleResult);
        }
    }
}
