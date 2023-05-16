using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace ONE.Abp.DataPermission.Rules
{
    public interface IUserDataRuleRepository:IRepository<UserDataRule, Guid>
    {
        public Task<UserDataRule> FindByUserDataIdAsync([NotNull] string dataTarget, [NotNull] Guid userRuleId, [NotNull] Guid dataRuleId, CancellationToken cancellationToken = default);
        public Task<bool> CheckDataTargetNameConsistent([NotNull] string dataTarget, [NotNull] Guid dataRuleId, CancellationToken cancellationToken = default);
        public Task<List<UserDataRule>> GetListAsync([NotNull] string dataTarget,CancellationToken cancellationToken = default);
    }
}
