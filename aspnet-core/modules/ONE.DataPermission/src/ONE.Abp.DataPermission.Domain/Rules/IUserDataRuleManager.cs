using JetBrains.Annotations;
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace ONE.Abp.DataPermission.Rules
{
    public interface IUserDataRuleManager : IDomainService
    {
        [NotNull]
        Task<UserDataRule> CreateAsync([NotNull] string dataTarget, [NotNull] Guid userRuleId, [NotNull] Guid dataRuleId);

        Task ChangeeUserDataIdAsync([NotNull] UserDataRule userDataRule, [NotNull] string dataTarget, [NotNull] Guid userRuleId, [NotNull] Guid dataRuleId);
    }
}
