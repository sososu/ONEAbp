using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using ONE.Abp.DataPermission.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ONE.Abp.DataPermission.EntityFrameworkCore
{
    public class EfCoreUserDataRuleRepository : EfCoreRepository<IDataPermissionDbContext, UserDataRule, Guid>, IUserDataRuleRepository
    {
        public EfCoreUserDataRuleRepository(IDbContextProvider<IDataPermissionDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public virtual async Task<bool> CheckDataTargetNameConsistent([NotNull] string dataTarget, [NotNull] Guid dataRuleId, CancellationToken cancellationToken = default)
        {
            var dataRule=await (await GetDbContextAsync()).Set<DataRule>().FindAsync(dataRuleId);

            if(dataRule == null) { return false; }
            
            return dataRule.DataTargetName== dataTarget;
        }

        public virtual async Task<UserDataRule> FindByUserDataIdAsync([NotNull] string dataTarget, [NotNull] Guid userRuleId, [NotNull] Guid dataRuleId, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .FirstOrDefaultAsync(t => t.DataTargetName == dataTarget && t.UserRuleId == userRuleId && t.DataRuleId == dataRuleId, GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<UserDataRule>> GetListAsync([NotNull] string dataTarget, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .Where(r => r.IsEnable)
                .Where(r => r.DataTargetName == dataTarget)
                .OrderByDescending(r => r.RuleType)
                .ThenByDescending(r=>r.Priority)
                .ToListAsync();
        }
    }
}
